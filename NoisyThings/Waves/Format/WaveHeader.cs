using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisyThings.Waves.Format
{
    //http://blogs.msdn.com/b/dawate/archive/2009/06/24/intro-to-audio-programming-part-3-synthesizing-simple-wave-audio-using-c.aspx
    public class WaveHeader
    {
        // RIFF
        public string GroupId {get { return "RIFF";}}

        // always WAVE
        public string RiffType { get { return "WAVE"; } } 

        public uint FileLength; // total file length minus 8, which is taken up by RIFF

        /// <summary>
        /// Initializes a WaveHeader object with the default values.
        /// </summary>
        public WaveHeader()
        {
            FileLength = 0;            
        }
    }
}
