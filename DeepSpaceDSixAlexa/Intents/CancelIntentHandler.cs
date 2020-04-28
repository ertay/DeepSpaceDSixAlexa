using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace DeepSpaceDSixAlexa.Intents
{
    public class CancelIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var intentRequest = (IntentRequest)information.SkillRequest.Request;
            return intentRequest.Intent.Name == BuiltInIntent.Cancel|| intentRequest.Intent.Name == BuiltInIntent.Stop;
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            string message = "Thank you for playing Deep Space D6.";
            return ResponseBuilder.Tell(message);
        }
    }
}
