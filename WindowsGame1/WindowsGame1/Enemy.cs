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
	public abstract class Enemy : Object
	{
		Spawn spawn;
		int maxHealth;
		int currentHealth;
		int attackDamage;
		Texture2D vulnerable;
		Texture2D overlay;
		public int[] attack;
		public int[] die;
		public bool alert = false;
 		public Status action;
		public ProjectileData projectileData;
		int attackCooldown;

        public override Texture2D GetVulnerable()
        {
            return this.vulnerable;
        }

		public Enemy(Spawn spawn, ProjectileData projectileData, int maxHealth, int attackDamage, double worldX, double worldY, Texture2D vulnerable, Texture2D hitBox, Texture2D overlay, Vector2 destCoords, int numFrames, int frameNum, int sizeMultiplier, bool mirrorX, Nullable<int> cycleStart)
			: base(worldX, worldY, overlay, destCoords, numFrames, frameNum, mirrorX, hitBox)
		{
			this.spawn = spawn;
			this.projectileData = projectileData;
			this.maxHealth = maxHealth;
			this.currentHealth = maxHealth;
			this.attackDamage = attackDamage;
			this.vulnerable = vulnerable;
			this.overlay = overlay;
			this.attack = new int[] { 0 };
			this.die = new int[] { 0 };
			this.action = new Status("idle", 0);
			this.attackCooldown = 120;
		}

		public void TakeDamage(int amount, Game1 game)
		{
			this.currentHealth -= amount;
			Hit();
			this.alert = true;
			if (this.currentHealth < 0)
			{
				this.Die(game);
			}
		}

		public void DeathAnimation(Game1 game)
		{
			if (this.status.duration == 0)
			{
				game.RemoveObjectToDraw(this);
				game.worldMap[game.currentRoom].enemies.Remove(this);
				this.Reset();
			}
			else
			{
				if (this.status.duration % this.FRAME_OFFSET == 0) this.frameNum = die[die.GetLength(0)-(this.status.duration / this.FRAME_OFFSET)];
				this.status.duration--;
			}
		}

		public virtual void Die(Game1 game)
		{
			this.spawn.dead = true;
			this.status = new Status("die", this.die.GetLength(0) * this.FRAME_OFFSET);
		}

		public void GravityUpdate(Game1 game)
		{
			if (CollisionDetector.CheckMapCollision(0, 1, this, game.worldMap[game.currentRoom]) == false)
			{
				int i = 0;
				do
				{
					i++;
				} while (CollisionDetector.CheckMapCollision(0, i, this, game.worldMap[game.currentRoom]) == false && i < 9);
				this.worldY += (i - 1);
			}
		}

		public void Reset()
		{
			this.currentHealth = maxHealth;
			this.status.state = "ok";
			this.frameNum = 0;
			this.worldX = spawn.worldX;
			this.worldY = spawn.worldY;
		}

		public void ChooseNewBehavior(Game1 game)
		{
			if (action.state.Equals("attack"))
			{
				action = new Status("idle", attackCooldown);
			}
			else
			{
				if (alert)
				{
					this.action = new Status("attack", this.attack.GetLength(0) * this.FRAME_OFFSET);
				}
				else
				{
					this.action = new Status("idle", attackCooldown);
				}
			}
		}

		public void ChooseNewDirection(Game1 game)
		{
			if (alert)
			{
				if (destRect.X < (Game1.screenWidth / 2) + game.worldMap[game.currentRoom].offset.X) this.mirrorX = true;
				else this.mirrorX = false;
			}
		}

		public void Attack(Game1 game)
		{
			if (this.action.duration % this.FRAME_OFFSET == 0)
			{
				if (attack[attack.GetLength(0) - (this.action.duration / this.FRAME_OFFSET)] == 999)
				{
					game.CreateProjectile(this);
					this.action.duration -= this.FRAME_OFFSET;
				}
				this.frameNum = attack[attack.GetLength(0) - (this.action.duration / this.FRAME_OFFSET)];
			}
		}

		public void UpdateSprite(Game1 game)
		{
			if (this.status.state.Equals("die"))
			{
				DeathAnimation(game);
			}
			else
			{
				if (action.duration == 0)
				{
					ChooseNewBehavior(game);
				}
				if (status.state.Equals("hit"))
				{
					if (status.duration > 0) status.duration--;
				}
				if (action.state.Equals("attack"))
				{
					Attack(game);
				}
				this.onScreen = IsOnScreen(game.worldMap[game.currentRoom]);
				if (IsNearScreen(game.worldMap[game.currentRoom]) == false)
				{
					game.RemoveObjectToDraw(this);
					game.worldMap[game.currentRoom].enemies.Remove(this);
					this.alert = false;
				}
				this.nearScreen = IsNearScreen(game.worldMap[game.currentRoom]);

				ChooseNewDirection(game);
				this.action.duration--;
			}
				this.sourceRect = new Rectangle(this.spriteWidth * this.frameNum, 0, this.texture.Width / numFrames, this.texture.Height);
				if (nearScreen)
				{
					this.destRect.X = (int)(this.worldX - game.worldMap[game.currentRoom].mapCoordinates.X + game.worldMap[game.currentRoom].offset.X);
					this.destRect.Y = (int)(this.worldY - game.worldMap[game.currentRoom].mapCoordinates.Y + game.worldMap[game.currentRoom].offset.Y);
				}
				GravityUpdate(game);
		}
	}
}
