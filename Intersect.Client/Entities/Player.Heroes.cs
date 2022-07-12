using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.General;
using Intersect.Client.Networking;

namespace Intersect.Client.Entities
{
    public partial class Player
    {
        public bool QuicklyShortcutPressed()
        {
            return Globals.InputManager.KeyDown(Keys.Shift);
        }

        public bool QuicklyDrop(int index)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendDropItem(index, Inventory[index].Quantity);
                return true;
            }

            return false;
        }

        public bool QuicklySell(int index)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendSellItem(index, Inventory[index].Quantity);
                return true;
            }

            return false;
        }

        public bool QuicklyBuy(int index)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendBuyItem(index, 1);
                return true;
            }

            return false;
        }

        public bool QuicklyBankDeposit(int index, int bankSlot)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendDepositItem(index, Inventory[index].Quantity, bankSlot);
                return true;
            }

            return false;
        }

        public bool QuicklyBankWithdraw(int index, int invSlot)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendWithdrawItem(index, Globals.Bank[index].Quantity, invSlot);
                return true;
            }

            return false;
        }

        public bool QuicklyBagDeposit(int index, int bagSlot)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendStoreBagItem(index, Inventory[index].Quantity, bagSlot);
                return true;
            }

            return false;
        }

        public bool QuicklyBagWithdraw(int index, int invSlot)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendRetrieveBagItem(index, Globals.Bag[index].Quantity, invSlot);
                return true;
            }

            return false;
        }

        public bool QuicklyTrade(int index)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendOfferTradeItem(index, Inventory[index].Quantity);
                return true;
            }

            return false;
        }

        public bool QuicklyRevokeTrade(int index)
        {
            if (QuicklyShortcutPressed())
            {
                PacketSender.SendRevokeTradeItem(index, Globals.Trade[0, index].Quantity);
                return true;
            }

            return false;
        }
    }
}
