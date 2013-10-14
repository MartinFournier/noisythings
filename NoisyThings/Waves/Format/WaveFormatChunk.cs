using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings.Waves.Format
{
    public class WaveFormatChunk
    {
        // Four bytes: "fmt "
        public string ChunkId { get { return "fmt "; } }

        // Length of header in bytes
        public uint ChunkSize { get { return 16; } }

        // 1 (MS PCM)
        public ushort FormatTag { get { return 1; } }

        // Number of channels
        public ushort Channels { get; set; }

        // Frequency of the audio in Hz... 44100
        public uint SamplesPerSecond { get; set; }

        // for estimating RAM allocation
        public uint AverageBytesPerSecond
        {
            get
            {
                return SamplesPerSecond * BlockAlign;
            }
        }

        public uint BaseSamples
        {
            get
            {
                return SamplesPerSecond * Channels;
            }
        }

        // sample frame size, in bytes
        public ushort BlockAlign
        {
            get
            {
                return (ushort)(Channels * (BitsPerSample / 8));
            }
        }

        // bits per sample
        public ushort BitsPerSample { get; set; }

        /// <summary>
        /// Initializes a format chunk with the following properties:
        /// Sample rate: 44100 Hz
        /// Channels: Stereo
        /// Bit depth: 16-bit
        /// </summary>
        public WaveFormatChunk()
        {
            Channels = 2;
            SamplesPerSecond = 44100;
            BitsPerSample = 16;
        }
    }
}
