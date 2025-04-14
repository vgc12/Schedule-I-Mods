using HarmonyLib;
using Il2CppScheduleOne;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.UI.Items;
using MelonLoader;

using UnityEngine;

[assembly: MelonInfo(typeof(ImprovedStackSelection.ImprovedStackSelection), "Improved Stack Selection", "1.0.0", "Vgc12")]
namespace ImprovedStackSelection
{
    public class ImprovedStackSelection : MelonMod
    {
        
        [HarmonyPatch(typeof(ItemUIManager), "UpdateCashDragAmount", typeof(CashInstance))]
        private static class UpdateCashDragAmountPatch
        {
            
            
            public static bool Prefix(ItemUIManager __instance, CashInstance instance)
            {
            
               
                if(GameInput.MouseScrollDelta == 0) return false;
                
                
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
               
                __instance.draggedCashAmount = Mathf.Clamp(__instance.draggedCashAmount + denomination, 1f,
                   Mathf.Min(instance.Balance, 1000f));
                return false;
                
            }
        }
      
    }
}