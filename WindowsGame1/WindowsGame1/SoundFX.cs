using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Spaceman
{
    public class SoundFX
    {
        public string sound;

        AudioEngine engine;
        SoundBank SB;
        WaveBank WB;

        Cue first;

        public SoundFX(string sound)
        //track names go here. The Cue and the SOUND must be called the same name
        {
            this.sound = sound;
            engine = new AudioEngine("content//Sounds\\sound effects.xgs");
            SB = new SoundBank(engine, "content//Sounds\\EffectsSB.xsb");
            WB = new WaveBank(engine, "content//Sounds\\EffectsWB.xwb");
        }

        public void play()
        {            
            first = SB.GetCue(sound);
            if (!first.IsPlaying)
            {
                first.Play();
            }
        }
    }
}
