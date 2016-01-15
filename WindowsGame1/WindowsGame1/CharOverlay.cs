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
	public interface CharOverlay
	{
		int getXOffset();
		int getYOffset();
		Texture2D getTexture();
		Rectangle getDestRect(Game1 game);
		Rectangle getSourceRect(Game1 game);
		bool getMirrorX(Game1 game);

	}
}
