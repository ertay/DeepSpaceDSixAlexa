using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa.Intents
{
    public class RepeatIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var intentRequest = (IntentRequest)information.SkillRequest.Request;
            return intentRequest.Intent.Name== BuiltInIntent.Repeat;
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;

            return ResponseCreator.Ask(game.RepeatMessage,game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
