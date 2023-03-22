using JoinHouses.StaticUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace JoinHouses {
    public class JoinHousesCampaignBehavior : CampaignBehaviorBase {
        private CampaignGameStarter game;

        public JoinHousesCampaignBehavior(CampaignGameStarter game) {
            this.game = game;

            AddJoinHousesDialog();
        }

        public override void RegisterEvents() {
            //
        }

        public override void SyncData(IDataStore dataStore) {
            //
        }

        private void AddJoinHousesDialog() {
            // hero_main_options = self explanatory
            // lord_pretalk = make them ask "anything else?"
            // close_window = EXIT // seems to cause attack bug when done on map, so avoid.
            // lord_talk_speak_diplomacy_2 = "There is something I'd like to discuss."

            // BASE MOD DIALOG

            game.AddPlayerLine("JoinHousesEventStart", "lord_talk_speak_diplomacy_2", "JoinHousesEventStartOutput",
                "We should join our houses and consider marriage with each other.",
                () => Conversation.JoinHousesAllowed(), null, 100, null, null);

            game.AddDialogLine("JoinHousesEventStartInterested", "JoinHousesEventStartOutput", "JoinHousesEventConfirm",
                "I think that can be arranged.[rf:positive, rb:unsure]",
                () => Conversation.JoinHousesAccepted(), null, 100, null);
            game.AddDialogLine("JoinHousesEventStartNotInterested", "JoinHousesEventStartOutput", "lord_pretalk",
                "I don't think so.[rf:very_negative_ag, rb:negative]",
                null, null, 90, null);

            game.AddPlayerLine("JoinHousesEventConfirmYes", "JoinHousesEventConfirm", "JoinHousesEventGo",
                "It's settled then.",
                null, null, 100, null);
            game.AddPlayerLine("JoinHousesEventConfirmNo", "JoinHousesEventConfirm", "lord_pretalk",
                "Uh, nevermind.",
                null, null, 90, null);

            game.AddDialogLine("JoinHousesEventDoThatThang", "JoinHousesEventGo", "lord_pretalk",
                "We should prepare immediately.[rf:positive, rb:positive]",
                null, () => Conversation.JoinHousesHandle(), 100, null);
        }
    }
}
