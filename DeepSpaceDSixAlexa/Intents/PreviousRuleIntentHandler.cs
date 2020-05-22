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
    public class PreviousRuleIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            var request = (IntentRequest)information.SkillRequest.Request;
            return game.GameState == Enums.GameState.Rules && request.Intent.Name == BuiltInIntent.Previous;
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if (game.RuleSelector <= 0)
                return ResponseCreator.Ask("You are at the beginning of the rules. Say next to continue. You can say new game at any point to start a new game. ", "Say next to continue to the next rule, or new game to start a new game. ", information.SkillRequest.Session);


            game.RuleSelector--;
            string message = Utilities.GetRule(game.RuleSelector);


            game.RepromptMessage = message;
            game.Message = message;
            game.RepeatMessage = message;
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}

