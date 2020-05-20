using System;
using System.Collections.Generic;
using System.Text;
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
    public class LaunchIntentHandler : SynchronousRequestHandler
    {
        private bool _firstRequest;
        public LaunchIntentHandler(bool firstRequest) : base()
        {
            _firstRequest = firstRequest;
        }


        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            return _firstRequest || information.SkillRequest.GetRequestType() == typeof(LaunchRequest);
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            game.Welcome();
            
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);

        }
    }
}
