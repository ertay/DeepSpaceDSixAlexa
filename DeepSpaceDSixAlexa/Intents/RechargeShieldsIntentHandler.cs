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

    public class RechargeShieldsIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "RechargeShieldsIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if (!game.IsGameInProgress)
                return ResponseCreator.Ask("You need to start a new game before recharging your shields. Say new game to begin. ", "To start, say new game. ", information.SkillRequest.Session);

            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("This ship cannot recharge the shields. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Science && c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available science crew to recharge the shields. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            
            if(ship.ShipSystems["ScienceUnavailable"])
                return ResponseCreator.Ask("Our science crew are having an existentialism crisis and are unavailable. Send a science crew on a mission to deal with cosmic existentialism to be able to use them again. ", game.RepromptMessage, information.SkillRequest.Session);

            if (ship.ShipSystems["ShieldsOffline"])
                return ResponseCreator.Ask("Our shields are offline and cannot be recharged. This is caused by the Nebula we are in. ", game.RepromptMessage, information.SkillRequest.Session);
            if (ship.Shields == ship.MaxShields)
                return ResponseCreator.Ask("Our shields are already at  maximum power, there is no need to recharge them. ", game.RepromptMessage, information.SkillRequest.Session);

            // we are good to go, recharge the shields
            ship.RechargeShields();

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
