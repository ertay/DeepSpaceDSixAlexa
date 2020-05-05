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
        public bool ExtraRepair { get; set; }

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

        public void FireStasisBeam(Threat threat)
        {
            threat.IsDisabled = true;
            Crew.First(c => c.Type == CrewType.Science && c.State == CrewState.Available).State = CrewState.Returning;
            string message = $"Our scientist crew fired the stasis beam and disabled {threat.Name} until the next round. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));

        }

        public void RechargeShields()
        {
            Shields = MaxShields;
            Crew.First(c => c.Type == CrewType.Science && c.State == CrewState.Available).State = CrewState.Returning;
            string message = $"Our science crew recharged the shields back to full power. We now have {Shields} shields. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));

        }
        
        public void RepairShip()
        {
            int repairValue = ExtraRepair ? 2 : 1;
            ExtraRepair = true;
            Hull += repairValue;
            Hull = Math.Min(Hull, MaxHull);
            Crew.First(c => c.Type == CrewType.Engineering && c.State == CrewState.Available).State = CrewState.Returning;
            string message = $"Engineering crew repaired {repairValue} damage and now we have {Hull} out of {MaxHull} hull. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public void HealCrew()
        {
            int infirmaryCount = InfirmaryCrewCount;
            string plural = infirmaryCount > 1 ? "s" : ""; 
            foreach (var item in Crew)
            {
                // if crew is in infirmary, put it in the returning state, otherwise keep the state that it is in if it wasn't in infirmary
                item.State = item.State == CrewState.Infirmary ? CrewState.Returning : item.State;
            }
            Crew.First(c => c.Type == CrewType.Medic && c.State == CrewState.Available).State = CrewState.Returning;
            string message = $"A medic healed {infirmaryCount} crew member{plural} that will be available next round. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public void RemoveThreatFromScanner()
        {
            Crew.First(c => c.Type == CrewType.Threat && c.State == CrewState.Locked).State = CrewState.Returning;
            Crew.First(c => c.Type == CrewType.Medic&& c.State == CrewState.Available).State = CrewState.Returning;
            string message = $"A medic was used to remove a threat die from our scanners. The number of locked threats on our scanners is {ScannerCount}. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public void TransformCrew(CrewType sourceType, CrewType targetType)
        {
            // first use a commander die
            Crew.First(c => c.Type == CrewType.Commander && c.State == CrewState.Available).State = CrewState.Returning;
            Crew.First(c => c.Type == sourceType && c.State == CrewState.Available).Type= targetType;
            string message = $"You used a commander to transform one {sourceType.ToString()} crew member to {targetType.ToString()}. ";
            _eventManager.Trigger("AppendMessage", new DefaultEvent(message));
        }

        public void CommanderRollAvailableCrew()
        {

        }

        public override void EndTurn()
        {
            ExtraDamage = false;
            ExtraRepair = false;
        }

    }
}
