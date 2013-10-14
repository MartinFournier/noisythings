using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings
{
    public enum Notes
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6
    }

    public enum Intonations
    {
        Flat = -1,
        None = 0,
        Sharp = 1,
    }

    public class MusicNote
    {
        private const double baseFreq = 440.00;
        private const int baseOctave = 4;
        private const Notes baseNote = Notes.A;
        private const Intonations baseIntonations = Intonations.None;

        private double FrequencyFromSteps(int stepsFromBase) {
            //http://www.phy.mtu.edu/~suits/NoteFreqCalcs.html
            var twelth = 1.0f / 12.0f;
            var freq = baseFreq * Math.Pow(Math.Pow(2.0f, twelth), stepsFromBase);
            return freq;
        }

        private int StepsFromBase()
        {
            var octaveDiff = (Octave - baseOctave);
            var octaveDifferenceSteps = 0;
            if (octaveDiff < 0) {
                octaveDifferenceSteps  = (octaveDiff+1) * 6 * 2;
            }else {
                octaveDifferenceSteps  = (octaveDiff) * 6 * 2;
            }
           
            var diffSteps = 0;
            if (Octave >= baseOctave)
            {
                diffSteps = ((int)Note - (int)baseNote) * 2;
                if (Note >= Notes.C)
                {
                    diffSteps--;
                }
                if (Note >= Notes.F)
                {
                    diffSteps--;
                }
            }
            else
            {
                diffSteps = -((7 - (int)Note) * 2);
                if (Note <= Notes.E)
                {
                    diffSteps++;
                }
                if (Note <= Notes.B)
                {
                    diffSteps++;
                }
            }

                diffSteps += octaveDifferenceSteps;
         
            diffSteps += (int)Intonation;

            return diffSteps;
        }

        

        public Notes Note {get;set;}
        public int Octave { get; set; }
        public Intonations Intonation { get; set; }
        public double Frequency { 
            get {
                var steps = StepsFromBase();
                return FrequencyFromSteps(steps);
            } 
        }


        public override string ToString()
        {
            var friendlyOctave = Octave;
            
            if (Note > Notes.B)
            {
                friendlyOctave++;
            }
            var s = Note.ToString() + friendlyOctave;
            if ((int)Intonation == (int)Intonations.Flat)
            {
                s += "b";
            }
            else if ((int)Intonation == (int)Intonations.Sharp)
            {
                s += "#";
            }
            s += " : " + Frequency + " Hz";
            return s;
        }

        public MusicNote(Notes note, int octave, Intonations intonation = Intonations.None)
        {
            this.Note = note;
            this.Octave = octave;
            this.Intonation = intonation;
        }
    }
}
