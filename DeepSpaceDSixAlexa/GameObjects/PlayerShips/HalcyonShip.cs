using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Events;
using DeepSpaceDSixAlexa.GameObjects.Dice;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using System;
using System.Linq;

namespace DeepSpaceDSixAlexa.GameObjects.PlayerShips
{
    public class HalcyonShip : Ship
    {
        public override int MaxHull => 8;
        public override int MaxShields => 4;
        public bool ExtraDamage { get; set; }

        public HalcyonShip() { }


        public override void InitializeShip()
        {
            base.InitializeShip();
            Hull = MaxHull;
            Shields = MaxShields;
        }

        public void FireWeapons(ExternalThreat threat)
        {
            // if we have already fired once, subsequent actions deal two damage instaed of one
            int damage = ExtraDamage ? 2 : 1;
            ExtraDamage = true;
            threat.Health -= damage;
            threat.Health = Math.Max(0, threat.Health);
            if (threat.Health <= 0)
            {
                _eventManager.Trigger("ThreatDestroyed", new DefaultEvent() { Message = $"Our tactical crew dealt {damage} damage and destroyed the {threat.Name}. " });
            }
            else
                _eventManager.Trigger("AttackThreat", new DefaultEvent() { Message = $"Our tactical crew opened fire at {threat.Name} and dealt {damage} damage. {threat.Name} now has {threat.Health} health. "});
            // mark tactical crew as returning
            Crew.First(c => c.Type == CrewType.Tactical && c.State == CrewState.Available).State = CrewState.Returning;


        }

        public void FireStasisBeam()
        {

        }

        public void RechargeShields()
        {

        }

        public void RepairShip(int engineeringCount)
        {

        }

        public void HealCrew()
        {

        }

        public void RemoveThreatFromScanner()
        {

        }

        public void TransformCrew(CrewType sourceType, CrewType targetType)
        {

        }

        public void CommanderRollAvailableCrew()
        {

        }

        public override void EndTurn()
        {
            ExtraDamage = false;
        }

    }
}
