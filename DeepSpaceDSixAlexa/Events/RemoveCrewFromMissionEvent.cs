using DeepSpaceDSixAlexa.Enums;

namespace DeepSpaceDSixAlexa.Events
{
    public class RemoveCrewFromMissionEvent : IEvent
    {
        public CrewType Type { get; set; }
        public string ThreatName { get; set; }

        public RemoveCrewFromMissionEvent(string threatName, CrewType crewType)
        {
            ThreatName = threatName;
            Type = crewType;

        }
    }


}
