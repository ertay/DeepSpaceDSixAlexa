using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class NebulaThreat : ExternalThreat
    {
        public override string SpawnMessage => $"{Name} drawn from the threat deck. External threat with {Health} health. ";
        public override void OnSpawn(EventManager eventManager = null)
        {
            base.OnSpawn(eventManager);
            eventManager.Trigger("NebulaSpawned");
        }

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;

            Health--;
            string message = "";
            if (Health > 0)
            {
                message = $"{Name} now has  {Health} health. ";
                eventManager.Trigger("AppendMessage", new DefaultEvent(message));
            }
            else
            {
                eventManager.Trigger("DiscardThreat", new DefaultThreatEvent(this));
            }
        }

        public override void OnDestroy(EventManager eventManager = null)
        {
            eventManager.Trigger("NebulaDestroyed");
            string message = $"We escaped from the {Name} and can recharge our shields. ";
            eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name}. External threat with {Health} out of {MaxHealth} health. Our shields are down and cannot be recharged while {Name} is active. ";
            message += Damage > 0 ? $"{Name}'s health is reduced by one when activated with {GetActivationListAsString()}. " : "";
            message += AwayMissions.Count > 0 ? $"This threat can be destroyed by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }
    }
}
