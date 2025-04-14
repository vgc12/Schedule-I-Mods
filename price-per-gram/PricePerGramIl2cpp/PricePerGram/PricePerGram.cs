using System.Text.RegularExpressions;
using HarmonyLib;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.UI.Phone;
using MelonLoader;


[assembly: MelonInfo(typeof(PricePerGram.PricePerGram), "Price per gram", "1.0", "Vgc12")]
namespace PricePerGram;

public class PricePerGram : MelonMod
{
  
    public override void OnInitializeMelon()
    {
        
        HarmonyInstance.Patch(AccessTools.Method(typeof(CounterofferInterface), "ChangePrice"), null,
            new HarmonyMethod(typeof(ChangePricePatch), "Postfix"));
        HarmonyInstance.Patch(AccessTools.Method(typeof(CounterofferInterface), "UpdateFairPrice"), null,
            new HarmonyMethod(typeof(UpdateFairPricePatch), "Postfix"));
    }

    private static void UpdateLabel(ref CounterofferInterface instance)
    {
        
        var ppg = instance.price / instance.quantity;

        
        var regex = new Regex(@"Price per gram: \$\d+");
        var indexOf = regex.Match(instance.FairPriceLabel.text).Index;
        
     
        if (indexOf > 0)
        {
            
            instance.FairPriceLabel.text = instance.FairPriceLabel.text.Remove(indexOf);
            instance.FairPriceLabel.text += "Price per gram: " + MoneyManager.FormatAmount(ppg);
            return;
        }
        
       
        instance.FairPriceLabel.text += "\n" + "Price per gram: " + MoneyManager.FormatAmount(ppg);
        

 
    }

    [HarmonyPatch(typeof(CounterofferInterface), "ChangePrice")]
    private static class ChangePricePatch
    {
        public static void Postfix(CounterofferInterface __instance)
        {
        
            UpdateLabel(ref __instance);
        }
    }
        
    [HarmonyPatch(typeof(CounterofferInterface), "UpdateFairPrice")]
    private static class  UpdateFairPricePatch
    {
        public static void Postfix(CounterofferInterface __instance)
        {
        
            UpdateLabel(ref __instance);
        }
    }
}