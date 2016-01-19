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
        public void UpdateLiquid(double speed, double h)
        {

            int dir=0;
            if (speed > 0) dir = 1;
            if (speed < 0) dir = -1;
            if (speed == 0) dir = 0;

            
                switch (dir)
                {
                    case 1:
                        for (int i = 0; i < 17; i++)
                        {
                            int drop = CheckDrop(dropletList[i], dropletList[i + 1]);
                            if ((dropletList[i].GetDestRect().Y) <= h + 13 && CheckBigDrop() == 1)
                            {
                                if (drop == 0)
                                {
                                    dropletList[i].SetDestCoord(2, 0);
                                }
                                if (drop == 1)
                                {
                                    dropletList[i].SetDestCoord(+1, +1);

                                    if (dropletList[i].GetMapCollide() == false)
                                {
                                    dropletList[i].SetDestCoord(0, +1);
                                }
                                }

                                if (CheckDrop(dropletList[17], dropletList[0]) == 0)
                                {
                                    dropletList[17].SetDestCoord(2, 0);
                                }
                                if (CheckDrop(dropletList[17], dropletList[0]) == 1)
                                {
                                    dropletList[17].SetDestCoord(+1, +1);

                                    if (dropletList[17].GetMapCollide() == false)
                                    {
                                        dropletList[17].SetDestCoord(0, +1);
                                    }
                                }
                            }
                            if (drop == -1)
                            {
                                dropletList[i].SetDestCoord(+1, -1);
                            }
                            if (CheckDrop(dropletList[17], dropletList[0]) == -1)
                            {
                                dropletList[17].SetDestCoord(+1, -1);
                            }
                        }
                    
                        break;
                    case -1:
                        for (int i = 17; i > 0; i--)
                        {
                            int drop = CheckDrop(dropletList[i], dropletList[i - 1]);
                            if ((dropletList[i].GetDestRect().Y) <= h + 13 && CheckBigDrop() == 1)
                            {
                                if (drop == 0)
                                {
                                    dropletList[i].SetDestCoord(-2, 0);
                                }
                                if (drop == 1)
                                {
                                    dropletList[i].SetDestCoord(-1, 1);

                                    if (dropletList[i].GetMapCollide() == false)
                                    {
                                        dropletList[i].SetDestCoord(0, +1);
                                    }
                                }

                                if (CheckDrop(dropletList[0], dropletList[17]) == 0)
                                {
                                    dropletList[0].SetDestCoord(-2, 0);
                                }
                                if (CheckDrop(dropletList[0], dropletList[17]) == 1)
                                {
                                    dropletList[0].SetDestCoord(-1, +1);

                                    if (dropletList[0].GetMapCollide() == false)
                                    {
                                        dropletList[0].SetDestCoord(0, +1);
                                    }
                                }

                            }
                            if (drop == -1)
                            {
                                dropletList[i].SetDestCoord(-1, -1);
                            }
                            if (CheckDrop(dropletList[0], dropletList[17]) == -1)
                            {
                                dropletList[0].SetDestCoord(-1, -1);
                            }
                        }
                        break;
                case 0:
                    break;
                }
        }

        //returns immediate drop in y between two pixels
        private int CheckDrop(Droplet d1, Droplet d2)
        {
            return (int)(d2.GetDestRect().Y - d1.GetDestRect().Y);
        }

        //returns largest change in Y across all 
        private int CheckBigDrop()
        {
            int max = 0;
            for (int i = 0; i < 17; i++)
            {
                int next = Math.Abs(CheckDrop(dropletList[i], dropletList[i + 1]));
                if (next > max)
                {
                    max = next;
                }

            }
            if (Math.Abs(CheckDrop(dropletList[17], dropletList[0])) > max)
            {
                max = Math.Abs(CheckDrop(dropletList[17], dropletList[0]));
            }

            return max;
        }
    }
}
