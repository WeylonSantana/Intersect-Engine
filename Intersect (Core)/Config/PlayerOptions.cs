﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Intersect.Config
{

    public class PlayerOptions
    {

        /// <summary>
        /// A percentage between 0 and 100 which determines the chance in which they will lose any given item in their inventory when killed.
        /// </summary>
        public int ItemDropChance = 0;

        /// <summary>
        /// Number of bank slots a player has.
        /// </summary>
        public int MaxBank = 100;

        /// <summary>
        /// Number of characters an account may create.
        /// </summary>
        public int MaxCharacters = 1;

        /// <summary>
        /// Number of inventory slots a player has.
        /// </summary>
        public int MaxInventory = 35;

        /// <summary>
        /// Max level a player can achieve.
        /// </summary>
        public int MaxLevel = 100;

        /// <summary>
        /// Number of spell slots a player has.
        /// </summary>
        public int MaxSpells = 35;

        /// <summary>
        /// The highest value a single stat can be for a player.
        /// </summary>
        public int MaxStat = 255;

        /// <summary>
        /// How long a player must wait before sending a trade/party/friend request after the first as been denied.
        /// </summary>
        public int RequestTimeout = 300000;

        /// <summary>
        /// Distance (in tiles) between players in which a trade offer can be sent and accepted.
        /// </summary>
        public int TradeRange = 6;

        /// <summary>
        /// Unlinks the timers for combat and movement to facilitate complex combat (e.g. kiting)
        /// </summary>
        public bool AllowCombatMovement = true;

        /// <summary>
        /// Configures whether or not the level of a player is shown next to their name.
        /// </summary>
        public bool ShowLevelByName = false;

        /// <summary>
        /// First thanks for letting me share an idea, of losing XP when dying <3
        /// </summary>
        public int ExpLossOnDeathPercent = 0;

        /// <summary>
        /// If true, it will remove the associated exp, otherwise you will lose the exp based on the exp required to level up.
        /// </summary>
        public bool ExpLossFromCurrentExp = true;

        /// <summary>
        /// The amount to divide strength by when determining its HP bonus 
        /// </summary>
        public int AttackHealthDivider = 5;

        /// <summary>
        /// The amount to divide AP by when determining its HP bonus 
        /// </summary>
        public int AbilityPowerManaDivider = 5;

        public float AmmoRetrieveChance = 30.0f;

        /// <summary>
        /// Contains the slots used during character creation - instead of relating these with equipment.
        /// </summary>
        public List<string> DecorSlots = new List<string>()
        {
            "Hair",
            "Eyes",
            "Shirt",
            "Extra",
            "Beard",
        };

        public int HairSlot = 0;

        public int BeardSlot = 4;

        public int ExtraSlot = 3;

        public string ClassRankVarGuid = "df280bcf-f0f4-448a-9430-45d1022b9aa8";

        public string TasksCompletedVarGuid = "cd6f7e2b-7f2e-477b-9425-70c66d2d5dd6";
        
        public string TasksRemainingVarGuid = "5b18da07-f676-44f5-9050-3a66f1ed0a23";

        public string SpecialAssignmentAvailableGuid = "21267b3b-1c6b-434a-ac87-dd77724fe70a";
        
        public string OnSpecialAssignmentVarGuid = "a1c38f8f-919f-4dda-94cf-88551abbbc81";
        
        public string OnTaskVarGuid = "8cd211a9-b27e-460d-b82a-cf620dfe63a3";

        public string InGuildVarGuid = "ed1040cb-7018-4862-b0b5-1f9f081bb562";
        
        public string TaskCompletedVarGuid = "546e9490-f152-4318-9cc6-2c26555fea78";

        [OnDeserializing]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            DecorSlots.Clear();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            DecorSlots = new List<string>(DecorSlots.Distinct());
        }
    }

}
