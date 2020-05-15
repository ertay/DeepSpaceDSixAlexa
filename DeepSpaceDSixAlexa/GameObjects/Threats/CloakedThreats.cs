using DeepSpaceDSixAlexa.Events;
using System.Runtime.InteropServices.ComTypes;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class CloakedThreats : InternalThreat
    {
        public override string SpawnMessage => "Cloaked Threats drawn from the threat deck. If the threat die rolls a two, after activating the threats, the threat die will be rolled again. ";

        public override void Activate(EventManager eventManager)
        {
            string message = "Cloaked threats activated, threat die will be rolled again. ";
            eventManager.Trigger("AppendMessage", new DefaultEvent(message));
            eventManager.Trigger("CloakedThreatsActivated");
        }
    }
}
