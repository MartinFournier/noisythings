using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings
{
    public class Tempo
    {
        public double BPM { get; set; }
        public double BPms { 
            get {
                return BPM;
            } 
        }

        public Tempo(int bpm)
        {
            this.BPM = bpm;
        }
    }

    public class TimeSignature
    {
        public int Beats { get; set; }
        public int NoteValue { get; set; }

        public override string ToString()
        {
            return String.Format("{0}/{1}", Beats, NoteValue);
        }
    }

    public class TimeSignaturePattern
    {
        public Tempo Tempo { get; set; }
        public TimeSignature TimeSignature { get; set; }

        public TimeSignaturePattern(int bpm, int beats, int noteValue)
        {
            Tempo = new Tempo(bpm);
            TimeSignature = new TimeSignature() { Beats = beats, NoteValue = noteValue };
        }

        public double NoteLength(NoteValues noteValue, int amountDotted = 0)
        {
            var tv = noteValue.GetTimeValue();
            var length = tv * Tempo.BPms;
            return length;
        }
    }

    public enum NoteValues : long
    {
        [NoteValue(TimeValue = 4)]
        Whole = 0,
        [NoteValue(TimeValue = 2)]
        Half = 1,
        [NoteValue(TimeValue = 1)]
        Quarter = 2,
        [NoteValue(TimeValue = 0.5f)]
        Eighth = 3,
        [NoteValue(TimeValue = 0.25)]
        Sixteeth = 4
    }

    public class NoteValueAttribute : Attribute
    {
        public double TimeValue { get; set; }
    }



    //http://codereview.stackexchange.com/questions/5352/getting-the-value-of-a-custom-attribute-from-an-enum
    public static class NoteValueExtensions
    {
        public static double GetTimeValue(this NoteValues p)
        {
            var attr = p.GetAttribute<NoteValueAttribute>();
            return attr.TimeValue;
        }
        public static double GetTimeValueDotted(this NoteValues p)
        {
            var attr = p.GetAttribute<NoteValueAttribute>();
            return TransformTimeValueDotted(attr.TimeValue);
        }

        private static double TransformTimeValueDotted(double v)
        {
            return v * 1.5;
        }

        public static double GetTimeValueDottedX(this NoteValues p, int nbDotted)
        {
            var val = p.GetTimeValue();
            for (var i = 0; i < nbDotted; i++)
            {
                val = TransformTimeValueDotted(val);
            }
            return val;
        }
    }
}
