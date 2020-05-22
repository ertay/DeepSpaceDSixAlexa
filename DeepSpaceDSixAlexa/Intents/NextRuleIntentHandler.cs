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
    public class NextRuleIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            var request = (IntentRequest)information.SkillRequest.Request;
            return game.GameState == Enums.GameState.Rules && request.Intent.Name == BuiltInIntent.Next;
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if(game.RuleSelector >= Utilities.NUMBER_OF_RULES)
                return ResponseCreator.Ask("You reached the end of the rules. Say new game to start a new game, or say back to go to the previous rule. ", "Say new game to begin, or back to go back to the previous rule. ", information.SkillRequest.Session);


            game.RuleSelector++;
            string message = Utilities.GetRule(game.RuleSelector);


            game.RepromptMessage = message;
            game.Message = message;
            game.RepeatMessage = message;
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}

