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

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            var damageEvent = new DamageShipEvent(Name, Damage);
            eventManager.Trigger("DamageShip", damageEvent);
            
        }

    }
}
