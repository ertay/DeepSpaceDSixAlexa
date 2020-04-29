using DeepSpaceDSixAlexa.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    /// <summary>
    /// Base threat class. External and internal threats inherit from this.
    /// </summary>
    public class Threat
    {
        public string Name { get; set; }
        public List<int> ActivationList { get; set; }
        [JsonIgnore]
        public bool HasMission => MissionCrewTypes.Count > 0;
        public List<CrewType> MissionCrewTypes { get; set; }
        public bool IsDisabled { get; set; }

        public Threat()
        {
            ActivationList = new List<int>();
            MissionCrewTypes = new List<CrewType>();
        }

        /// <summary>
        /// This method should fire when the threat die rolls the number that is on the activation list.
        /// </summary>
        public virtual void Activate(int threatDieValue)
        {

        }

        /// <summary>
        /// Some threats may have an action when destroyed.
        /// </summary>
        public virtual void OnDestroy()
        {

        }
    }
}
