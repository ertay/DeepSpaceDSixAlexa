using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using Alexa.NET.Response.Ssml;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using DeepSpaceDSixAlexa.Helpers;

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
            message += SoundFx.MoreTime(); ;

            string reprompt = "If you need more time, say more time. ";
            game.Message = message;

            return ResponseCreator.Ask(game.Message, reprompt, information.SkillRequest.Session);
        }
    }
}
