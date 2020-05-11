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
            // we have a valid available crew die to send on a mission
            // check the threat
            if (game.ThreatManager.ExternalThreats.Count < 1 && game.ThreatManager.InternalThreats.Count < 1)
                return ResponseCreator.Ask("There are no threats at the moment. You can send your crew on missions when there is an active threat. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if enemy target is present
            string threatId = request.Intent.Slots["Threat"].GetSlotId();
            var threat = game.ThreatManager.ExternalThreats.FirstOrDefault(t => t.Id == threatId);
            if (threat == null)
            {
                string threatName = request.Intent.Slots["Threat"].Value;
                // TODO: Show threats with missions only.
                return ResponseCreator.Ask($"{threatName} is not a valid target. Try sending your crew on a mission again and provide one of the following: {game.ThreatManager.GetThreatsAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
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
