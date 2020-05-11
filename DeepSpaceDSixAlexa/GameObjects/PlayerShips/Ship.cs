using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.GameObjects.Dice;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

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

        protected EventManager _eventManager;

        public Ship() { }
        public virtual void InitializeEvents(EventManager eventManager)
        {
            _eventManager = eventManager;

            _eventManager.On("DamageShip", (e) => ProcessDamage((DamageShipEvent)e));
            _eventManager.On("BoardingShipMissionComplete", (e) => ProcessBoardingShipMission((DefaultEvent)e));
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
            if(e.IgnoreShields || Shields == 0)
            {
                Hull -= e.Damage;
                Hull = Math.Max(0, Hull);
                string msg = $"{e.ThreatName} opened fire and caused {e.Damage} hull damage. ";
                _eventManager.Trigger("AppendMessage", new DefaultEvent(msg));
                return;
            }
            string message = "";

            
            Shields -= e.Damage;
            if (Shields < 0)
            {
                Hull -= Math.Abs(Shields);
                Hull = Math.Max(0, Hull);
                message = $"{e.ThreatName} opened fire that destroyed our shields and caused {Math.Abs(Shields)} hull damage. ";
                Shields = 0;
            }
            else
                message = $"{e.ThreatName} opened fire and caused {e.Damage} damage to our shields. ";

            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));

        }

        public virtual void InitializeShip()
        {
            // initialize the crew
            Crew = new List<CrewDie>();
            for (int i = 0; i < 6; i++)
            {
                Crew.Add(new CrewDie());
            }

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
