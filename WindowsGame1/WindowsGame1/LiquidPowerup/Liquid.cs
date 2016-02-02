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

    public class Liquid
    {
        List<Droplet> dropletList;
        Texture2D pixel;
        int X;
        int Y;


        public Liquid(Texture2D pixel, int X, int Y)
        {
            this.pixel = pixel;
            this.X = X;
            this.Y = Y;
            dropletList = new List<Droplet>();
            for (int h = 0; h <= 1; h++)
            {
                for (int w = 0; w < 9; w++)
                {
                    if (h == 0)
                    {
                        dropletList.Add(new Droplet(pixel, new Vector2(X + w + 1, Y + 13 + h), 18, w));
                    }
                    else
                    {
                        dropletList.Add(new Droplet(pixel, new Vector2(X + 9 - w, Y + 13 + h), 18, w + 9));
                    }
                }
            }
        }


        public ISprite Pixel(int i)
        {
            return dropletList[i];
        }
        public void UpdateLiquid(double dist,double movespeed, Directions dir)
        { 
                int rotation = (int) (dist / movespeed);
                if (rotation % 2 ==0 && movespeed != 0)
                {
                    if (dir == Directions.right)
                    {
                        {
                            for (int i = 0; i < 17; i++)
                            {
                                Vector2 drop = CheckDrop(dropletList[i], dropletList[i + 1]);
                                dropletList[i].SetDestCoord(drop);
                                dropletList[17].SetDestCoord(CheckDrop(dropletList[17], dropletList[0]));
                            }
                        }
                    }
                    if (dir == Directions.left)
                    {
                        {
                            for (int i = 17; i > 0; i--)
                            {
                                Vector2 drop = CheckDrop(dropletList[i], dropletList[i - 1]);
                                dropletList[i].SetDestCoord(drop);
                                dropletList[0].SetDestCoord(CheckDrop(dropletList[0], dropletList[17]));
                            }
                        }
                    }
                }
            }
        
        //returns immediate drop in y between two pixels
        private Vector2 CheckDrop(Droplet d1, Droplet d2)
        {
            return (d2.GetDestRect() - d1.GetDestRect());
        }

        //returns largest change in Y across all 
        private int CheckBigDrop()
        {
            int max = 0;
            for (int i = 0; i < 17; i++)
            {
                int next = (int) Math.Abs(CheckDrop(dropletList[i], dropletList[i + 1]).Y);
                if (next > max)
                {
                    max = next;
                }

            }
            if (Math.Abs(CheckDrop(dropletList[17], dropletList[0]).Y) > max)
            {
                max = (int) Math.Abs(CheckDrop(dropletList[17], dropletList[0]).Y);
            }

            return max;
        }
    }
}
