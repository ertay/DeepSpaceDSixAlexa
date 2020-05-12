namespace DeepSpaceDSixAlexa.Events
{
    public class MeteoroidDestroyedEvent : IEvent
    {
public bool IsDisabled { get; set; }

        public MeteoroidDestroyedEvent(bool isDisabled)
        {
            IsDisabled = isDisabled;
        }
    }


}
