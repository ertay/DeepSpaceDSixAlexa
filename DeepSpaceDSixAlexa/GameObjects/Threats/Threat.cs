using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

        [JsonIgnore]
        public virtual string SpawnMessage => string.Empty;

        public Threat()
        {
            ActivationList = new List<int>();
            AwayMissions = new List<Mission>();
        }

        public virtual void OnSpawn(EventManager eventManager = null) 
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent(SpawnMessage));
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
        public virtual void OnDestroy(EventManager eventManager = null) { }

            public virtual void OnMissionComplete(EventManager eventManager) { }

        public virtual string GetInfoMessage() { return string.Empty;  }

        public string GetActivationListAsString()
        {
            if (ActivationList.Count == 1)
                return ActivationList.First().ToString();
            else if (ActivationList.Count == 2)
                return $"{ActivationList.First()}, and {ActivationList.Last()}";
            else if (ActivationList.Count > 2)
            {
                string numbers = "";
                for (int i = 0; i < ActivationList.Count; i++)
                {
                    if (i == ActivationList.Count - 1)
                        numbers += $"and {ActivationList[i]}";
                    else
                        numbers += $"{ActivationList[i]}, ";

                }
                return numbers;
            }
            return string.Empty;
        }

        public string GetMissionsAsString()
        {
            if (AwayMissions.Count == 1)
                return $"one {AwayMissions.First().Type.ToString()} crew";
            else if (AwayMissions.Count == 2)
            {
                var firstCrewType = AwayMissions.First().Type;
                var secondCrewType = AwayMissions.Last().Type;
                return firstCrewType == secondCrewType ? $"two {firstCrewType.ToString()} crew" : $"one {firstCrewType.ToString()}, and one {secondCrewType.ToString()} crew";
            }

            return string.Empty;
        }
    }
}
