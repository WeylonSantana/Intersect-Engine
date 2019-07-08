﻿using System;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Graphics;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.Framework.Gwen.Input;
using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Items;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.GameObjects;

namespace Intersect.Client.UI.Game.Inventory
{
    public class InventoryItem
    {
        private Guid mCurrentItemId;
        private int mCurrentAmt = 0;
        private ItemDescWindow mDescWindow;

        //Drag/Drop References
        private InventoryWindow mInventoryWindow;

        private bool mIsEquipped;

        //Slot info
        private int mMySlot;

        //Dragging
        private bool mCanDrag;

        private long mClickTime;
        public ImagePanel Container;
        private Draggable mDragIcon;
        public ImagePanel EquipPanel;
        public Label EquipLabel;
        private Label mCooldownLabel;
        public bool IsDragging;
        private bool mIconCd;

        //Mouse Event Variables
        private bool mMouseOver;

        private int mMouseX = -1;
        private int mMouseY = -1;
        public ImagePanel Pnl;
        private string mTexLoaded = "";

        public InventoryItem(InventoryWindow inventoryWindow, int index)
        {
            mInventoryWindow = inventoryWindow;
            mMySlot = index;
        }

        public void Setup()
        {
            Pnl = new ImagePanel(Container, "InventoryItemIcon");
            Pnl.HoverEnter += pnl_HoverEnter;
            Pnl.HoverLeave += pnl_HoverLeave;
            Pnl.RightClicked += pnl_RightClicked;
            Pnl.Clicked += pnl_Clicked;
            EquipPanel = new ImagePanel(Pnl, "InventoryItemEquippedIcon");
            EquipPanel.Texture = GameGraphics.Renderer.GetWhiteTexture();
            EquipLabel = new Label(Pnl, "InventoryItemEquippedLabel");
            EquipLabel.IsHidden = true;
            EquipLabel.Text = Strings.Inventory.equippedicon;
            EquipLabel.TextColor = new Color(0,255,255,255);
            mCooldownLabel = new Label(Pnl, "InventoryItemCooldownLabel");
            mCooldownLabel.IsHidden = true;
            mCooldownLabel.TextColor = new Color(0, 255, 255, 255);
        }

        void pnl_Clicked(Base sender, ClickedEventArgs arguments)
        {
            mClickTime = Globals.System.GetTimeMs() + 500;
        }

