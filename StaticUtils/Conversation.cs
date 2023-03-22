using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace JoinHouses.StaticUtils {
    public static class Conversation {
        public static bool JoinHousesAllowed() {
            Hero otherHero = Hero.OneToOneConversationHero;

            // If either hero is already married
            if (Hero.MainHero.Spouse != null || otherHero.Spouse != null) return false;

            // If same gender
            if (Hero.MainHero.IsFemale == otherHero.IsFemale) return false;

            Clan otherClan = otherHero.Clan;

            // if other clan is not existent, not noble, or if other hero is not the leader
            if (otherClan == null || !otherClan.IsNoble || otherClan.Leader != otherHero) return false;

            return true;
        }

        public static bool JoinHousesAccepted() {
            Hero otherHero = Hero.OneToOneConversationHero;

            float relationNeeded = (float)SubModule.Config.GetValueDouble("baseRelationNeeded");

            if (Hero.MainHero.IsFactionLeader) relationNeeded += -40f;
            if (otherHero.IsFactionLeader) relationNeeded += 50f;

            relationNeeded += (Hero.MainHero.Clan.Tier * -8);
            relationNeeded += (otherHero.Clan.Tier * 10);

            return otherHero.GetRelationWithPlayer() >= relationNeeded;
        }

        public static void JoinHousesHandle() {
            Clan playerClan = Hero.MainHero.Clan;
            Clan otherClan = Hero.OneToOneConversationHero.Clan;

            Utils.JoinHouses(otherClan);

            // Marry
            MarriageAction.Apply(Hero.MainHero, Hero.OneToOneConversationHero);

            // Join party
            if (Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.LeaderHero == Hero.MainHero)
                AddHeroToPartyAction.Apply(Hero.OneToOneConversationHero, Hero.MainHero.PartyBelongedTo);
        }
    }
}
