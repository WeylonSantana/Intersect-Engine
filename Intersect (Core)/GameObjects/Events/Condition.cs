﻿using System;

using Intersect.Enums;

namespace Intersect.GameObjects.Events
{

    public enum ConditionTypes
    {

        VariableIs = 0,

        HasItem = 4,

        ClassIs,

        KnowsSpell,

        LevelOrStat,

        SelfSwitch, //Only works for events.. not for checking if you can destroy a resource or something like that

        AccessIs,

        TimeBetween,

        CanStartQuest,

        QuestInProgress,

        QuestCompleted,

        NoNpcsOnMap,

        GenderIs,

        MapIs,

        IsItemEquipped,

        HasFreeInventorySlots,

        InGuildWithRank,

        MapZoneTypeIs,

        HasItemWithTag,

        ItemEquippedWithTag,

        EquipmentInSlot,

        InVehicle,

        InPartyWith,

        InNpcGuildWithRank,

        HasSpecialAssignmentForClass,

        IsOnGuildTaskForClass,

        HasTaskCompletedForClass,

        TaskIsOnCooldownForClass,

        HighestClassRankIs,

    }

    public class Condition
    {

        public virtual ConditionTypes Type { get; }

        public bool Negated { get; set; }

        /// <summary>
        /// Configures whether or not this condition does or does not have an else branch.
        /// </summary>
        public bool ElseEnabled { get; set; } = true;

    }

    public class VariableIsCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.VariableIs;

        public VariableTypes VariableType { get; set; } = VariableTypes.PlayerVariable;

        public Guid VariableId { get; set; }

