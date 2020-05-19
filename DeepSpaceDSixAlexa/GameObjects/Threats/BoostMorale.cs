using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class BoostMorale : InternalThreat
    {
        public override string SpawnMessage => "Boost Morale was drawn from the threat deck. A threat will be removed from the scanners if the threat die rolls a 6. ";
        public override string Description => $"A locked threat is removed from our scanners when activated with {GetActivationListAsString()}. ";

        public override void Activate(EventManager eventManager)
        {
            eventManager.Trigger("BoostMoraleActivated");
            eventManager.Trigger("DiscardThreat", new DefaultThreatEvent(this));
        }

        public override string GetInfoMessage()
        {
            string message = $"{Name} is a positive card. ";
            message += Description;
            message += AwayMissions.Count > 0 ? $"This threat can be dealth with by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }


    }
}
