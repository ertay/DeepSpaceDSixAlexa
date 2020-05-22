using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using DeepSpaceDSixAlexa.Helpers;

namespace DeepSpaceDSixAlexa.Intents
{
    public class RulesIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "RulesIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            game.IsGameInProgress = false;
            game.GameState = Enums.GameState.Rules;
            game.RuleSelector = 0;
            string message = Utilities.GetRule(game.RuleSelector);
            

            game.RepromptMessage= message;
            game.Message = message;
            game.RepeatMessage = message;
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}

