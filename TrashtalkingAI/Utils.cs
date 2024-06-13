using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TrashtalkingAI
{
    public static class Utils
    {
        public static IEnumerator AddMinecraftMessage(CharacterBody body, string action, float firstDelay, float secondDelay)
        {
            yield return new WaitForSeconds(firstDelay);

            var globalTrashTalkingController = Stage.instance.GetComponent<TrashtalkingController>();

            if (!globalTrashTalkingController.canTrashTalkGlobally)
            {
                yield break;
            }

            var bestLocalizedBodyName = Util.GetBestBodyName(body.gameObject);
            if (bestLocalizedBodyName == "" || bestLocalizedBodyName == string.Empty)
            {
                yield break;
            }

            if (bestLocalizedBodyName == "???")
            {
                bestLocalizedBodyName = "Unknown monster";
            }

            var finalString = "<color=#FFFF55>" + bestLocalizedBodyName + " #" + body.GetComponent<TrashtalkingController>().randomAssignedID + " " + action + " the game</color>";

            yield return new WaitForSeconds(secondDelay);
            Chat.AddMessage(finalString);
            Util.PlaySound("Play_UI_chatMessage", RoR2Application.instance.gameObject);
        }

        public static void SendRandomPhrase(float chance, CharacterBody body, List<string> phrasesList, string prefix, bool nonPlayer = true)
        {
            if (body.teamComponent.teamIndex == TeamIndex.Player && nonPlayer)
            {
                return;
            }

            if (body.master == null)
            {
                return;
            }

            var trashTalkingController = body.GetComponent<TrashtalkingController>();

            if (!trashTalkingController.canTrashTalk)
            {
                return;
            }

            var globalTrashTalkingController = Stage.instance.GetComponent<TrashtalkingController>();

            if (!globalTrashTalkingController.canTrashTalkGlobally)
            {
                return;
            }

            var scalar = 1.5f / Mathf.Pow(Run.instance.difficultyCoefficient, 0.6f);

            // Main.TAILogger.LogError("final chance is " + (chance * scalar));

            if (Util.CheckRoll(chance * scalar))
            {
                var bestLocalizedBodyName = Util.GetBestBodyName(body.gameObject);

                if (bestLocalizedBodyName == "???")
                {
                    bestLocalizedBodyName = "Unknown monster";
                }

                var formattedBodyName = "<color=#A5A5A5>*" + prefix.ToUpper() + "*</color> <style=cIsHealth>" + bestLocalizedBodyName + " #" + trashTalkingController.randomAssignedID + "</style>:";

                var randomPhrase = phrasesList[Run.instance.runRNG.RangeInt(0, phrasesList.Count)];
                if (Util.CheckRoll(Main.uppercasePhraseChance.Value))//if (!char.IsUpper(randomPhrase[0]) && Util.CheckRoll(25f))
                {
                    randomPhrase = randomPhrase.ToUpper();
                }

                var finalString = formattedBodyName + randomPhrase;
                body.StartCoroutine(SendPhrase(finalString, body));

                trashTalkingController.canTrashTalk = false;
                globalTrashTalkingController.canTrashTalkGlobally = false;
            }
        }

        public static IEnumerator SendPhrase(string finalString, CharacterBody body)
        {
            yield return new WaitForSeconds(0.5f);
            Chat.AddMessage(finalString);
            Util.PlaySound("Play_UI_chatMessage", RoR2Application.instance.gameObject);
        }
    }
}