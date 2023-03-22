using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace JoinHouses.StaticUtils {
    public static class Utils {
        public static void PrintToMessages(string str, float r = 255, float g = 255, float b = 255) {
            float[] newValues = { r / 255.0f, g / 255.0f, b / 255.0f };
            Color col = new(newValues[0], newValues[1], newValues[2]);
            InformationManager.DisplayMessage(new InformationMessage(str, col));
        }

        public static void JoinHouses(Clan otherClan) {
            List<Clan> clans = (List<Clan>)AccessTools.Field(typeof(CampaignObjectManager), "_clans").GetValue(Campaign.Current.CampaignObjectManager);
            List<IFaction> factions = (List<IFaction>)AccessTools.Field(typeof(CampaignObjectManager), "_factions").GetValue(Campaign.Current.CampaignObjectManager);
            // Some vars
            Clan playerClan = Hero.MainHero.Clan;
            Kingdom? playerKingdom = playerClan.Kingdom;
            Kingdom? otherKingdom = otherClan.Kingdom;

            // JOIN CLANS
            GiveGoldAction.ApplyBetweenCharacters(otherClan.Leader, Hero.MainHero, otherClan.Gold, true);
            GainRenownAction.Apply(Hero.MainHero, otherClan.Renown, true);

            List<Town> townsToTransfer = otherClan.Fiefs.ToList();
            foreach (Town town in townsToTransfer)
                town.OwnerClan = playerClan;

            List<Hero> heroesToTransfer = otherClan.Heroes.ToList();
            foreach (Hero hero in heroesToTransfer)
                hero.Clan = playerClan;

            // Calculate which kingdom to be in if other clan has a kingdom
            if (otherKingdom != null) {
                if (playerKingdom == null) {
                    if (otherKingdom.RulingClan == otherClan)
                        otherKingdom.RulingClan = playerClan;
                    playerClan.Kingdom = otherKingdom;
                }
                else if (playerKingdom != null && playerKingdom.RulingClan != playerClan && otherKingdom.RulingClan == otherClan) {
                    otherKingdom.RulingClan = playerClan;
                    playerClan.Kingdom = otherKingdom;
                }
            }

            /*if (otherKingdom != null) {
                bool toJoinIsRuler = otherKingdom.RulingClan == otherClan;
                bool playerIsRuler = playerKingdom.RulingClan == playerClan;
                if (toJoinIsRuler && !playerIsRuler) {
                    otherKingdom.RulingClan = playerClan;
                    playerClan.Kingdom = otherKingdom;
                }
            }*/

            // Notify
            PrintToMessages(otherClan.Name + " has become one with " + playerClan.Name + "!", 102, 255, 102);

            // Destroy clan WITHOUT KILLING PREVIOUS LEADER
            DestroyClanAction.ApplyByClanLeaderDeath(otherClan);
            clans.Remove(otherClan);
            factions.Remove(otherClan);

            // JOIN KINGDOM (if both are rulers)
            if (playerKingdom != null && playerKingdom.RulingClan == playerClan &&
                otherKingdom != null && otherKingdom.MapFaction.Leader == Hero.OneToOneConversationHero) {
                List<Kingdom> kingdoms = (List<Kingdom>)AccessTools.Field(typeof(CampaignObjectManager), "_kingdoms").GetValue(Campaign.Current.CampaignObjectManager);

                List<Clan> clansToTransfer = otherKingdom.Clans.ToList();
                foreach (Clan clan in clansToTransfer)
                    ChangeKingdomAction.ApplyByJoinToKingdom(clan, playerKingdom, false);

                // Notify
                PrintToMessages(otherKingdom.Name + " has become one with " + playerKingdom.Name + "!", 102, 255, 102);

                // Destroy kingdom
                DestroyKingdomAction.Apply(otherKingdom);
                kingdoms.Remove(otherKingdom);
                factions.Remove(otherKingdom);
            }
        }
    }
}
