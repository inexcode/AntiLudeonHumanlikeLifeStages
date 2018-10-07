using System;
using Verse;

namespace HumanlikeLifeStages
{
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