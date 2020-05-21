using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa.Intents
{
    public class ThreatStatusIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "ThreatStatusIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;

            var threatManager= game.ThreatManager;
            string message = "";
            message += threatManager.InternalThreats.Count > 0 ? $"Number of active internal threats is {threatManager.InternalThreats.Count}. {threatManager.GetThreatsAsString(true, false)}. " : "";
            message += threatManager.ExternalThreats.Count > 0 ? $"Number of active external threats is {threatManager.ExternalThreats.Count}. {threatManager.GetThreatsAsString(false, true)}. " : "";
            message += threatManager.ExternalThreats.Count + threatManager.InternalThreats.Count < 1 ? "There are no active threats at the moment. " : "";
            message += threatManager.ThreatDeck.Count > 0 ? $"Number of threats remaining in the threat deck is {threatManager.ThreatDeck.Count}. " : "There are no more cards in the threat deck. Destroy all external threats to win! ";    
            
            message += "What are your orders, captain? ";

            game.Message = message;

            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
