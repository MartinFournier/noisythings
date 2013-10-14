using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings.Waves
{
    public class SawtoothWaveGenerator : WaveGenerator
    {
        public SawtoothWaveGenerator(double frequency, TimeSpan duration)
            : base(frequency, duration)
        {
            int samplesPerPeriod = Convert.ToInt32(
                Format.SamplesPerSecond /(frequency / Format.Channels));
            short sampleStep = Convert.ToInt16(
                (Amplitude * 2) / samplesPerPeriod);
            short tempSample = 0;

            int i = 0;
            int totalSamplesWritten = 0;
            while (totalSamplesWritten < NumberOfSamples)
            {
                tempSample = (short)-Amplitude;
                for (i = 0; i < samplesPerPeriod && totalSamplesWritten < NumberOfSamples; i++)
                {
                    tempSample += sampleStep;
                    Data.SamplesData[totalSamplesWritten] = tempSample;
                    totalSamplesWritten++;
                }
            }

            CalculateChunkSize();
        }

    }
}
