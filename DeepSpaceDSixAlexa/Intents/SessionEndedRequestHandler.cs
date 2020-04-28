using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;

namespace DeepSpaceDSixAlexa.Intents
{
    public class SessionEndedRequestHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            return information.SkillRequest.GetRequestType() == typeof(SessionEndedRequest);
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            return ResponseBuilder.Empty();
        }
    }
}
