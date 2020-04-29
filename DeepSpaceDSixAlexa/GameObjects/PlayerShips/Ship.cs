using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.GameObjects.Dice;
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
        public virtual int MaxHull { get; }
        public virtual int Shield { get; set; }
        public virtual int MaxShield { get; }

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
    }
}
