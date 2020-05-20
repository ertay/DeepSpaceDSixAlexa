using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa.Intents
{
    public class ContinuePromptIntentHandler: SynchronousRequestHandler
    {
        private string _intentName;

        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var intentRequest = (IntentRequest)information.SkillRequest.Request;
            bool isContinuePrompt = ((Game)information.Context).GameState == GameState.ContinueGamePrompt;
            _intentName = intentRequest.Intent.Name;
            bool yesOrNoIntent = _intentName == BuiltInIntent.Yes || _intentName == BuiltInIntent.No;
            
            return isContinuePrompt && yesOrNoIntent;
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;

            if(_intentName == BuiltInIntent.Yes)
            {
                // we should continue previous game
                game.Message = game.RepeatMessage;
                game.RepromptMessage = "Awaiting your orders, captain. ";
                game.GameState = game.LastGameState;
                game.SaveData();
            }
            else
            {
                // answer was No, go back to main menu
                game.IsGameInProgress = false;
                game.Welcome();
            }

            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
