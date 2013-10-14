using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NoisyThings.Waves.Format;

namespace NoisyThings.Waves
{
    abstract public class WaveGenerator
    {
        protected WaveHeader Header;
        protected WaveFormatChunk Format;
        protected WaveDataChunk Data;

        protected const int Amplitude = 32760;  // Max amplitude for 16-bit audio

        protected uint NumberOfSamples;
        protected double Angle;

        public WaveGenerator(double frequency, TimeSpan duration)
        {
            Header = new WaveHeader();
            Format = new WaveFormatChunk();
            Data = new WaveDataChunk();

            // Number of samples = sample rate * channels * bytes per sample

            NumberOfSamples = (uint)(Format.BaseSamples * duration.TotalSeconds);

            // Initialize the 16-bit array
            Data.SamplesData = new short[NumberOfSamples];

            // The "angle" used in the function, adjusted for the number of channels and sample rate.
            // This value is like the period of the wave.
            Angle = (Math.PI * 2 * frequency) / (Format.SamplesPerSecond * Format.Channels);
        }

        public void AppendWave(WaveGenerator otherWave)
        {
            short[] arr1 = Data.SamplesData;
            short[] arr2 = otherWave.Data.SamplesData;
            var currLen = arr1.Length;
            var otherLen = arr2.Length;
            var newArr = new short[currLen+otherLen];
            Array.Copy(arr1, 0, newArr, 0, currLen);
            Array.Copy(arr2, 0, newArr, currLen, otherLen);
            Data.SamplesData = newArr;
            CalculateChunkSize();
        }

        public static WaveGenerator GetGenerator(WaveTypes type, double frequency, TimeSpan duration)
        {
            switch (type)
            {
                case WaveTypes.Sine:
                    return new SineWaveGenerator(frequency, duration);
                case WaveTypes.Sawtooth:
                    return new SawtoothWaveGenerator(frequency, duration);
                case WaveTypes.Square:
                    return new SquareWaveGenerator(frequency, duration);
                case WaveTypes.Noise:
                    return new NoiseWaveGenerator(frequency, duration);
            }
            return null;
        }

        private Stream WriteToStream(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            // Write the header
            writer.Write(Header.GroupId.ToCharArray());
            writer.Write(Header.FileLength);
            writer.Write(Header.RiffType.ToCharArray());

            // Write the format chunk
            writer.Write(Format.ChunkId.ToCharArray());
            writer.Write(Format.ChunkSize);
            writer.Write(Format.FormatTag);
            writer.Write(Format.Channels);
            writer.Write(Format.SamplesPerSecond);
            writer.Write(Format.AverageBytesPerSecond);
            writer.Write(Format.BlockAlign);
            writer.Write(Format.BitsPerSample);

            // Write the data chunk
            writer.Write(Data.ChunkId.ToCharArray());
            writer.Write(Data.ChunkSize);
            foreach (short dataPoint in Data.SamplesData)
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

        public void CalculateChunkSize()
        {
            // Calculate data chunk size in bytes
            Data.ChunkSize = (uint)(Data.SamplesData.Length * (Format.BitsPerSample / 8));
        }

        public byte[] ToBuffer()
        {
            return ToMemoryStream().GetBuffer();
        }


        public string ToBase64()
        {
            return "data:audio/wav;base64," + Convert.ToBase64String(ToBuffer());
        }
    }
}
