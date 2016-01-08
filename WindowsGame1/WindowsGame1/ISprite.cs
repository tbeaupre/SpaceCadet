using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Spaceman
{
    public interface ISprite
    {
        int GetHitDuration();
        Texture2D GetTexture();
        Status GetStatus();
        bool GetMirrorX();
        Rectangle GetDestRect();
        Rectangle GetSourceRect();
        int GetSpriteWidth();
        int GetSpriteHeight();
        void NextFrame(int offset);
        int GetFrameNum();
        void SetFrameNum(int frameNum);
        Texture2D MirrorTexture(ISprite sprite, Game1 game, Texture2D texture);
    }
}
