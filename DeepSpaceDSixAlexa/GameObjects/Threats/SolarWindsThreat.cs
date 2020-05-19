using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class SolarWindsThreat : InternalThreat
    {
        public override string SpawnMessage => "Solar Winds drawn from the threat deck. When it activates with two, the card gets discarded, but our ship receives five damage. ";
        public override string Description => $"Deals five damage and gets discarded when it is activated with {GetActivationListAsString()}. ";



        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;

            var eventArgs = new DamageShipEvent(Name, 5, $"The extreme heat from  the {Name} affected the ship systems");
            eventManager.Trigger("DamageShip", eventArgs);
            eventManager.Trigger("DiscardThreat", new DefaultThreatEvent(this));
        }
    }
}
