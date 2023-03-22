using HarmonyLib;
using SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Localization;

namespace JoinHouses.Patches {
    [HarmonyPatch(typeof(DefaultCutscenesCampaignBehavior), "OnHeroComesOfAge")]
    internal class HeroComesOfAgeCutscenePatch {
        [HarmonyPrefix]
        private static bool Prefix(Hero hero) {
            if ((hero.Father == null || hero.Mother == null) && hero.Clan == Hero.MainHero.Clan) {
                TextObject textObject = new TextObject("{=t4KwQOB7}{HERO.NAME} is now of age.");
                textObject.SetCharacterProperties("HERO", hero.CharacterObject);
                Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new HeirComeOfAgeMapNotification(hero, hero.Clan.Leader, textObject));

                return false;
            }
            return true;
        }
    }
}
