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

        public const int NUMBER_OF_RULES = 33;
        public static string GetRule(int ruleNumber)
        {
            string rule = "";
            switch (ruleNumber)
            {
                case 0:
                    rule = "Great! Don't worry, this won't take long! You can say Next to proceed to the next step, back to go back to the previous step, and repeat if you want me to repeat the current rule. Say new game at any time to start a new game. Now, say next to continue.";
                    break;
                case 1:
                    rule = "You are the Captain of the USS Crypsis - a RPTR class starship on routine patrol of the Auborne system when a distress call was received. Upon warping in you quickly realize it was a trap! With the help of your crew, you must survive until a rescue fleet appears. Say next to continue.";
                    break;
                case 2:
                    rule = "To win, you must survive by destroying all external threats in the threat deck. Say next to continue.";
                    break;
                case 3:
                    rule = "At the beginning of the game, you need to specify the number of Don't Panic cards that will be included in the threat deck. For your first game, I recommend playing with all six Don't Panic cards. If you are looking for a challenge, try playing with zero Don't Panic cards. Say next to continue.";
                    break;
                case 4:
                    rule = "When the game starts, two threats are immediately drawn from the threat deck. The threat deck has a total of 36 threats, plus any Don't Panic cards that you've decided to include in the deck. Say next to continue.";
                    break;
                case 5:
                    rule = "Let me tell you about your ship. The ship has three trackers. These are the hull tracker, the shields tracker, and the scanners. Say next to continue.";
                    break;
                case 6:
                    rule = "The hull Represents the condition of your ship. If this track reaches 0, you lose the game. You start the game with eight hull. Say next to continue.";
                    break;
                case 7:
                    rule = "The shields protect your hull from getting damaged. When a threat deals damage to  your ship, the damage goes to the shields first. When the shields reach zero, the damage is dealt to  your hull. You start the game with four shields. Say next to continue.";
                    break;
                case 8:
                    rule = "The scanners tracker on your ship is used to detect new threats. One of the results on your crew dice is a threat. When you roll a threat, it is immediately locked in the scanners. When three threat dice are locked in the scanners, a new threat is revealed from the deck. Now, let me tell you about your crew. Say next to continue.";
                    break;
                case 9:
                    rule = "The crew in your ship is represented by six crew dice that you roll every time a new round starts. Here is a list of possible results: 1. Commander. 2. Tactical. 3. Medical. 4. Science. 5. Engineering. 6. Threat. Say next to continue.";
                    break;
                case 10:
                    rule = "Each crew member can do a unique action. The commander can transform another crew member to a different type. To do this, you use the transform keyword. For example, you can say transform tactical to medical. Say next to continue.";
                    break;
                case 11:
                    rule = "The tactical crew member is used to operate your weapons systems. To use your tactical crew, you must first assign them to the weapons. For example, you can say, assign two tactical crew to the weapons. Say next to continue.";
                    break;
                case 12:
                    rule = "Once your tactical crew are assigned to the weapons, you can fire your weapons. When you fire your weapons, you need to provide a target and the damage amount you want to deal to that target. Say next to continue.";
                    break;
                case 13:
                    rule = "Keep in mind that the first tactical crew member that you assign to the weapons in a round gives you one damage to deal to the threats. All additional tactical crew give you two more damage to deal. For example, if you assign two tactical crew, you can deal up to three damage. If you assign three tactical crew, you can deal up to five damage. Say next to continue.";
                    break;
                case 14:
                    rule = "The medical crew can do two actions. The first one is heal crew which heals all the crew located in the infirmary. Sometimes your crew will be moved to the infirmary and you need to use your medical crew to treat them to be able to use them again. Say next to continue.";
                    break;
                case 15:
                    rule = "The second medical action is remove threat. This action removes a locked threat crew die from the scanners. The locked threat is then returned to your crew dice pool that will be rolled on the following round. Say next to continue.";
                    break;
                case 16:
                    rule = "The science crew can also do two actions. The first action is recharge shields which recharges your ship's shields to full power. Say next to continue.";
                    break;
                case 17:
                    rule = "The second science crew action is fire stasis beam. The stasis beam is used to temporarily disable the threats to your ship. When a threat is disabled, it cannot be activated in the current round. The stasis beam can be used on both external and internal threats. Say next to continue.";
                    break;
                case 18:
                    rule = "The engineering crew have a single action and that is to repair the hull. When you use the repair hull command, an engineer is used to recover damage that was dealt to your hull. The first repair action in the round gives you one hull point, while each subsequent repair action gives you two points. So, if you use the repair hull command twice, you can recover up to three hull damage. Say next to continue.";
                    break;
                case 19:
                    rule = "Finally, the threat die result on your crew dice is the only negative outcome. As mentioned before, these instantly get locked in your scanners, and once there are three of them, a new threat is introduced. You can remove the locked threats from the scanners by using your medical crew. Say next to continue.";
                    break;
                case 20:
                    rule = "Before we move to the threats, let's briefly talk about sending  your crew on missions. Most internal threats, and some external threats, can be dealt with by sending your crew on a mission. You can send a crew member on a mission by using the send crew on a mission command. Say next to continue.";
                    break;
                case 21:
                    rule = "Some threats may require more than one crew member to be sent on a mission to deal with the threat. When you send a crew member on a mission it will stay on the mission until the mission is complete or you decide to return it to your pool of crew dice. Crew sent on missions will not be rerolled at the start of a round. Say next to continue.";
                    break;
                case 22:
                    rule = "For example, the Hijackers threat is drawn from the threat deck which can be dealt with by sending two commander crew on a mission. If you only have one available commander in this round, you can send that commander on a mission to deal with the hijackers. You will now need one more commander to complete the mission. When another commander becomes available in a following round, you can send that one on a mission to complete it. Say next to continue.";
                    break;
                case 23:
                    rule = "We are almost done! Let's talk about the threats. There are two types of threats, external threats and internal threats. Say next to continue.";
                    break;
                case 24:
                    rule = "External threats typically deal damage to your ship. They can be destroyed by using your weapons. All external threats have a certain amount of health. Say next to continue.";
                    break;
                case 25:
                    rule = "Internal threats create adverse effects for your ship and crew. These threats do not have a health value, and they can be dealt with by sending your crew on missions. Say next to continue.";
                    break;
                case 26:
                    rule = "All threats have certain activation values. When you end your turn, a standard six sided die is rolled. All threats that can be activated with that number get triggered and perform their action. The order in which the threats are activated starts from internal threats and continues with the external threats. Say next to continue. ";
                    break;
                case 27:
                    rule = "When you are done with your actions, you are required  to end your turn. When you issue the end turn command, the following happens. 1. A new threat is drawn from the deck. 2. The threat die is rolled which activates the enemy threats. 3. After all threats are activated, your crew dice are rolled and the next  round can begin. Say next to continue.";
                    break;
                case 28:
                    rule = "You can get the status of your ship by simply saying status. To get the status of the threats, you can say threat status. To get information about a specific active threat, you can use the threat information command. Say next to continue.";
                    break;
                case 29:
                    rule = "If you ever need more time to make a decision, just say I need more time. Say next to continue.";
                    break;
                case 30:
                    rule = "You win the game when all cards in the threat deck are revealed, and you destroy all external threats. Say next to continue.";
                    break;
                case 31:
                    rule = "You lose the game if your hull reaches zero, or your crew become incapacitated and you have no more crew dice to roll at the beginning of a round. Say next to continue.";
                    break;
                case 32:
                    rule = "You can say help at any point during the game to get information about valid commands. Say next to continue.";
                    break;
                case 33:
                    rule = "And that's it! You should now be ready to face the onslaught! Good luck, captain! Say new game to start a new game. ";
                    break;

            }
            return rule;
        }

