using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace HumanlikeLifeStages
{
    public static class PubertySettingPage
    {
        public static HediffDef Which(this PubertySetting that) =>
            DefDatabase<HediffDef>.GetNamedSilentFail(that.which);

        public static BodyPartDef Where(this PubertySetting that)
        {
            if (that.@where != null)
                switch (that.@where)
                {
                    case 1:
                        return BodyPartDefOf.Groin;
                    case 2:
                        return BodyPartDefOf.LifeStages_ReproductiveOrgans;
                    case 3:
                        return BodyPartDefOf.Chest;
                }
            else that.@where = 0;
            
            return null;
        }
        
        public static void WidgetDraw(this PubertySetting that, Rect rect)
        {
            var rightPart = rect.RightPart(.85f);
            if (that.description?.value.NullOrEmpty() ?? true)
                Widgets.Label(rightPart, that.label);
            else
                Widgets.Label(rightPart, that.label + " - " + that.description);

            var splits = rect.SplitX(14).ToArray();
            var texture2Ds = buttonIcons();
            
            
            var organ = Widgets.ButtonImage(splits[0].ContractedBy(2f), texture2Ds[that.genderRoleIndex]);

            if (organ)
            {
                that.genderRoleIndex++;
                that.genderRoleIndex %= texture2Ds.Count;
            }

            var secondary =
                Widgets.ButtonImage(splits[1].ContractedBy(2f), texture2Ds[that.secondaryGenderRoleIndex]);

            if (secondary)
            {
                that.secondaryGenderRoleIndex++;
                that.secondaryGenderRoleIndex %= texture2Ds.Count;
            }


            var clicked = Widgets.ButtonText(splits[2].ContractedBy(2f), that.Where()?.label ?? "Skin");
            if (clicked)
            {
                that.where++;
                that.where %= 4;
            }

        }


        private static Texture2D off() => Widgets.CheckboxOffTex;

        private static Texture2D other() => GenderUtility.GetIcon(Gender.None);

        private static Texture2D male() => GenderUtility.GetIcon(Gender.Male);

        private static Texture2D female() => GenderUtility.GetIcon(Gender.Female);

        private static Texture2D all() => Widgets.CheckboxOnTex;

        private static List<Texture2D> buttonIcons() => new List<Texture2D>()
        {
            off(), male(), female(), other(), all()
        };
    }
}