using Alexa.NET.Request;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.GameObjects.Managers;
using DeepSpaceDSixAlexa.GameObjects.PlayerShips;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepSpaceDSixAlexa.GameObjects
{
    /// <summary>
    /// This class holds the current game data.
    /// This class is serialized to store it as a json string file on DynamoDB to track game progress
    /// </summary>
    public class Game
    {
        private Session _session;
        public Ship Ship { get; set; }
        public ThreatManager ThreatManager { get; set; }
        public GameState GameState { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string Message { get; set; }
        public string RepromptMessage { get; set; }
        public string RepeatMessage { get; set; }

        private EventManager _eventManager;

        public Game() { }
        public Game(Session session)
        {
            _session = session;
            

            if(_session.Attributes == null)
            {
                // new session, check dynamodb if we have any data for this user and load it if so
                _session.Attributes = new Dictionary<string, object>();
                GameState = GameState.MainMenu;
            }
            else
            {
                // not a new session, load data
                LoadData();
            }
        }

        public void InitializeEventManager()
        {
            _eventManager = new EventManager();
            _eventManager.On("AppendMessage", (e) => Message += ((DefaultEvent)e).Message);
            _eventManager.On("ScannerDrawThreatCard", (e) => Message += $"Our scanners have detected a new threat: ");
            _eventManager.On("NewThreat", (e) => Message += $"{((DefaultEvent)e).Message}. ");
            

        }

        public void CreateNewGame()
        {
            InitializeEventManager();

            Message = "Starting a new game. ";

            Ship = new HalcyonShip();
            Ship.InitializeEvents(_eventManager);
            ThreatManager = new ThreatManager();
            ThreatManager.InitializeEvents(_eventManager);
            Ship.InitializeShip();
            Message += "Captain, we are receiving a transmission from ";
            ThreatManager.Initialize(6);
            
            Message += "Rolling  the crew dice. ";
            Ship.RollCrewDice();
            
            GameState = GameState.PlayerTurn;

            Message += $"We have {Ship.GetAvailableCrewAsString()}. ";
            Message += Ship.ScannerCount > 0 ? $"Number of threats on our scanners is {Ship.ScannerCount}. What are your orders, captain?": "What are your orders, captain?";
            RepeatMessage = Message;
            RepromptMessage = $"{ThreatManager.GetThreatsAsString()}. We have {Ship.GetAvailableCrewAsString()}. What are your orders, Captain?";
            
            SaveData();

        }

        public void EndTurn()
        {
            Ship.EndTurn();
            if (ThreatManager.ThreatDeck.Count > 0)
            {
                Message += "New threat has entered the battle: ";
                ThreatManager.DrawThreat();
            }
            ThreatManager.ActivateThreats();
            // reset threats when their turn is over
            ThreatManager.ResetThreats();
            Message += "Rolling the crew dice. ";
            Ship.RollCrewDice();
            Message += $"We have {Ship.GetAvailableCrewAsString()}. ";
            Message += $"There are {ThreatManager.ExternalThreats.Count} external threats: {ThreatManager.GetThreatsAsString()}. ";
            RepeatMessage = Message;
            RepromptMessage = "What are your orders, captain? ";
            SaveData(); ;
        }

        public void SaveData()
        {
            if (_session == null || _session.Attributes == null)
            {
                throw new NullReferenceException("Session or session attributes not initialized.");
            }

            var attributes = new Dictionary<string, object>();
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto};
            attributes.Add("game", JsonConvert.SerializeObject(this, settings));
            _session.Attributes = attributes;
        }

        private void LoadData()
        {
            if (_session == null)
            {
                throw new NullReferenceException("Session not initialized. Cannot load data.");
            }

            if (_session.Attributes.Count < 1)
                return;

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto};
            Game gameObject = JsonConvert.DeserializeObject<Game>(_session.Attributes["game"].ToString(), settings);

            InitializeEventManager();

            Ship = gameObject.Ship;
            Ship.InitializeEvents(_eventManager);
            ThreatManager = gameObject.ThreatManager;
            ThreatManager.InitializeEvents(_eventManager);
            RepromptMessage = gameObject.RepromptMessage;
            RepeatMessage = gameObject.RepeatMessage;
            GameState = gameObject.GameState;
        }
    }
}
