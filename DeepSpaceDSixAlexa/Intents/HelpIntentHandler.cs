using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa.Intents
{
    public class HelpIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var intentRequest = (IntentRequest)information.SkillRequest.Request;
            return intentRequest.Intent.Name== BuiltInIntent.Help;
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
string             message = "";
            switch (game.GameState)
            {
                case Enums.GameState.MainMenu:
                    message = "You are in the main menu. Say new game to begin, or say rules to learn how to play. ";
                    break;
                case Enums.GameState.PlayerTurn:
                    message = "Here's a reference for all commands: 1. End turn. 2. Transform. 3. Assign tactical crew to weapons. 4. Fire weapons. 5. Fire stasis beam. 6. Recharge Shields. 7. Heal Crew. 8. Remove Threat. 9. Repair Hull. 10. Send crew on a mission. 11. Return crew from a mission. 12. I need more  time. 13. Status. 14. Threat status. 15. Threat information. ";
                    break;
                case Enums.GameState.FiringWeapons:
                    message = "You have assigned tactical crew to your weapons. Say fire weapons to open fire. ";
                    break;
                case Enums.GameState.ContinueGamePrompt:
                    message = "Say yes to continue playing your last unfinished game. Say no to go to the main menu.";
                    break;
                case Enums.GameState.Rules:
                    message = "Say next to continue to the next rule, back to go back to the previous rule, or repeat to repeat the current rule. You can say new game at any point to start a new game. ";
                    break;
            }


            return ResponseCreator.Ask(message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
