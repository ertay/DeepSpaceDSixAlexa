namespace DeepSpaceDSixAlexa.Events
{
    public class DamageShipEvent : IEvent
    {
        public int Damage { get; set; }
        public string ThreatName { get; set; }
        public bool IgnoreShields { get; set; }

        public  DamageShipEvent(string threatName, int damage, bool ignoreShields = false)
        {
            ThreatName = threatName;
            Damage = damage;
            IgnoreShields = ignoreShields;
        }
    }
}
