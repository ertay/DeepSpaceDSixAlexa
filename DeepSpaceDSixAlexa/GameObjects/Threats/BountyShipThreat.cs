using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BountyShipThreat : ExternalThreat
    {
        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            // disable player's shields
            eventManager.Trigger("DisableShields");
            base.Activate(eventManager);
        }
    }
}
