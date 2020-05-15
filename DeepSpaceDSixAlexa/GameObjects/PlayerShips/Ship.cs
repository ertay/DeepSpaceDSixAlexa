using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.GameObjects.Dice;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;

namespace DeepSpaceDSixAlexa.GameObjects.PlayerShips
{
    /// <summary>
    /// Base ship class. All player ships extend from this class.
    /// </summary>
    public class Ship
    {
        public virtual int Hull { get; set; }
        [JsonIgnore]
        public virtual int MaxHull { get; }
        public virtual int Shields { get; set; }
        [JsonIgnore]
        public virtual int MaxShields { get; }
        [JsonIgnore]
        public virtual int ScannerSize => 3;

        public List<CrewDie> Crew { get; set; }

        public Dictionary<string, bool> ShipSystems { get; set; }
        
        [JsonIgnore]
        public int AvailableCrewCount => Crew.Count(c => c.State == CrewState.Available);
        [JsonIgnore]
        public int ReturningCrewCount => Crew.Count(c => c.State == CrewState.Returning);
        [JsonIgnore]
        public int InfirmaryCrewCount => Crew.Count(c => c.State == CrewState.Infirmary);
        [JsonIgnore]
        public int MissionCrewCount => Crew.Count(c => c.State == CrewState.Mission);
        [JsonIgnore]
        public int ScannerCount => Crew.Count(c => c.State == CrewState.Locked);

        private bool _shipAttackedThisRound;

        protected EventManager _eventManager;

        public Ship() { }
        public virtual void InitializeEvents(EventManager eventManager)
        {
            _eventManager = eventManager;

            _eventManager.On("DamageShip", (e) => ProcessDamage((DamageShipEvent)e));
            _eventManager.On("BoardingShipMissionComplete", (e) => ProcessBoardingShipMission((DefaultEvent)e));
            _eventManager.On("BomberAttack", (e) => AddCrewToInfirmary());
            _eventManager.On("InvadersActivated", (e) => AddCrewToInfirmary());
            _eventManager.On("PandemicActivated", (e) => AddCrewToInfirmary());
            _eventManager.On("RobotUprisingActivated", (e) => AddCrewToInfirmary()
            );
            _eventManager.On("DisableShields", (e) => DisableShields());
            _eventManager.On("NebulaSpawned", (e) => NebulaSpawned());
            _eventManager.On("NebulaDestroyed", (e) => ShipSystems["ShieldsOffline"] = false);
            _eventManager.On("ScoutAttack", (e) => ProcessScoutAttack((DamageShipEvent)e));
            _eventManager.On("BoostMoraleActivated", (e) => ProcessBoostMorale());
            eventManager.On("CommsOfflineActivated", (e) => ShipSystems["CommsOffline"] = true);
            eventManager.On("CommsOfflineDeactivated", (e) => ShipSystems["CommsOffline"] = false);
            eventManager.On("CosmicExistentialismActivated", (e) => ShipSystems["ScienceUnavailable"] = true);
            eventManager.On("CosmicExistentialismDeactivated", (e) => ShipSystems["ScienceUnavailable"] = false);
            _eventManager.On("DistractedActivated", (e) => ProcessDistracted());
            _eventManager.On("DistractedDeactivated", (e) => ProcessDistracted(true));
            _eventManager.On("FriendlyFire", (e) => ProcessFriendlyFire());
            eventManager.On("PanelExplosionActivated", (e) => ShipSystems["EngineeringUnavailable"] = true);
            eventManager.On("PanelExplosionDeactivated", (e) => ShipSystems["EngineeringUnavailable"] = false);
        }

        private void ProcessFriendlyFire()
        {
            var tacticalCrew = Crew.Where(c => c.Type == CrewType.Tactical && c.State != CrewState.Infirmary && c.State != CrewState.Distracted).ToList();
            int tacticalCount= tacticalCrew.Count;
            string message = "";
            if(tacticalCount < 1)
            {
                message = "Friendly fire drawn from the threat deck, but we had no tactical crew in this round to get wounded and moved to infirmary. ";
                _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
                return;
            }
            // wew have tactical crew let's move them to infirmary
            message = $"Friendly fire drawn from the threat deck. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
            AddCrewToInfirmary(tacticalCrew, tacticalCount);            

        }

        private void ProcessDistracted(bool isComplete = false)
        {
            // processes the distracted event
            // when is complete is true, it means that we should return the distracted unit
            string message = "";
            if(isComplete)
            {
                Crew.First(c => c.State == CrewState.Distracted).State = CrewState.Returning;
                message = "A distracted crew member will be available on the next round. ";
                _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
                return;
            }
            // distract a crew mebmer, look in returning crew first
            CrewDie crew = null;
            if (ReturningCrewCount > 0)
                crew = Crew.First(c => c.State == CrewState.Returning);
            else if (AvailableCrewCount > 0)
                crew = Crew.First(c => c.State == CrewState.Available);
            else
            {
                message = "We have no returning or available crew to distract. ";
                _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
                return;
            }
            crew.State = CrewState.Distracted;
            message = "One crew member is distracted and cannot be used until you send two medical crew on a mission to deal with the distracted  threat, or the threat die rolls a three or a four. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));

        }

