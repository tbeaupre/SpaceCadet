using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Spaceman
{
    public class SoundFX
    {
        AudioEngine engine;
        SoundBank SB;
        WaveBank WB;
        AudioCategory firstCat;


        Cue first;

        public SoundFX()
        //track names go here. The Cue and the SOUND must be called the same name
        {
            engine = new AudioEngine("content//Sounds\\sound effects.xgs");
            SB = new SoundBank(engine, "content//Sounds\\EffectsSB.xsb");
            WB = new WaveBank(engine, "content//Sounds\\EffectsWB.xwb");
        }

        public void Play(string sound)
        {

            first = SB.GetCue(sound);
            first.Play();
            engine.Update();
        }
    }
}
