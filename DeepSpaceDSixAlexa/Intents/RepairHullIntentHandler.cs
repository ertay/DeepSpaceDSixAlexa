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
            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("This ship cannot be repaired. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Engineering&& c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available engineering crew to repair the hull. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            // TODO: Prevent hull repairs if engineering crew cannot be used
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
