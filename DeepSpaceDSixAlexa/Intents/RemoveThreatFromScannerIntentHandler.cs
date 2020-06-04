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
    public class RemoveThreatFromScannerIntentHandler : SynchronousRequestHandler
    {
        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            return request.Intent.Name == "RemoveThreatFromScannerIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;
            if (!game.IsGameInProgress)
                return ResponseCreator.Ask("You need to start a new game before removing locked threats from your scanners. Say new game to begin. ", "To start, say new game. ", information.SkillRequest.Session);

            var ship = game.Ship as HalcyonShip;
            if (ship == null)
                return ResponseCreator.Ask("You cannot remove the locked threats from  your scanners in this ship. ", game.RepromptMessage, information.SkillRequest.Session);
            if (!ship.Crew.Any(c => c.Type == Enums.CrewType.Medical && c.State == CrewState.Available))
                return ResponseCreator.Ask($"We have no available medical crew to remove a locked threat from our scanners. We have {ship.GetAvailableCrewAsString()}. ", game.RepromptMessage, information.SkillRequest.Session);
            
            if (ship.ScannerCount< 1)
                return ResponseCreator.Ask("There are no locked threats on our scannerse. Use this command when you roll a threat that gets locked on the scanners. ", game.RepromptMessage, information.SkillRequest.Session);

            // we are good to go, remove a threat from the scanner
            ship.RemoveThreatFromScanner();

            game.Message += "Awaiting further orders, captain. ";
            game.RepeatMessage = game.Message;
            game.RepromptMessage = "Waiting for further orders, captain. ";
            game.SaveData();
            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
