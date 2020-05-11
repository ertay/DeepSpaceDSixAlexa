using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BomberThreat : ExternalThreat
    {
        public override void Activate(EventManager eventManager)
        {
            base.Activate(eventManager);
            if (IsDisabled)
                return;
            eventManager.Trigger("BomberAttack");
        }
    }
}
