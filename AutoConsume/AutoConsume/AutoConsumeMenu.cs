using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace AutoConsume
{
    public class AutoConsumeMenu : IClickableMenu
    {
        // Fields
        private readonly List<ClickableComponent> Labels = new List<ClickableComponent>();
        private readonly List<ClickableTextureComponent> CheckBoxes = new List<ClickableTextureComponent>();
        private ClickableTextureComponent ExitButton;

        public static int menuWidth = (int)(Game1.uiViewport.Width/1.5);
        public static int menuHeight = (int)(Game1.uiViewport.Height/1.5); 

        // Public Method
        public AutoConsumeMenu()
            : base((int)getAppropriateMenuPosition().X, (int)getAppropriateMenuPosition().Y, menuWidth, menuHeight)
        {
            this.setUpPositions();
        }

        public static Vector2 getAppropriateMenuPosition()
        {
            Vector2 defaultPosition = new Vector2((Game1.uiViewport.Width - menuWidth)/2 , (Game1.uiViewport.Height - menuHeight)/2);

            //Force the viewport into a position that it should fit into on the screen???
            if (defaultPosition.X + menuWidth > Game1.uiViewport.Width)
            {
                defaultPosition.X = 0;
            }

            if (defaultPosition.Y + menuHeight > Game1.uiViewport.Height)
            {
                defaultPosition.Y = 0;
            }
            return defaultPosition;

        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.xPositionOnScreen = (int)getAppropriateMenuPosition().X;
            this.yPositionOnScreen = (int)getAppropriateMenuPosition().Y;
            this.setUpPositions();

        }

        // private method
        private void setUpPositions()
        {
            string healLabelText = "Auto Heal";
            string buffLabelText = "Auto Buff";
            // clear and initialized
            this.ExitButton = new ClickableTextureComponent("exit-button", new Rectangle(this.xPositionOnScreen + menuWidth, this.yPositionOnScreen, Game1.tileSize, Game1.tileSize), "", null, Game1.mouseCursors, new Rectangle(337, 493, 13, 13), 3f);
            this.Labels.Clear();
            this.CheckBoxes.Clear();
            // set labels and checkboxes position
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + borderWidth + 45, this.yPositionOnScreen + borderWidth, 1, 1), healLabelText));
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + borderWidth + 45, this.yPositionOnScreen + borderWidth*2, 1, 1), buffLabelText));
            this.CheckBoxes.Add(new ClickableTextureComponent("check-box", new Rectangle(this.xPositionOnScreen + borderWidth, this.yPositionOnScreen + borderWidth, Game1.tileSize, Game1.tileSize), "", null, Game1.mouseCursors, new Rectangle(227, 425, 9, 9), 3f));
            this.CheckBoxes.Add(new ClickableTextureComponent("check-box", new Rectangle(this.xPositionOnScreen + borderWidth, this.yPositionOnScreen + borderWidth*2, Game1.tileSize, Game1.tileSize), "", null, Game1.mouseCursors, new Rectangle(227, 425, 9, 9), 3f));
        }

        private void handleButtonClick(string name)
        {
            Game1.playSound("coin");
        }


        // overide
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            foreach(ClickableTextureComponent checkbox in this.CheckBoxes.ToList())
            {
                if (checkbox.containsPoint(x, y))
                {
                    this.handleButtonClick(checkbox.name);
                }
            }

            if (ExitButton.containsPoint(x, y))
            {
                this.handleButtonClick(ExitButton.name);
            }
        }

        public override void draw(SpriteBatch b)
        {
            // draw menu box
            IClickableMenu.drawTextureBox(b, this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.Beige);
            //Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            // draw exit button
            this.ExitButton.draw(b);
            // draw text
            foreach (ClickableComponent label in this.Labels)
            {
                Color color = Color.Violet;
                Utility.drawTextWithShadow(b, label.name, Game1.smallFont, new Vector2(label.bounds.X, label.bounds.Y), color);
            }
  
            foreach (ClickableComponent label in this.Labels)
            {
                string text = "";
                Color color = Game1.textColor;
                Utility.drawTextWithShadow(b, label.name, Game1.smallFont, new Vector2(label.bounds.X, label.bounds.Y), color);
                if (text.Length > 0)
                    Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2(label.bounds.X + Game1.tileSize / 3 - Game1.smallFont.MeasureString(text).X / 2f, label.bounds.Y + Game1.tileSize / 2), color);
            }
            
            // draw check boxes
            foreach (ClickableTextureComponent checkbox in this.CheckBoxes)
            {
                checkbox.draw(b);
            }

            const string Cheese_ID = "424";
            Item cheese = new StardewValley.Object(Cheese_ID, 1, false, -1, 2);
            StardewValley.Object cheeseObj = new StardewValley.Object(Cheese_ID, 1, false, -1, 2);

            // draw cursor
            this.drawMouse(b);
        }

    }
}