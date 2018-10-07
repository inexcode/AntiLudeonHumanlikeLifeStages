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
            if (that.where != null)
                switch (that.where)
                {
                    case 1:
                        return BodyPartDefOf.Groin;
                    case 2:
                        return BodyPartDefOf.LifeStages_ReproductiveOrgans;
                    case 3:
                        return BodyPartDefOf.Chest;
                }
            else that.where = 0;
            
            return null;
        }
        
        
        private static string WhereLabel(PubertySetting that)
        {
            var loc = that.Where();
            if (loc == BodyPartDefOf.LifeStages_ReproductiveOrgans)
                return "Inside";
            return loc?.label?.CapitalizeFirst() ?? "Skin";
        }
        
        public static void WidgetDraw(this PubertySetting that, Rect rect)
        {
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


            var clicked = Widgets.ButtonText(splits[2].ContractedBy(2f), WhereLabel(that));
            if (clicked)
            {
                that.where++;
                that.where %= 4;
            }

            var rightPart = new Rect(splits[3].x, rect.y, rect.width-(splits[3].x-rect.x) ,rect.height);
            if (that.description?.value.NullOrEmpty() ?? true)
                Widgets.Label(rightPart, that.label);
            else
                Widgets.Label(rightPart, that.label + " - " + that.description);
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