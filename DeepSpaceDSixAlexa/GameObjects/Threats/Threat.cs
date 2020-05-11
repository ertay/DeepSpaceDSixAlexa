using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.Helpers;
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
        public string Id { get; set; }
        public string Name { get; set; }
        public List<int> ActivationList { get; set; }
        [JsonIgnore]
        public bool HasMission => AwayMissions.Count > 0;
        public List<Mission> AwayMissions { get; set; }
        [JsonIgnore]
        public virtual int MinimumMissionsToComplete => AwayMissions.Count;
        public bool IsDisabled { get; set; }

        public Threat()
        {
            ActivationList = new List<int>();
            AwayMissions = new List<Mission>();
        }

        /// <summary>
        /// This method should fire when the threat die rolls the number that is on the activation list.
        /// </summary>
        public virtual void Activate(EventManager eventManager)
        {

        }

        /// <summary>
        /// Some threats may have an action when destroyed.
        /// </summary>
        public virtual void OnDestroy() { }

            public virtual void OnMissionComplete(EventManager eventManager) { }
    }
}
