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

namespace AutoKegManagement
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        int[] dy = new int[8]{-1,0,1,-1,1,-1,0,1};
        int[] dx = new int[8]{-1,-1,-1,0,0,1,1,1};

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;

        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>

        private void OnUpdateTicked(object sender, EventArgs e)
        {

        }

        private void OnOneSecondUpdateTicked(object sender, EventArgs e)
        {
            // this.Monitor.Log($"Grab: {Game1.player.TilePoint}", LogLevel.Info);
            Vector2 playerPos = Game1.player.TilePoint.ToVector2();

            for (int i = 0; i < 8; i++)
            {
                Vector2 newPlayerPos = playerPos;
                newPlayerPos.X += dx[i];
                newPlayerPos.Y += dy[i];

                if(StardewValley.Utility.canGrabSomethingFromHere((int)newPlayerPos.X, (int)newPlayerPos.Y, Game1.player))
                {
                    this.Monitor.Log($"X:{newPlayerPos.X} Y:{newPlayerPos.Y} Can Grab");

                }
            }
        }

    }
}