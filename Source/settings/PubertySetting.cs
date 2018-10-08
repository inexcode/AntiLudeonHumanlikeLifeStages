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
        public BoolWrapper instantPubertySetting;
        public List<PubertySetting> list;

        public void ExposeData()
        {
            Scribe_Deep.Look(ref this.appliedTo, "appliedTo");
            Scribe_Deep.Look(ref this.instantPubertySetting, "instantPubertySetting");
            Scribe_Collections.Look(ref this.list, "appliedToList");
        }
    }

    [Serializable]
    public class PubertySetting : IExposable
    {
        public StringWrapper which, label, description;
        public IntWrapper genderRoleIndex, secondaryGenderRoleIndex, where;

        public PubertySetting()
        {
        }

        public PubertySetting(HediffDef which, int genderRoleIndex, int secondaryGenderRoleIndex, int where)
        {
            this.which = which.defName;
            this.description = which.description;
            this.label = which.label;
            this.genderRoleIndex = genderRoleIndex;
            this.secondaryGenderRoleIndex = secondaryGenderRoleIndex;
            this.where = where;

        }

        public void ExposeData()
        {
            Scribe_Deep.Look(ref this.which, "which");
            Scribe_Deep.Look(ref this.description, "description");
            Scribe_Deep.Look(ref this.label, "label");
            Scribe_Deep.Look(ref this.genderRoleIndex, "genderRoleIndex");
            Scribe_Deep.Look(ref this.secondaryGenderRoleIndex, "secondaryGenderRoleIndex");
            Scribe_Deep.Look(ref this.where, "where");
            
        }

        public bool IsAssigned()
        {
            return genderRoleIndex != BodyPartsByRace.Off;
        }

        public bool IsAll()
        {
            return genderRoleIndex == BodyPartsByRace.All;
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
            return secondaryGenderRoleIndex != 0;
        }

        public bool IsSecondaryAll()
        {
            return secondaryGenderRoleIndex == BodyPartsByRace.All;
        }

        public Gender GetSecondaryGender()
        {
            switch (secondaryGenderRoleIndex)
            {
                case 1:
                    return Gender.Male;
                case 2:
                    return Gender.Female;
            }

            return Gender.None;
        }
    }
}