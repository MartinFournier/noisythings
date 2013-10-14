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

    }
}
