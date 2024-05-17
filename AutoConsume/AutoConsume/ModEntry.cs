using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley.GameData;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace AutoConsume
{
    // create Class for Keybinding
    public sealed class ModConfig
    {
        public KeybindList OpenMenuKey { get; set; } = KeybindList.Parse("O");
        public bool AutoHealKey { get; set; }
        public bool AutoBuffKey { get; set; }
        public string HealItemID { get; set; } = "424";
        public int HealItemQuality { get; set; }
        public string BuffItemID { get; set; } = "253";
        public int BuffItemQuality { get; set; }
    }

    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        bool ShouldEat = false;
        bool ShouldDrink = false;

        private ModConfig Config;
        private List<Item> InventoryItems = new List<Item>();

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
            helper.Events.Input.ButtonsChanged += this.OnButtonChanged;
        }

        
        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        ///
        private void OnButtonChanged(object? sender, ButtonsChangedEventArgs e)
        {
            if (!Game1.player.canMove) return;

            

            if (Config.OpenMenuKey.JustPressed())
            {
                this.Monitor.Log("pressed O.", LogLevel.Debug);
                // Get Inventroy Items
                GetInventoryItems();

                Monitor.Log($"{Config.HealItemID} : {Config.HealItemQuality}", LogLevel.Debug);
                Monitor.Log($"{Config.BuffItemID} : {Config.BuffItemQuality}", LogLevel.Debug);
                foreach (Item curitem in InventoryItems)
                {
                    Monitor.Log($"{curitem.Name} : {curitem.Stack}", LogLevel.Debug);
                }

                // if0 Auto Consume Menu is Open then close
                if (Game1.activeClickableMenu is AutoConsumeMenu autoConsumeMenu)
                {
                    autoConsumeMenu.exitThisMenu();
                }
                else Game1.activeClickableMenu = (IClickableMenu)(object)new AutoConsumeMenu(Config, InventoryItems);
            }
        }

        private void GetInventoryItems()
        {
            if (!Context.IsWorldReady) return;
            InventoryItems.Clear();

            foreach (Item curItem in Game1.player.Items)
            {
                if (curItem == null) continue;
                StardewValley.Object o = new StardewValley.Object(curItem.ItemId, 1);
                if (o.Edibility != -300) InventoryItems.Add(curItem);
            }
        }

        private void OnOneSecondUpdateTicked(object sender, EventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady) return;
            if (!Config.AutoBuffKey) return;
            
            // check Buff
            if (!Game1.player.hasBuff("drink") && Game1.player.canMove && Game1.timeOfDay < 2400 && !Game1.IsFading()) ShouldDrink = true;
            else ShouldDrink = false;

            if (ShouldDrink)
            {
                if (Game1.activeClickableMenu is AutoConsumeMenu autoConsumeMenu) return;
                DrinkTrippleShotEspresso();
            }

        }

        private void OnUpdateTicked(object sender, EventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady) return;
            if (!Config.AutoHealKey) return;

            // check health
            if (Game1.player.health <= Game1.player.maxHealth * 0.3 && Game1.player.canMove && !Game1.IsFading()) ShouldEat = true;
            else ShouldEat = false;

            if (ShouldEat)
            {
                if (Game1.activeClickableMenu is AutoConsumeMenu autoConsumeMenu) return;
                EatCheese();
            }

        }

        private void OnDayStarted(object sender, EventArgs e)
        {
            // drink Triple Shot Espresso when the player wakes up
            // ShouldDrink = true;
        }

        private void EatCheese()
        {
            // set variables
            const string Cheese_ID = "424";
            Item cheese = new StardewValley.Object(Cheese_ID, 1, false, -1, 2);
            StardewValley.Object cheeseObj = new StardewValley.Object(Cheese_ID, 1, false, -1, 2);
            // find cheese 
            int idx = Game1.player.getIndexOfInventoryItem(cheese);
            // check inventory
            if (idx >= 0)
            {
                Game1.player.eatObject(cheeseObj);
                Game1.player.Items.ReduceId(Cheese_ID, 1);
            }
        }
        
        private void DrinkTrippleShotEspresso()
        {
            // set variable
            const string TSE_ID = "253"; 
            StardewValley.Object TSEObj = new StardewValley.Object(TSE_ID, 1);
            Item TSE = TSEObj;
            // find TSE
            int idx = Game1.player.getIndexOfInventoryItem(TSEObj);
            // check inventory
            if (idx >= 0)
            {
                Game1.player.eatObject(TSEObj);
                Game1.player.removeFirstOfThisItemFromInventory(TSE_ID);
                //Game1.player.Items.ReduceId(TSE_ID, 1);
            }
        }


        private bool HasItem(StardewValley.Object itemObject)
        {
            if (Game1.player.getIndexOfInventoryItem(itemObject) >= 0) return true;
            return false;
        }

        private void ConsumeItemInInventory(StardewValley.Object itemObject)
        {
            Game1.player.eatObject(itemObject);
            Game1.player.Items.ReduceId(itemObject.itemId, 1);
        }

    }
}