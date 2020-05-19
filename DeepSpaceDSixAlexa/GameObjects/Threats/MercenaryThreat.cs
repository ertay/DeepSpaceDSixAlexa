namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class MercenaryThreat : ExternalThreat
    {
        public override string SpawnMessage => $"{Name} drawn from the threat deck. External threat with {Health} health that deals {Damage} damage if no other threats are activated. ";
        public override string GetInfoMessage()
        {
            string message = $"{Name}. External threat with {Health} out of {MaxHealth} health. ";
            message += Damage > 0 ? $"Deals {Damage} damage if no other threats get activated. " : "";
            message += AwayMissions.Count > 0 ? $"This threat can be destroyed by sending {GetMissionsAsString()} on a mission. " : "";
            return message;
        }
    }
}
