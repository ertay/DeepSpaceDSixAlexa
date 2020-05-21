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
            
            
            return isContinuePrompt;
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
            else if (_intentName == BuiltInIntent.No)
            {
                // answer was No, go back to main menu
                game.IsGameInProgress = false;
                game.Welcome();
            }
            else
            {
                // invalid answer, ask the continue game prompt again
                return ResponseCreator.Ask("I'm sorry, I didn't get that. Do you want to continue playing your last unfinished game? ", "To continue your last game, say yes. Otherwise, say no. ", information.SkillRequest.Session);
            }

            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
