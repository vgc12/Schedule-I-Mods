using System.Text.RegularExpressions;
using HarmonyLib;
using MelonLoader;
using ScheduleOne.Money;
using ScheduleOne.UI.Phone;

[assembly: MelonInfo(typeof(PricePerGram.PricePerGram), "Price per gram", "1.0", "Vgc12")]

namespace PricePerGram
{
    public class PricePerGram : MelonMod
    {
        public override void OnEarlyInitializeMelon()
        {
            using (var harmony = new HarmonyLib.Harmony("Vgc12.PricePerGram"))
            {
                harmony.Patch(AccessTools.Method(typeof(CounterofferInterface), "ChangePrice"), null,
                    new HarmonyMethod(typeof(ChangePricePatch), "Postfix"));
                harmony.Patch(AccessTools.Method(typeof(CounterofferInterface), "UpdateFairPrice"), null,
                    new HarmonyMethod(typeof(UpdateFairPricePatch), "Postfix"));
            }
        }

        private static void UpdateLabel(ref CounterofferInterface instance)
        {
            var price = Traverse.Create(instance).Field("price").GetValue<float>();

            var quantity = Traverse.Create(instance).Field("quantity").GetValue<int>();

            var ppg = price / quantity;
            

            var regex = new Regex(@"Price per gram: \$\d+");
            var indexOf = regex.Match(instance.FairPriceLabel.text).Index;
           
            if (indexOf > 0)
            {

                instance.FairPriceLabel.text = instance.FairPriceLabel.text.Remove(indexOf);
                instance.FairPriceLabel.text += "Price per gram: " + MoneyManager.FormatAmount(ppg);
            }
            else
            {
                instance.FairPriceLabel.text += "\n" + "Price per gram: " + MoneyManager.FormatAmount(ppg);
            }

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
}