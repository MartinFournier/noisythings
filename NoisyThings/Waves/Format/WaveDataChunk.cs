using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings.Waves.Format
{
    public class WaveDataChunk
    {
        public string ChunkId { get { return "data"; } }
        public uint ChunkSize { get; set; }     // Length of header in bytes
        public short[] SamplesData { get; set; }  // 8-bit audio

        public WaveDataChunk()
        {
            SamplesData = new short[0];
            ChunkSize = 0;
        }
    }
}
