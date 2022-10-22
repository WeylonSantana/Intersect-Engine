using Intersect.Models;
using Intersect.Server.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intersect.GameObjects
{
    public partial class ProfessionBase : DatabaseObject<ProfessionBase>, IFolderable
    {
        public const long DEFAULT_BASE_EXPERIENCE = 100;

        public const long DEFAULT_EXPERIENCE_INCREASE = 50;

        [JsonIgnore] public int mMaxLevel;

        [JsonIgnore] private long mBaseExp;

        [JsonIgnore] private long mExpIncrease;

        public string Description { get; set; }

        public string Icon { get; set; }

        [JsonIgnore]
        [NotMapped]
        public ExperienceCurve ExperienceCurve { get; }

        [NotMapped] public Dictionary<int, long> ExperienceOverrides = new Dictionary<int, long>();

        [JsonConstructor]
        public ProfessionBase(Guid id) : base(id)
        {
            Name = "New Profession";

            ExperienceCurve = new ExperienceCurve();
            ExperienceCurve.Calculate(1);
            BaseExp = DEFAULT_BASE_EXPERIENCE;
            ExpIncrease = DEFAULT_EXPERIENCE_INCREASE;
        }

        //Parameterless constructor for EF
        public ProfessionBase()
        {
            Name = "New Profession";

            ExperienceCurve = new ExperienceCurve();
            ExperienceCurve.Calculate(1);
            BaseExp = DEFAULT_BASE_EXPERIENCE;
            ExpIncrease = DEFAULT_EXPERIENCE_INCREASE;
        }

        public long MaxLevel
        {
            get => mMaxLevel;
            set
            {
                mMaxLevel = (int)Math.Max(1, value);
            }
        }

        public long BaseExp
        {
            get => mBaseExp;
            set
            {
                mBaseExp = Math.Max(0, value);
                ExperienceCurve.BaseExperience = Math.Max(1, mBaseExp);
            }
        }

        public long ExpIncrease
        {
            get => mExpIncrease;
            set
            {
                mExpIncrease = Math.Max(0, value);
                ExperienceCurve.Gain = 1 + value / 100.0;
            }
        }

        [Column("ExperienceOverrides")]
        public string ExpOverridesJson
        {
            get => JsonConvert.SerializeObject(ExperienceOverrides);
            set
            {
                ExperienceOverrides = JsonConvert.DeserializeObject<Dictionary<int, long>>(value ?? "");
                if (ExperienceOverrides == null)
                {
                    ExperienceOverrides = new Dictionary<int, long>();
                }
            }
        }

        public long ExperienceToNextLevel(int level)
        {
            if (ExperienceOverrides.ContainsKey(level))
            {
                return ExperienceOverrides[level];
            }

            return ExperienceCurve.Calculate(level);
        }

        /// <inheritdoc />
        public string Folder { get; set; } = "";
    }
}
