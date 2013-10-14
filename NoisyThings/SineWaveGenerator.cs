using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings
{
    public class SineWaveGenerator : WaveGenerator
    {
        public SineWaveGenerator(double frequency) : base()
        {
            // Number of samples = sample rate * channels * bytes per sample
            uint numSamples = format.dwSamplesPerSec * format.wChannels;

            // Initialize the 16-bit array
            data.shortArray = new short[numSamples];

            int amplitude = 32760;  // Max amplitude for 16-bit audio

            // The "angle" used in the function, adjusted for the number of channels and sample rate.
            // This value is like the period of the wave.
            double t = (Math.PI * 2 * frequency) / (format.dwSamplesPerSec * format.wChannels);

            for (uint i = 0; i < numSamples - 1; i++)
            {
                // Fill with a simple sine wave at max amplitude
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(t * i));
                }
            }

            // Calculate data chunk size in bytes
            data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
        }

    }
}
