using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.Helpers;
using Newtonsoft.Json;
using System.ComponentModel;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class InternalThreat : Threat
    {
        
        [JsonIgnore]
        public virtual string Description => string.Empty;

        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent($"{SoundFx.InternalThreatAlarm}"));
            base.OnSpawn(eventManager);
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name} is an internal threat. ";
            message += Description;
            message += AwayMissions.Count > 0 ? $"This threat can be dealth with by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }


    }
}
