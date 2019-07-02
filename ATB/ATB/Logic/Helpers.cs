using ATB.Models;
using ATB.Utilities;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.RemoteAgents;
using ff14bot.RemoteWindows;
using System.Threading.Tasks;
using TreeSharp;

namespace ATB.Logic
{
    public static class Helpers
    {
        private static readonly Composite HelpersComposite;

        static Helpers()
        {
            HelpersComposite = new Decorator(new PrioritySelector(new ActionRunCoroutine(r => HelpersMethod())));
        }

        public static Composite Execute()
        {
            return HelpersComposite;
        }

        private static async Task<bool> HelpersMethod()
        {
            if (MainSettingsModel.Instance.UseAutoCutscene)
            {
                if (QuestLogManager.InCutscene)
                {
                    if (AgentCutScene.Instance.CanSkip && !SelectString.IsOpen)
                    {
                        AgentCutScene.Instance.PromptSkip();
                        if (await Coroutine.Wait(600, () => SelectString.IsOpen))
                        {
                            SelectString.ClickSlot(0);
                            await Coroutine.Sleep(1000);
                        }
                    }
                }
            }

            if (MainSettingsModel.Instance.UseAutoTalk)
            {
                if (Talk.DialogOpen)
                    Talk.Next();

                //if (Request.IsOpen)
                //{
                //    Logger.ATBLog("Handing over any item(s) in your Key Items.");
                //    foreach (var s in InventoryManager.GetBagByInventoryBagId(InventoryBagId.KeyItems))
                //    {
                //        s.Handover();
                //        Logger.ATBLog(s.EnglishName);
                //        await Coroutine.Wait(250, () => Request.HandOverButtonClickable);
                //        if (Request.HandOverButtonClickable) { break; }
                //    }

                //    Logger.ATBLog("Handing over any item(s) in your Inventory.");
                //    foreach (var s in InventoryManager.FilledSlots)
                //    {
                //        s.Handover();
                //        Logger.ATBLog(s.EnglishName);
                //        await Coroutine.Wait(250, () => Request.HandOverButtonClickable);
                //        if (Request.HandOverButtonClickable) { break; }
                //    }

                //    if (Request.HandOverButtonClickable) { Request.HandOver(); }

                //    Logger.ATBLog("Handing over any item(s) in your Armory.");
                //    foreach (var s in InventoryManager.FilledArmorySlots)
                //    {
                //        s.Handover();
                //        Logger.ATBLog(s.EnglishName);
                //        await Coroutine.Wait(250, () => Request.HandOverButtonClickable);
                //        if (Request.HandOverButtonClickable) { break; }
                //    }

                //    if (Request.HandOverButtonClickable) { Request.HandOver(); }
                //    else { await Coroutine.Wait(3000, () => !Request.IsOpen); }
                //}
            }

            if (MainSettingsModel.Instance.UseAutoQuest)
            {
                if (JournalAccept.IsOpen)
                    JournalAccept.Accept();

                if (JournalResult.IsOpen)
                    JournalResult.Complete();
            }
            return false;
        }
    }
}