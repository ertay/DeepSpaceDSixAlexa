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
    public class HealCrewIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "HealCrewIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("You cannot heal the crew in this ship. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Medic&& c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available medic crew to heal our units in the infirmary. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            // TODO: Prevent healing if medic crew cannot be used
            if (ship.InfirmaryCrewCount< 1)
                return ResponseCreator.Ask("There are no crew members in the infirmary. Use this command when someone ends up in the infirmary. ", game.RepromptMessage, information.SkillRequest.Session);

            // we are good to go, heal the units in t he infirmary
            ship.HealCrew();

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
