using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.GameObjects.Managers;
using DeepSpaceDSixAlexa.GameObjects.PlayerShips;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSpaceDSixAlexa.GameObjects
{
    /// <summary>
    /// This class holds the current game data.
    /// This class is serialized to store it as a json string file on DynamoDB to track game progress
    /// </summary>
    public class Game
    {
        public Ship Ship { get; set; }
        public ThreatManager ThreatManager { get; set; }
        public GameState GameState { get; set; }

        public string Message { get; set; }
        public string RepromptMessage { get; set; }
        public string RepeatMessage { get; set; }

        public void CreateNewGame()
        {
            Ship = new HalcyonShip();
            Ship.InitializeShip();
            Ship.RollCrewDice();
            ThreatManager = new ThreatManager();
            GameState = GameState.PlayerTurn;

            Message = $"Starting a new game. Rolling crew dice. You have {Ship.GetAvailableCrewAsString()}. There are {Ship.ScannerCount} threats on the scanners. There are {Ship.ReturningCrewCount} returning crew. ";
            RepeatMessage = Message;
            RepromptMessage = "What are your orders, Captain?";

        }
    }
}
