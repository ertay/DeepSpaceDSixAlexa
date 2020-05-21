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
    public class FireWeaponsIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "FireWeaponsIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if (!game.IsGameInProgress)
                return ResponseCreator.Ask("You need to start a new game before firing your weapons. Say new game to begin. ", "To start, say new game. ", information.SkillRequest.Session);

            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("That is not a valid action. ", game.RepromptMessage, information.SkillRequest.Session);
            if (game.GameState != GameState.FiringWeapons)
                return ResponseCreator.Ask($"You need to assign tactical crew to the weapons to fire them. Say assign tactical crew to weapons to do that. ", game.RepromptMessage, information.SkillRequest.Session);

            // check if enemy target is present
            var request = (IntentRequest)information.SkillRequest.Request;
            string threatId = request.Intent.Slots["ExternalThreat"].GetSlotId();
            var threat = game.ThreatManager.ExternalThreats.FirstOrDefault(t => t.Id == threatId);
            if(threat == null)
            {
                string threatName = request.Intent.Slots["ExternalThreat"].Value;
                return ResponseCreator.Ask($"{threatName} is not a valid target. Try firing weapons again and provide one of the following: {game.ThreatManager.GetThreatsAsString(false, true)}. ", game.RepromptMessage, information.SkillRequest.Session);
            }
            // let's check the damage amount 
            int damage = request.Intent.Slots["DamageAmount"].ExtractNumber();
            if(damage < 1 || damage > ship.DamagePool)
                return ResponseCreator.Ask($"Please provide a valid damage amount. You can deal up to {ship.DamagePool} damage. Say fire weapons to try again. ", game.RepromptMessage, information.SkillRequest.Session);

            // damage amount is correct, open fire!
            bool canFireAgain = ship.FireWeapons(threat, damage);

            // first check if we are dead after destroying threat, some threats may cause damage to use when they die
            if (game.IsShipDestroyed())
            {
                game.RepeatMessage = "Oh no, our hull is in critical condition. It was nice serving with you, captain! Farewell! Game over. To play again, say new game. ";
                game.RepromptMessage = "To start a new game, say new game. ";
                game.Message += game.RepeatMessage;
                game.GameOver();
                game.SaveData();
                return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
            }

            if (canFireAgain && game.ThreatManager.ExternalThreats.Count > 0)
            {
                game.Message += $"We have {ship.DamagePool} more damage to spend. Say fire weapons again and  choose one of the following targets: {game.ThreatManager.GetThreatsAsString(false, true)}. ";
                game.RepeatMessage = game.Message;
                game.RepromptMessage = $"Our tactical crew is waiting for your orders to open fire. We need to spend {ship.DamagePool} more damage. Say fire weapons and provide one of the following targets: {game.ThreatManager.GetThreatsAsString(false, true)}. ";
                game.SaveData();
                return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
            }
            // we're either out of damage to spend or  there are no more threats
            // go back to player turn state
            game.GameState = GameState.PlayerTurn;
            if (ship.DamagePool < 1)
                game.Message += "All weapons fired, captain. Awaiting further orders. ";
            else if (game.ThreatManager.ExternalThreats.Count < 1)
                game.Message += "All external threats destroyed! Awaiting further orders, sir. ";
            ship.DamagePool = 0;
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "What are your orders, sir? ";
            game.SaveData();
            
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
