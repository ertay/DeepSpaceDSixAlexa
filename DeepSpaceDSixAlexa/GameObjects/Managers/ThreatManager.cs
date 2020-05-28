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
        public List<InternalThreat> InternalThreats { get; set; }

        private bool _rollThreatDieAgain;

        private EventManager _eventManager;
        public ThreatManager() { }

        public void InitializeEvents(EventManager eventManager)
        {
            _eventManager = eventManager;
            // create events
            _eventManager.On("ScannerDrawThreatCard", (e) =>
            {
                // scanners detected a threat, draw a card
                if(ThreatDeck.Count < 1)
                {
                    // if threat deck is empty deal 1 damage to the player ship
                    var damageEvent = new DamageShipEvent(string.Empty, 1, $"We have three locked threats in our scanners, but the threat deck is empty. An unknown entity opened fire");
                    eventManager.Trigger("DamageShip", damageEvent);
                    return;
                }
                _eventManager.Trigger("AppendMessage", new DefaultEvent("Our scanners are detecting a new threat. "));
                DrawThreat();
                
            });
            _eventManager.On("DiscardThreat", (e) =>
            {
                var threat = ((DefaultThreatEvent)e).Threat;
                threat.OnDestroy(_eventManager);
                _eventManager.Trigger("MissionCleanup", (DefaultThreatEvent)e);
                if (threat is InternalThreat it)
                    InternalThreats.Remove(it);
                else if (threat is ExternalThreat et)
                    ExternalThreats.Remove(et);
            });

            _eventManager.On("CloakedThreatsActivated", (e) => _rollThreatDieAgain = true);

            _eventManager.On("RemoveCrewFromMission", (e) => RemoveCrewFromMission((RemoveCrewFromMissionEvent)e));
            _eventManager.On("TimeWarpActivated", (e) => HealExternalThreats());
        }

        public void HealExternalThreats()
        {
            string message = "";
            if(ExternalThreats.Count < 1)
            {
                return;
            }

            foreach (var item in ExternalThreats)
            {
                item.Health = Math.Min(++item.Health, item.MaxHealth);
            }
            message = "Time warp activated. All external threats recovered one health. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public void RemoveCrewFromMission(RemoveCrewFromMissionEvent e)
        {
            // search external threats first
            var threat = (Threat)ExternalThreats.FirstOrDefault(t => t.Name == e.ThreatName);
            if (threat == null)
                threat = InternalThreats.FirstOrDefault(t => t.Name == e.ThreatName);

            threat.AwayMissions.First(a => a.IsAssigned && a.Type == e.Type).IsAssigned = false;
            string message = $"{e.Type} crew was removed from the {e.ThreatName} mission. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public void Initialize(int noPanicCardNumber)
        {
            ThreatDeck = new Queue<Threat>(Utilities.GenerateThreatDeck(noPanicCardNumber));
            ExternalThreats = new List<ExternalThreat>();
            InternalThreats = new List<InternalThreat>();

            DrawThreat();
            DrawThreat();
        }

        public Threat DrawThreat()
        {
            // check if deck is empty
            if (ThreatDeck.Count < 1)
                return null;

            var threat = ThreatDeck.Dequeue();
            if (threat is ExternalThreat et)
            {
                ExternalThreats.Add(et);
                // TODO: Fire the on spawn method here to trigger abilities that should run when enemy spawns
                
            }
            else if(threat is InternalThreat it)
            {
                InternalThreats.Add(it);
            }
            
            threat.OnSpawn(_eventManager);
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
            var threatsToActivate = InternalThreats.Where(t => !t.IsDisabled && t.ActivationList.Contains(threatDie.Value)).ToList<Threat>();

            threatsToActivate.AddRange(ExternalThreats.Where(t => !t.IsDisabled && t.ActivationList.Contains(threatDie.Value)).OrderByDescending(t => t.Health).ToList<Threat>());
            
            
            // if no threats, check if we have mercenary
            if(threatsToActivate.Count < 1)
            {
                var mercenary = ExternalThreats.FirstOrDefault(t => t is MercenaryThreat && !t.IsDisabled);
                if (mercenary != null)
                    threatsToActivate.Add(mercenary);
            }
            // check if Scout needs to be added to the list
            if(threatsToActivate.Count > 0)
            {
                var scout = ExternalThreats.FirstOrDefault(t => t is ScoutingShipThreat && !t.IsDisabled);
                if (scout != null)
                    threatsToActivate.Add(scout);

            }

            
            if (threatsToActivate.Count < 1)
            {
                message = "No threats were activated on this roll. ";
                _eventManager.Trigger("AppendMessage", new DefaultEvent(message));

            }

                foreach (var item in threatsToActivate)
            {
                item.Activate(_eventManager);
            }

                // if cloaked threats activated, we run this method again
                if(_rollThreatDieAgain)
            {
                _rollThreatDieAgain = false;
                ActivateThreats();
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

                threat.OnMissionComplete(_eventManager);
                _eventManager.Trigger("DiscardThreat",  new DefaultThreatEvent(threat));
                
                return true;
            }
            return false;
        }

        public Threat GetActiveThreat(string id)
        {
            Threat threat = ExternalThreats.FirstOrDefault(t => t.Id == id) as Threat;
            if (threat == null)
            {
                // no active external threat with that id, let's check internal threats
                threat = InternalThreats.FirstOrDefault(t => t.Id == id) as Threat;
            }
            return threat;
        }

        public string GetThreatsAsString(bool includeInternalThreats = true, bool includeExternalThreats= true)
        {
            var activeThreats = new List<Threat>();
            if(includeInternalThreats)
                activeThreats.AddRange(InternalThreats);
            if(includeExternalThreats)
                activeThreats.AddRange(ExternalThreats);
            if (activeThreats.Count == 1)
                return activeThreats.First().Name;
            else if (activeThreats.Count == 2)
                return $"{activeThreats.First().Name}, and {activeThreats.Last().Name}";
            else if (activeThreats.Count > 2)
            {
                string threats = "";
                for (int i = 0; i < activeThreats.Count; i++)
                {
                    if (i == activeThreats.Count - 1)
                        threats += $"and {activeThreats[i].Name}";
                    else
                        threats += $"{activeThreats[i].Name}, ";

                }
                return threats;
            }
            else
                return "no active threats. ";
        }
    }
}
