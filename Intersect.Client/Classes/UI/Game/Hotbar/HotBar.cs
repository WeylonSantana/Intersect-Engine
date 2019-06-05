﻿using System;
using System.Collections.Generic;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.General;
using Intersect.Client.Localization;

namespace Intersect.Client.UI.Game.Hotbar
{
    public class HotBarWindow
    {
        //Controls
        public ImagePanel HotbarWindow;

        //Item List
        public List<HotbarItem> Items = new List<HotbarItem>();

        //Init
        public HotBarWindow(Canvas gameCanvas)
        {
            HotbarWindow = new ImagePanel(gameCanvas, "HotbarWindow");
            HotbarWindow.ShouldCacheToTexture = true;
            InitHotbarItems();
            HotbarWindow.LoadJsonUi(GameContentManager.UI.InGame, GameGraphics.Renderer.GetResolutionString());

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].EquipPanel.Texture == null)
                    Items[i].EquipPanel.Texture = GameGraphics.Renderer.GetWhiteTexture();
            }
        }

        private void InitHotbarItems()
        {
            int x = 12;
            for (int i = 0; i < Options.MaxHotbar; i++)
            {
                Items.Add(new HotbarItem((byte)i, HotbarWindow));
                Items[i].Pnl = new ImagePanel(HotbarWindow, "HotbarContainer" + i);
                Items[i].Setup();
                Items[i].KeyLabel = new Label(Items[i].Pnl, "HotbarLabel" + i);
                Items[i].KeyLabel.SetText(Strings.Keys.keydict[Enum.GetName(typeof(Keys), GameControls.ActiveControls.ControlMapping[Controls.Hotkey1 + i].Key1).ToLower()]);
            }
        }

        public void Update()
        {
            if (Globals.Me == null)
            {
                return;
            }
            for (int i = 0; i < Options.MaxHotbar; i++)
            {
                Items[i].Update();
            }
        }

        public FloatRect RenderBounds()
        {
            FloatRect rect = new FloatRect()
            {
                X = HotbarWindow.LocalPosToCanvas(new Framework.GenericClasses.Point(0, 0)).X,
                Y = HotbarWindow.LocalPosToCanvas(new Framework.GenericClasses.Point(0, 0)).Y,
                Width = HotbarWindow.Width,
                Height = HotbarWindow.Height
            };
            return rect;
        }
    }
}