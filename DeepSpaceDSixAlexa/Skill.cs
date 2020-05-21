using Alexa.NET;
using Alexa.NET.LocaleSpeech;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepSpaceDSixAlexa.Extensions;
using Alexa.NET.RequestHandlers;
using DeepSpaceDSixAlexa.Intents;
using DeepSpaceDSixAlexa.GameObjects;

namespace DeepSpaceDSixAlexa
{
    public static class Skill
    {
        [FunctionName("DeepSpaceDSixAlexa")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            // Verifies that the request is indeed coming from Alexa.
            var isValid = await skillRequest.ValidateRequestAsync(req, log);
            if (!isValid)
            {
                return new BadRequestResult();
            }


            bool isFirstRequest = skillRequest.Session.Attributes == null ? true : false;
            Game game = new Game();
            await game.InitializeGame(skillRequest.Session);

            var pipeline = new AlexaRequestPipeline();

            pipeline.RequestHandlers.Add(new LaunchIntentHandler(isFirstRequest));
            pipeline.RequestHandlers.Add(new SessionEndedRequestHandler());
            pipeline.RequestHandlers.Add(new CancelIntentHandler());
            pipeline.RequestHandlers.Add(new HelpIntentHandler());
            pipeline.RequestHandlers.Add(new RepeatIntentHandler());
            pipeline.RequestHandlers.Add(new FallbackIntentHandler());
            pipeline.RequestHandlers.Add(new NewGameIntentHandler());
            pipeline.RequestHandlers.Add(new MoreTimeIntentHandler());
            pipeline.RequestHandlers.Add(new ShipStatusIntentHandler());
            pipeline.RequestHandlers.Add(new ThreatStatusIntentHandler());
            pipeline.RequestHandlers.Add(new ThreatInfoIntentHandler());
            pipeline.RequestHandlers.Add(new AssignTacticalToWeaponsIntentHandler());
            pipeline.RequestHandlers.Add(new FireWeaponsIntentHandler());
            pipeline.RequestHandlers.Add(new FireStasisBeamIntentHandler());
            pipeline.RequestHandlers.Add(new RechargeShieldsIntentHandler());
            pipeline.RequestHandlers.Add(new RepairHullIntentHandler());
            pipeline.RequestHandlers.Add(new HealCrewIntentHandler());
            pipeline.RequestHandlers.Add(new RemoveThreatFromScannerIntentHandler());
            pipeline.RequestHandlers.Add(new TransformCrewIntentHandler());
            pipeline.RequestHandlers.Add(new SendCrewOnAMissionIntentHandler());
            pipeline.RequestHandlers.Add(new ReturnCrewFromMissionIntentHandler());
            pipeline.RequestHandlers.Add(new EndTurnIntentHandler());
            pipeline.RequestHandlers.Add(new ContinuePromptIntentHandler());
            pipeline.RequestHandlers.Add(new AlwaysTrueIntentHandler());

            var response = await pipeline.Process(skillRequest, game);
         
            return new OkObjectResult(response);
        }

        
    }
}
