﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Barotrauma
{
    static class HUDLayoutSettings
    {
        public static bool DebugDraw;

        public static Rectangle ButtonAreaTop
        {
            get; private set;
        }

        public static Rectangle MessageAreaTop
        {
            get; private set;
        }

        public static Rectangle InventoryAreaUpper
        {
            get; private set;
        }

        public static Rectangle CrewArea
        {
            get; private set;
        }

        public static Rectangle ChatBoxArea
        {
            get; private set;
        }

        public static Alignment ChatBoxAlignment
        {
            get; private set;
        }

        public static Rectangle InventoryAreaLower
        {
            get; private set;
        }

        public static Rectangle HealthBarAreaLeft
        {
            get; private set;
        }
        public static Rectangle AfflictionAreaLeft
        {
            get; private set;
        }

        public static Rectangle HealthBarAreaRight
        {
            get; private set;
        }
        public static Rectangle AfflictionAreaRight
        {
            get; private set;
        }

        public static Rectangle HealthWindowAreaLeft
        {
            get; private set;
        }

        public static Rectangle HealthWindowAreaRight
        {
            get; private set;
        }

        public static Rectangle ReportArea
        {
            get; private set;
        }

        public static int Padding
        {
            get; private set;
        }

        static HUDLayoutSettings()
        {
            GameMain.Instance.OnResolutionChanged += CreateAreas;
            GameMain.Config.OnHUDScaleChanged += CreateAreas;
            CreateAreas();
        }
        
        public static RectTransform ToRectTransform(Rectangle rect, RectTransform parent)
        {
            return new RectTransform(new Vector2(rect.Width / (float)GameMain.GraphicsWidth, rect.Height / (float)GameMain.GraphicsHeight), parent)
            {
                RelativeOffset = new Vector2(rect.X / (float)GameMain.GraphicsWidth, rect.Y / (float)GameMain.GraphicsHeight)
            };
        }

        public static void CreateAreas()
        {
            Padding = (int)(10 * GUI.Scale);

            //slice from the top of the screen for misc buttons (info, end round, server controls)
            ButtonAreaTop = new Rectangle(Padding, Padding, GameMain.GraphicsWidth - Padding * 2, (int)(50 * GUI.Scale));

            MessageAreaTop = new Rectangle(GameMain.GraphicsWidth / 4, ButtonAreaTop.Bottom, GameMain.GraphicsWidth / 2, ButtonAreaTop.Height);

            //slice for the upper slots of the inventory (clothes, id card, headset)
            int inventoryAreaUpperWidth = (int)Math.Min(GameMain.GraphicsWidth * 0.2f, 300);
            int inventoryAreaUpperHeight = (int)Math.Min(GameMain.GraphicsHeight * 0.2f, 200);
            InventoryAreaUpper = new Rectangle(GameMain.GraphicsWidth - inventoryAreaUpperWidth - Padding, ButtonAreaTop.Bottom + Padding, inventoryAreaUpperWidth, inventoryAreaUpperHeight);

            CrewArea = new Rectangle(Padding, ButtonAreaTop.Bottom + Padding, GameMain.GraphicsWidth - InventoryAreaUpper.Width - Padding * 3, InventoryAreaUpper.Height);

            //horizontal slices at the corners of the screen for health bar and affliction icons
            int healthBarWidth = (int)Math.Max(500 * GUI.Scale, 100);
            int healthBarHeight = (int)Math.Max(20 * GUI.Scale, 15);
            int afflictionAreaHeight = (int)(60 * GUI.Scale);
            HealthBarAreaLeft = new Rectangle(Padding, GameMain.GraphicsHeight - healthBarHeight - Padding, healthBarWidth, healthBarHeight);
            AfflictionAreaLeft = new Rectangle(Padding, HealthBarAreaLeft.Y - afflictionAreaHeight - Padding, healthBarWidth, afflictionAreaHeight);
            
            HealthBarAreaRight = new Rectangle(GameMain.GraphicsWidth - Padding - healthBarWidth, HealthBarAreaLeft.Y, healthBarWidth, HealthBarAreaLeft.Height);
            AfflictionAreaRight = new Rectangle(HealthBarAreaRight.X, AfflictionAreaLeft.Y, healthBarWidth, afflictionAreaHeight);
            
            //chatbox between upper and lower inventory areas, can be on either side depending on the alignment
            ChatBoxAlignment = Alignment.Left;
            int chatBoxWidth = (int)(500 * GUI.Scale);
            int chatBoxHeight = Math.Min((int)(250 * GUI.Scale), AfflictionAreaLeft.Y - InventoryAreaUpper.Bottom - Padding * 2);
            ChatBoxArea = ChatBoxAlignment == Alignment.Left ?
                new Rectangle(Padding, AfflictionAreaLeft.Y - chatBoxHeight - Padding, chatBoxWidth, chatBoxHeight) :
                new Rectangle(GameMain.GraphicsWidth - Padding - chatBoxWidth, AfflictionAreaLeft.Y - chatBoxHeight - Padding, chatBoxWidth, chatBoxHeight);

            int lowerAreaHeight = (int)Math.Min(GameMain.GraphicsHeight * 0.35f, 280);
            //entire bottom side of the screen for inventory, minus health and affliction areas at the sides
            InventoryAreaLower = new Rectangle(ChatBoxArea.Right + Padding, GameMain.GraphicsHeight - lowerAreaHeight, GameMain.GraphicsWidth - Padding * 2 - ChatBoxArea.Right, lowerAreaHeight);

            int healthWindowY = (int)(MessageAreaTop.Bottom + 10 * GUI.Scale);
            Rectangle healthWindowArea = ChatBoxAlignment == Alignment.Left ?
                new Rectangle(ChatBoxArea.Right + Padding, healthWindowY, GameMain.GraphicsWidth - ChatBoxArea.Width - inventoryAreaUpperWidth, GameMain.GraphicsHeight - healthWindowY - lowerAreaHeight / 2) :
                new Rectangle(Padding - ChatBoxArea.Width, healthWindowY, GameMain.GraphicsWidth - ChatBoxArea.Width - inventoryAreaUpperWidth, GameMain.GraphicsHeight - healthWindowY - lowerAreaHeight / 2);

            //split the health area vertically, left side for the player's own health and right side for the character they're treating
            //int healthWindowWidth = Math.Min((int)(healthWindowArea.Width * 0.75f) - Padding / 2, 500);
            HealthWindowAreaLeft = healthWindowArea;// new Rectangle(healthWindowArea.X, healthWindowArea.Y, healthWindowWidth, healthWindowArea.Height);
            HealthWindowAreaRight = healthWindowArea;// new Rectangle(healthWindowArea.Right - healthWindowWidth, healthWindowArea.Y, healthWindowWidth, healthWindowArea.Height);

            //report buttons (report breach etc) appear center right, not visible when health window is open
            int reportAreaWidth = (int)Math.Min(150 * GUI.Scale, 200);
            ReportArea = new Rectangle(GameMain.GraphicsWidth - Padding - reportAreaWidth, InventoryAreaUpper.Bottom + Padding, reportAreaWidth, InventoryAreaLower.Y - InventoryAreaUpper.Bottom - Padding * 2);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            GUI.DrawRectangle(spriteBatch, ButtonAreaTop, Color.White * 0.5f);
            GUI.DrawRectangle(spriteBatch, MessageAreaTop, Color.Orange * 0.5f);
            GUI.DrawRectangle(spriteBatch, InventoryAreaUpper, Color.Yellow * 0.5f);
            GUI.DrawRectangle(spriteBatch, CrewArea, Color.Blue * 0.5f);
            GUI.DrawRectangle(spriteBatch, ChatBoxArea, Color.Cyan * 0.5f);
            GUI.DrawRectangle(spriteBatch, HealthBarAreaLeft, Color.Red * 0.5f);
            GUI.DrawRectangle(spriteBatch, AfflictionAreaLeft, Color.Red * 0.5f);
            GUI.DrawRectangle(spriteBatch, HealthBarAreaRight, Color.Red * 0.5f);
            GUI.DrawRectangle(spriteBatch, AfflictionAreaRight, Color.Red * 0.5f);
            GUI.DrawRectangle(spriteBatch, InventoryAreaLower, Color.Yellow * 0.5f);
            GUI.DrawRectangle(spriteBatch, HealthWindowAreaLeft, Color.Red * 0.5f);
            GUI.DrawRectangle(spriteBatch, HealthWindowAreaRight, Color.Red * 0.5f);
        }
    }
}
