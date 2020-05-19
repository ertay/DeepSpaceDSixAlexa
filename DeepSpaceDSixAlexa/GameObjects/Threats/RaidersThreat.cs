using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class RaidersThreat : ExternalThreat
    {
        public override string SpawnMessage => $"{Name} drawn from the threat deck. External threat with {Health} health that ignores our shields and deals {Damage} damage when activated with {GetActivationListAsString()}. ";
        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            var damageEvent = new DamageShipEvent(Name, Damage, $"{Name} opened fire", true);
            eventManager.Trigger("DamageShip", damageEvent);
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name}. External threat with {Health} out of {MaxHealth} health. ";
            message += Damage > 0 ? $"Ignores our shields and deals {Damage} damage when activated with {GetActivationListAsString()}. " : "";
            message += AwayMissions.Count > 0 ? $"This threat can be destroyed by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }

    }
}
