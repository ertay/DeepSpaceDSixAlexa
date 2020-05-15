using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class FriendlyFire : InternalThreat
    {
        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("FriendlyFire");
            eventManager.Trigger("DiscardThreat", new DefaultThreatEvent(this));
        }
    }
}
