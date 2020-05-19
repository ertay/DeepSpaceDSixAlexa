using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BountyShipThreat : ExternalThreat
    {
        public override string SpawnMessage => $"{Name} drawn from the threat deck. External threat with {Health} health that disables our shields and deals {Damage} damage when activated with {GetActivationListAsString()}. ";
        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            // disable player's shields
            eventManager.Trigger("DisableShields");
            base.Activate(eventManager);
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name}. External threat with {Health} out of {MaxHealth} health. ";
            message += Damage > 0 ? $"Disables our shields and deals {Damage} damage when activated with {GetActivationListAsString()}. " : "";
            message += AwayMissions.Count > 0 ? $"This threat can be destroyed by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }
    }
}
