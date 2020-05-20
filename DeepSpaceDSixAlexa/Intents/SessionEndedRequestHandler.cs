using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.GameObjects;
using System.Threading.Tasks;

namespace DeepSpaceDSixAlexa.Intents
{
    public class SessionEndedRequestHandler : IAlexaRequestHandler
    {
        public bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            bool isTrue =  information.SkillRequest.GetRequestType() == typeof(SessionEndedRequest);
            return isTrue;
        }

        public async Task<SkillResponse> Handle(AlexaRequestInformation<SkillRequest> information)
        {
            // session ended save current session data to dynamo db
            var game = (Game)information.Context;
            await game.SaveDataToDb();
            return ResponseBuilder.Empty();
        }

        
    }
}
