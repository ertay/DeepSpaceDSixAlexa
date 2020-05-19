using DeepSpaceDSixAlexa.Events;
using System.Runtime.InteropServices.ComTypes;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    /// <summary>
    /// All external threats should inherit from this class.
    /// This class describes generic external threats that have no special abilities.
    /// </summary>
    public class ExternalThreat : Threat

    {
public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Damage { get; set; }

        public override string SpawnMessage => AwayMissions.Count < 1 ? $"{Name} drawn from the threat deck. External threat with {Health} health that deals {Damage} damage when activated with {GetActivationListAsString()}. " : $"{Name} drawn from the threat deck. External threat with {Health} health that deals {Damage} damage when activated with {GetActivationListAsString()}. We can send {GetMissionsAsString()} on a mission to deal with the {Name}. ";

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            var damageEvent = new DamageShipEvent(Name, Damage, $"{Name} opened fire");
            eventManager.Trigger("DamageShip", damageEvent);
            
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name}. External threat with {Health} out of {MaxHealth} health. ";
            message += Damage > 0 ? $"Deals {Damage} damage when activated with {GetActivationListAsString()}. " : "";
            message += AwayMissions.Count > 0 ? $"This threat can be destroyed by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }
    }
}
