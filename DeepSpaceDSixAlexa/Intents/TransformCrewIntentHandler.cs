using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using DeepSpaceDSixAlexa.GameObjects.PlayerShips;
using System;
using System.Linq;

namespace DeepSpaceDSixAlexa.Intents
{
    public class TransformCrewIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "TransformCrewIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if (!game.IsGameInProgress)
                return ResponseCreator.Ask("You need to start a new game before transforming your crew. Say new game to begin. ", "To start, say new game. ", information.SkillRequest.Session);

            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("That is not a valid command for this ship. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Commander&& c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available commander to transform crew. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);

            if (ship.ShipSystems["CommsOffline"])
                return ResponseCreator.Ask("Our comms are offline. We cannot use our commander crew until you send an engineer on a mission to fix the communication systems. ", game.RepromptMessage, information.SkillRequest.Session);

            if (ship.AvailableCrewCount < 2)
                return ResponseCreator.Ask("There are no other available crew members to transform. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if source crew type is available and present
            var request = (IntentRequest)information.SkillRequest.Request;
            string sourceCrewName = request.Intent.Slots["SourceCrew"].Value;
            bool isSourceValid = Enum.TryParse(sourceCrewName.FirstCharToUpper(), out CrewType sourceCrew);
            if(!isSourceValid)
                return ResponseCreator.Ask($"{sourceCrewName} is not a valid crew type. Try the transform crew command again and provide one of the following  types: Tactical, Medical, Engineering, Science, or Commander. ", game.RepromptMessage, information.SkillRequest.Session);
            
            // check if source is commander, and there is no second commander to transform
            if(sourceCrew == CrewType.Commander && ship.Crew.Count(c => c.Type == CrewType.Commander && c.State == CrewState.Available) < 2)
                return ResponseCreator.Ask($"You do not have another commander that you can transform into a different crew type. A commander cannot transform itself into a different crew type. Try the transform command again and provide a different crew type. ", game.RepromptMessage, information.SkillRequest.Session);

            // valid source crew provided, let's check if it is available in our crew
            if (!ship.Crew.Any(c => c.Type == sourceCrew && c.State == CrewState.Available))
                return ResponseCreator.Ask($"{sourceCrewName} crew is not available. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);

            // source is valid, let's check the target now
            string targetCrewName = request.Intent.Slots["TargetCrew"].Value;
            bool isTargetValid = Enum.TryParse(targetCrewName.FirstCharToUpper(), out CrewType targetCrew);
            if (!isTargetValid)
                return ResponseCreator.Ask($"{targetCrewName} is not a valid crew type to transform to. Try the transform crew command again and provide one of the following  types: Tactical, Medical, Engineering, Science, or Commander. ", game.RepromptMessage, information.SkillRequest.Session);

            // finally let's check if source is equal to target
            if (sourceCrew == targetCrew)
                return ResponseCreator.Ask($"You cannot transform {sourceCrewName} to {targetCrewName}. Try the command again and provide a different crew type to transform to. ", game.RepromptMessage, information.SkillRequest.Session);


            // we are all good, transform the crew!
            ship.TransformCrew(sourceCrew, targetCrew);

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
