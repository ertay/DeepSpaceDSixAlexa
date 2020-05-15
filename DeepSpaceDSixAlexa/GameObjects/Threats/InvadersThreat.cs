using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class InvadersThreat : InternalThreat
    {
        public override string SpawnMessage => "Invaders drawn from the threat deck. When it activates with two or four, one crew member is sent to the infirmary. Send two tactical crew on a mission to defeat the invaders. ";

        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent(SpawnMessage));
        }

        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;

            string message = "Invaders opened fire. ";
            eventManager.Trigger("AppendMessage", new DefaultEvent(message));
            eventManager.Trigger("InvadersActivated");
            

        }
    }
}
