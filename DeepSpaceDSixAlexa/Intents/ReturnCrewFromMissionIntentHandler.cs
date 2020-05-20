using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using DeepSpaceDSixAlexa.GameObjects.Dice;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using System;
using System.Linq;

namespace DeepSpaceDSixAlexa.Intents
{
    public class ReturnCrewFromMissionIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "ReturnCrewFromMissionIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            var ship = game.Ship;

            if (ship.MissionCrewCount< 1)
                return ResponseCreator.Ask("We currently do not have any crew members that are sent on a mission. You can use this command when you have crew members that are sent on missions. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if source crew type is available and present
            var request = (IntentRequest)information.SkillRequest.Request;
            string crewName = request.Intent.Slots["Crew"].Value;
            bool isCrewValid = Enum.TryParse(crewName.FirstCharToUpper(), out CrewType crewType);
            if (!isCrewValid)
                return ResponseCreator.Ask($"{crewName} is not a valid crew type. Try the send crew on a mission command again and provide one of the following  types: Tactical, Medical, Engineering, Science, or Commander. ", game.RepromptMessage, information.SkillRequest.Session);

            // valid crew provided, check if iti s on a mission
            
            
            
            // check the threat
            if (game.ThreatManager.ExternalThreats.Count < 1 && game.ThreatManager.InternalThreats.Count < 1)
                return ResponseCreator.Ask("There are no threats at the moment. This shouldn't have happened, please report this to the developer. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if enemy target is present
            string threatId = request.Intent.Slots["Threat"].GetSlotId();
            var threat = game.ThreatManager.ExternalThreats.FirstOrDefault(t => t.Id == threatId) as Threat;
            if (threat == null)
            {
                // check internal threats
                threat = game.ThreatManager.InternalThreats.FirstOrDefault(t => t.Id == threatId) as Threat;
                if (threat == null)
                {
                    string threatName = request.Intent.Slots["Threat"].Value;
                    // TODO: Show threats with missions only.
                    return ResponseCreator.Ask($"{threatName} is not a valid target. Try the return crew from a mission command again and provide a different target. ", game.RepromptMessage, information.SkillRequest.Session);
                }

            }
            // we have a valid target, check if it has missions
            if (!threat.HasMission)
                return ResponseCreator.Ask($"{threat.Name} is not a valid threat. Try the return crew from a mission command again and provide a different threat. ", game.RepromptMessage, information.SkillRequest.Session);
            // target has mission, check if we can assign this crew type


            CrewDie crewDie = ship.Crew.FirstOrDefault(c => c.Type == crewType && c.State == CrewState.Mission && c.MissionName == threat.Name);
            if (crewDie == null)
                return ResponseCreator.Ask($"We do not have any {crewName} crew assigned on a mission to deal with {threat.Name}. ", game.RepromptMessage, information.SkillRequest.Session);

            // check if all the slots are filled for this type
            if (!threat.AwayMissions.Any(a => a.Type == crewDie.Type && a.IsAssigned))
                return ResponseCreator.Ask($"There are no {crewName} crew assigned to {threat.Name}. Use the return crew from mission command again and provide a valid crew type and threat. ", game.RepromptMessage, information.SkillRequest.Session);


            // we're good, return crew from this threat
            ship.ReturnCrewFromMission(crewDie, threat);

            

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
