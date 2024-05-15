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
        private readonly List<ClickableTextureComponent> Arrows = new List<ClickableTextureComponent>();
        private readonly List<ClickableTextureComponent> ItemBoxes = new List<ClickableTextureComponent>();
        private readonly List<ClickableTextureComponent> InfoBoxes = new List<ClickableTextureComponent>();
        private ClickableTextureComponent ExitButton;
        private ModConfig Config;
        private Texture2D letterTexture;
        private Rectangle EmptyCheckBox = new Rectangle(227, 425, 9, 9);
        private Rectangle FullCheckBox = new Rectangle(236, 425, 9, 9);
        private Rectangle RightArrow = new Rectangle(365, 494, 12, 12);
        private Rectangle LeftArrow = new Rectangle(352, 494, 12, 12);
        private Rectangle ItemBox = new Rectangle(293, 360, 24, 24);
        private Rectangle InfoBox = new Rectangle(0, 0, 320, 180);
        private const float ScaleFactor = 3f;

        private static int menuWidth = (int)(Game1.uiViewport.Width/2.5);
        private static int menuHeight = (int)(Game1.uiViewport.Height/1.5); 

        // Public Method
        public AutoConsumeMenu(ModConfig Config)
            : base((int)getAppropriateMenuPosition().X, (int)getAppropriateMenuPosition().Y, menuWidth, menuHeight)
        {
            this.Config = Config;
            letterTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\letterBG");
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
            string healItemText = "Heal Item";
            string buffItemText = "Buff Item";
            int paddingSize = 30;
            // clear and initialized
            this.ExitButton = new ClickableTextureComponent("exit-button", new Rectangle(this.xPositionOnScreen + menuWidth, this.yPositionOnScreen, Game1.tileSize, Game1.tileSize), "", null, Game1.mouseCursors, new Rectangle(337, 493, 13, 13), 3f);
            this.Labels.Clear();
            this.CheckBoxes.Clear();
            this.Arrows.Clear();
            this.ItemBoxes.Clear();
            // set labels and checkboxes position
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + borderWidth + 45, this.yPositionOnScreen + borderWidth, 1, 1), healLabelText));
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + borderWidth + 45, this.yPositionOnScreen + borderWidth * 2, 1, 1), buffLabelText));
            this.CheckBoxes.Add(new ClickableTextureComponent("autoheal-check-box", new Rectangle(this.xPositionOnScreen + borderWidth, this.yPositionOnScreen + borderWidth, (int)(EmptyCheckBox.Width * ScaleFactor), (int)(EmptyCheckBox.Height * ScaleFactor)), "", null, Game1.mouseCursors, EmptyCheckBox, ScaleFactor));
            this.CheckBoxes.Add(new ClickableTextureComponent("autobuff-check-box", new Rectangle(this.xPositionOnScreen + borderWidth, this.yPositionOnScreen + borderWidth*2, (int)(EmptyCheckBox.Width * ScaleFactor), (int)(EmptyCheckBox.Height * ScaleFactor)), "", null, Game1.mouseCursors, EmptyCheckBox, ScaleFactor));
            // set arrows and itemboxes position and labels position
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + borderWidth, this.yPositionOnScreen + borderWidth * 4, 1, 1), healItemText));
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + borderWidth, this.yPositionOnScreen + borderWidth * 8, 1, 1), buffItemText));

            this.Arrows.Add(new ClickableTextureComponent("heal-item-left-arrow", new Rectangle(this.xPositionOnScreen + borderWidth, this.yPositionOnScreen + borderWidth * 5 + ItemBox.Height/4, (int)(LeftArrow.Height * ScaleFactor),(int)(LeftArrow.Height * ScaleFactor)), "", "", Game1.mouseCursors, LeftArrow, ScaleFactor));
            this.Arrows.Add(new ClickableTextureComponent("heal-item-right-arrow", new Rectangle(this.xPositionOnScreen + borderWidth + ItemBox.Width + LeftArrow.Width + paddingSize * 2, this.yPositionOnScreen + borderWidth * 5 + ItemBox.Height/4, (int)(RightArrow.Width * ScaleFactor), (int)(RightArrow.Height * ScaleFactor)), "", "", Game1.mouseCursors, RightArrow, ScaleFactor));
            this.ItemBoxes.Add(new ClickableTextureComponent("heal-item-box", new Rectangle(this.xPositionOnScreen + borderWidth + LeftArrow.Width + paddingSize , this.yPositionOnScreen + borderWidth * 5, (int)(ItemBox.Width * ScaleFactor), (int)(ItemBox.Height * ScaleFactor)), "", "", Game1.mouseCursors, ItemBox, 2f));

            this.Arrows.Add(new ClickableTextureComponent("buff-item-left-arrow", new Rectangle(this.xPositionOnScreen + borderWidth, this.yPositionOnScreen + borderWidth * 9 + ItemBox.Height / 4, (int)(LeftArrow.Height * ScaleFactor), (int)(LeftArrow.Height * ScaleFactor)), "", "", Game1.mouseCursors, LeftArrow, ScaleFactor));
            this.Arrows.Add(new ClickableTextureComponent("buff-item-right-arrow", new Rectangle(this.xPositionOnScreen + borderWidth + ItemBox.Width + LeftArrow.Width + paddingSize * 2, this.yPositionOnScreen + borderWidth * 9 + ItemBox.Height / 4, (int)(RightArrow.Width * ScaleFactor), (int)(RightArrow.Height * ScaleFactor)), "", "", Game1.mouseCursors, RightArrow, ScaleFactor));
            this.ItemBoxes.Add(new ClickableTextureComponent("buff-item-box", new Rectangle(this.xPositionOnScreen + borderWidth + LeftArrow.Width + paddingSize, this.yPositionOnScreen + borderWidth * 9, (int)(ItemBox.Width * ScaleFactor), (int)(ItemBox.Height * ScaleFactor)), "", "", Game1.mouseCursors, ItemBox, 2f));
            // set Info box position
            this.InfoBoxes.Add(new ClickableTextureComponent("heal-info-box", new Rectangle(this.xPositionOnScreen + borderWidth* 5, this.yPositionOnScreen + borderWidth * 4, 1, 1), "", "", letterTexture, InfoBox, 0.7f));
            this.InfoBoxes.Add(new ClickableTextureComponent("buff-info-box", new Rectangle(this.xPositionOnScreen + borderWidth * 5, this.yPositionOnScreen + borderWidth * 8, 1, 1), "", "", letterTexture, InfoBox, 0.7f));

            // Game1.cropSpriteSheet
        }

        private void handleButtonClick(string name)
        {
            switch (name)
            {
                case "autoheal-check-box":
                    Config.AutoHealKey = !Config.AutoHealKey;
                    break;
                case "autobuff-check-box":
                    Config.AutoBuffKey = !Config.AutoBuffKey;
                    break;
                case "exit-button":
                    this.exitThisMenu();
                    break;
            }

            Game1.playSound("Ostrich");
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
            // draw exit button
            this.ExitButton.draw(b);
            // draw text
            foreach (ClickableComponent label in this.Labels)
            {
                // draw in a violet color so that the text can be seen when the background is dark
                Color color = Color.Violet;
                Utility.drawTextWithShadow(b, label.name, Game1.smallFont, new Vector2(label.bounds.X, label.bounds.Y), color);
                color = Game1.textColor;
                Utility.drawTextWithShadow(b, label.name, Game1.smallFont, new Vector2(label.bounds.X, label.bounds.Y), color);
            }
            
            // draw check boxes
            foreach (ClickableTextureComponent checkbox in this.CheckBoxes)
            {
                switch (checkbox.name)
                {
                    case "autoheal-check-box":
                        if (Config.AutoHealKey) checkbox.sourceRect = FullCheckBox;
                        else checkbox.sourceRect = EmptyCheckBox;
                        break;

                    case "autobuff-check-box":
                        if (Config.AutoBuffKey) checkbox.sourceRect = FullCheckBox;
                        else checkbox.sourceRect = EmptyCheckBox;
                        break;

                }
                checkbox.draw(b);     
            }

            // draw item box and arrows
            foreach (ClickableTextureComponent arrow in this.Arrows)
            {
                arrow.draw(b);
            }
            foreach (ClickableTextureComponent itembox in this.ItemBoxes)
            {
                itembox.draw(b);
            }

            // draw info box
            foreach (ClickableTextureComponent infobox in this.InfoBoxes)
            {
                infobox.draw(b);
            }

            const string Cheese_ID = "424";
            Item cheese = new StardewValley.Object(Cheese_ID, 1, false, -1, 2);
            StardewValley.Object cheeseObj = new StardewValley.Object(Cheese_ID, 1, false, -1, 2);

            // draw cursor
            this.drawMouse(b);
        }

    }
}