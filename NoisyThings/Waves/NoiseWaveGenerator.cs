using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings.Waves
{
    public class NoiseWaveGenerator : WaveGenerator
    {
        public NoiseWaveGenerator(double frequency, TimeSpan duration)
            : base(frequency, duration)
        {
            Random rnd = new Random();
            for (int i = 0; i < NumberOfSamples; i++)
            {
                Data.SamplesData[i] = Convert.ToInt16(rnd.Next(-Amplitude, Amplitude));
            }

            CalculateChunkSize();
        }
    }
}
