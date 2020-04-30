using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSpaceDSixAlexa.GameObjects.Dice
{
    public class CrewDie
    {
        public CrewType Type { get; set; }
        public CrewState State { get; set; }
        /// <summary>
        /// Holds the name of the threat when this crew member is assigned to a mission.
        /// </summary>
        public string MissionName { get; set; }
        public void Roll()
        {
            State = State == CrewState.Returning ? CrewState.Available : State;
            if (State != CrewState.Available)
                return;
            int dieValue = ThreadSafeRandom.ThisThreadsRandom.Next(6);
            Type = (CrewType)dieValue;
            // check if threat was rolled, and lock the die if so
            if (Type == CrewType.Threat)
                State = CrewState.Locked;


        }
    }
}
