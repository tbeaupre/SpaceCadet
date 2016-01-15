using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Spaceman
{
    public class Song
    {
        public string songName;
        string cue1;
        string cue2;
        string cue3;

        AudioEngine engine;
        SoundBank SB;
        WaveBank WB;

        Cue first;
        Cue second;
        Cue third;

        AudioCategory firstCat;
        float track1 = 0;
        AudioCategory secondCat;
        float track2 = 0;
        AudioCategory thirdCat;
        float track3 = 0;

        private int musicDynamic = 2;

        public Song(string songName, string cue1, string cue2, string cue3)
            //track names go here. The Cue and the Catagory must be called the same name
            //the songName is always lowercase and matches the name of the local file respectively
        {
            this.songName = songName;
            this.cue1 = cue1;
            this.cue2 = cue2;
            this.cue3 = cue3;
        }
        public void initializeCue(string directory)
        {
            engine = new AudioEngine(directory + "//Music\\" + songName + "\\" + songName + ".xgs");
            SB = new SoundBank(engine, directory + "//Music\\" + songName + "\\" + songName + "SoundBank.xsb");
            WB = new WaveBank(engine, directory + "//Music\\" + songName + "\\" + songName + "WaveBank.xwb");

            first = SB.GetCue(cue1);
            second = SB.GetCue(cue2);
            third = SB.GetCue(cue3);

            first.Play();
            second.Play();
            third.Play();
        }
        public void UpdateMusic()
        {
            engine.Update();
            firstCat = engine.GetCategory(cue1);
            firstCat.SetVolume(track1);
            secondCat = engine.GetCategory(cue2);
            secondCat.SetVolume(track2);
            thirdCat = engine.GetCategory(cue3);
            thirdCat.SetVolume(track3);

            switch (musicDynamic)
            {
                case 0:
                    track1 = 0.0f;
                    track2 = 0.0f;
                    track3 = 0.0f;
                    break;
                case 1:
                    track1 = 1.0f;
                    track2 = 0.0f;
                    track3 = 0.0f;
                    break;
                case 2:
                    track1 = 1.0f;
                    track2 = 1.0f;
                    track3 = 0.0f;
                    break;
                case 3:
                    track1 = 1.0f;
                    track2 = 1.0f;
                    track3 = 1.0f;
                    break;
                case 4:
                    track1 = 0.0f;
                    track2 = 1.0f;
                    track3 = 1.0f;
                    break;
                case 5:
                    track1 = 0.0f;
                    track2 = 0.0f;
                    track3 = 1.0f;
                    break;
                default:
                    track1 = 0.0f;
                    track2 = 1.0f;
                    track3 = 0.0f;
                    break;
            }
        }
        public void SetMusicDynamic(int i)
        {
            musicDynamic = i;
        }
    }
}
