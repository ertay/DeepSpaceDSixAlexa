namespace DeepSpaceDSixAlexa.Events
{
    public class DamageShipEvent : IEvent
    {
        public int Damage { get; set; }
        public string ThreatName { get; set; }
        public bool IgnoreShields { get; set; }
        public string Message { get; set; }

        public  DamageShipEvent(string threatName, int damage, string message, bool ignoreShields = false)
        {
            ThreatName = threatName;
            Damage = damage;
            Message = message;
            IgnoreShields = ignoreShields;
        }
    }
}