        void pnl_RightClicked(Base sender, ClickedEventArgs arguments)
        {
            if (Globals.GameShop != null)
            {
                Globals.Me.TrySellItem(mMySlot);
            }
            else if (Globals.InBank)
            {
                Globals.Me.TryDepositItem(mMySlot);
            }
            else if (Globals.InBag)
            {
                Globals.Me.TryStoreBagItem(mMySlot);
            }
            else if (Globals.InTrade)
            {
                Globals.Me.TryTradeItem(mMySlot);
            }
            else
            {
                Globals.Me.TryDropItem(mMySlot);
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
            mMouseOver = true;
            mCanDrag = true;
            if (Globals.InputManager.MouseButtonDown(GameInput.MouseButtons.Left))
            {
                mCanDrag = false;
                return;
            }
            if (mDescWindow != null)
            {
                mDescWindow.Dispose();
                mDescWindow = null;
            }
            if (Globals.GameShop == null)
            {
                mDescWindow = new ItemDescWindow(Globals.Me.Inventory[mMySlot].Item, Globals.Me.Inventory[mMySlot].Quantity, mInventoryWindow.X, mInventoryWindow.Y, Globals.Me.Inventory[mMySlot].StatBuffs);
            }
            else
            {
                ItemInstance invItem = Globals.Me.Inventory[mMySlot];
                ShopItem shopItem = null;
                for (int i = 0; i < Globals.GameShop.BuyingItems.Count; i++)
                {
                    var tmpShop = Globals.GameShop.BuyingItems[i];

                    if (invItem.ItemId == tmpShop.ItemId)
                    {
                        shopItem = tmpShop;
                        break;
                    }
                }

                if (Globals.GameShop.BuyingWhitelist && shopItem != null)
                {
                    var hoveredItem = ItemBase.Get(shopItem.CostItemId);
                    if (hoveredItem != null)
                    {
                        mDescWindow = new ItemDescWindow(Globals.Me.Inventory[mMySlot].Item, Globals.Me.Inventory[mMySlot].Quantity, mInventoryWindow.X, mInventoryWindow.Y, Globals.Me.Inventory[mMySlot].StatBuffs, "", Strings.Shop.sellsfor.ToString( shopItem.CostItemQuantity, hoveredItem.Name));
                    }
                }
                else if (shopItem == null)
                {
                    var hoveredItem = ItemBase.Get(invItem.ItemId);
                    var costItem = Globals.GameShop.DefaultCurrency;
                    if (hoveredItem != null && costItem != null)
                    {
                        mDescWindow = new ItemDescWindow(Globals.Me.Inventory[mMySlot].Item, Globals.Me.Inventory[mMySlot].Quantity, mInventoryWindow.X, mInventoryWindow.Y, Globals.Me.Inventory[mMySlot].StatBuffs, "", Strings.Shop.sellsfor.ToString( hoveredItem.Price, costItem.Name));
                    }
                }
                else
                {
                    mDescWindow = new ItemDescWindow(invItem.Item, invItem.Quantity, mInventoryWindow.X, mInventoryWindow.Y, invItem.StatBuffs, "", Strings.Shop.wontbuy);
                }
            }
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
            bool equipped = false;
            for (int i = 0; i < Options.EquipmentSlots.Count; i++)
            {
                if (Globals.Me.MyEquipment[i] == mMySlot)
                {
                    equipped = true;
	                break;
                }
            }
            var item = ItemBase.Get(Globals.Me.Inventory[mMySlot].ItemId);
            if (Globals.Me.Inventory[mMySlot].ItemId != mCurrentItemId || Globals.Me.Inventory[mMySlot].Quantity != mCurrentAmt || equipped != mIsEquipped ||
                (item == null && mTexLoaded != "") || (item != null && mTexLoaded != item.Icon) || mIconCd != Globals.Me.ItemOnCd(mMySlot) || Globals.Me.ItemOnCd(mMySlot))
            {
                mCurrentItemId = Globals.Me.Inventory[mMySlot].ItemId;
                mCurrentAmt = Globals.Me.Inventory[mMySlot].Quantity;
                mIsEquipped = equipped;
                EquipPanel.IsHidden = !mIsEquipped;
                EquipLabel.IsHidden = !mIsEquipped;
                mCooldownLabel.IsHidden = true;
                if (item != null)
                {
                    GameTexture itemTex = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Item,
                        item.Icon);
                    if (itemTex != null)
                    {
                        Pnl.Texture = itemTex;
                        if (Globals.Me.ItemOnCd(mMySlot))
                        {
                            Pnl.RenderColor = new Color(100, 255, 255, 255);
                        }
                        else
                        {
                            Pnl.RenderColor = new Color(255, 255, 255, 255);
                        }
                    }
                    else
                    {
                        if (Pnl.Texture != null)
                        {
                            Pnl.Texture = null;
                        }
                    }
                    mTexLoaded = item.Icon;
                    mIconCd = Globals.Me.ItemOnCd(mMySlot);
                    if (mIconCd)
                    {
                        mCooldownLabel.IsHidden = false;
                        var secondsRemaining = (float)(Globals.Me.ItemCdRemainder(mMySlot)) / 1000f;
                        if (secondsRemaining > 10f)
                        {
                            mCooldownLabel.Text = Strings.Inventory.cooldown.ToString((secondsRemaining).ToString("N0"));
                        }
                        else
                        {
                            mCooldownLabel.Text = Strings.Inventory.cooldown.ToString((secondsRemaining).ToString("N1").Replace(".", Strings.Numbers.dec));
                        }
                    }
                }
                else
                {
                    if (Pnl.Texture != null)
                    {
                        Pnl.Texture = null;
                    }
                    mTexLoaded = "";
                }
                if (mDescWindow != null)
                {
                    mDescWindow.Dispose();
                    mDescWindow = null;
                    pnl_HoverEnter(null, null);
                }
            }
            if (!IsDragging)
            {
                if (mMouseOver)
                {
                    if (!Globals.InputManager.MouseButtonDown(GameInput.MouseButtons.Left))
                    {
                        mCanDrag = true;
                        mMouseX = -1;
                        mMouseY = -1;
                        if (Globals.System.GetTimeMs() < mClickTime)
                        {
                            Globals.Me.TryUseItem(mMySlot);
                            mClickTime = 0;
                        }
                    }
                    else
                    {
                        if (mCanDrag)
                        {
                            if (mMouseX == -1 || mMouseY == -1)
                            {
                                mMouseX = InputHandler.MousePosition.X -
                                         Pnl.LocalPosToCanvas(new Point(0, 0)).X;
                                mMouseY = InputHandler.MousePosition.Y -
                                         Pnl.LocalPosToCanvas(new Point(0, 0)).Y;
                            }
                            else
                            {
                                int xdiff = mMouseX -
                                            (InputHandler.MousePosition.X -
                                             Pnl.LocalPosToCanvas(new Point(0, 0))
                                                 .X);
                                int ydiff = mMouseY -
                                            (InputHandler.MousePosition.Y -
                                             Pnl.LocalPosToCanvas(new Point(0, 0))
                                                 .Y);
                                if (Math.Sqrt(Math.Pow(xdiff, 2) + Math.Pow(ydiff, 2)) > 5)
                                {
                                    IsDragging = true;
                                    mDragIcon = new Draggable(
                                        Pnl.LocalPosToCanvas(new Point(0, 0)).X +
                                        mMouseX,
                                        Pnl.LocalPosToCanvas(new Point(0, 0)).X +
                                        mMouseY, Pnl.Texture);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (mDragIcon.Update())
                {
                    //Drug the item and now we stopped
                    IsDragging = false;
                    FloatRect dragRect = new FloatRect(
                        mDragIcon.X - (Container.Padding.Left + Container.Padding.Right) / 2,
                        mDragIcon.Y - (Container.Padding.Top + Container.Padding.Bottom) / 2,
                        (Container.Padding.Left + Container.Padding.Right) / 2 + Pnl.Width,
                        (Container.Padding.Top + Container.Padding.Bottom) / 2 + Pnl.Height);

                    float bestIntersect = 0;
                    int bestIntersectIndex = -1;
                    //So we picked up an item and then dropped it. Lets see where we dropped it to.
                    //Check inventory first.
                    if (mInventoryWindow.RenderBounds().IntersectsWith(dragRect))
                    {
                        for (int i = 0; i < Options.MaxInvItems; i++)
                        {
                            if (mInventoryWindow.Items[i].RenderBounds().IntersectsWith(dragRect))
                            {
                                if (FloatRect.Intersect(mInventoryWindow.Items[i].RenderBounds(), dragRect).Width *
                                    FloatRect.Intersect(mInventoryWindow.Items[i].RenderBounds(), dragRect).Height >
                                    bestIntersect)
                                {
                                    bestIntersect =
                                        FloatRect.Intersect(mInventoryWindow.Items[i].RenderBounds(), dragRect).Width *
                                        FloatRect.Intersect(mInventoryWindow.Items[i].RenderBounds(), dragRect).Height;
                                    bestIntersectIndex = i;
                                }
                            }
                        }
                        if (bestIntersectIndex > -1)
                        {
                            if (mMySlot != bestIntersectIndex)
                            {
                                //Try to swap....
                                PacketSender.SendSwapInvItems(bestIntersectIndex, mMySlot);
                                Globals.Me.SwapItems(bestIntersectIndex, mMySlot);
                            }
                        }
                    }
                    else if (Gui.GameUi.Hotbar.RenderBounds().IntersectsWith(dragRect))
                    {
                        for (int i = 0; i < Options.MaxHotbar; i++)
                        {
                            if (Gui.GameUi.Hotbar.Items[i].RenderBounds().IntersectsWith(dragRect))
                            {
                                if (FloatRect.Intersect(Gui.GameUi.Hotbar.Items[i].RenderBounds(), dragRect).Width *
                                    FloatRect.Intersect(Gui.GameUi.Hotbar.Items[i].RenderBounds(), dragRect).Height >
                                    bestIntersect)
                                {
                                    bestIntersect =
                                        FloatRect.Intersect(Gui.GameUi.Hotbar.Items[i].RenderBounds(), dragRect).Width *
                                        FloatRect.Intersect(Gui.GameUi.Hotbar.Items[i].RenderBounds(), dragRect).Height;
                                    bestIntersectIndex = i;
                                }
                            }
                        }
                        if (bestIntersectIndex > -1)
                        {
                            Globals.Me.AddToHotbar((byte)bestIntersectIndex, 0, mMySlot);
                        }
                    }

                    mDragIcon.Dispose();
                }
            }
        }
    }
}