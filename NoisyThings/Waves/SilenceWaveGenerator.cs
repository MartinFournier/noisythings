using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings.Waves
{
    public class SilenceWaveGenerator : WaveGenerator
    {
        public SilenceWaveGenerator(TimeSpan duration)
            : base(0.001, duration)
        {
            for (uint i = 0; i < NumberOfSamples - 1; i++)
            {
                // Fill with a simple sine wave at max amplitude
                for (int channel = 0; channel < Format.Channels; channel++)
                {
                    Data.SamplesData[i + channel] = 0;
                }
            }

            CalculateChunkSize();
        }
    }
}
