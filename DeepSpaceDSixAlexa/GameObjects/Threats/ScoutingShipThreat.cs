using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class ScoutingShipThreat : ExternalThreat
    {
        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            var damageEvent = new DamageShipEvent(Name, Damage, $"{Name} opened fire");
            eventManager.Trigger("ScoutAttack", damageEvent);
        }
    }
}
