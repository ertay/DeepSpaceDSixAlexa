using DeepSpaceDSixAlexa.GameObjects.Threats;

namespace DeepSpaceDSixAlexa.Events
{
    public class DefaultThreatEvent : IEvent
    {
        public Threat Threat { get; set; }

        public DefaultThreatEvent(Threat threat)
        {
            Threat = threat;
        }
    }
}
