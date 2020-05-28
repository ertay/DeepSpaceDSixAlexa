using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.Helpers;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class MeteoroidThreat : ExternalThreat
    {
        public override string SpawnMessage => $"{Name} drawn from the threat deck. External threat with {Health} health. {Name}'s health is reduced by one when activated with {GetActivationListAsString()}. Deals {Damage} damage when destroyed. ";
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
                eventManager.Trigger("DiscardThreat", new DefaultThreatEvent(this));

            }

        }

        public override void OnDestroy(EventManager eventManager = null)
        {
            if (IsDisabled)
            {
                eventManager.Trigger("AppendMessage", new DefaultEvent($"{SoundFx.ThreatDestroyed()}{Name} exploded, but our stasis beam prevented damage to our ship. "));
                return;
            }

            eventManager.Trigger("AppendMessage", new DefaultEvent($"{SoundFx.ThreatDestroyed()} "));
            var eventArgs = new DamageShipEvent(Name, Damage, $"{Name} exploded");
            eventManager.Trigger("DamageShip", eventArgs);
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name}. External threat with {Health} out of {MaxHealth} health. ";
            message += Damage > 0 ? $"{Name}'s health is reduced by one when activated with {GetActivationListAsString()}. Deals {Damage} damage when destroyed. " : "";
            message += AwayMissions.Count > 0 ? $"This threat can be destroyed by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }
    }
}