        private void ProcessBoostMorale()
        {
            string message = "";
            if (ScannerCount < 1)
                message = "Boost morale activated, but our scanneres were already empty. ";
            else
            {
                Crew.First(c => c.State == CrewState.Locked).State = CrewState.Returning;
                message = "Boost morale was activated and one threat was removed from our scanners! ";
            }
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public void NebulaSpawned()
        {
            ShipSystems["ShieldsOffline"] = true;
            Shields = 0;
            string message = $"We have entered a Nebula. Our shields are down and cannot be recharged while the Nebula is active. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }
        
        public void DisableShields()
        {
            // if shields already down, no need to disable
            if (Shields < 1)
                return;
            Shields = 0;
            string message = "Our shields are down. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }
        /// <summary>
        /// Adds crew to the infirmary. If no parameters specified, one crew member will be added.
        /// Otherwise if a list is supplied as a parameter, crew in that list will be moved to infirmary.
        /// Count is equal to how many units from the lsit will be moved to infirmary.
        /// </summary>
        /// <param name="crew"></param>
        /// <param name="count"></param>
        public void AddCrewToInfirmary(List<CrewDie> crew= null, int count = 1)
        {
            if (crew == null)
            {
                crew = Crew.Where(c => c.State == CrewState.Available || c.State == CrewState.Returning || c.State == CrewState.Mission).OrderBy(c => c.State).ToList();
            }
            if (crew.Count < 1)
                return;
            int totalCount = count;
            foreach (var item in crew)
            {
                if (count < 1)
                    break;
                item.State = CrewState.Infirmary;
                if(!string.IsNullOrEmpty(item.MissionName))
                {
                    // we moved a crew member from a mission to the infirmary, trigger an event to notify threat manager to update the threat
                    _eventManager.Trigger("RemoveCrewFromMission", new RemoveCrewFromMissionEvent(item.MissionName, item.Type));
                    item.MissionName = string.Empty;
                }
                count--;
            }
            string message = $"{totalCount} crew moved to the infirmary. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));

        }

        public void ProcessBoardingShipMission(DefaultEvent e)
        {
            // the tactical crew used to complete the boarding ship mission goes to infirmary
            var tactical = Crew.First(c => c.Type == CrewType.Tactical&& c.MissionName == e.Message);
            tactical.State = CrewState.Infirmary;
            tactical.MissionName = string.Empty;
        }

        public void ProcessDamage(DamageShipEvent e)
        {
            _shipAttackedThisRound = true;
            if(e.IgnoreShields || Shields == 0)
            {
                Hull -= e.Damage;
                Hull = Math.Max(0, Hull);
                string msg = $"{e.Message} and caused {e.Damage} hull damage. ";
                _eventManager.Trigger("AppendMessage", new DefaultEvent(msg));
                return;
            }
            string message = "";

            
            Shields -= e.Damage;
            if (Shields < 0)
            {
                Hull -= Math.Abs(Shields);
                Hull = Math.Max(0, Hull);
                message = $"{e.Message} which destroyed our shields and caused {Math.Abs(Shields)} hull damage. ";
                Shields = 0;
            }
            else
                message = $"{e.Message} and caused {e.Damage} damage to our shields. ";

            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));

        }

        public void ProcessScoutAttack(DamageShipEvent e)
        {
            if (_shipAttackedThisRound)
                ProcessDamage(e);
        }

        public virtual void InitializeShip()
        {
            // initialize the crew
            Crew = new List<CrewDie>();
            for (int i = 0; i < 6; i++)
            {
                Crew.Add(new CrewDie());
            }
            ShipSystems = new Dictionary<string, bool>();
            ShipSystems.Add("ShieldsOffline", false);
            ShipSystems.Add("CommsOffline", false);
            ShipSystems.Add("ScienceUnavailable", false);
            ShipSystems.Add("EngineeringUnavailable", false);

        }

        public void RollCrewDice()
        {
            Crew.ForEach(c => c.Roll());
            // check if scanners have 3 threats and notify ThreatManager to draw a new threat
            
            while(Crew.Count(c => c.State == CrewState.Locked) >= ScannerSize)
            {
                for (int i = 0; i < ScannerSize; i++)
                {
                    Crew.First(c => c.State == CrewState.Locked).State = CrewState.Returning;
                }
                _eventManager.Trigger("ScannerDrawThreatCard");
            }
        }

        public virtual void EndTurn() { }

        public void SendCrewOnMission(CrewDie crew, Threat threat)
        {
            threat.AwayMissions.First(a => a.Type == crew.Type && !a.IsAssigned).IsAssigned = true;
            crew.State = CrewState.Mission;
            crew.MissionName = threat.Name;

        }

        public void CompleteMission(Threat threat)
        {
            foreach (var item in Crew)
            {
                if(item.State == CrewState.Mission && item.MissionName == threat.Name)
                {
                    item.State = CrewState.Returning;
                    item.MissionName = string.Empty;
                }
            }
        }

        public string GetAvailableCrewAsString()
        {
            var groups = Crew.Where(c => c.State == CrewState.Available).GroupBy(g => g.Type.ToString()).OrderByDescending(g => g.Count()).Select(g => new { Key = g.Key, Count = g.Count() });

            string message = "";
            if (groups.Count() == 1)
            {
                message = $"{groups.First().Count} {groups.First().Key} crew";
            }
            else if (groups.Count() == 2)
            {
                message = $"{groups.First().Count} {groups.First().Key}, and {groups.Last().Count} {groups.Last().Key} crew";
            }
            else if (groups.Count() > 2)
            {
                for (int i = 0; i < groups.Count() - 1; i++)
                {
                    var crew = groups.ElementAt(i);
                    message += $"{crew.Count} {crew.Key}, ";
                }
                message += $"and {groups.Last().Count} {groups.Last().Key} crew";
            }
            else
                message = "no available crew";
                    return message;
        }
    }
}
