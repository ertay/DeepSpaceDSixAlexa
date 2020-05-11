using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BoardingShipThreat : ExternalThreat
    {
        public override void OnMissionComplete(EventManager eventManager)
        {
            eventManager.Trigger("BoardingShipMissionComplete", new DefaultEvent(Name));
        }
    }
}
