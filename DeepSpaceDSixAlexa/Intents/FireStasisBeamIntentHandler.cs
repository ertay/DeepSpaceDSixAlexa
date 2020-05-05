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
    public class FireStasisBeamIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "FireStasisBeamIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("Our ship does not have a stasis beam. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Science && c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available science crew to fire the stasis beam. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);

            if (game.ThreatManager.ExternalThreats.Count < 1 && game.ThreatManager.InternalThreats.Count < 1)
                return ResponseCreator.Ask("There are no threats to disable with the stasis beam. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if enemy target is present
            var request = (IntentRequest)information.SkillRequest.Request;
            string threatId = request.Intent.Slots["Threat"].GetSlotId();
            var threat = game.ThreatManager.ExternalThreats.FirstOrDefault(t => t.Id == threatId);
            if (threat == null)
            {
                string threatName = request.Intent.Slots["ExternalThreat"].Value;
                return ResponseCreator.Ask($"{threatName} is not a valid target. Try firing the stasis beam again and provide one of the following: {game.ThreatManager.GetThreatsAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            }
            // we have a valid target, let's check if it is already disabled
            if(threat.IsDisabled)
                return ResponseCreator.Ask($"{threat.Name} is already disabled. We can use the stasis beam on targets that are still active. ", game.RepromptMessage, information.SkillRequest.Session);
            // we have a valid target, fire the stasis beam
            ship.FireStasisBeam(threat);

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
