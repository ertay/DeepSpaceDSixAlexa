using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa.Intents
{
    public class ThreatInfoIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "ThreatInfoIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;

            if(game.ThreatManager.ExternalThreats.Count < 1 && game.ThreatManager.InternalThreats.Count < 1)
            {
                return ResponseCreator.Ask("There are no active threats at the moment. Use the threat information command when you need information about an active threat. ", game.RepromptMessage, information.SkillRequest.Session);
            }

            string message = "";
            var request = (IntentRequest)information.SkillRequest.Request;
            string threatId = request.Intent.Slots["Threat"].GetSlotId();
            var threat = game.ThreatManager.GetActiveThreat(threatId);
            if (threat == null)
            {
                string threatName = request.Intent.Slots["Threat"].Value;
                return ResponseCreator.Ask($"{threatName} is not a valid active threat. Try the threat information command again and provide one of the following: {game.ThreatManager.GetThreatsAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            }

            // valid active threat let's grab the info message
            message += threat.GetInfoMessage();
            message += "What are your orders, captain? ";

            game.Message = message;

            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
