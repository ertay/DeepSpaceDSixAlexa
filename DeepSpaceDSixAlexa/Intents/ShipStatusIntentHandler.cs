using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.RequestHandlers;
using Alexa.NET.RequestHandlers.Handlers;
using Alexa.NET.Response;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using System.Linq;

namespace DeepSpaceDSixAlexa.Intents
{
    public class ShipStatusIntentHandler : SynchronousRequestHandler
    {
        private string _intentName = string.Empty;

        public override bool CanHandle(AlexaRequestInformation<SkillRequest> information)
        {
            var request = (IntentRequest)information.SkillRequest.Request;
            _intentName = request.Intent.Name;
            return _intentName == "ShipStatusIntent" || _intentName == "CrewStatusIntent"
                || _intentName == "ScannerStatusIntent" || _intentName == "ShipHealthStatusIntent";
        }

        public override SkillResponse HandleSyncRequest(AlexaRequestInformation<SkillRequest> information)
        {
            var game = (Game)information.Context;

            if (!game.IsGameInProgress)
                return ResponseCreator.Ask("To learn about the ship's status, you need to start a game. Say new game to begin. ", "To start, say new game. ", information.SkillRequest.Session);


            var ship = game.Ship;
            string message = "";
            if (_intentName == "ShipStatusIntent" || _intentName == "CrewStatusIntent")
            {
                message += ship.AvailableCrewCount > 0 ? $"{ship.GetAvailableCrewAsString()} waiting for orders. " : "We have no available crew. ";
                if (ship.MissionCrewCount > 0)
                {
                    var crewOnMissions = ship.Crew.Where(c => c.State == Enums.CrewState.Mission);
                    foreach (var item in crewOnMissions)
                    {
                        message += $"{item.Type.ToString()} is sent on a mission to deal with {item.MissionName}. ";
                    }
                }
                message += ship.InfirmaryCrewCount > 0 ? $"We have {ship.InfirmaryCrewCount} crew in the infirmary. " : "";
            }

            if(_intentName == "ShipStatusIntent")
                message += ship.ScannerCount > 0 ? $"The number of locked threats on our scanners is {ship.ScannerCount}. " : "";

            if(_intentName == "ScannerStatusIntent")
                message += ship.ScannerCount > 0 ? $"The number of locked threats on our scanners is {ship.ScannerCount}. " : "There are no locked threats on our scanners. ";
            if(_intentName == "ShipStatusIntent" || _intentName == "ShipHealthStatusIntent")
            message += $"We have {ship.Shields} out of {ship.MaxShields} shields, and {ship.Hull} out of {ship.MaxHull} hull. ";

            message += "What are your orders, captain? ";

            game.Message = message;

            return ResponseCreator.Ask(game.Message, game.RepromptMessage, information.SkillRequest.Session);
        }
    }
}
