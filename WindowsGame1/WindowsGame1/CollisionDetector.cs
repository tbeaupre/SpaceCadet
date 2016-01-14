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
    public static class CollisionDetector
    {
        public static bool CheckMapCollision(int xOffset, int yOffset, Sprite sprite, Map map)
        {
            return MapCollisionDetect(sprite.spriteWidth, sprite.spriteHeight, OffsetRect(sprite.destRect, xOffset, yOffset).ToRectangle(), map);
        }

        public static bool CheckMapCollision(int xOffset, int yOffset, Spaceman sprite, Map map)
        {
            Rectangle newRect = new Rectangle((int)(sprite.destRect.X + 1), (int)(sprite.destRect.Y + 1), sprite.spriteWidth - 2, sprite.spriteHeight - 1);
            return MapCollisionDetect(sprite.spriteWidth - 2, sprite.spriteHeight - 1, OffsetRect(newRect, xOffset, yOffset), map);
        }

        public static CollisionState PerPixelSprite(ISprite sprite1, ISprite sprite2, GraphicsDeviceManager graphics)
        {
            int xOffset = 0;
            int yOffset = 0;
            int widthOffset = 0;
            int heightOffset = 0;

            if (sprite2 is Spaceman)
            {
                xOffset = 2;
                yOffset = 1;
                widthOffset = -4;
                heightOffset = -1;
            }
            Rectangle rect = new Rectangle((int)(sprite2.GetDestRect().X - sprite1.GetDestRect().X + xOffset), (int)(sprite2.GetDestRect().Y - sprite1.GetDestRect().Y + yOffset), sprite2.GetSpriteWidth() + widthOffset, sprite2.GetSpriteHeight() + heightOffset);
            return PerPixel(sprite1, sprite2, rect, graphics);
        }

        public static CollisionState PerPixel(ISprite sprite1, ISprite sprite2, Rectangle rect, GraphicsDeviceManager graphics)
        {
            // sets the coordinates relative to (0,0) being the top left corner of this.
            Texture2D projTexture = sprite2.GetTexture();
            Texture2D hitBoxTexture = sprite1.GetHitbox();
            Texture2D vulnerableTexture = sprite1.GetVulnerable();

            Color[] hitBoxPixels;
            Color[] vulnerablePixels;
            Color[] projectilePixels;
            CollisionState results = CollisionState.None;
            Rectangle objRect = rect;
            Rectangle projRect = new Rectangle(0, 0, rect.Width, rect.Height);

            //initial tests to see if the box is even applicable to the object texure being checked
            if (rect.X + rect.Width <= 0 || rect.Y + rect.Height <= 0) return CollisionState.None;
            if (rect.X >= sprite1.GetSpriteWidth() || rect.Y >= sprite1.GetSpriteHeight()) return CollisionState.None;

            if (rect.X < 0)
            {
                objRect.X = 0;
                objRect.Width += rect.X;
                projRect.X -= rect.X;
                projRect.Width += rect.X;
            }

            if (rect.Y < 0)
            {
                objRect.Height += rect.Y;
                objRect.Y = 0;
                projRect.Y -= rect.Y;
                projRect.Height = objRect.Height;
            }

            for (int i = 0; i <= objRect.Width; i++)
            {
                if (objRect.X + i == sprite1.GetSpriteWidth())
                {
                    objRect.Width = i;
                    projRect.Width = objRect.Width;
                    break;
                }
            }
            for (int i = 0; i <= objRect.Height; i++)
            {
                if (objRect.Y + i == sprite1.GetSpriteHeight())
                {
                    objRect.Height = i;
                    projRect.Height = objRect.Height;
                    break;
                }
            }

            if (objRect.Width == 0 || objRect.Height == 0) return 0;

            hitBoxPixels = new Color[objRect.Width * objRect.Height];
            vulnerablePixels = new Color[objRect.Width * objRect.Height];
            projectilePixels = new Color[objRect.Width * objRect.Height];

            if (sprite2.GetMirrorX())
            {
                projTexture = sprite2.MirrorTexture(sprite2, graphics, sprite2.GetTexture());
            }

            if (sprite1.GetMirrorX())
            {
                hitBoxTexture = sprite1.MirrorTexture(sprite1, graphics, sprite1.GetHitbox());
                vulnerableTexture = sprite1.MirrorTexture(sprite1, graphics, sprite1.GetVulnerable());
            }


            projTexture.GetData<Color>(
                0, projRect, projectilePixels, 0, objRect.Width * objRect.Height
                );

            hitBoxTexture.GetData<Color>(
                0, objRect, hitBoxPixels, 0, objRect.Width * objRect.Height
                );

            vulnerableTexture.GetData<Color>(
                0, objRect, vulnerablePixels, 0, objRect.Width * objRect.Height
                );

            for (int y = 0; y < objRect.Height; y++)
            {
                for (int x = 0; x < objRect.Width; x++)
                {
                    Color colorA = hitBoxPixels[y * objRect.Width + x];
                    Color colorB = projectilePixels[y * objRect.Width + x];
                    Color colorC = vulnerablePixels[y * objRect.Width + x];
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        if (results == CollisionState.None) results = CollisionState.Standard;
                    }
                    if (colorC.A != 0 && colorB.A != 0)
                    {
                        results = CollisionState.Hurtbox;
                        break;
                    }
                }
            }
            return results;
        }

        // for rectangular collisions between two sprites.
        public static bool RectCollisionDetect(ISprite sprite1, ISprite sprite2)
        {
            Color[] pixels;
            
            int spriteWidth = sprite2.GetSpriteWidth() + (sprite2 is Spaceman? -4 : 0); // because the hitbox on the spaceman should be slightly smaller than it is.
            int spriteHeight = sprite2.GetSpriteHeight() + (sprite2 is Spaceman ? -1 : 0);

            Rectangle rect = new Rectangle((int)(sprite2.GetDestRect().X - sprite1.GetDestRect().X + (sprite2 is Spaceman ? 2 : 0)), (int)(sprite2.GetDestRect().Y - sprite1.GetDestRect().Y + (sprite2 is Spaceman ? 1 : 0)), spriteWidth, spriteHeight);  // this rectangle represents the space the sprite takes up relative to "this"'s top left corner
            Rectangle newRect = rect; // newRect is the actual rectangle to check

            //initial tests to see if the box is even applicable to the object texure being checked
            if (rect.X + rect.Width <= 0 || rect.Y + rect.Height <= 0) return false;
            if (rect.X >= sprite1.GetSpriteWidth() || rect.Y >= sprite1.GetSpriteHeight()) return false;

            // Removes the space on the rectangle that is outside of the bounds of "this"'s texture
            if (rect.X < 0)
            {
                newRect.X = 0;
                newRect.Width += rect.X;
            }
            if (rect.Y < 0)
            {
                newRect.Y = 0;
                newRect.Height += rect.Y;
            }

            if (newRect.X + newRect.Width > sprite1.GetSpriteWidth())
            {
                for (int i = 0; i <= sprite1.GetSpriteWidth(); i++)
                {
                    if (newRect.X + i == sprite1.GetSpriteWidth())
                    {
                        newRect.Width = i;
                        break;
                    }
                }
            }
            if (newRect.Y + newRect.Height > sprite1.GetSpriteHeight())
            {
                for (int i = 0; i <= sprite1.GetSpriteHeight(); i++)
                {
                    if (newRect.Y + i == sprite1.GetSpriteHeight())
                    {
                        newRect.Height = i;
                        break;
                    }
                }
            }

            if (newRect.Width == 0 || newRect.Height == 0) return false;

            pixels = new Color[newRect.Width * newRect.Height];

            sprite1.GetTexture().GetData<Color>(
                0, newRect, pixels, 0, newRect.Width * newRect.Height
                );

            for (int y = 0; y < newRect.Height; y++)
            {
                for (int x = 0; x < newRect.Width; x++)
                {
                    Color colorA = pixels[y * newRect.Width + x];
                    if (colorA.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // for rectangular collisions between a sprite and the map
        public static bool MapCollisionDetect(int spritewidth, int spriteheight, Rectangle rect, Map map)
        {
            Color[] pixels;
            Rectangle newRect = new Rectangle((rect.X + (int)map.mapCoordinates.X - (int)map.offset.X),
                (rect.Y + (int)map.mapCoordinates.Y - (int)map.offset.Y),
                rect.Width,
                rect.Height);

            pixels = new Color[spritewidth * spriteheight];

            // Check to see if rectangle is outside of map.
            if (newRect.X < 0
                || newRect.Y < 0
                || newRect.X + spritewidth > map.hitbox.Width
                || newRect.Y + spriteheight > map.hitbox.Height) return false;

            map.hitbox.GetData<Color>(
                0, newRect, pixels, 0, spritewidth * spriteheight
                );
            for (int y = 0; y < spriteheight; y++)
            {
                for (int x = 0; x < spritewidth; x++)
                {
                    Color colorA = pixels[y * spritewidth + x];
                    if (colorA.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Rectangle OffsetRect(Rectangle rect, int xOffset, int yOffset)
        {
            return new Rectangle(rect.X + xOffset, rect.Y + yOffset, rect.Width, rect.Height);
        }

        public static DRectangle OffsetRect(DRectangle rect, int xOffset, int yOffset)
        {
            return new DRectangle(rect.X + xOffset, rect.Y + yOffset, rect.Width, rect.Height);
        }

    }
}
