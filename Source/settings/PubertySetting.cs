using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace HumanlikeLifeStages
{
    [Serializable]
    public class RacePubertySetting : IExposable
    {
        public StringWrapper appliedTo;
        public BoolWrapper pubertySetting;
        public List<PubertySetting> list;

        public void ExposeData()
        {
            Scribe_Deep.Look(ref this.appliedTo, "appliedTo");
            Scribe_Deep.Look(ref this.pubertySetting, "pubertySetting");
            Scribe_Collections.Look(ref this.list, "appliedToList");
        }
    }

    [Serializable]
    public class PubertySetting : IExposable
    {
        public StringWrapper which, label, description;
        public IntWrapper genderRoleIndex, secondaryGenderRoleIndex;

        public PubertySetting()
        {
        }

        public PubertySetting(HediffDef which, int genderRoleIndex, int secondaryGenderRoleIndex)
        {
            this.which = which.defName;
            this.description = which.description;
            this.label = which.label;
            this.genderRoleIndex = genderRoleIndex;
            this.secondaryGenderRoleIndex = secondaryGenderRoleIndex;
        }

        public void ExposeData()
        {
            Scribe_Deep.Look(ref this.which, "which");
            Scribe_Deep.Look(ref this.description, "description");
            Scribe_Deep.Look(ref this.label, "label");
            Scribe_Deep.Look(ref this.genderRoleIndex, "genderRoleIndex");
            Scribe_Deep.Look(ref this.secondaryGenderRoleIndex, "secondaryGenderRoleIndex");
        }

        public void WidgetDraw(Rect rect)
        {
            var rightPart = rect.RightPart(.85f);
            if (description?.value.NullOrEmpty() ?? true)
                Widgets.Label(rightPart, label);
            else
                Widgets.Label(rightPart, label + " - " + description);


            var texture2Ds = buttonIcons();
            var organ = Widgets.ButtonImage(rect.LeftPart(.06f), texture2Ds[genderRoleIndex]);

            if (organ)
            {
                var last = genderRoleIndex;
                genderRoleIndex++;
                genderRoleIndex %= texture2Ds.Count;

                Log.Message("Clicked! - " + which + " from [" + last + "] to [" + genderRoleIndex + "]");
            }

            var secondary =
                Widgets.ButtonImage(rect.LeftPart(.12f).RightPart(0.5f), texture2Ds[secondaryGenderRoleIndex]);

            if (secondary)
            {
                var last = secondaryGenderRoleIndex;
                secondaryGenderRoleIndex++;
                secondaryGenderRoleIndex %= texture2Ds.Count;

                Log.Message("Clicked! - " + which + " from [" + last + "] to [" + secondaryGenderRoleIndex + "]");
            }
        }

        public bool IsAssigned()
        {
            return genderRoleIndex != 0;
        }

        public bool IsAll()
        {
            return genderRoleIndex == 4;
        }

        public Gender GetGender()
        {
            switch (genderRoleIndex)
            {
                case 1:
                    return Gender.Male;
                case 2:
                    return Gender.Female;
            }

            return Gender.None;
        }
        
        
        public bool IsSecondaryAssigned()
        {
            return genderRoleIndex != 0;
        }

        public bool IsSecondaryAll()
        {
            return genderRoleIndex == 4;
        }

        public Gender GetSecondaryGender()
        {
            switch (genderRoleIndex)
            {
                case 1:
                    return Gender.Male;
                case 2:
                    return Gender.Female;
            }

            return Gender.None;
        }

        public static Texture2D off() => Widgets.CheckboxOffTex;

        public static Texture2D other() => GenderUtility.GetIcon(Gender.None);

        public static Texture2D male() => GenderUtility.GetIcon(Gender.Male);

        public static Texture2D female() => GenderUtility.GetIcon(Gender.Female);

        public static Texture2D all() => Widgets.CheckboxOnTex;

        private static List<Texture2D> buttonIcons() => new List<Texture2D>()
        {
            off(), male(), female(), other(), all()
        };
    }

    public class StringWrapper : IExposable
    {
        public string value;

        public void ExposeData() => Scribe_Values.Look(value: ref this.value, label: "stringWrapper");

        public static implicit operator string(StringWrapper sw) => sw.value;
        public static implicit operator StringWrapper(string s) => new StringWrapper() {value = s};
    }

    public class IntWrapper : IExposable
    {
        public int value;

        public void ExposeData() => Scribe_Values.Look(value: ref this.value, label: "intWrapper");

        public static implicit operator int(IntWrapper sw) => sw.value;
        public static implicit operator IntWrapper(int s) => new IntWrapper() {value = s};
    }
    public class BoolWrapper : IExposable
    {
        public bool value;

        public void ExposeData() => Scribe_Values.Look(value: ref this.value, label: "boolWrapper");

        
        public static implicit operator bool(BoolWrapper sw) => sw.value;
        public static implicit operator BoolWrapper(bool s) => new BoolWrapper() {value = s};
        
        public static implicit operator int(BoolWrapper sw) => sw.value?1:0;
        public static implicit operator BoolWrapper(int s) => new BoolWrapper() {value = s>0};
        
        public static implicit operator string(BoolWrapper sw) => sw.value.ToStringSafe();
        public static implicit operator BoolWrapper(string s) => new BoolWrapper() {value = Convert.ToBoolean(s)};
    }
    
}