using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using DeepSpaceDSixAlexa.GameObjects.PlayerShips;
using System.Linq;

namespace DeepSpaceDSixAlexa.Intents
{
    public class FireWeaponsIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "FireWeaponsIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("That is not a valid action. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Tactical && c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available tactical crew to fire your weapons. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);

            if (game.ThreatManager.ExternalThreats.Count < 1)
                return ResponseCreator.Ask("There are no external threats to fire upon. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if enemy target is present
            var request = (IntentRequest)information.SkillRequest.Request;
            string threatId = request.Intent.Slots["ExternalThreat"].GetSlotId();
            var threat = game.ThreatManager.ExternalThreats.FirstOrDefault(t => t.Id == threatId);
            if(threat == null)
            {
                string threatName = request.Intent.Slots["ExternalThreat"].Value;
                return ResponseCreator.Ask($"{threatName} is not a valid target. Try firing weapons again and provide one of the following: {game.ThreatManager.GetThreatsAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            }
            // we have a valid target, open fire!
            ship.FireWeapons(threat);

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
