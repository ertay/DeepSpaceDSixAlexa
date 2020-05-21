using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using DeepSpaceDSixAlexa.GameObjects.Dice;
using DeepSpaceDSixAlexa.GameObjects.PlayerShips;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using System;
using System.Linq;

namespace DeepSpaceDSixAlexa.Intents
{
    public class SendCrewOnAMissionIntentHandler: SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "SendCrewOnAMissionIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if (!game.IsGameInProgress)
                return ResponseCreator.Ask("You need to start a new game before sending your crew on a mission. Say new game to begin. ", "To start, say new game. ", information.SkillRequest.Session);

            var ship = game.Ship;

            if (ship.AvailableCrewCount < 1)
                return ResponseCreator.Ask("We have no available crew to send on a mission. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if source crew type is available and present
            var request = (IntentRequest)information.SkillRequest.Request;
            string crewName = request.Intent.Slots["Crew"].Value;
            bool isCrewValid = Enum.TryParse(crewName.FirstCharToUpper(), out CrewType crewType);
            if (!isCrewValid)
                return ResponseCreator.Ask($"{crewName} is not a valid crew type. Try the send crew on a mission command again and provide one of the following  types: Tactical, Medical, Engineering, Science, or Commander. ", game.RepromptMessage, information.SkillRequest.Session);

            // valid crew provided, check if iti s available
            CrewDie crewDie = ship.Crew.FirstOrDefault(c => c.Type == crewType && c.State == CrewState.Available);
            if (crewDie == null)
                return ResponseCreator.Ask($"{crewName} crew is not available. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            // if crew is commander and comms are offline, commander can't go on a mission
            if (crewDie.Type == CrewType.Commander && ship.ShipSystems["CommsOffline"])
                return ResponseCreator.Ask("Our comms are offline. We cannot use our commander crew until you send an engineer on a mission to fix the communication systems. ", game.RepromptMessage, information.SkillRequest.Session);

            if (crewDie.Type == CrewType.Engineering&& ship.ShipSystems["EngineeringUnavailable"])
                return ResponseCreator.Ask("Our engineering crew is incapacitated from the panel explosion and cannot be used. Send a medical crew member on a mission to deal with the panel explosion. ", game.RepromptMessage, information.SkillRequest.Session);
            // we have a valid available crew die to send on a mission
            // check the threat
            if (game.ThreatManager.ExternalThreats.Count < 1 && game.ThreatManager.InternalThreats.Count < 1)
                return ResponseCreator.Ask("There are no threats at the moment. You can send your crew on missions when there is an active threat. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if enemy target is present
            string threatId = request.Intent.Slots["Threat"].GetSlotId();
            var threat = game.ThreatManager.ExternalThreats.FirstOrDefault(t => t.Id == threatId) as Threat;
            if (threat == null)
            {
                // check internal threats
                threat = game.ThreatManager.InternalThreats.FirstOrDefault(t => t.Id == threatId) as Threat;
                if(threat == null)
                {
                    string threatName = request.Intent.Slots["Threat"].Value;
                    // TODO: Show threats with missions only.
                    return ResponseCreator.Ask($"{threatName} is not a valid target. Try sending your crew on a mission again and provide one of the following: {game.ThreatManager.GetThreatsAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
                }
                
            }
            // we have a valid target, check if it has missions
            if (!threat.HasMission)
                return ResponseCreator.Ask($"{threat.Name} cannot be destroyed by sending crew on a mission. Use this command on targets that have away missions. ", game.RepromptMessage, information.SkillRequest.Session);
            // target has mission, check if we can assign this crew type
            // TODO: list the possible mission crew types for this threat in the response
            if(!threat.AwayMissions.Any(a => a.Type == crewDie.Type))
                return ResponseCreator.Ask($"You cannot send {crewName} on a mission to destroy {threat.Name}. Try a different crew type. ", game.RepromptMessage, information.SkillRequest.Session);

            // valid crew type for this threat selected
            // check if all the slots are filled for this type
            if(!threat.AwayMissions.Any(a => a.Type == crewDie.Type && !a.IsAssigned))
                return ResponseCreator.Ask($"You have already assigned the required number of {crewName} crew to {threat.Name}. Try sending a different crew type on a mission to destroy this  target. ", game.RepromptMessage, information.SkillRequest.Session);

            // check if science was sent on a mission while cosmic existentialism is active
            if (crewDie.Type == CrewType.Science&& ship.ShipSystems["ScienceUnavailable"])
            {
// science sent on mission while cosmic existentialism is active
// check if the mission is cosmic existentialism
if(threat.Id != "CE")
                    return ResponseCreator.Ask("Our science crew are having an existentialism crisis and are unavailable. Send a science crew on a mission to deal with cosmic existentialism to be able to use them again. ", game.RepromptMessage, information.SkillRequest.Session);
            }


            // we're good, assign this crew type to this threat
            ship.SendCrewOnMission(crewDie, threat);

            // check if mission complete and process cleanup
            if (game.ThreatManager.CheckIfMissionComplete(threat))
            {
                ship.CompleteMission(threat);
                game.Message += $"Mission complete! {threat.Name} is no longer a threat to our ship. ";
            }
            else
                game.Message += $"You sent one {crewName} crew on a mission to deal with the {threat.Name}. ";

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
