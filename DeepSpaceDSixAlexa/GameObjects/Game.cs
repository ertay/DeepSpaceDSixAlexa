using Alexa.NET.Request;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.GameObjects.Managers;
using DeepSpaceDSixAlexa.GameObjects.PlayerShips;
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
            _eventManager.On("ScannerDrawThreatCard", (e) => Message += $"Our scanners are detecting a new threat. ");
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
                Message = "Welcome to Deep Space D6! Say new game to begin. ";
                RepeatMessage = Message;
                RepromptMessage = "To start a new game, say new game. ";
                GameState = GameState.MainMenu;

            }
            SaveData();
        }

        public void CreateNewGame()
        {
            InitializeEventManager();
            IsGameInProgress = true;
            Message = "Starting a new game. ";

            Ship = new HalcyonShip();
            Ship.InitializeEvents(_eventManager);
            ThreatManager = new ThreatManager();
            ThreatManager.InitializeEvents(_eventManager);
            Ship.InitializeShip();

            ThreatManager.Initialize(6);

            Message += "Rolling  the crew dice. ";
            Ship.RollCrewDice();

            GameState = GameState.PlayerTurn;

            Message += $"We have {Ship.GetAvailableCrewAsString()}. ";
            Message += Ship.ScannerCount > 0 ? $"Number of threats on our scanners is {Ship.ScannerCount}. What are your orders, captain?" : "What are your orders, captain?";
            RepeatMessage = Message;
            RepromptMessage = $"{ThreatManager.GetThreatsAsString()}. We have {Ship.GetAvailableCrewAsString()}. What are your orders, Captain?";

            SaveData();

        }

        public void EndTurn()
        {
            Ship.EndTurn();
            GameState = GameState.PlayerTurn;
            if (ThreatManager.ThreatDeck.Count > 0)
            {
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
