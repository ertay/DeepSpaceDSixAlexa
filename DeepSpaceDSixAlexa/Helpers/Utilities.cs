using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using System.Collections.Generic;

namespace DeepSpaceDSixAlexa.Helpers
{
    /// <summary>
    /// Creats the threat deck and adds different number of don't panic cards based on difficulty selected
    /// </summary>
    public static class Utilities
    {
public static List<Threat> GenerateThreatDeck(int difficulty)
        {
            var threats = new List<Threat>()
            {
                { new ExternalThreat(){ Id = "ACOne", Name = "Assault Cruiser One", Health = 4, MaxHealth = 4, Damage = 2, ActivationList = new List<int>(){1, 2 } } },
                { new ExternalThreat(){ Id = "ACTwo", Name = "Assault Cruiser Two", Health = 4, MaxHealth = 4, Damage = 2, ActivationList = new List<int>(){4, 5 } } },
                { new ExternalThreat(){ Id = "C", Name = "Corsair", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){4, 5, 6 } } },
                { new ExternalThreat(){ Id = "DOne", Name = "Drone One", Health = 1, MaxHealth = 1, Damage = 1, ActivationList = new List<int>(){2, 4, 6 } } },
                { new ExternalThreat(){ Id = "DTwo", Name = "Drone Two", Health = 1, MaxHealth = 1, Damage = 1, ActivationList = new List<int>(){3, 5} } },
                { new ExternalThreat(){ Id = "FS", Name = "Flagship", Health = 4, MaxHealth = 4, Damage = 3, ActivationList = new List<int>(){4, 5, 6} } },
                { new ExternalThreat(){ Id = "I", Name = "Intercepter", Health = 3, MaxHealth = 3, Damage = 1, ActivationList = new List<int>(){1, 2, 3, 4, 5} } },
                { new ExternalThreat(){ Id = "IX", Name = "Intercepter X", Health = 4, MaxHealth = 3, Damage = 1, ActivationList = new List<int>(){1, 2, 3, 4, 5} } },
                { new ExternalThreat(){ Id = "SPOne", Name = "Space Pirates One", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){3, 6} } },
                { new ExternalThreat(){ Id = "SPTwo", Name = "Space Pirates Two", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){3, 6} } },
                { new ExternalThreat(){ Id = "SPThree", Name = "Space Pirates Three", Health = 3, MaxHealth = 3, Damage = 2, ActivationList = new List<int>(){1, 3} } },
            };
            threats.Shuffle();
            return threats;
        }
    }
}