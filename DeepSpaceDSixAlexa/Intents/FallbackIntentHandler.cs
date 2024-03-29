﻿using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using System.Threading.Tasks;

namespace DeepSpaceDSixAlexa.Intents
{
    public class FallbackIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var intentRequest = (IntentRequest)information.SkillRequest.Request;
            return intentRequest.Intent.Name == BuiltInIntent.Fallback;
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            string message = "I'm sorry Captain, that is not a valid command. ";
            return ResponseCreator.Ask(message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }

    public class AlwaysTrueIntentHandler : AlwaysTrueRequestHandler
    {
        

        public override async Task<SkillResponse> Handle(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            string message = "I'm sorry Captain, that is not a valid command. Say help if you need assistance. ";
            return ResponseCreator.Ask(message, game.RepromptMessage, information.SkillRequest.Session);
        }

        
    }
}
