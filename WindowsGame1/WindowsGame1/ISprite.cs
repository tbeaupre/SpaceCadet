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
        Texture2D GetHitbox();
        Texture2D GetVulnerable();
        Status GetStatus();
        bool GetMirrorX();
        DRectangle GetDestRect();
        Rectangle GetSourceRect();
        int GetSpriteWidth();
        int GetSpriteHeight();
        void NextFrame(int offset);
        int GetFrameNum();
        void SetFrameNum(int frameNum);
        Texture2D MirrorTexture(ISprite sprite, GraphicsDeviceManager graphics, Texture2D texture);
    }
}
