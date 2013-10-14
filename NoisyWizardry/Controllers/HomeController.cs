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
        Random rnd;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ToJson(WaveGenerator w, string extra = null)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new { audio = w.ToBase64(), d = extra };
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
            WaveGenerator w = WaveGenerator.GetGenerator(wave, n.Frequency, new TimeSpan(0, 0, 0, 0, ms));
            return ToJson(w, n.ToString());
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

        public ActionResult Random(int? seed = null)
        {

            if (!seed.HasValue)
            {
                var randSeed = new Random().Next(0, int.MaxValue);
                seed = randSeed;
            }
            rnd = new Random(seed.Value);

            var bpm = rnd.Next(20, 150);
            var timePattern = new TimeSignaturePattern(bpm, 4, 4);
            var nbOfNotes = rnd.Next(25, 250);
            var master = GetRandomGenerator(GetRandomNote().Frequency, timePattern);
            for (var i = 1; i < nbOfNotes; i++)
            {

                var newNote = GetRandomGenerator(GetRandomNote().Frequency, timePattern);
                master.AppendWave(newNote);

            }
            return ToJson(master, seed.Value.ToString());
        }

        private MusicNote GetRandomNote()
        {
            var note = rnd.Next(0, 6);
            var octave = rnd.Next(2, 3);
            var intonation = rnd.Next(0, 1);
            var randomNote = new MusicNote((Notes)note, octave, (Intonations)intonation);
            return randomNote;
        }

        private WaveGenerator GetRandomGenerator(double frequency, TimeSignaturePattern pattern)
        {
            var noteTypeValue = (NoteValues)rnd.Next(0, 3);
            var noteDurationFromType = rnd.Next(1, 4); // this does not work extensively;
            var randomNoteTimeValue = pattern.NoteLength(noteTypeValue);
            var funkyOffset = 25;
            if (randomNoteTimeValue > funkyOffset) randomNoteTimeValue = randomNoteTimeValue - funkyOffset;
            double noteTimePlaying = 0.0f;
            if (noteDurationFromType == 0) {
                noteTimePlaying = 0.0f; ;
            } else {
                noteTimePlaying = (double)randomNoteTimeValue / (double)noteDurationFromType;
            }
            
            var noteTimePaused = randomNoteTimeValue - noteTimePlaying;

            if ((int)noteTimePlaying == 0) noteTimePlaying++;
            if ((int)noteTimePaused == 0) noteTimePaused++;
            var waveType = (WaveTypes)rnd.Next(0, 2);
            waveType = WaveTypes.Sawtooth;
            var wave = WaveGenerator.GetGenerator(waveType, frequency, new TimeSpan(0, 0, 0, 0, (int)noteTimePlaying));

            var silence = new SilenceWaveGenerator(new TimeSpan(0, 0, 0, 0, (int)noteTimePaused));
            wave.AppendWave(silence);
            return wave;
        }
    }
}
