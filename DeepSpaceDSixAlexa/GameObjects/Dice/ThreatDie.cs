using DeepSpaceDSixAlexa.Helpers;

namespace DeepSpaceDSixAlexa.GameObjects.Dice
{
    public class ThreatDie
    {
        public int Value { get; private set; }

        public ThreatDie()
        {
            Roll();
        }

        public void Roll()
        {
            Value = ThreadSafeRandom.ThisThreadsRandom.Next(6);
        }
    }
}
