using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BoostMorale : InternalThreat
    {
        public override string SpawnMessage => "Boost Morale was drawn from the threat deck. A threat will be removed from the scanners if the threat die rolls a 6. ";
// TODO: Consider using OnSpawn to trigger spawn messages.
        public override void Activate(EventManager eventManager)
        {
            eventManager.Trigger("BoostMoraleActivated");
            eventManager.Trigger("DiscardThreat", new DefaultThreatEvent(this));
        }


    }
}
