using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace JoinHouses {
    public class SubModule : MBSubModuleBase {
        public static Config Config = new();

        protected override void OnSubModuleLoad() {
            new Harmony("JoinHouses").PatchAll();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter) {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign) {
                CampaignGameStarter campaignStarter = (CampaignGameStarter)gameStarter;

                campaignStarter.AddBehavior(new JoinHousesCampaignBehavior(campaignStarter));
            }
        }
    }
}