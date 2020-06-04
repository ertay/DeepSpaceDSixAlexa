using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using DeepSpaceDSixAlexa.GameObjects.PlayerShips;
using System.Linq;

namespace DeepSpaceDSixAlexa.Intents
{
    public class RepairHullIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "RepairHullIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if (!game.IsGameInProgress)
                return ResponseCreator.Ask("You need to start a new game before repairing your ship. Say new game to begin. ", "To start, say new game. ", information.SkillRequest.Session);

            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("This ship cannot be repaired. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Engineering&& c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available engineering crew to repair the hull. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            
            if (ship.ShipSystems["EngineeringUnavailable"])
                return ResponseCreator.Ask("Our engineering crew is incapacitated from the panel explosion and cannot be used. Send a medical crew member on a mission to deal with the panel explosion. ", game.RepromptMessage, information.SkillRequest.Session);
            if (ship.Hull== ship.MaxHull)
                return ResponseCreator.Ask("The ship's hull has not sustained any damage  yet. There is nothing to repair at the moment. ", game.RepromptMessage, information.SkillRequest.Session);
            
            // we are good to go, repair the hull
            ship.RepairShip();

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
