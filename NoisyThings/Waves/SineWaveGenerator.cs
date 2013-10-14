using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings.Waves
{
    public class SineWaveGenerator : WaveGenerator
    {
        public SineWaveGenerator(double frequency, TimeSpan duration)
            : base(frequency, duration)
        {
            for (uint i = 0; i < NumberOfSamples - 1; i++)
            {
                    Data.SamplesData[i] = Convert.ToInt16(Amplitude *  Math.Sin(Angle * i));
            }

            CalculateChunkSize();
        }

    }

     
}
