using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrashtalkingAI
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "HIFU";
        public const string PluginName = "TrashtalkingAI";
        public const string PluginVersion = "1.0.2";

        public static ManualLogSource TAILogger;

        public static ConfigEntry<string> onFrozenPhrases { get; set; }
        public static List<string> onFrozenPhrasesList = new();
        public static ConfigEntry<float> onFrozenChance { get; set; }

        public static ConfigEntry<string> onStunnedPhrases { get; set; }
        public static List<string> onStunnedPhrasesList = new();
        public static ConfigEntry<float> onStunnedChance { get; set; }

        public static ConfigEntry<string> onShockedPhrases { get; set; }
        public static List<string> onShockedPhrasesList = new();
        public static ConfigEntry<float> onShockedChance { get; set; }

        public static ConfigEntry<string> onDeathPhrases { get; set; }
        public static List<string> onDeathPhrasesList = new();
        public static ConfigEntry<float> onDeathChance { get; set; }

        public static ConfigEntry<string> onPlayerKilledPhrases { get; set; }
        public static List<string> onPlayerKilledPhrasesList = new();
        public static ConfigEntry<float> onPlayerKilledChance { get; set; }

        public static ConfigEntry<float> uppercasePhraseChance { get; set; }

        public static ConfigEntry<float> trashTalkingInterval { get; set; }
        public static ConfigEntry<float> globalTrashTalkingInterval { get; set; }

        public static ConfigEntry<float> joinMessageChance { get; set; }
        public static ConfigEntry<float> leaveMessageChance { get; set; }

        public static ConfigEntry<bool> scaleChance { get; set; }
        public static ConfigEntry<float> eliteChanceMultiplier { get; set; }
        public static ConfigEntry<float> bossChanceMultiplier { get; set; }

        public void Awake()
        {
            TAILogger = base.Logger;

            onStunnedPhrases = Config.Bind("Phrases", "On Stun",
            "IKE$R(PFD | @#*%()RETIWFDS()K@!)_QWEKDSF(RK(@#$%*$#(@)UTEJWFDSIXC | WESDFU(CJ(EPWSODVPOWEISDKFPOVDMPOVMPV | I CANT EVEN FUCKING PLAY THE GAME | i love counterplay | good game design | STUN MECHANICS ARE SO FUN | stunning my ass bro u ugly as fuck | read stun seed backwards | stun deez nuts bro | what can i even do | its so fair bro | kys | ggwp bad team | get a job loser | is this overwatch | am i playing a fucking 2024 hero shooter | i swear to fucking god if you stun me again | go fuck yourself | respectfully, go fuck yourself | get that smell out of your chair | your life is as valuable as a summer ant | i would call you a cunt but youre shallow as fuck | your life means nothing | you serve zero purpose | get cancer | i hope u get cancer | bro was NOT loved by their mother | stop spamming | I bet you stun cause your too fat to fucking move | ok | ... | FUCK | Really? | lag | fucking ping again | you literally have aimbot lmaoo youre so blatant | showers are not monthly a thing btw | ok dude | alright dude. | nice hitbox | I love this hitreg | I can totally do anytihng ahaHAHASDHAILSCOLCFVJNERIOTFSD(PIJMIPWO$EKSTD | U JUST ANNOYIN AS FUCK OK SHUT THE FUCK UP U CUNT | im gonna break my monitor i swear | dog | u a bitch you know that? | your father left you | fatherless fuck | bet you fun at parties | im blocking you | of courseee | im gonna cancel you on twitter | yo momma so fat i took a pic of her last christmas and its still printing | yo momma so ugly when she tried to join an ugly contest they said sorry no professionals | yo mommas so fat and old when god said let there be light he asked your mother to move out of the way | yo momma so ugly she made one direction look another direction | yo mama so fat and stupid she brought a spoon to the super bowl | your ugly ass thinks salt is spicy | youre literally a fucking dweeb | neckbeard | u got the gen z cut | i bet you have a bowl cut | fuck this | fuck off | hop off my dick",
            "The list of phrases that the AI uses on getting stunned, separated by a pipe symbol | - this one, and space in between it.");
            onStunnedPhrasesList = onStunnedPhrases.Value.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            onStunnedChance = Config.Bind("Phrases", "On Stun Chance", 12f, "Chance to say a random phrase on getting stunned");

            //

            onShockedPhrases = Config.Bind("Phrases", "On Shock",
            "DSKFLSdfh | DFJGDFIGDFIG | W@#*REJF*WIEJDMIKASMD | I CANT EVEN FUCKING PLAY THE GAME | i love counterplay | good game design | STUN MECHANICS ARE SO FUN | stunning my ass bro u ugly as fuck | read stun seed backwards | stun deez nuts bro | what can i even do | its so fair bro | kys | ggwp bad team | get a job loser | is this overwatch | am i playing a fucking 2024 hero shooter | i swear to fucking god if you stun me again | go fuck yourself | respectfully, go fuck yourself | get that smell out of your chair | your life is as valuable as a summer ant | you should listen to lowtiergods advice | i would call you a cunt but youre shallow as fuck | your life means nothing | you serve zero purpose | get cancer | i hope you get cancer | bro was NOT loved by their mother | stop spamming | I bet you stun cause your too fat to fucking move | ok | ... | FUCK | Really? | lag | fucking ping again | you literally have aimbot lmaoo youre so blatant | showers are not a monthly thing btw | ok dude | alright dude. | nice hitbox | I love this hitreg | i can totally fucking do ANYTIRNMFDGISMIOFEWSJMDIFOAYNEULRFNSDUOJNROUIFSJMDIPO | U JUST ANNOYIN AS FUCK OK SHUT THE FUCK UP U CUNT | im gonna break my monitor i swear | dog | u a bitch you know that? | your father left you | fatherless fuck | bet you fun at parties | im blocking you | of courseeeeee | im gonna cancel you on twitter | yo momma so fat i took a pic of her last christmas and its still printing | yo momma so ugly when she tried to join an ugly contest they said sorry no professionals | yo mommas so fat and old when god said let there be light he asked your mother to move out of the way | yo momma so ugly she made one direction look another direction | yo mama so fat and stupid she brought a spoon to the super bowl | your ugly ass thinks salt is spicy | youre literally a fucking dweeb | neckbeard | u got the gen z cut | i bet you have a bowl cut | fuck this | fuck off | hop off my dick",
            "The list of phrases that the AI uses on getting shocked, separated by a pipe symbol | - this one, and space in between it");
            onShockedPhrasesList = onShockedPhrases.Value.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            onShockedChance = Config.Bind("Phrases", "On Shock Chance", 25f, "Chance to say a random phrase on getting shocked");

            //

            onFrozenPhrases = Config.Bind("Phrases", "On Freeze",
            "FEJK*GR%EGFJKD*ET | $ERTU*FGD(B*#@WREJDSFIJIQ@E#WOSD | @*()EW#QUDSFJC()J@#QWESDI | I CANT EVEN FUCKING PLAY THE GAME | I GET STUNNED AND KILLED INSTANTLY ITS SO FAIR | STUN MECHANICS ARE SO FUN | well played bro that was totally fair | i have your address right here | my dad works at microsoft | your life is miserable | i love not being able to do anything | great fucking game design | what the fuck do i even do | loser | kys | so true | get a job loser | is this overwatch | am i playing a fucking 2024 hero shooter | I FUCKING SWEAR TO GOD IF YOU FREEZE ME AGAIN | GO FUCK YOURSELF | respectfully, go fuck yourself | get that smell out of your chair | your life is as valuable as a summer ant | i would call you a cunt but youre shallow as fuck | your life means nothing | you serve zero purpose | get cancer | i hope you get cancer | bro was NOT loved by their mother | stop spamming | I bet you stun cause your too fat to fucking move | ok | ... | FUCK | Really? | lag | fucking ping again | you literally have aimbot lmaoo youre so blatant | showers are not a monthly thing btw | ok dude | alright dude. | nice hitbox | I love this hitreg | i can totally do ANYTIGNMTGFHIDKJMNOILKJUERDJMNFISDOM | U JUST ANNOYIN AS FUCK OK SHUT THE FUCK UP U CUNT | im gonna break my monitor i swear | dog | u a bitch you know that? | dog | u a bitch you know that? | your father left you | fatherless fuck | bet you fun at parties | im blocking you | ofc ofc dude fuckingewsoiufhjdiuhjuiodfiogh | im gonna cancel you on twitter | yo momma so fat i took a pic of her last christmas and its still printing | yo momma so ugly when she tried to join an ugly contest they said sorry no professionals | yo mommas so fat and old when god said let there be light he asked your mother to move out of the way | yo momma so ugly she made one direction look another direction | yo mama so fat and stupid she brought a spoon to the super bowl | your ugly ass thinks salt is spicy | youre literally a fucking dweeb | neckbeard | u got the gen z cut | i bet you have a bowl cut | fuck this | fuck off | hop off my dick",
            "The list of phrases that the AI uses on getting frozen, separated by a pipe symbol | - this one, and space in between it");
            onFrozenPhrasesList = onFrozenPhrases.Value.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            onFrozenChance = Config.Bind("Phrases", "On Freeze Chance", 12f, "Chance to say a random phrase on getting frozen");

            //

            onDeathPhrases = Config.Bind("Phrases", "On Death",
            "DFGMVICXMICXV | DFKGI:DFGI:DFGKDFOI:G | #@$(REWIF#KW(RE | ERI(F)DGVK(ERSDF | take a shower lil bro | have you ever heard of a shower before | so fair | i bet your family loves you /s | sweaty tryhard | what no pussy does to a mf | go outside | touch grass | what am i supposed to do if my idiot team doesnt help me | my team is ass | bullshit | i love getting spawnkilled | get a life | you smell that? sweat mixed with [REDACTED] and [REDACTED] and [REDACTED]. | not cheating btw | i bet your parents dont miss you | you don't have friends. your parents don't love you. | at least I have a life unlike you | it's tryhards like you who ruin Petrichor V | that was bullshit | you didnt even aim at me | of course im supposed to react to that | nobody loves you | im gonna dox you | 100.120.244.30 | 214.46.28.73 | 117.248.59.197 | im just gonna drop this 122.182.207.178 | 41.33.35.176 this u? | 133.138.50.44 i got your address right here lil bro you sure you wanna keep doing this? | 132.57.41.1 | 232.68.68.47 gg | 226.175.14.20 :33 | 14.128.247.29 you finna cooked | i know where you live | your life is meaningless | you have achieved nothing in life | no help gg | kys | go fuck yourself | respectfully, go fuck yourself | get that smell out of your chair | you look like a redditor | incel | you literally fucking use 4chan | you look like a discord moderator | lowlife | you got bullied in school | your life is as valuable as a summer ant | saying you look like your mom would be generous | i would call you a cunt but youre shallow as fuck | your life means nothing | you serve zero purpose | i bet your ass thinks a kilogram of feathers is heavier | i hope you get cancer | get cancer | bro was NOT loved by their mother | ok | ... | FUCK | Really? | my support is a moron | lag | fucking ping again | you literally have aimbot lmaoo youre so blatant | showers are not a monthly thing btw | nah i'd win | nah i'd lose | nice hitbox | I love this hitreg | i can TOTYAELKOY DO ANYITINJFGDUIOJNJMDFIUOLJIOLDSJIFJKDOP | U JUST ANNOYIN AS FUCK OK SHUT THE FUCK UP U CUNT | im gonna break my monitor i swear | dog | u a bitch you know that? | your father left you | fatherless fuck | bet you fun at parties | bet that took a lot of skill lil bro | you barely did it | OF FUCKING COURSEEWIOSUADRHFNJUOREIDFGJV | im blocking you | im gonna cancel you on twitter | yo momma so fat i took a pic of her last christmas and its still printing | yo momma so ugly when she tried to join an ugly contest they said sorry no professionals | yo mommas so fat and old when god said let there be light he asked your mother to move out of the way | yo momma so ugly she made one direction look another direction | yo mama so fat and stupid she brought a spoon to the super bowl | your ugly ass thinks salt is spicy | youre literally a fucking dweeb | neckbeard | u got the gen z cut | i bet you have a bowl cut | fuck this | fuck off | hop off my dick | wow you killed me so good! | youre such a skilled gamer!",
            "The list of phrases that the AI uses upon dying, separated by a pipe symbol | - this one, and space in between it");
            onDeathPhrasesList = onDeathPhrases.Value.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            onDeathChance = Config.Bind("Phrases", "On Death Chance", 8f, "Chance to say a random phrase on death");

            //

            onPlayerKilledPhrases = Config.Bind("Phrases", "On Player Kill",
            "get fucked | bro is playing on a controller | lmao you suck | learn to dodge loser | imagine being that bad | gg ez | ez | is this your first time playing | this mf using steam deck | go back to roblox lil bro | you have to not get owned to win btw | trash | garbage | bro youre ass | uninstall | are you even trying | when are some actual players joining this is pissing me off | go back to drizzle bitch | bro needs mods to win | are you 5 | not even noahs ark can carry you | your life is as valuable as a summer ant | room temperature iq | middle schoolers playing krunker on the school chrome book have more skill than you | sorry its gotta be hard to aim with the sound of me railing your mom in the next room | motherfucker would lose to a dolphin if it had a fuckin bow and arrow | easy killz holy shit you suck | you fucking suck :3 | get cancer | i hope you get cancer | bro was NOT loved by their mother | you call it a criminal record i call it my lore. we are not the same | skill gap | Approximatively six million years of human evolution to produce some pile of garbage like you. | bro got absolutely fucking bodied | gargantuan twat | you fucking broomstick | you fucking melon | I like ya cut g | watch yo tone | skill diff | youre so good dude | go take a shower | bro couldnt construct a normal fucking build with scrappers and printers existing | imagine dying | yo momma so fat she makes whales look like needles in a haystack | yo momma so stupid she got hit by a parked car | sit down dog | know your place, trash | nice try /s | how fucking stupid do you have to be in order to think that would have worked | i didnt even try | go play minecraft | youre a failure at everything you do, you never succeed and nothing you ever do truly matters. you are a failure. | thats what happens when you do something in your miserable life | worthless",
            "The list of phrases that the AI uses upon dying, separated by a pipe symbol | - this one, and space in between it");
            onPlayerKilledPhrasesList = onPlayerKilledPhrases.Value.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            onPlayerKilledChance = Config.Bind("Phrases", "On Player Kill Chance", 100f, "Chance to say a random phrase on killing a player");

            //

            uppercasePhraseChance = Config.Bind("Phrases", "All Uppercase Chance", 30f, "Chance for a phrase to be fully uppercase");

            //

            trashTalkingInterval = Config.Bind("Phrases", "Interval", 3f, "Minimum interval between trashtalking for the same monster");
            globalTrashTalkingInterval = Config.Bind("Phrases", "Global Interval", 3f, "Minimum interval between trashtalking for all monsters");

            //

            joinMessageChance = Config.Bind("Messages", "Join Chance", 5f, "Chance to say a monster has joined the game");
            leaveMessageChance = Config.Bind("Messages", "Leave Chance", 7f, "Chance to say a monster has left the game");

            scaleChance = Config.Bind("Chances", "Scale chances over difficulty?", true, "Chances start out higher than base (config) and fall over time to prevent spam and repetitiveness (Base Chance * (1.5 / (Difficulty Coefficient ^ 0.6))");
            eliteChanceMultiplier = Config.Bind("Chances", "Elite chance scaling multiplier", 2f, "Chances are multiplied by this value for elite monsters");
            bossChanceMultiplier = Config.Bind("Chances", "Boss chance scaling multiplier", 10f, "Chances are multiplied by this value for bosses with red healthbars");

            TAILogger.LogDebug("On Stunned Phrases Count: " + onStunnedPhrasesList.Count);
            TAILogger.LogDebug("On Shocked Phrases Count: " + onShockedPhrasesList.Count);
            TAILogger.LogDebug("On Frozen Phrases Count: " + onFrozenPhrasesList.Count);
            TAILogger.LogDebug("On Death Phrases Count: " + onDeathPhrasesList.Count);
            TAILogger.LogDebug("On Player Kill Phrases Count: " + onPlayerKilledPhrasesList.Count);

            Hooks.Init();
        }
    }
}