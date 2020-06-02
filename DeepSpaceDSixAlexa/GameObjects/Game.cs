using Alexa.NET.Request;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.GameObjects.Managers;
using DeepSpaceDSixAlexa.GameObjects.PlayerShips;
using DeepSpaceDSixAlexa.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public GameState LastGameState { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public string Message { get; set; }
        public string RepromptMessage { get; set; }
        public string RepeatMessage { get; set; }

        public bool IsGameInProgress { get; set; }
        public int RuleSelector { get; set; }

        private EventManager _eventManager;

        public Game() { }

        public async Task InitializeGame(Session session)
        {
            _session = session;


            if (_session.Attributes == null)
            {
                // new session, check dynamodb if we have any data for this user and load it if so
                await LoadFromDb();
            }
            else
            {
                // not a new session, load data from session attributes
                LoadDataFromSessionAttributes();
            }
        }

        public void InitializeEventManager()
        {
            _eventManager = new EventManager();
            _eventManager.On("AppendMessage", (e) => Message += ((DefaultEvent)e).Message);
            // _eventManager.On("NewThreat", (e) => Message += $"{((DefaultEvent)e).Message}. ");


        }

        public void Welcome()
        {
            if (IsGameInProgress)
            {
                // previous game was not finished let's ask user whether they want to load game
                Message = "Welcome back, captain! Do you want to finish your last game? ";
                RepromptMessage = "To continue playing your last unfinished game, say yes. To go to the main menu, say no. ";

                GameState = GameState.ContinueGamePrompt;
            }
            else
            {
                Message = "Welcome to Deep Space D6 Beta version 2! Say new game to begin. Say rules to learn how to play. ";
                RepeatMessage = Message;
                RepromptMessage = "To start a new game, say new game. ";
                GameState = GameState.MainMenu;

            }
            SaveData();
        }

        public void CreateNewGame(int difficulty)
        {
            InitializeEventManager();
            IsGameInProgress = true;
            Message = "Starting a new game. ";

            Ship = new HalcyonShip();
            Ship.InitializeEvents(_eventManager);
            ThreatManager = new ThreatManager();
            ThreatManager.InitializeEvents(_eventManager);
            Ship.InitializeShip();

            ThreatManager.Initialize(difficulty);

            Message += $"Rolling  the crew dice. {SoundFx.Dice} ";
            Ship.RollCrewDice();

            GameState = GameState.PlayerTurn;

            Message += $"We have {Ship.GetAvailableCrewAsString()}. ";
            Message += Ship.ScannerCount > 0 ? $"Number of locked threats in our scanners is {Ship.ScannerCount}. " : "";
            Message += ThreatManager.InternalThreats.Count > 0 ? $"Number of active internal threats is {ThreatManager.InternalThreats.Count}. {ThreatManager.GetThreatsAsString(true, false)}. " : "";
            Message += ThreatManager.ExternalThreats.Count > 0 ? $"Number of active external threats is {ThreatManager.ExternalThreats.Count}. {ThreatManager.GetThreatsAsString(false, true)}. " : "";
            Message += ThreatManager.ExternalThreats.Count + ThreatManager.InternalThreats.Count < 1 ? "There are no active threats at the moment. " : "";
            Message += "What are your orders, captain? ";
            RepeatMessage = Message;
            RepromptMessage = $"Awaiting your orders, captain. ";

            SaveData();

        }

        public void EndTurn()
        {
            // check if we won
            if(IsVictorious())
            { 
                GameOver();
                SaveData();
                return;
            }

            // check if we need to return crew from a mission
            if(Ship.AvailableCrewCount + Ship.ReturningCrewCount < 1 && Ship.MissionCrewCount > 0)
            {
                Message = "You need to return a crew member from a mission because we won't have any available crew for the next turn. Say return crew from a mission to do that. ";
                return;
            }
            Ship.EndTurn();
            GameState = GameState.PlayerTurn;
            if (ThreatManager.ThreatDeck.Count > 0)
            {
                ThreatManager.DrawThreat();
            }
            ThreatManager.ActivateThreats();
            // reset threats when their turn is over
            ThreatManager.ResetThreats();
            // check if we are dead
            if(IsShipDestroyed())
            {
                GameOver();
                SaveData();
                return;
            }
            // check if we won, some threats can kill themselves when activating
            if(IsVictorious())
            {
                GameOver();
                SaveData();
                return;
            }
            // check if we're out of crew to roll
            if(AreCrewIncapacitated())
            {
                RepeatMessage = "I am sorry, captain. All of our crew is incapacitated. Game over. To start a new game, say new  game. ";
                Message += RepeatMessage;
                RepromptMessage = "Game over. To play again, say new game. ";
                GameOver();
                SaveData();
                return;
            }

            Message += $"Rolling the crew dice. {SoundFx.Dice} ";
            Ship.RollCrewDice();
            // check if ship is destroyed, this can happen if threat deck is empty and we rolled three scanners that deals damage
            if (IsShipDestroyed())
            {
                GameOver();
                SaveData();
                return;
            }
            Message += $"We have {Ship.GetAvailableCrewAsString()}. ";
            Message +=Ship.ScannerCount > 0 ? $"Number of locked threats in our scanners is {Ship.ScannerCount}. " : "";
            Message += ThreatManager.InternalThreats.Count > 0 ? $"Number of active internal threats is {ThreatManager.InternalThreats.Count}. {ThreatManager.GetThreatsAsString(true,false)}. " : "";
            Message += ThreatManager.ExternalThreats.Count > 0 ? $"Number of active external threats is {ThreatManager.ExternalThreats.Count}. {ThreatManager.GetThreatsAsString(false, true)}. " : "";
            Message += ThreatManager.ExternalThreats.Count + ThreatManager.InternalThreats.Count < 1 ? "There are no active threats at the moment. " : "";
            Message += "What are your orders, captain? ";
            RepeatMessage = Message;
            RepromptMessage = "What are your orders, captain? ";
            SaveData();
        }

        public void GameOver()
        {
            GameState = GameState.MainMenu;
            IsGameInProgress = false;
        }

        public bool IsVictorious()
        {
            if (ThreatManager == null)
                return false;
            // returns true if threat deck is empty and all external threats are destroyed
            bool victory =  ThreatManager.ThreatDeck.Count == 0 && ThreatManager.ExternalThreats.Count == 0;
            if(victory)
            {
                Message += "Congratulations, captain! All external threats destroyed and there are no more threats in the threat deck. To start a new game, say new game. ";
                RepromptMessage = "You are victorious! Say new game to play again. ";
                RepeatMessage = Message;
            }
            return victory;
        }

        public bool IsShipDestroyed()
        {
            if (Ship == null)
                return false;

            bool shipDestroyed =  Ship.Hull < 1;
            if(shipDestroyed)
            {
                RepeatMessage = "Oh no, our hull is in critical condition. It was nice serving with you, captain! Farewell! Game over. To play again, say new game. ";
                RepromptMessage = "To start a new game, say new game. ";
                Message += RepeatMessage;
            }
            return shipDestroyed;
            
        }

        public bool AreCrewIncapacitated()
        {
            if (Ship == null)
                return false;
            // returns true if there's no crew to roll for the next turn
            return Ship.ReturningCrewCount + Ship.AvailableCrewCount < 1;
        }

        public void SaveData()
        {
            if (_session == null)
            {
                throw new NullReferenceException("Session or session attributes not initialized.");
            }

            var attributes = new Dictionary<string, object>();
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            attributes.Add("game", JsonConvert.SerializeObject(this, settings));
            _session.Attributes = attributes;
        }

        private void LoadData(string jsonData)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            Game gameObject = JsonConvert.DeserializeObject<Game>(jsonData, settings);

            InitializeEventManager();
            if(gameObject.Ship != null)
            {
                Ship = gameObject.Ship;
                Ship.InitializeEvents(_eventManager);
            }
            if(gameObject.ThreatManager != null)
            {
                ThreatManager = gameObject.ThreatManager;
                ThreatManager.InitializeEvents(_eventManager);
            }
            
            RepromptMessage = gameObject.RepromptMessage;
            RepeatMessage = gameObject.RepeatMessage;
            GameState = gameObject.GameState;
            LastGameState = gameObject.LastGameState;
            IsGameInProgress = gameObject.IsGameInProgress;
            RuleSelector = gameObject.RuleSelector;

        }

        private void LoadDataFromSessionAttributes()
        {
            if (_session == null)
            {
                throw new NullReferenceException("Session not initialized. Cannot load data.");
            }

            if (_session.Attributes.Count < 1)
                return;

            
            LoadData(_session.Attributes["game"].ToString());
        }

        public async Task LoadFromDb()
        {
            var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWSAccessId"), Environment.GetEnvironmentVariable("AWSAccessSecret"));
            var client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);

            try
            {
                var table = Table.LoadTable(client, "DeepSpaceDSix");
                var item = await table.GetItemAsync(_session.User.UserId);
                if (item == null)
                {
                    // this user does not have any data on the server
                    // let's just initialize the attributes
                    _session.Attributes = new Dictionary<string, object>();
                    GameState = GameState.MainMenu;
                    return;
                }
                // this is a returning user, load data from db
                string gameSessionJson = item["GameData"];
                LoadData(gameSessionJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SaveDataToDb()
        {
            // saves the game object to dynamo db
            // save the last game state because we will need a different state for the prompt
            // but first check if game state is already the prompt
            if (GameState != GameState.ContinueGamePrompt)
                LastGameState = GameState;
            
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto};
            string gameSessionJson = JsonConvert.SerializeObject(this, settings);
            var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWSAccessId"), Environment.GetEnvironmentVariable("AWSAccessSecret"));

            var client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            try
            {
                var table = Table.LoadTable(client, "DeepSpaceDSix");
                var item = new Document();
                item["UserId"] = _session.User.UserId;
                item["GameData"] = gameSessionJson;

                await table.PutItemAsync(item);
                Console.WriteLine("Saved to DynamoDB.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

    }

}
