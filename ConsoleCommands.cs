using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace JoinHouses {
    public class ConsoleCommands {
        [CommandLineFunctionality.CommandLineArgumentFunction("reloadconfig", "joinhouses")]
        private static string CommandReloadConfig(List<string> args) {
            SubModule.Config.LoadConfig();
            return "Config reloaded!";
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("debug_marry", "joinhouses")]
        private static string DebugCommandMarry(List<string> args) {
            if (Hero.MainHero.Spouse != null) return "You are already married.";

            if (args.Count > 0) {
                string heroName = args[0];

                foreach (Hero hero in Campaign.Current.AliveHeroes) {
                    string heroNameWithoutSpaces = hero.Name.ToString().Replace(" ", "");

                    if (heroName == heroNameWithoutSpaces) {
                        Hero.MainHero.Spouse = hero;
                        hero.SetNewOccupation(Occupation.Lord);
                        return heroNameWithoutSpaces + " made your spouse!";
                    }
                }
                return "No hero with that name found. If the name has spaces, try without spaces.";
            }
            return "Proper usage: joinhouses.debug_marry Rhagaea";
        }
    }
}