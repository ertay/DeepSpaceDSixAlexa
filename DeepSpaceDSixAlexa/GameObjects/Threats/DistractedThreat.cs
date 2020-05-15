using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class DistractedThreat : InternalThreat
    {
        public override string SpawnMessage => "Distracted drawn from the threat deck. ";

        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent(SpawnMessage));
            eventManager.Trigger("DistractedActivated");
        }

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            eventManager.Trigger("DiscardThreat", new DefaultThreatEvent(this));
        }

        public override void OnDestroy(EventManager eventManager = null)
        {
            eventManager.Trigger("DistractedDeactivated");
        }
    }
}
