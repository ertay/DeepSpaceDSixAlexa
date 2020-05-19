using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class PanelExplosionThreat : InternalThreat
    {
        public override string SpawnMessage => "Panel Explosion drawn from the threat deck. Our engineering crew have been incapacitated and cannot be used until you send a medical crew member on a mission to deal with the Panel Explosion. ";
        public override string Description => "Our engineering crew cannot be used while this threat is active. ";
        public override void OnSpawn(EventManager eventManager = null)
    {
            base.OnSpawn(eventManager);
        eventManager.Trigger("PanelExplosionActivated");
    }

    public override void OnMissionComplete(EventManager eventManager)
    {
        eventManager.Trigger("AppendMessage", new DefaultEvent("Our engineering crew recovered from the panel explosion and are ready for action. "));
        eventManager.Trigger("PanelExplosionDeactivated");
    }


}
}
