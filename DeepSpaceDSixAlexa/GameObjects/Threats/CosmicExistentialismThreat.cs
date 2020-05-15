﻿using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class CosmicExistentialismThreat : InternalThreat
    {
        public override string SpawnMessage => "Cosmic Existentialism drawn from the threat deck. Our science crew cannot be used until you send a science crew member on a mission to deal with this threat. ";

        public override void OnSpawn(EventManager eventManager = null)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent(SpawnMessage));
            eventManager.Trigger("CosmicExistentialismActivated");
        }

        public override void OnMissionComplete(EventManager eventManager)
        {
            eventManager.Trigger("AppendMessage", new DefaultEvent("Our science crew dealt with their existentialism crisis. You can assign science crew again. "));
            eventManager.Trigger("CosmicExistentialismDeactivated");
        }
    }
}
