using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class NoPanicCard : InternalThreat
    {
        public override string SpawnMessage => "Do not Panic drawn from the threat deck. No new threats detected. ";

        public override void OnSpawn(EventManager eventManager = null)
        {
            base.OnSpawn(eventManager);
            eventManager.Trigger("DiscardThreat", new DefaultThreatEvent(this));
        }
    }
}

