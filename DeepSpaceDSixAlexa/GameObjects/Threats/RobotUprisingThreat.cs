using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.Helpers;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class RobotUprisingThreat : InternalThreat
    {
        public override string SpawnMessage => "Robot Uprising drawn from the threat deck. When it activates with one, two, or three, one crew member is sent to the infirmary. Send one engineering crew on a mission to deal with the Robot Uprising. ";
        public override string Description => $"One crew member is sent to the infirmary when activated with {GetActivationListAsString()}. ";

        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent(SoundFx.RobotUprising + SpawnMessage));
        }

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;

            string message = "Our robots started an uprising! ";
            eventManager.Trigger("AppendMessage", new DefaultEvent(message));
            eventManager.Trigger("RobotUprisingActivated");


        }
    }
}
