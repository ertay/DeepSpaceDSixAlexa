using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa.Intents
{

    public class MoreTimeIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "NeedMoreTimeIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if (!game.IsGameInProgress)
                return ResponseCreator.Ask("You can ask for more time when you're playing the game. Say new game to begin. ", "To start, say new game. ", information.SkillRequest.Session);
            string message = "When ready, say Alexa to issue your next command. ";
            message += "<audio src=\"soundbank://soundlibrary/ui/gameshow/amzn_ui_sfx_gameshow_countdown_loop_64s_minimal_01\"/> What are your orders, captain? ";

            string reprompt = "If you need more time, say more time. ";
            game.Message = message;

            return ResponseCreator.Ask(game.Message, reprompt, information.SkillRequest.Session);
        }
    }
}
