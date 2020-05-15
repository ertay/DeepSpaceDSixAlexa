using DeepSpaceDSixAlexa.Enums;
using DeepSpaceDSixAlexa.Extensions;
using DeepSpaceDSixAlexa.GameObjects.Threats;
using System.Collections.Generic;
using System.Linq;

namespace DeepSpaceDSixAlexa.Helpers
{
    /// <summary>
    /// Creats the threat deck and adds different number of don't panic cards based on difficulty selected
    /// </summary>
    public static class Utilities
    {
public static List<Threat> GenerateThreatDeck(int noPanicNumber)
        {
            var testThreats = new List<Threat>()
            {


                { new NoPanicCard() },
                { new CloakedThreats(){ Id = "CT", Name = "Cloaked Threats", ActivationList = new List<int>(){2}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Science}, new Mission() { Type = CrewType.Commander} } } },
                { new BoostMorale(){ Id = "BM", Name = "Boost Morale", ActivationList = new List<int>(){6 } } },
                { new ScoutingShipThreat(){ Id = "SS", Name = "Scouting Ship", Health = 3, MaxHealth = 3, Damage = 1} },
                { new ExternalThreat(){ Id = "ACOne", Name = "Assault Cruiser One", Health = 4, MaxHealth = 4, Damage = 2, ActivationList = new List<int>(){1, 2 } } },

            };
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
                { new ExternalThreat(){ Id = "H", Name = "Hijackers", Health = 4, MaxHealth = 4, Damage = 2, ActivationList = new List<int>(){4, 5}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Commander}, new Mission() { Type = CrewType.Commander} } } },
                { new BoardingShipThreat(){ Id = "BS", Name = "Boarding Ship", Health = 4, MaxHealth = 4, Damage = 2, ActivationList = new List<int>(){3, 4}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Tactical}} } },
                { new BomberThreat(){ Id = "BOne", Name = "Bomber One", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){2, 4} } },
                { new BomberThreat(){ Id = "BTwo", Name = "Bomber Two", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){2, 4} } },
                { new BomberThreat(){ Id = "BThree", Name = "Bomber Three", Health = 3, MaxHealth = 3, Damage = 1, ActivationList = new List<int>(){3, 4} } },
                { new ExternalThreat(){ Id = "M", Name = "Mercenary", Health = 3, MaxHealth = 3, Damage = 2, } },
                { new BountyShipThreat(){ Id = "BtyShip", Name = "Bounty Ship", Health = 4, MaxHealth = 4, Damage = 1, ActivationList = new List<int>(){1, 2} } },
                { new RaidersThreat(){ Id = "ROne", Name = "Raiders One", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){4, 6} } },
                { new RaidersThreat(){ Id = "RTwo", Name = "Raiders Two", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){4, 6} } },
                { new RaidersThreat(){ Id = "RThree", Name = "Raiders Three", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){1, 4} } },
                { new MeteoroidThreat(){ Id = "Meteoroid", Name = "Meteoroid", Health = 4, MaxHealth = 4, Damage = 5, ActivationList = new List<int>(){1} } },
                { new NebulaThreat(){ Id = "N", Name = "Nebula", Health = 3, MaxHealth = 3, Damage = 0, ActivationList = new List<int>(){1, 2, 3, 4, 5} } },
                { new ScoutingShipThreat(){ Id = "SS", Name = "Scouting Ship", Health = 3, MaxHealth = 3, Damage = 1} },
                { new BoostMorale(){ Id = "BM", Name = "Boost Morale", ActivationList = new List<int>(){6 } } },
                { new CloakedThreats(){ Id = "CT", Name = "Cloaked Threats", ActivationList = new List<int>(){2}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Science}, new Mission() { Type = CrewType.Commander} } } },
                { new CosmicExistentialismThreat(){ Id = "CE", Name = "Cosmic Existentialism", AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Science} } } },
                { new CommsOfflineThreat(){ Id = "CO", Name = "Comms Offline", AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Engineering} } } },
                { new DistractedThreat(){ Id = "D", Name = "Distracted", ActivationList = new List<int>(){3, 4}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Medical}, new Mission() { Type = CrewType.Medical} } } },
                { new FriendlyFire(){ Id = "FF", Name = "Friendly Fire", } },
                { new PandemicThreat(){ Id = "P", Name = "Pandemic", ActivationList = new List<int>(){1}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Science}, new Mission() { Type = CrewType.Medical} } } },
                { new InvadersThreat(){ Id = "INV", Name = "Invaders", ActivationList = new List<int>(){2, 4}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Tactical}, new Mission() { Type = CrewType.Tactical} } } },
                { new PanelExplosionThreat(){ Id = "PE", Name = "Panel Explosion", AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Medical} } } },
                { new TimeWarpThreat(){ Id = "TW", Name = "Time Warp", ActivationList = new List<int>(){2}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Science}, new Mission() { Type = CrewType.Science} } } },
                { new SolarWindsThreat(){ Id = "SW", Name = "Solar Winds", ActivationList = new List<int>(){2 } } },
                { new RobotUprisingThreat(){ Id = "RU", Name = "Robot Uprising", ActivationList = new List<int>(){1, 2, 3 }, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Engineering} } } },
            };
            for (int i = 0; i < noPanicNumber; i++)
            {
                threats.Add(new NoPanicCard());

            }
            threats.Shuffle();
            return threats;
        }
    }
}