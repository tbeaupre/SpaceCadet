using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Spaceman
{
    public class MapResource
    {
        public Texture2D background;
        public Texture2D hitbox;
        public Texture2D foreground;

		public MapResource(Game1 game, string i, bool foreground)
        {
			this.background = game.Content.Load<Texture2D>(string.Concat("MapResources\\Backgrounds\\Background", i));
			this.hitbox = game.Content.Load<Texture2D>(string.Concat("MapResources\\Hitboxes\\Hitbox", i));
			if (foreground)
				this.foreground = game.Content.Load<Texture2D>(string.Concat("MapResources\\Foregrounds\\Foreground", i));
			else
				this.foreground = null;
        }
    }
}
