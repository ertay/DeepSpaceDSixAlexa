using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;

namespace DeepSpaceDSixAlexa.Intents
{
    public class LaunchIntentHandler : LaunchSynchronousRequestHandler
    {
        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            string message = "Welcome to Deep Space D6! Say new game to begin. ";
            return ResponseCreator.Ask(message, RepromptBuilder.Create(message), information.SkillRequest.Session);

        }
    }
}
