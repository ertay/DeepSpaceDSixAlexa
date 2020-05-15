using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BoardingShipThreat : ExternalThreat
    {
        public override void OnMissionComplete(EventManager eventManager)
        {
            string message = "Our tactical crew destroyed the boarding ship and was sent to the infirmary. ";
            
            eventManager.Trigger("BoardingShipMissionComplete", new DefaultEvent(Name));
            eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }
    }
}
