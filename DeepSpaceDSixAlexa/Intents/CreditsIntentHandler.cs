using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa.Intents
{
    public class CreditsIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var intentRequest = (IntentRequest)information.SkillRequest.Request;
            return intentRequest.Intent.Name == "CreditsIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            string message = "Deep Space D-6 Beta Version 3, created by Ertay Shashko. This is a fanmade Alexa adaptation of the free print and play board game designed by Tony Go, and published by Tau Leader Games. This Alexa variant was created to make the game fully accessible to blind players. If you like the game, you can buy the physical board game. If you have any questions or comments about the game, you can get in touch by sending an email to SightlessFun@outlook.com. Special thanks to Christopher Lehman for becoming a Guardian on the Sightless Fun Patreon to support this and other Alexa projects. To find out more about Sightless Fun and other Alexa games that are adapted from Board Games, check out www.sightless.fun. To start a new game, say new game. To learn how to play, say rules. ";
            return ResponseCreator.Ask(message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
