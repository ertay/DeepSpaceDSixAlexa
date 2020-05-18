﻿using DeepSpaceDSixAlexa.Events;

namespace DeepSpaceDSixAlexa.GameObjects.Threats
{
    public class RaidersThreat : ExternalThreat
    {
        public override void Activate(EventManager eventManager)
        {
            if (IsDisabled)
                return;
            var damageEvent = new DamageShipEvent(Name, Damage, $"{Name} opened fire", true);
            eventManager.Trigger("DamageShip", damageEvent);
        }

    }
}