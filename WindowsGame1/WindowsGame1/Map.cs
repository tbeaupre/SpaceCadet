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
	public class Map
	{
		public Texture2D hitbox;
        public Texture2D background;
        public Texture2D foreground;
		public bool active;
		public List<Portal> portals;
		public List<Door> doors;
		public Vector2 mapCoordinates;
        public Vector2 offset;
		public int parallaxFactor;
		public List<PickUp> pickUps = new List<PickUp>();
		public List<Enemy> enemies = new List<Enemy>();
		public List<IObject> objectsToDraw = new List<IObject>();
		public List<Spawn> spawns = new List<Spawn>();
		public List<MapAsset> assets = new List<MapAsset>();
		public List<IMapItem> mapItems = new List<IMapItem>();
        public List<Projectile> enemyProjectiles = new List<Projectile>();
        public List<Projectile> allyProjectiles = new List<Projectile>();
        private bool wasJustActivated = false;
		public SaveStation saveStation = null;

		public Map(MapResource resource, int parallaxFactor)
		{
            this.hitbox = resource.hitbox;
            this.background = resource.background;
            this.foreground = resource.foreground;
			this.active = false;
			this.portals = new List<Portal>();
			this.doors = new List<Door>();
			this.mapCoordinates = new Vector2(0, 0);
			this.parallaxFactor = parallaxFactor;
		}

        public void AddProjectile(Projectile projectile, bool enemy)
        {
            if (enemy) this.enemyProjectiles.Add(projectile);
            else this.allyProjectiles.Add(projectile);
        }

        public bool GetWasJustActivated()
        {
            return wasJustActivated;
        }
        public void SetWasJustActivated(bool set)
        {
            wasJustActivated = set;
        }

        public Map() { }

		public void InitializeMap(List<IMapItem> items)
		{
			foreach (IMapItem item in items)
			{
				switch (item.GetType()) {
					case "MapItem": this.mapItems.Add(item);
						break;
					default: this.mapItems.Add(item);
						break;
				}
			}
		}

		public void UpdateMap(Game1 game)
		{
			foreach (IMapItem item in mapItems)
			{
				item.UpdateSprite(game);
			}
		}

		public void AddDoor(Door d)
		{
			this.doors.Add(d);
		}

		public void AddPortal(Portal p)
		{
			this.portals.Add(p);
			this.doors.Add(p.door1);
			this.doors.Add(p.door2);
		}

		public void ActivateMap(Door door, Game1 game)
		{
			if (door.isLeft)
			{
				SetCoordinates((float)door.worldX - Game1.screenWidth / 2 + game.player.spriteWidth - 1, (float)door.worldY - Game1.screenHeight / 2 + game.player.spriteHeight + 7);
			}
			else
			{
                SetCoordinates((float)door.worldX - Game1.screenWidth / 2 + door.spriteWidth - game.player.spriteWidth + 1, (float)door.worldY - Game1.screenHeight / 2 + game.player.spriteHeight + 7);
			}
            OffsetCheck();
			this.objectsToDraw.Clear();
			this.active = true;
			this.wasJustActivated = true;
		}

		public void SetCoordinates(float X, float Y)
		{
			this.mapCoordinates.X = X;
			this.mapCoordinates.Y = Y;
		}

        public void ChangeCoords(float X, float Y)
        {
            this.mapCoordinates.X += X;
            this.mapCoordinates.Y += Y;
            OffsetCheck();
        }

        public void OffsetCheck()
        {
            if (this.mapCoordinates.X + Game1.screenWidth > this.hitbox.Width)
            {
                this.offset.X = this.mapCoordinates.X + Game1.screenWidth - this.hitbox.Width;
            }
            else
            {
                if (this.mapCoordinates.X < 0)
                {
                    this.offset.X = this.mapCoordinates.X;
                }
                else this.offset.X = 0;
            }
            if (this.mapCoordinates.Y + Game1.screenHeight > this.hitbox.Height)
            {
                this.offset.Y = this.mapCoordinates.Y + Game1.screenHeight - this.hitbox.Height;
            }
            else
            {
                if (this.mapCoordinates.Y< 0)
                {
                    this.offset.Y = this.mapCoordinates.Y;
                }
                else this.offset.Y = 0;
            }
        }

		public void DeactivateMap(Game1 game)
		{
			this.active = false;
			this.objectsToDraw.Clear();
		}
	}
}
