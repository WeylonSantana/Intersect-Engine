﻿using System;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Graphics;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.GameObjects;

namespace Intersect.Client.UI.Game.Shop
{
    public class ShopWindowItem
    {
        private int mCurrentItem = -2;
        private ItemDescWindow mDescWindow;
        private bool mIsEquipped;

        //Slot info
        private int mMySlot;

        //Drag/Drop References
        private ShopWindow mShopWindow;

        public ImagePanel Container;

        //Mouse Event Variables
        private bool mMouseOver;

        private int mMouseX = -1;
        private int mMouseY = -1;
        public ImagePanel Pnl;

        //Textures
        private GameRenderTexture mSfTex;

        public ShopWindowItem(ShopWindow shopWindow, int index)
        {
            mShopWindow = shopWindow;
            mMySlot = index;
        }

        public void Setup()
        {
            Pnl = new ImagePanel(Container, "ShopItemIcon");
            Pnl.HoverEnter += pnl_HoverEnter;
            Pnl.HoverLeave += pnl_HoverLeave;
            Pnl.RightClicked += Pnl_RightClicked;
            Pnl.DoubleClicked += Pnl_RightClicked; //Allow buying via double click OR right click
        }

        private void Pnl_RightClicked(Base sender, ClickedEventArgs arguments)
        {
            //Confirm the purchase
            var item = ItemBase.Get(Globals.GameShop.SellingItems[mMySlot].ItemId);
            if (item != null)
            {
                if (item.IsStackable())
                {
                    InputBox iBox = new InputBox(Strings.Shop.buyitem,
                        Strings.Shop.buyitemprompt.ToString(item.Name), true, InputBox.InputType.NumericInput,
                        BuyItemInputBoxOkay, null, mMySlot);
                }
                else
                {
                    PacketSender.SendBuyItem(mMySlot, 1);
                }
            }
        }

        public void LoadItem()
        {
            var item = ItemBase.Get(Globals.GameShop.SellingItems[mMySlot].ItemId);
            if (item != null)
            {
                GameTexture itemTex = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Item, item.Icon);
                if (itemTex != null)
                {
                    Pnl.Texture = itemTex;
                }
            }
        }

        private void BuyItemInputBoxOkay(object sender, EventArgs e)
        {
            int value = (int) ((InputBox) sender).Value;
            if (value > 0)
            {
                PacketSender.SendBuyItem((int)((InputBox) sender).UserData, value);
            }
        }

        void pnl_HoverLeave(Base sender, EventArgs arguments)
        {
            mMouseOver = false;
            mMouseX = -1;
            mMouseY = -1;
            if (mDescWindow != null)
            {
                mDescWindow.Dispose();
                mDescWindow = null;
            }
        }

        void pnl_HoverEnter(Base sender, EventArgs arguments)
        {
            if (Globals.InputManager.MouseButtonDown(GameInput.MouseButtons.Left))
            {
                return;
            }
            if (mDescWindow != null)
            {
                mDescWindow.Dispose();
                mDescWindow = null;
            }
            var item = ItemBase.Get(Globals.GameShop.SellingItems[mMySlot].CostItemId);
            if (item != null)
                mDescWindow = new ItemDescWindow(Globals.GameShop.SellingItems[mMySlot].Item, 1, mShopWindow.X, mShopWindow.Y, item.StatsGiven, "", Strings.Shop.costs.ToString(Globals.GameShop.SellingItems[mMySlot].CostItemQuantity, item.Name));
        }

        public FloatRect RenderBounds()
        {
            FloatRect rect = new FloatRect()
            {
                X = Pnl.LocalPosToCanvas(new Point(0, 0)).X,
                Y = Pnl.LocalPosToCanvas(new Point(0, 0)).Y,
                Width = Pnl.Width,
                Height = Pnl.Height
            };
            return rect;
        }

        public void Update()
        {
        }
    }
}