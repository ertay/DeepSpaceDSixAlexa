using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class TimeWarpThreat : InternalThreat
    {
        public override string SpawnMessage => "Time Warp drawn from the threat deck. When it activates with two, all external threats recover one health. Send two science crew on a mission to deal with the Time Warp. ";

        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent(SpawnMessage));
        }

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            eventManager.Trigger("TimeWarpActivated");
        }
    }
}
