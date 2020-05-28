using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class CommsOfflineThreat : InternalThreat
    {
        public override string SpawnMessage => "Comms offline drawn from the threat deck. Our commander crew cannot be used until you send an engineering crew member on a mission to deal with this. ";
        public override string Description => "Our commander crew cannot be used while this threat is active. ";
        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent(DeepSpaceDSixAlexa.Helpers.SoundFx.CommsOffline+ SpawnMessage));
            eventManager.Trigger("CommsOfflineActivated");
        }

        public override void OnMissionComplete(EventManager eventManager)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent("Comms are back online. You can assign commander crew again. "));
            eventManager.Trigger("CommsOfflineDeactivated");
        }


    }
}
