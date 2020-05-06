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
    public class AssignTacticalToWeaponsIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "AssignTacticalCrewIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("That is not a valid action. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Tactical && c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available tactical crew to fire weapons. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);

            if (game.ThreatManager.ExternalThreats.Count < 1)
                return ResponseCreator.Ask("Our scanners do not show any external threats at the moment. There is no need to assign tactical crew to the weapon systems. ", game.RepromptMessage, information.SkillRequest.Session);
            // check if enemy target is present
            var request = (IntentRequest)information.SkillRequest.Request;
            int tacticalCount= request.Intent.Slots["NumberOfUnits"].ExtractNumber();
            int availableTacticalCount = ship.Crew.Count(c => c.Type == CrewType.Tactical && c.State == CrewState.Available);
            if (tacticalCount < 1 || tacticalCount > availableTacticalCount)
                return ResponseCreator.Ask($"You can assign up to {availableTacticalCount} tactical crew. Try the assign tactical crew command again and provide a valid number. ", game.RepromptMessage, information.SkillRequest.Session);

            // we have valid tactical count, assign them
            ship.AssignTacticalToWeapons(tacticalCount);
            // set game state to Firing Weapons
            game.GameState = GameState.FiringWeapons;
            
            game.RepeatMessage = game.Message;
            game.RepromptMessage = $"We are in combat, captain! We can deal up to {ship.DamagePool} damage. Awaiting your orders to fire the weapons! We can fire the weapons at the following: {game.ThreatManager.GetThreatsAsString()}. ";
            game.SaveData();

            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
