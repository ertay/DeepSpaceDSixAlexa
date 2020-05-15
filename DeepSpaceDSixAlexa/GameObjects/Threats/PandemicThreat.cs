using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class PandemicThreat : InternalThreat
    {
        public override string SpawnMessage => "Pandemic drawn from the threat deck. When it activates with one, one crew member is sent to the infirmary. Send either one science, or one medical crew on a mission to deal with the Pandemic. ";

        public override int MinimumMissionsToComplete => 1;

        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent(SpawnMessage));
        }

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;

            string message = "Pandemic activated. ";
            eventManager.Trigger("AppendMessage", new DefaultEvent(message));
            eventManager.Trigger("PandemicActivated");


        }
    }
}
