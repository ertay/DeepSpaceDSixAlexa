using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSpaceDSixAlexa.Helpers
{
    /// <summary>
    /// Audio class used  to grab the links to the audio files
    /// </summary>
    public static class SoundFx
    {
        private const string BASE_URL = "https://dsdsixstorage.blob.core.windows.net/soundfx/";

        public static string Intro = $"<audio src=\"{BASE_URL}pm-infected-03.mp3\"/>";
        public static string Beeping = $"<audio src=\"{BASE_URL}pm-csph-text-6.mp3\"/>";
        public static string Warp = $"<audio src=\"{BASE_URL}pm-fssf2-tonal-energy-risers-4.mp3\"/>";
        public static string Victory = $"<audio src=\"{BASE_URL}pm-csph-calculations-20.mp3\"/>";
        public static string GameOver = $"<audio src=\"{BASE_URL}pm-sfm-impact-4.mp3\"/>";
        public const string Dice = "<audio src=\"soundbank://soundlibrary/toys_games/board_games/board_games_08\"/>";
        public static string ThreatRemoved = $"<audio src=\"{BASE_URL}u5BdjRcy-pm-fn-objects-items-interactions-menus-132.mp3\"/>";
        public static string FireWeapons = "<audio src=\"soundbank://soundlibrary/guns/futuristic/futuristic_03\"/>";
        public static string RechargeShields = $"<audio src=\"{BASE_URL}6dbmzLbt-pm-fssf2-xtras-risers-1.mp3\"/>";
        public static string StasisBeam = $"<audio src=\"{BASE_URL}bPdignj3-energygun-charge1.mp3\"/>";
        public static string HealCrew = "<audio src=\"soundbank://soundlibrary/hospital/respirator/respirator_03\"/>";
        public static string InternalThreatAlarm => "<audio src=\"soundbank://soundlibrary/scifi/amzn_sfx_scifi_alarm_01\"/>";
        public static string SolarWinds = $"<audio src=\"{BASE_URL}S9u1UtHR-bluezone-bc0253-texture-03.mp3\"/>";
        public static string PanelExplosion => "<audio src=\"soundbank://soundlibrary/explosions/electrical/electrical_04\"/>";
        public static string Invaders = $"<audio src=\"{BASE_URL}l2glMHqI-pm-fssf-door-big-3.mp3\"/>";
        public static string RobotUprising = $"<audio src=\"{BASE_URL}EBkQ2zu2-pm-fssf-transformation-5.mp3\"/>";
        public static string CommsOffline = "<audio src=\"soundbank://soundlibrary/radios_static/radios_static_02\"/>";


        public static string MoreTime()
        {
            
            return $"<audio src=\"{BASE_URL}SKmE4WzC-pm-atg-1-95bpm-b.mp3\"/><audio src=\"{BASE_URL}kgX4Rx41-pm-atg-1-95bpm-c.mp3\"/> What are your orders, captain? ";
        }

        public static string SpawnExternalThreat()
        {
            switch(ThreadSafeRandom.ThisThreadsRandom.Next(3))
            {
                case 0:
                    return $"<audio src=\"{BASE_URL}rKThIZrh-pm-fn-spawns-portals-teleports-37.mp3\"/>";
                case 1:
                    return $"<audio src=\"{BASE_URL}rUBJO0Mo-pm-fn-whooshes-2.mp3\"/>";
                case 2:
                    return $"<audio src=\"{BASE_URL}YPV1eBbO-pm-fn-spawns-portals-teleports-16.mp3\"/>";
            }
            return string.Empty;
        }

        public static string ThreatDestroyed()
        {
            int random = ThreadSafeRandom.ThisThreadsRandom.Next(11);
            switch (random)
            {
                case 0:
                    return "<audio src=\"soundbank://soundlibrary/explosions/electrical/electrical_01\"/>";
                case 1:
                    return "<audio src=\"soundbank://soundlibrary/explosions/explosions/explosions_05\"/>";
                case 2:
                    return "<audio src=\"soundbank://soundlibrary/explosions/explosions/explosions_04\"/>";
                case 3:
                    return "<audio src=\"soundbank://soundlibrary/explosions/explosions/explosions_06\"/>";
                case 4:
                    return "<audio src=\"soundbank://soundlibrary/explosions/explosions/explosions_08\"/>";
                case 5:
                    return "<audio src=\"soundbank://soundlibrary/explosions/explosions/explosions_13\"/>";
                case 6:
                    return "<audio src=\"soundbank://soundlibrary/scifi/amzn_sfx_scifi_close_large_explosion_01\"/>";
                case 7:
                    return "<audio src=\"soundbank://soundlibrary/scifi/amzn_sfx_scifi_explosion_2x_01\"/>";
                case 8:
                    return "<audio src=\"soundbank://soundlibrary/explosions/fireballs/fireballs_02\"/>";
                case 9:
                    return "<audio src=\"soundbank://soundlibrary/scifi/amzn_sfx_scifi_long_explosion_1x_01\"/>";
                case 10:
                    return "<audio src=\"soundbank://soundlibrary/scifi/amzn_sfx_scifi_short_low_explosion_01\"/>";
                    
            }
            return string.Empty;
        }
    }
}