public static List<Threat> GenerateThreatDeck(int noPanicNumber)
        {
            var testThreats = new List<Threat>()
            {
                //{ new BomberThreat(){ Id = "BOne", Name = "Strike Bomber One", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){2, 4} } },
                { new PanelExplosionThreat(){ Id = "PE", Name = "Panel Explosion", AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Medical} } } },
                { new DistractedThreat(){ Id = "D", Name = "Distracted", ActivationList = new List<int>(){3, 4}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Medical}, new Mission() { Type = CrewType.Medical} } } },
                //{ new NoPanicCard()},
                //{ new ExternalThreat(){ Id = "ACTwo", Name = "Assault Cruiser Two", Health = 4, MaxHealth = 4, Damage = 2, ActivationList = new List<int>(){4, 5 } } },



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
                { new ExternalThreat(){ Id = "IX", Name = "Intercepter X", Health = 4, MaxHealth = 4, Damage = 1, ActivationList = new List<int>(){1, 2, 3, 4, 5} } },
                { new ExternalThreat(){ Id = "SPOne", Name = "Space Pirates One", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){3, 6} } },
                { new ExternalThreat(){ Id = "SPTwo", Name = "Space Pirates Two", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){3, 6} } },
                { new ExternalThreat(){ Id = "SPThree", Name = "Space Pirates Three", Health = 3, MaxHealth = 3, Damage = 2, ActivationList = new List<int>(){1, 3} } },
                { new ExternalThreat(){ Id = "H", Name = "Hijackers", Health = 4, MaxHealth = 4, Damage = 2, ActivationList = new List<int>(){4, 5}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Commander}, new Mission() { Type = CrewType.Commander} } } },
                { new BoardingShipThreat(){ Id = "BS", Name = "Boarding Ship", Health = 4, MaxHealth = 4, Damage = 2, ActivationList = new List<int>(){3, 4}, AwayMissions = new List<Mission>(){ new Mission() { Type = CrewType.Tactical}} } },
                { new BomberThreat(){ Id = "BOne", Name = "Strike Bomber One", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){2, 4} } },
                { new BomberThreat(){ Id = "BTwo", Name = "Strike Bomber Two", Health = 2, MaxHealth = 2, Damage = 2, ActivationList = new List<int>(){2, 4} } },
                { new BomberThreat(){ Id = "BThree", Name = "Strike Bomber Three", Health = 3, MaxHealth = 3, Damage = 1, ActivationList = new List<int>(){3, 4} } },
                { new MercenaryThreat(){ Id = "M", Name = "Mercenary", Health = 3, MaxHealth = 3, Damage = 2, } },
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