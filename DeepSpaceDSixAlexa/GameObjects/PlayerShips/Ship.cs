using DeepSpaceDSixAlexa.Enums;
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

        public Ship()
        {
            Crew = new List<CrewDie>();
        }

        public void RollCrewDice()
        {

        }

        public void SendCrewOnMission(CrewType crew, Threat threat)
        {

        }
    }
}
