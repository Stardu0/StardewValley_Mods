using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley.GameData;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Inventories;
using StardewValley.Tools;

namespace AutoEat
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // print button presses to the console window
            //this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }

        private void OnUpdateTicked(object sender, EventArgs e)
        {
            //this.Monitor.Log($"{Game1.player.health}", LogLevel.Debug);
            if(Game1.player.health < Game1.player.maxHealth*0.3)
            {
                this.Monitor.Log("Warning!!", LogLevel.Debug);
                const string Cheese_ID = "424";
                Item cheese = new StardewValley.Object(Cheese_ID, 1, false, -1, 2);
                int idx = Game1.player.getIndexOfInventoryItem(cheese);
                if(idx >= 0)
                {
                    this.Monitor.Log($"idx:{idx}", LogLevel.Info);
                }
            }
        }
    }
}