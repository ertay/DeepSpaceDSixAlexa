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
    public class CancelIntentHandler : IAlexaRequestHandler
    {
        public bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var intentRequest = (IntentRequest)information.SkillRequest.Request;
            return intentRequest.Intent.Name == BuiltInIntent.Cancel|| intentRequest.Intent.Name == BuiltInIntent.Stop;
        }

        public async Task<SkillResponse> Handle(AlexaRequestInformation<SkillRequest> information)
        {
            // session ended save current session data to dynamo db
            var game = (Game)information.Context;
            await game.SaveDataToDb();
            string message = "Thank you for playing Deep Space D6.";
            return ResponseBuilder.Tell(message);
        }
    }
}
