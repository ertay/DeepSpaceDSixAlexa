using DeepSpaceDSixAlexa.GameObjects.Threats;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSpaceDSixAlexa.GameObjects.Managers
{
    public class ThreatManager
    {
        public List<Threat> ThreatDeck { get; set; }
        public List<Threat> ExternalThreats { get; set; }
        public List<Threat> InternalThreats { get; set; }
        
        public ThreatManager()
        {
            ThreatDeck = new List<Threat>();
            ExternalThreats = new List<Threat>();
            InternalThreats = new List<Threat>();
        }
    }
}
