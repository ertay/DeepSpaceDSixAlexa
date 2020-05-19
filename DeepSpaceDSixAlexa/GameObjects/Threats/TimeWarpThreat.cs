using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class TimeWarpThreat : InternalThreat
    {
        public override string SpawnMessage => "Time Warp drawn from the threat deck. When it activates with two, all external threats recover one health. Send two science crew on a mission to deal with the Time Warp. ";
        public override string Description => $"All external threats recover one health when this threat is activated with {GetActivationListAsString()}. ";

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            eventManager.Trigger("TimeWarpActivated");
        }
    }
}
