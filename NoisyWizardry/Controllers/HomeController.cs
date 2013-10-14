using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoisyThings;
using NoisyThings.Waves;
using System.Web.Script.Serialization;

namespace NoisyWizardry.Controllers
{
    public class HomeController : Controller
    {
        Random rnd = new Random();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ToJson(WaveGenerator w, MusicNote n = null)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var note = "";
            if (n != null) {
                note = n.ToString();
            }

            var resultData = new { audio = w.ToBase64(), d =  note};
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        public ActionResult Audio(double freq, WaveTypes wave, int ms = 200)
        {
            WaveGenerator w = WaveGenerator.GetGenerator(wave, freq, new TimeSpan(0, 0, 0, 0, ms));
            return ToJson(w);
        }

        public ActionResult Note(Notes note, int octave, Intonations intonation, WaveTypes wave, int ms = 200)
        {
            var n = new MusicNote(note, octave, intonation);
            WaveGenerator w = WaveGenerator.GetGenerator(wave, n.Frequency, new TimeSpan(0,0,0, 0, ms));
            return ToJson(w, n);
        }

        public ActionResult Merge()
        {
            var n1 = new MusicNote(Notes.C, 4);
            var n2 = new MusicNote(Notes.E, 2);
            WaveGenerator w1 = WaveGenerator.GetGenerator(WaveTypes.Sawtooth, n1.Frequency, new TimeSpan(0, 0, 0, 0, 50));
            WaveGenerator w2 = WaveGenerator.GetGenerator(WaveTypes.Sawtooth, n2.Frequency, new TimeSpan(0, 0, 0, 0, 500));
            w1.AppendWave(w2);
            return ToJson(w1);
        }

        public ActionResult Random()
        {
            var nbOfNotes = rnd.Next(0, 100);
            var master = GetRandomGenerator(GetRandomNote());
            for (var i = 1; i < nbOfNotes; i++)
            {
                var newNote = GetRandomGenerator(GetRandomNote());
                master.AppendWave(newNote);
            }
            return ToJson(master);
        }

        private MusicNote GetRandomNote()
        {
            var note = rnd.Next(0, 6);
            var octave = rnd.Next(3, 5);
            var intonation = rnd.Next(-1, 1);
            var randomNote = new MusicNote((Notes)note, octave, (Intonations)intonation);
            return randomNote;
        }

        private WaveGenerator GetRandomGenerator(MusicNote note)
        {
            var duration = rnd.Next(25, 250);
            var waveType = rnd.Next(0, 3);
            var wave = WaveGenerator.GetGenerator((WaveTypes)waveType, note.Frequency, new TimeSpan(0,0,0,0, duration);
            return wave;
        }
    }
}
