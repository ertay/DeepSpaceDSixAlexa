using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class MeteoroidThreat : ExternalThreat
    {
        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            Health--;
            string message = "";
            if(Health > 0)
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
            if (IsDisabled)
            {
                eventManager.Trigger("AppendMessage", new DefaultEvent($"{Name} exploded, but our stasis beam prevented damage to our ship. "));
                return;
            }
                

            var eventArgs = new DamageShipEvent(Name, Damage, $"{Name} exploded");
            eventManager.Trigger("DamageShip", eventArgs);
        }
    }
}
