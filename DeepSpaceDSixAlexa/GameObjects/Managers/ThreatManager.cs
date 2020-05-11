using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.GameObjects.Dice;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using DeepSpaceDSixAlexa.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSpaceDSixAlexa.GameObjects.Managers
{
    public class ThreatManager
    {
        public Queue<Threat> ThreatDeck { get; set; }
        public List<ExternalThreat> ExternalThreats { get; set; }
        public List<Threat> InternalThreats { get; set; }

        private EventManager _eventManager;
        public ThreatManager() { }

        public void InitializeEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
            // create events
            _eventManager.On("ScannerDrawThreatCard", (e) =>
            {
                // scanners detected a threat, draw a card

                DrawThreat();
                
            });
            _eventManager.On("ThreatDestroyed", (e) =>
            {
                var threat = ExternalThreats.First(t => t.Health <= 0);
                ExternalThreats.Remove(threat);
            });
        }

        public void Initialize(int difficulty)
        {
            ThreatDeck = new Queue<Threat>(Utilities.GenerateThreatDeck(difficulty));
            ExternalThreats = new List<ExternalThreat>();
            InternalThreats = new List<Threat>();

            DrawThreat();
        }

        public Threat DrawThreat()
        {
            // check if deck is empty
            if (ThreatDeck.Count < 1)
                return null;

            var threat = ThreatDeck.Dequeue();
            if (threat is ExternalThreat)
            {
                ExternalThreats.Add(threat as ExternalThreat);
                // TODO: Fire the on spawn method here to trigger abilities that should run when enemy spawns
                
            }
            _eventManager.Trigger("NewThreat", new DefaultEvent() { Message = threat.Name});
            return threat;

        }

        public void ActivateThreats()
        {
            if (InternalThreats.Count < 1 && ExternalThreats.Count < 1)
                return;
            // TODO: Activate internal threats first

            ThreatDie threatDie = new ThreatDie();
            string message = $"Rolling the threat die... The result is {threatDie.Value}. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
            
            var threatsToActivate = ExternalThreats.Where(t => !t.IsDisabled && t.ActivationList.Contains(threatDie.Value)).OrderByDescending(t => t.Health).ToList();
            if (threatsToActivate.Count < 1)
            {
                message = "No threats were activated this round. ";
                _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
            }

                foreach (var item in threatsToActivate)
            {
                item.Activate(_eventManager);
            }
            
        }

        public void ResetThreats()
        {
            // fire this before the player turn starts, used to remove any effects that disappear at the end of enemey's turn
            ExternalThreats.ForEach(t => t.IsDisabled = false);
            InternalThreats.ForEach(t => t.IsDisabled = false);
        }

        public bool CheckIfMissionComplete(Threat threat)
        {
            if(threat.AwayMissions.Count(a => a.IsAssigned) >= threat.MinimumMissionsToComplete)
            {
                // mission is complete, fire on destroy for this threat and remove it from the threat list
                threat.OnDestroy();
                threat.OnMissionComplete(_eventManager);
                ExternalThreats.Remove(threat as ExternalThreat);
                return true;
            }
            return false;
        }

        public string GetThreatsAsString()
        {
            if (ExternalThreats.Count == 1)
                return ExternalThreats.First().Name;
            else if (ExternalThreats.Count == 2)
                return $"{ExternalThreats.First().Name}, and {ExternalThreats.Last().Name}";
            else if (ExternalThreats.Count > 2)
            {
                string threats = "";
                for (int i = 0; i < ExternalThreats.Count; i++)
                {
                    if (i == ExternalThreats.Count - 1)
                        threats += $"and {ExternalThreats[i].Name}";
                    else
                        threats += $"{ExternalThreats[i].Name}, ";

                }
                return threats;
            }
            else
                return "no active threats. ";
        }
    }
}
