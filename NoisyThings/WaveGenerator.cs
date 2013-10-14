using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NoisyThings
{
    public class WaveGenerator
    {
        protected WaveHeader header;
        protected WaveFormatChunk format;
        protected WaveDataChunk data;

        public WaveGenerator()
        {
            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();
        }

        private Stream WriteToStream(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            // Write the header
            writer.Write(header.sGroupID.ToCharArray());
            writer.Write(header.dwFileLength);
            writer.Write(header.sRiffType.ToCharArray());

            // Write the format chunk
            writer.Write(format.sChunkID.ToCharArray());
            writer.Write(format.dwChunkSize);
            writer.Write(format.wFormatTag);
            writer.Write(format.wChannels);
            writer.Write(format.dwSamplesPerSec);
            writer.Write(format.dwAvgBytesPerSec);
            writer.Write(format.wBlockAlign);
            writer.Write(format.wBitsPerSample);

            // Write the data chunk
            writer.Write(data.sChunkID.ToCharArray());
            writer.Write(data.dwChunkSize);
            foreach (short dataPoint in data.shortArray)
            {
                writer.Write(dataPoint);
            }

            writer.Seek(4, SeekOrigin.Begin);
            uint filesize = (uint)writer.BaseStream.Length;
            writer.Write(filesize - 8);

            return stream;
        }

        public MemoryStream ToMemoryStream()
        {
            MemoryStream stream = new MemoryStream();
            var s = WriteToStream(stream);
            return stream;
        }

        public void ToFileSystem(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            WriteToStream(fileStream);
            fileStream.Close();
        }
    }
}
