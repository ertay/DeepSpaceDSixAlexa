using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BomberThreat : ExternalThreat
    {
        public override string SpawnMessage => $"{Name} drawn from the threat deck. External threat with {Health} health that deals {Damage} damage and sends a unit to the infirmary when activated with {GetActivationListAsString()}. ";

        public override void Activate(EventManager eventManager)
        {
            base.Activate(eventManager);
            if (IsDisabled)
                return;
            eventManager.Trigger("BomberAttack");
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name}. External threat with {Health} out of {MaxHealth} health. ";
            message += Damage > 0 ? $"Deals {Damage} damage and a unit is sent to the infirmary when activated with {GetActivationListAsString()}. " : "";
            message += AwayMissions.Count > 0 ? $"This threat can be destroyed by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }
    }
}
