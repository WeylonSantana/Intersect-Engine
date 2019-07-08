﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Models;
using Newtonsoft.Json;

namespace Intersect.GameObjects
{
    public class ShopBase : DatabaseObject<ShopBase>, IFolderable
    {
        public bool BuyingWhitelist { get; set; } = true;

        //Spawn Info
        [Column("DefaultCurrency")]
        [JsonProperty]
        public Guid DefaultCurrencyId { get; set; }
        [NotMapped]
        [JsonIgnore]
        public ItemBase DefaultCurrency
        {
            get => ItemBase.Get(DefaultCurrencyId);
            set => DefaultCurrencyId = value?.Id ?? Guid.Empty;
        }

        [Column("BuyingItems")]
        [JsonIgnore]
        public string JsonBuyingItems
        {
            get => JsonConvert.SerializeObject(BuyingItems);
            set => BuyingItems = JsonConvert.DeserializeObject<List<ShopItem>>(value);
        }
        [NotMapped]
        public List<ShopItem> BuyingItems = new List<ShopItem>();

        [Column("SellingItems")]
        [JsonIgnore]
        public string JsonSellingItems
        {
            get => JsonConvert.SerializeObject(SellingItems);
            set => SellingItems = JsonConvert.DeserializeObject<List<ShopItem>>(value);
        }
        [NotMapped]
        public List<ShopItem> SellingItems = new List<ShopItem>();

        /// <inheritdoc />
        public string Folder { get; set; } = "";

        [JsonConstructor]
        public ShopBase(Guid id) : base(id)
        {
            Name = "New Shop";
        }

        //EF is so damn picky about its parameters
        public ShopBase()
        {
            Name = "New Shop";
        }
    }

    public class ShopItem
    {
        public Guid CostItemId;
        public int CostItemQuantity;
        public Guid ItemId;

        [NotMapped]
        public ItemBase Item => ItemBase.Get(ItemId);

        [JsonConstructor]
        public ShopItem(Guid itemId, Guid costItemId, int costVal)
        {
            ItemId = itemId;
            CostItemId = costItemId;
            CostItemQuantity = costVal;
        }
    }
}