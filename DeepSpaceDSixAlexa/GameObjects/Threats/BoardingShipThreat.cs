using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BoardingShipThreat : ExternalThreat
    {
        public override string SpawnMessage => $"{Name} drawn from the threat deck. External threat with {Health} health that deals {Damage} damage when activated with {GetActivationListAsString()}. This threat can be destroyed by sending {GetMissionsAsString()} on a mission, but that crew is sent to the infirmary after completing the mission. ";

        public override void OnMissionComplete(EventManager eventManager)
        {
            string message = "Our tactical crew destroyed the boarding ship and was sent to the infirmary. ";
            
            eventManager.Trigger("BoardingShipMissionComplete", new DefaultEvent(Name));
            eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name}. External threat with {Health} out of {MaxHealth} health. ";
            message += Damage > 0 ? $"Deals {Damage} damage when activated with {GetActivationListAsString()}. " : "";
            message += AwayMissions.Count > 0 ? $"This threat can be destroyed by sending {GetMissionsAsString()} on a mission, but that crew is sent to the infirmary after completing the mission. " : "";
            return message;
        }
    }
}
