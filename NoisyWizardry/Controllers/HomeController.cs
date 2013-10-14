using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoisyThings;

namespace NoisyWizardry.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Audio(double freq)
        {
            WaveGenerator wave = new SineWaveGenerator(freq);
            var buffer = wave.ToMemoryStream().GetBuffer();
            return File(buffer, "audio/wav");
        }

        public ActionResult Note(Notes note, int octave, Intonations intonation)
        {
            var n = new MusicNote(note, octave, intonation);
            WaveGenerator wave = new SineWaveGenerator(n.Frequency);
            var buffer = wave.ToMemoryStream().GetBuffer();
            return File(buffer, "audio/wav");
        }

    }
}
