using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa.Intents
{
    public class NewGameIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "NewGameIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game) information.Context;
            var request = (IntentRequest)information.SkillRequest.Request;
            int difficulty = request.Intent.Slots["NoPanicNumber"].ExtractNumber();

            if (difficulty< 0 || difficulty > 6)
                return ResponseCreator.Ask($"Please provide a valid difficulty level. Say new game again and provide a number between zero and six. A Higher number of Don't Panic cards will make the game easier. ", game.RepromptMessage, information.SkillRequest.Session);

            game.CreateNewGame(difficulty);
            
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
