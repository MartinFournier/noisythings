using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings.Waves
{
    public class SquareWaveGenerator : WaveGenerator
    {
        public SquareWaveGenerator(double frequency, TimeSpan duration)
            : base(frequency, duration)
        {
            {
                for (int i = 0; i < NumberOfSamples; i++)
                {
                    // Generate a square wave in both channels.
                    if (Math.Sin(Angle * i) > 0)
                        Data.SamplesData[i] = Convert.ToInt16(Amplitude);
                    else
                        Data.SamplesData[i] = Convert.ToInt16(-Amplitude);
                }
            }

            CalculateChunkSize();
        }

    }
}
