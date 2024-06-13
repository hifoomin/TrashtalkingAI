using RoR2;
using UnityEngine;
using static TrashtalkingAI.Utils;
using static TrashtalkingAI.Main;

namespace TrashtalkingAI
{
    public static class Hooks
    {
        public static void Init()
        {
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            On.RoR2.SetStateOnHurt.SetStun += SetStateOnHurt_SetStun;
            On.RoR2.SetStateOnHurt.SetFrozen += SetStateOnHurt_SetFrozen;
            On.RoR2.SetStateOnHurt.SetShock += SetStateOnHurt_SetShock;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        private static void Stage_onStageStartGlobal(Stage stage)
        {
            if (stage.GetComponent<TrashtalkingController>() == null)
            {
                stage.gameObject.AddComponent<TrashtalkingController>();
            }
        }

        private static void CharacterBody_onBodyStartGlobal(CharacterBody body)
        {
            if (body.teamComponent.teamIndex == TeamIndex.Player || body.isPlayerControlled)
            {
                return;
            }

            if (body.master == null)
            {
                return;
            }

            if (body.GetComponent<TrashtalkingController>() == null)
            {
                body.gameObject.AddComponent<TrashtalkingController>();

                var scalar = 1.5f / Mathf.Pow(Run.instance.difficultyCoefficient, 0.6f);

                var chance = joinMessageChance.Value;

                if (body.isElite)
                {
                    chance *= eliteChanceMultiplier.Value;
                }

                if (body.isBoss)
                {
                    chance *= bossChanceMultiplier.Value;
                }

                if (RoR2.Util.CheckRoll(chance * scalar))
                {
                    body.StartCoroutine(Utils.AddMinecraftMessage(body, "joined", 0.1f, 0));
                }
            }
        }

        private static void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {
            var victimBody = report.victimBody;
            if (!victimBody)
            {
                return;
            }

            if (victimBody.isPlayerControlled)
            {
                // Main.TAILogger.LogError("victim is player controlled so running that and returning");
                var attackerBody = report.attackerBody;
                if (attackerBody)
                {
                    SendRandomPhrase(onPlayerKilledChance.Value, attackerBody, onPlayerKilledPhrasesList, "killer");
                }
                return;
            }

            // Main.TAILogger.LogError("trying to send random phrase in on death");

            var scalar = 1.5f / Mathf.Pow(Run.instance.difficultyCoefficient, 0.6f);

            SendRandomPhrase(onDeathChance.Value * scalar, victimBody, onDeathPhrasesList, "dead", true);

            if (RoR2.Util.CheckRoll(leaveMessageChance.Value * scalar))
            {
                // Main.TAILogger.LogError("trying to send leave message");
                victimBody.StartCoroutine(Utils.AddMinecraftMessage(victimBody, "left", 0, 1f));
            }
        }

        private static void SetStateOnHurt_SetShock(On.RoR2.SetStateOnHurt.orig_SetShock orig, SetStateOnHurt self, float duration)
        {
            orig(self, duration);
            var victimBody = self.GetComponent<CharacterBody>();

            var scalar = 1.5f / Mathf.Pow(Run.instance.difficultyCoefficient, 0.6f);

            SendRandomPhrase(onShockedChance.Value * scalar, victimBody, onShockedPhrasesList, "shocked");
        }

        private static void SetStateOnHurt_SetFrozen(On.RoR2.SetStateOnHurt.orig_SetFrozen orig, SetStateOnHurt self, float duration)
        {
            orig(self, duration);
            var victimBody = self.GetComponent<CharacterBody>();

            var scalar = 1.5f / Mathf.Pow(Run.instance.difficultyCoefficient, 0.6f);

            SendRandomPhrase(onFrozenChance.Value * scalar, victimBody, onFrozenPhrasesList, "frozen");
        }

        private static void SetStateOnHurt_SetStun(On.RoR2.SetStateOnHurt.orig_SetStun orig, SetStateOnHurt self, float duration)
        {
            orig(self, duration);
            var victimBody = self.GetComponent<CharacterBody>();

            var scalar = 1.5f / Mathf.Pow(Run.instance.difficultyCoefficient, 0.6f);

            SendRandomPhrase(onStunnedChance.Value * scalar, victimBody, onStunnedPhrasesList, "stunned");
        }
    }
}