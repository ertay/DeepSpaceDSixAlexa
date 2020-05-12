using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class NebulaThreat : ExternalThreat
    {
        public override void OnSpawn(EventManager eventManager = null)
        {
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
                eventManager.Trigger("ThreatDestroyed");
            }
        }

        public override void OnDestroy(EventManager eventManager = null)
        {
            eventManager.Trigger("NebulaDestroyed");
            string message = $"We escaped from the {Name} and can recharge our shields. ";
            eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }
    }
}
