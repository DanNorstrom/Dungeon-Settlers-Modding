using System;
using HarmonyLib;
using Il2CppRefactor.Main;
using MelonLoader;

[assembly: MelonInfo(typeof(DS.FreeReroll.FreeRerollMod), "Free Reroll", "1.0.0", "danno")]

namespace DS.FreeReroll
{
    public class FreeRerollMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Free Reroll loaded: level-up rerolls are now free after the first prayer attempt.");
        }
    }

    // Forces the actual item consumption plan to require zero items, regardless of
    // what CalculateRequiredItem reported. Avoids relying on the (unreliable, in this
    // Il2CppInterop build) Il2CppSystem.ValueTuple<string,int,int> return marshaling.
    [HarmonyPatch(typeof(LevelUpHelper), nameof(LevelUpHelper.TryBuildRequiredItemConsumePlan))]
    internal static class TryBuildRequiredItemConsumePlan_FreePatch
    {
        private static void Prefix(ref int requiredAmount)
        {
            if (requiredAmount != 0)
            {
                MelonLogger.Msg($"[FreeReroll] TryBuildRequiredItemConsumePlan: forcing requiredAmount from {requiredAmount} to 0.");
                requiredAmount = 0;
            }
        }
    }

    // Forces the tracked prayer-attempt index to always read as the first (free) attempt.
    [HarmonyPatch(typeof(CampaignRandomStateContainer), nameof(CampaignRandomStateContainer.GetLevelUpPrayerAttemptIndex))]
    internal static class GetLevelUpPrayerAttemptIndex_FreePatch
    {
        private static void Postfix(ref int __result)
        {
            if (__result != 0)
            {
                MelonLogger.Msg($"[FreeReroll] GetLevelUpPrayerAttemptIndex: forcing attempt index from {__result} to 0.");
                __result = 0;
            }
        }
    }
}
