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
	public interface IMenuItem
	{
		Texture2D GetTexture();
		void SetIsHighlighted(bool val);
		bool GetIsHighlighted();
		void ActivateItem(Game1 game, IMenu from);
        Rectangle GetSourceRect();
        Nullable<Rectangle> GetAnchorRect();
	}
}
