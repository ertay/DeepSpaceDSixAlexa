using Newtonsoft.Json;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class InternalThreat : Threat
    {
        [JsonIgnore]
        public virtual string SpawnMessage => string.Empty;
        

    }
}
