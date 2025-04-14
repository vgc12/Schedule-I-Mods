using HarmonyLib;
using MelonLoader;
using ScheduleOne;
using ScheduleOne.ItemFramework;
using ScheduleOne.UI.Items;
using UnityEngine;

[assembly: MelonInfo(typeof(ImprovedStackSelection.ImprovedStackSelection), "Improved Stack Selection", "1.0.0", "Vgc12")]
namespace ImprovedStackSelection
{
    public class ImprovedStackSelection : MelonMod
    {
        public override void OnEarlyInitializeMelon()
        {
            MelonLogger.Msg("Improved Stack Selection loaded!");
            using (var harmony = new HarmonyLib.Harmony("Vgc12.ImprovedStackSelection"))
            {
                harmony.Patch(AccessTools.Method(typeof(ItemUIManager), "UpdateCashDragAmount"), new HarmonyMethod(typeof(UpdateCashDragAmountPatch), "Prefix"));
            }
        }

        [HarmonyPatch]
        private static class UpdateCashDragAmountPatch
        {
            
            [HarmonyPrefix]
            [HarmonyPatch(typeof(ItemUIManager), "UpdateCashDragAmount", typeof(CashInstance))]
            public static bool Prefix(ItemUIManager __instance, CashInstance instance)
            {
               // MelonLogger.Msg("Loaded");
               
                if(GameInput.MouseScrollDelta == 0) return false;
                var draggedCashAmount = Traverse.Create(__instance).Field("draggedCashAmount");
                
                var denomination = 1;
                
                if (GameInput.GetButton(GameInput.ButtonCode.Sprint))
                {
                    denomination = 10;
                }
                else if (GameInput.GetButton(GameInput.ButtonCode.Crouch))
                {
                    denomination = 100;
                }
                
                
                denomination = GameInput.MouseScrollDelta < 0 ? -denomination : denomination;
                
               
                draggedCashAmount.SetValue(Mathf.Clamp(draggedCashAmount.GetValue<float>() + denomination, 1f,
                   Mathf.Min(instance.Balance, 1000f)));
                return false;
                
            }
        }
      
    }
}