        public VariableCompaison Comparison { get; set; } = new VariableCompaison();

    }

    public class HasItemCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.HasItem;

        public Guid ItemId { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// Defines whether this event command will use a variable for processing or not.
        /// </summary>
        public bool UseVariable { get; set; } = false;

        /// <summary>
        /// Defines whether the variable used is a Player or Global variable.
        /// </summary>
        public VariableTypes VariableType { get; set; } = VariableTypes.PlayerVariable;

        /// <summary>
        /// The Variable Id to use.
        /// </summary>
        public Guid VariableId { get; set; }

        public bool CheckBank { get; set; }

    }

    public class ClassIsCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.ClassIs;

        public Guid ClassId { get; set; }

    }

    public class KnowsSpellCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.KnowsSpell;

        public Guid SpellId { get; set; }

    }

    public class LevelOrStatCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.LevelOrStat;

        public bool ComparingLevel { get; set; }

        public Stats Stat { get; set; }

        public VariableComparators Comparator { get; set; } = VariableComparators.Equal;

        public int Value { get; set; }

        public bool IgnoreBuffs { get; set; }

    }

    public class SelfSwitchCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.SelfSwitch;

        public int SwitchIndex { get; set; } //0 through 3

        public bool Value { get; set; }

    }

    public class AccessIsCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.AccessIs;

        public Access Access { get; set; }

    }

    public class TimeBetweenCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.TimeBetween;

        public int[] Ranges { get; set; } = new int[2];

    }

    public class CanStartQuestCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.CanStartQuest;

        public Guid QuestId { get; set; }

    }

    public class QuestInProgressCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.QuestInProgress;

        public Guid QuestId { get; set; }

        public QuestProgressState Progress { get; set; } = QuestProgressState.OnAnyTask;

        public Guid TaskId { get; set; }

    }

    public class QuestCompletedCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.QuestCompleted;

        public Guid QuestId { get; set; }

    }

    public class NoNpcsOnMapCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.NoNpcsOnMap;

    }

    public class GenderIsCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.GenderIs;

        public Gender Gender { get; set; } = Gender.Male;

    }

    public class MapIsCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.MapIs;

        public Guid MapId { get; set; }

    }

    public class IsItemEquippedCondition : Condition
    {

        public override ConditionTypes Type { get; } = ConditionTypes.IsItemEquipped;

        public Guid ItemId { get; set; }

    }

    /// <summary>
    /// Defines the condition class used when checking for a player's free inventory slots.
    /// </summary>
    public class HasFreeInventorySlots : Condition
    {
        /// <summary>
        /// Defines the type of condition.
        /// </summary>
        public override ConditionTypes Type { get; } = ConditionTypes.HasFreeInventorySlots;

        /// <summary>
        /// Defines the amount of inventory slots that need to be free to clear this condition.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Defines whether this event command will use a variable for processing or not.
        /// </summary>
        public bool UseVariable { get; set; } = false;

        /// <summary>
        /// Defines whether the variable used is a Player or Global variable.
        /// </summary>
        public VariableTypes VariableType { get; set; } = VariableTypes.PlayerVariable;

        /// <summary>
        /// The Variable Id to use.
        /// </summary>
        public Guid VariableId { get; set; }

    }

    /// <summary>
    /// Defines the condition class used when checking whether a player is in a guild with at least a specified rank
    /// </summary>
    public class InGuildWithRank : Condition
    {
        /// <summary>
        /// Defines the type of condition
        /// </summary>
        public override ConditionTypes Type { get; } = ConditionTypes.InGuildWithRank;

        /// <summary>
        /// The guild rank the condition checks for as a minimum
        /// </summary>
        public int Rank { get; set; }
    }

    /// <summary>
    /// Defines the condition class used when checking whether a player is on a specific map zone type.
    /// </summary>
    public class MapZoneTypeIs : Condition
    {
        /// <summary>
        /// Defines the type of condition.
        /// </summary>
        public override ConditionTypes Type { get; } = ConditionTypes.MapZoneTypeIs;

        /// <summary>
        /// Defines the map Zone Type to compare to.
        /// </summary>
        public MapZones ZoneType { get; set; }
    }

    /// <summary>
    /// Defines the condition class used when checking whether a player has some item in their inventory with a given tag
    /// </summary>
    public class InventoryTagCondition : Condition
    {
        /// <summary>
        /// Defines the type of condition.
        /// </summary>
        public override ConditionTypes Type { get; } = ConditionTypes.HasItemWithTag;

        /// <summary>
        /// Defines the tag to check for.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Defines whether or not to include bank in the search.
        /// </summary>
        public bool IncludeBank { get; set; }
    }

    /// <summary>
    /// Defines the condition class used when checking whether a player has some item equipped with a given tag
    /// </summary>
    public class EquipmentTagCondition : Condition
    {
        /// <summary>
        /// Defines the type of condition.
        /// </summary>
        public override ConditionTypes Type { get; } = ConditionTypes.HasItemWithTag;

        /// <summary>
        /// Defines the tag to check for.
        /// </summary>
        public string Tag { get; set; }
    }

    public class EquipmentInSlotCondition : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.EquipmentInSlot;

        public int slot { get; set; }
    }

    public class InVehicleCondition : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.InVehicle;
    }

    public class InPartyWithCondition : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.InPartyWith;

        public int Members { get; set; }
    }

    public class InNpcGuildWithRankCondition : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.InNpcGuildWithRank;

        public Guid ClassId { get; set; }

        public int ClassRank { get; set; }
    }

    public class HasSpecialAssignmentForClassCondition : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.HasSpecialAssignmentForClass;

        public Guid ClassId { get; set; }
    }

    public class IsOnGuildTaskForClassCondition : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.IsOnGuildTaskForClass;

        public Guid ClassId { get; set; }
    }

    public class HasTaskCompletedForClassCondition : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.HasTaskCompletedForClass;

        public Guid ClassId { get; set; }
    }

    public class TaskIsOnCooldownForClassCondition : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.TaskIsOnCooldownForClass;

        public Guid ClassId { get; set; }
    }

    public class HighestClassRankIs : Condition
    {
        public override ConditionTypes Type { get; } = ConditionTypes.HighestClassRankIs;

        public int ClassRank { get; set; }
    }

    public class VariableCompaison
    {

    }

    public class BooleanVariableComparison : VariableCompaison
    {

        public VariableTypes CompareVariableType { get; set; } = VariableTypes.PlayerVariable;

        public Guid CompareVariableId { get; set; }

        public bool ComparingEqual { get; set; }

        public bool Value { get; set; }

    }

    public class IntegerVariableComparison : VariableCompaison
    {

        public VariableComparators Comparator { get; set; } = VariableComparators.Equal;

        public VariableTypes CompareVariableType { get; set; } = VariableTypes.PlayerVariable;

        public Guid CompareVariableId { get; set; }

        public long Value { get; set; }

    }

    public class StringVariableComparison : VariableCompaison
    {

        public StringVariableComparators Comparator { get; set; } = StringVariableComparators.Equal;

        public string Value { get; set; }

    }

}
