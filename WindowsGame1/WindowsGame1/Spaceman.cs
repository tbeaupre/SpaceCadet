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
	public class Spaceman:Sprite
	{
		public Texture2D body;
		int bodyFrames;
		public int currentBodyFrame;
		public Texture2D head;
		int headFrames;
		int currentHeadFrame;
		public Rectangle headSource;
		public Rectangle bodySource;
		public Game1.Directions direction; // 1 = left, 2 = upLeft, 3 = up, 4 = upRight, 5 = right, 6 = down
		public Status bodyStatus;
		int runCycleStart = 3;
		bool crouch = false;
		public bool jump = true;
		public int jumpsRemaining;
		private int maxJumps;
		bool hold = false;
        double xVel;
        double yVel;
        KeyboardState newkeys;
		KeyboardState oldkeys;
		int gunCooldown;

		public void SetMaxJumps(int jumps)
		{
			this.maxJumps = jumps;
		}

		public Spaceman(Texture2D body,Texture2D head, Vector2 destCoords, int numFrames, int frameNum, bool mirrorX)
			: base(body, destCoords, numFrames, frameNum, mirrorX)
		{
			this.body = body;
			this.bodyFrames = 13;
			this.currentBodyFrame = 1;
			this.head = head;
			this.headFrames = 3;
			this.currentHeadFrame = 0;
			this.headSource = new Rectangle(0, 0, head.Width / headFrames, head.Height);
			this.bodySource = new Rectangle(0, 0, body.Width / bodyFrames, body.Height);
			this.direction = Game1.Directions.right;
			this.bodyStatus = new Status("idle", 0);
			this.spriteWidth = head.Width / headFrames;
			this.spriteHeight = body.Height;
			this.sourceRect = new Rectangle(0,0,spriteWidth,spriteHeight);
            this.xVel = 0;
            this.yVel = 0;
			this.gunCooldown = 0;
		}

		public void UpdateHead()// 1 = left, 2 = upLeft, 3 = up, 4 = upRight, 5 = right, 6 = down, 7 = downRight, 8 = downLeft
		{
			if (direction.Equals(Game1.Directions.left) || direction.Equals(Game1.Directions.right) || direction.Equals(Game1.Directions.down) || direction.Equals(Game1.Directions.downRight) || direction.Equals(Game1.Directions.downLeft))
			{
				if (currentBodyFrame == 1 || currentBodyFrame == 7 || currentBodyFrame == 8 || currentBodyFrame == 9 || currentBodyFrame == 10 || currentBodyFrame == 11)
				{
					currentHeadFrame = 0;
				}
				else currentHeadFrame = 1;
			}
			else
			{
				currentHeadFrame = 2;
			}
			headSource.X = head.Width / headFrames * currentHeadFrame;
		}

		public void UpdateBody(Game1 game)
		{
			if (bodyStatus.state.Equals("idle"))
			{
				currentBodyFrame = 1;
			}
			if (bodyStatus.state.Equals("walk"))
			{
				if (bodyStatus.duration % FRAME_OFFSET == 0)
				{
					currentBodyFrame = bodyStatus.duration / FRAME_OFFSET;
				}
			}
			if (crouch)
			{
				currentBodyFrame = 0;
		 	}
            if (bodyStatus.state.Equals("fall"))
            {
				if (bodyStatus.duration == 0)
				{
					game.boostJump.incrementTimer();
					currentBodyFrame = game.boostJump.getSpacemanFrame();
					if (currentBodyFrame == 7)
					{
						yVel = game.jumpSpeed;
					}
				}
				else
				{
					currentBodyFrame = 5;
				}
            }
			bodySource.X = spriteWidth * currentBodyFrame;
		}

		public int HeadYOffset()
		{
			if (currentBodyFrame == 0)
			{
				 return 3;
			}

			if (currentBodyFrame == 4 || currentBodyFrame == 5 || currentBodyFrame == 9 || currentBodyFrame == 10)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}

		public int HeadXOffset()
		{
			if (currentBodyFrame == 3 || currentBodyFrame == 7 || currentBodyFrame == 9 || currentBodyFrame == 12)
			{
				return 1;
			} 
			if (currentBodyFrame == 8)
			{
				return 2;
			}
			else
			{
				return 0;
			}
		}

		public void CreateTexture(Game1 game)
		{
			Texture2D newTexture = new Texture2D(game.GraphicsDevice, spriteWidth, spriteHeight);
			Color[] headPixels = new Color[headSource.Width * headSource.Height];
			Color[] bodyPixels = new Color[bodySource.Width * bodySource.Height];
			Color[] finalPixels = new Color[bodySource.Width * bodySource.Height];

			head.GetData<Color>(0, headSource, headPixels, 0, headSource.Width * headSource.Height);
			body.GetData<Color>(0, bodySource, bodyPixels, 0, bodySource.Width * bodySource.Height);

			for (int i = 0; i < headPixels.GetLength(0); i++)
			{
				int j = i + (HeadYOffset() * spriteWidth) + HeadXOffset();
				if (j >= 0 && j < finalPixels.GetLength(0) && j%spriteWidth >= i%spriteWidth)
				finalPixels[j] = headPixels[i];
			}
			for (int i = 0; i < bodyPixels.GetLength(0); i++)
			{
				if (finalPixels[i].A == 0)
				{
					finalPixels[i] = bodyPixels[i];
				}
			}

			newTexture.SetData<Color>(0, new Rectangle(0, 0, bodySource.Width, bodySource.Height), finalPixels, 0, bodySource.Width * bodySource.Height);
			this.texture = newTexture;
		}

        public void GravityUpdate(Game1 game)
        {
			if (!CheckMapCollision(game, 0, 1))
			{
				if (!jump && yVel > 2)
				{
					jumpsRemaining--;
					jump = true;
				}
				if (jump && CheckMapCollision(game, 0, -1) && yVel < 0)
				{
					yVel = 0;
					game.worldMap[game.currentMap].ChangeCoords(0, 1);
				}
				yVel += game.gravity;
				if (!bodyStatus.state.Equals("fall"))
				{
					bodyStatus = new Status("fall", maxJumps - 1);
				}
			}
			else
			{
				ResetJump(game);
			}
			if (yVel > game.terminalVel) yVel = game.terminalVel;
        }

		public void ResetJump(Game1 game)
		{
			if (jump) currentBodyFrame = 1;
			yVel = 0;
			jumpsRemaining = maxJumps;
			jump = false;
			game.boostJump.reset();
		}

		public void HandleKeys2(Game1 game)
		{
			if (newkeys.IsKeyDown(Game1.hold))
			{
				hold = true;
				xVel = 0;
				currentBodyFrame = 1;
			}
			else
			{
				hold = false;
			}

			if (newkeys.IsKeyDown(Game1.jump) && oldkeys.IsKeyUp(Game1.jump) && jumpsRemaining > 0)
			{
				jumpsRemaining--;
				crouch = false;
				if (bodyStatus.state.Equals("fall"))
				{
					bodyStatus.duration--;
				}
				else
				{
					bodyStatus = new Status("fall", maxJumps - 1);
					yVel = game.jumpSpeed;
				}
				jump = true;
			}

			if (newkeys.IsKeyDown(Game1.down))
			{
				if (!jump)
				{
					crouch = true;
					xVel = 0;
					if (mirrorX)
					{
						direction = Game1.Directions.left;
					}
					else
					{
						direction = Game1.Directions.right;
					}
				}
				else
				{
					direction = Game1.Directions.down;
				}
			}
			else
			{
				if (oldkeys.IsKeyDown(Game1.down))
					bodyStatus.duration = 0;
				crouch = false;
			}

			if (newkeys.IsKeyDown(Game1.right))
			{
				if (direction.Equals(Game1.Directions.down))
				{
					direction = Game1.Directions.downRight;
				}
				else
				{
					direction = Game1.Directions.right;
				}
				foreach (Sprite sprite in game.characterSprites)
				{
					sprite.mirrorX = false;
				}
				if (!crouch && !hold)
				{
					xVel = game.moveSpeed;

					if (!jump)
					{
						if (oldkeys.IsKeyDown(Game1.right))
						{
							if (!bodyStatus.state.Equals("walk")) bodyStatus = new Status("walk", FRAME_OFFSET);
							if (bodyStatus.duration == (bodyFrames) * FRAME_OFFSET - 1)
							{
								bodyStatus.duration = runCycleStart * FRAME_OFFSET;
							}
							else
							{
								bodyStatus.duration++;
							}
						}
						else
						{
							bodyStatus = new Status("walk", FRAME_OFFSET);
						}
					}
				}
			}

			if (newkeys.IsKeyDown(Game1.left))
			{
				if (direction.Equals(Game1.Directions.down))
				{
					direction = Game1.Directions.downLeft;
				}
				else
				{
					direction = Game1.Directions.left;
				}
				foreach (Sprite sprite in game.characterSprites)
				{
					sprite.mirrorX = true;
				}
				if (!crouch && !hold)
				{
					xVel = -game.moveSpeed;
					if (!jump)
					{
						if (oldkeys.IsKeyDown(Game1.left))
						{
							if (!bodyStatus.state.Equals("walk")) bodyStatus = new Status("walk", FRAME_OFFSET);
							if (bodyStatus.duration == (bodyFrames) * FRAME_OFFSET - 1)
							{
								bodyStatus.duration = runCycleStart * FRAME_OFFSET;
							}
							else
							{
								bodyStatus.duration++;
							}
						}
						else
						{
							bodyStatus = new Status("walk", FRAME_OFFSET);
						}
					}
				}
			}

			if (newkeys.IsKeyDown(Game1.up))
			{
				direction = Game1.Directions.up;
				if (newkeys.IsKeyDown(Game1.left)) direction = Game1.Directions.upLeft;
				if (newkeys.IsKeyDown(Game1.right)) direction = Game1.Directions.upRight;
			}

			if (newkeys.IsKeyUp(Game1.left) && newkeys.IsKeyUp(Game1.right))
			{
				if (!jump)
				bodyStatus = new Status("idle", 0);
				xVel = 0;
			}

			if (newkeys.IsKeyUp(Game1.left) && newkeys.IsKeyUp(Game1.right) && newkeys.IsKeyUp(Game1.up) && newkeys.IsKeyUp(Game1.down))
			{
				if (mirrorX)
				{
					direction = Game1.Directions.left;
				}
				else
				{
					direction = Game1.Directions.right;
				}
			}

			if (newkeys.IsKeyDown(Game1.nextGun) && oldkeys.IsKeyUp(Game1.nextGun))
            {
                game.NextGun();
            }

			if (newkeys.IsKeyDown(Game1.fire)
				&& this.gunCooldown == 0
				&& (game.arsenal[game.currentGun].automatic ? true : oldkeys.IsKeyUp(Game1.fire)))
			{
				game.CreateProjectile(this);
				game.RefreshGunCooldown();
			}
			else
			{
				if (gunCooldown > 0) gunCooldown--;
			}
		}

		public void HandleKeys(Game1 game)
		{
			int testXVel = 0;
			bool crouching = (newkeys.IsKeyDown(Game1.down) && !bodyStatus.state.Equals("fall"));
			bool jumping = (IsKeyPressed(Game1.jump) && jumpsRemaining > 0);
			bool holding = newkeys.IsKeyDown(Game1.hold);
			Game1.Directions lookDir = LookDirection();

			if (jumping)
			{
				jumpsRemaining--;
				crouching = false;
				if (bodyStatus.state.Equals("fall"))
				{
					bodyStatus.duration--;
				}
				else
				{
					bodyStatus = new Status("fall", maxJumps - 1);
					yVel = game.jumpSpeed;
				}
			}
			else if(bodyStatus.state.Equals("fall")) 
			{
				jumping = true;
			}

			//Left/Right controls
			if (newkeys.IsKeyDown(Game1.left))
			{
				testXVel = -(int)game.moveSpeed;
				if (!crouching && !jumping)
				{
					if (oldkeys.IsKeyDown(Game1.left))
					{
						if (!bodyStatus.state.Equals("walk")) bodyStatus = new Status("walk", FRAME_OFFSET);
						if (bodyStatus.duration == (bodyFrames) * FRAME_OFFSET - 1)
						{
							bodyStatus.duration = runCycleStart * FRAME_OFFSET;
						}
						else
						{
							bodyStatus.duration++;
						}
					}
					else
					{
						bodyStatus = new Status("walk", FRAME_OFFSET);
					}
				}
			}
			else if (newkeys.IsKeyDown(Game1.right))
			{
				testXVel = (int)game.moveSpeed;
				if (!crouching && !jumping)
				{
					if (oldkeys.IsKeyDown(Game1.right))
					{
						if (!bodyStatus.state.Equals("walk")) bodyStatus = new Status("walk", FRAME_OFFSET);
						if (bodyStatus.duration == (bodyFrames) * FRAME_OFFSET - 1)
						{
							bodyStatus.duration = runCycleStart * FRAME_OFFSET;
						}
						else
						{
							bodyStatus.duration++;
						}
					}
					else
					{
						bodyStatus = new Status("walk", FRAME_OFFSET);
					}
				}
			}

			// Changes the mirroring of sprites related to the spaceman depending upon lookDir
			if (lookDir == Game1.Directions.left || lookDir == Game1.Directions.upLeft || lookDir == Game1.Directions.downLeft)
			{
				foreach (Sprite sprite in game.characterSprites)
				{
					sprite.mirrorX = true;
				}
			}
			else if (lookDir == Game1.Directions.right || lookDir == Game1.Directions.upRight || lookDir == Game1.Directions.downRight)
			{
				foreach (Sprite sprite in game.characterSprites)
				{
					sprite.mirrorX = false;
				}
			}

			if (crouching)
			{
				crouch = true;
				testXVel = 0;
			}
			else crouch = false;

			// Checks for idle.
			if (newkeys.IsKeyUp(Game1.left) && newkeys.IsKeyUp(Game1.right))
			{
				if (!jump)
					bodyStatus = new Status("idle", 0);
				testXVel = 0;
			}

			// If the character is holding or crouching, the xVel should be 0.
			if (holding)
			{
				testXVel = 0;
			}

			// Check for Next Gun.
			if (IsKeyPressed(Game1.nextGun))
			{
				game.NextGun();
			}

			// Fires bullets if necessary.
			Fire(game, newkeys.IsKeyDown(Game1.fire) && this.gunCooldown == 0 && (game.arsenal[game.currentGun].automatic ? true : oldkeys.IsKeyUp(Game1.fire)));

			direction = lookDir;
			xVel = testXVel;
		}

		// Fires a projectile if conditions are correct or decreases the cooldown on the gun.
		public void Fire(Game1 game, bool condition)
		{
			if (condition)
			{
				game.CreateProjectile(this);
				game.RefreshGunCooldown();
			}
			else
			{
				if (gunCooldown > 0) gunCooldown--;
			}
		}

		public bool IsKeyPressed(Keys key)
		{
			return (newkeys.IsKeyDown(key) && oldkeys.IsKeyUp(key));
		}

		// Returns a Direction representing which way the character should look based upon which keys are currently pressed.
		public Game1.Directions LookDirection()
		{
			Game1.Directions result;

			// Initialize boolean variables to represent whether or not a button was pressed.
			bool up = newkeys.IsKeyDown(Game1.up);
			bool down = newkeys.IsKeyDown(Game1.down);
			bool left = newkeys.IsKeyDown(Game1.left);
			bool right = newkeys.IsKeyDown(Game1.right);

			// Check to see if opposite directions were both pressed.
			if (up && down) down = false;
			if (left && right)
			{
				//prioritize the LR direction that corresponds to the direction the character is already facing.
				if (mirrorX) right = false;
				else left = false;
			}

			// Sets result equal to a direction based upon which keys are currently pressed. 
			if (left)
			{
				if (up) result = Game1.Directions.upLeft;
				else if (down) result = Game1.Directions.downLeft;
				else result = Game1.Directions.left;
			}
			else if (right)
			{
				if (up) result = Game1.Directions.upRight;
				else if (down) result = Game1.Directions.downRight;
				else result = Game1.Directions.right;
			}
			else
			{
				if (up) result = Game1.Directions.up;
				else if (down) result = Game1.Directions.down;
				else if (mirrorX) result = Game1.Directions.left;
				else result = Game1.Directions.right;
			}

			return result;
		}

        public void UpdateCoords(Vector2 offset)
        {
            destRect.X = Game1.spaceManX + (int)offset.X;
            destRect.Y = Game1.spaceManY + (int)offset.Y;
        }

        public void UpdateWorldCoords(Game1 game)
        {
            int yOffset;
			int xOffset;
			bool stairs = false;

            yOffset = FindYOffset(game);
			if (yOffset > 0 && CheckMapCollision(game, 0, yOffset + 1))
			{
				SetXVel(0);
			}
			xOffset = FindXOffset(game, yOffset);
			if (yOffset == 0)
			{
				stairs = CheckDiagonalDown(game);
			}
			if (!stairs)
			{
				if (!CheckMapCollision(game, 0, yOffset))
				{
					game.worldMap[game.currentMap].ChangeCoords(0, yOffset);
					if (yOffset > 0 && CheckMapCollision(game, 0, 1)) ResetJump(game);
				}
				if (!CheckMapCollision(game, xOffset, 0))
					game.worldMap[game.currentMap].ChangeCoords(xOffset, 0);

				if (xOffset == 0 && !jump && Math.Abs(xVel) > 0)
				{
					CheckDiagonalUp(game);
				}
			}
			else
			{
				game.worldMap[game.currentMap].ChangeCoords(0, -1);
			}
        }

        public int FindYOffset(Game1 game)
        {
            if (yVel > 0)
            {
                for (int i = (int)yVel; i >= 0; i--)
                {
					if (!CheckMapCollision(game, 0, i))
                        return i;
                }
            }
            if (yVel < 0)
            {
                for (int i = (int)yVel; i <= 0; i++)
                {
					if (!CheckMapCollision(game, 0, i))
                        return i;
                }
            }
            return 0;
        }

        public int FindXOffset(Game1 game, int yOffset)
        {
            if (xVel > 0)
            {
                for (int i = (int)xVel; i >= 0; i--)
                {
					if (!CheckMapCollision(game, i, yOffset))
                        return i;
                }
            }
            if (xVel < 0)
            {
                for (int i = (int)xVel; i <= 0; i++)
                {
                    if (!CheckMapCollision(game, i, yOffset))
                        return i;
                }
            }
            return 0;
        }

		// Checks to see if the character is attempting to travel up stairs and reacts accordingly, allowing the player faster travel up stairs
        public void CheckDiagonalUp(Game1 game)
        {
            int xOffset = 0;
            int yOffset = 0;

            if (xVel > 0)
            {
                if (!CheckMapCollision(game, 1, -1))
                {
                    xOffset = 1;
                    yOffset = -1;
					ResetJump(game);
                }
            }
            else
            {
                if (!CheckMapCollision(game, -1, -1))
                {
                    xOffset = -1;
					yOffset = -1;
					ResetJump(game);
                }
            }
			game.worldMap[game.currentMap].ChangeCoords(xOffset, yOffset);
        }

		// Checks to see if the character is attempting to travel up stairs and reacts accordingly, allowing the player faster travel up stairs
		public bool CheckDiagonalDown(Game1 game)
		{
			bool flag = true;
			int xOffset = 0;
			int yOffset = 0;
			
			if (xVel > 0)
			{
				for (int i = 0; i < 1; i++)
				{
					if (!CheckMapCollision(game, 1, 1) && CheckMapCollision(game, 1, 2) && CheckMapCollision(game, 0, 1))
					{
						xOffset = 1;
						yOffset = 1;
						game.worldMap[game.currentMap].ChangeCoords(xOffset, yOffset);
					}
					else
					{
						if (i == 0)
						{
							if (!CheckMapCollision(game, 1, 0) && CheckMapCollision(game, 0, 1) && !CheckMapCollision(game, 2, 1) && CheckMapCollision(game, 2, 2) && CheckMapCollision(game, 1, 1))
							{
								game.worldMap[game.currentMap].ChangeCoords(1, 0);
							}
							else
							{
								flag = false;
								break;
							}
						}
						else
						{
							if (i > 0)
							{
								ResetJump(game);
								xVel -= 1;
							}
							xVel -= i;
							yVel -= i;
							break;
						}
					}
				}

			}
			else
			{
				for (int i = 0; i < 1; i++)
				{
					if (!CheckMapCollision(game, -1, 1) && CheckMapCollision(game, -1, 2) && CheckMapCollision(game, 0, 1))
					{
						xOffset = -1;
						yOffset = 1;
						game.worldMap[game.currentMap].ChangeCoords(xOffset, yOffset);
					}
					else
					{
						if (i == 0)
						{
							if (!CheckMapCollision(game, -1, 0) && CheckMapCollision(game, 0, 1) && !CheckMapCollision(game, -2, 1) && CheckMapCollision(game, -2, 2) && CheckMapCollision(game, -1, 1))
							{
								game.worldMap[game.currentMap].ChangeCoords(-1, 0);
							}
							else
							{
								flag = false;
								break;
							}
						}
						else
						{
							if (i > 0)
							{
								ResetJump(game);
								xVel += 1;
							}
							xVel += i;
							yVel -= i;
							break;
						}
					}
				}
			}
			game.worldMap[game.currentMap].ChangeCoords(xOffset, yOffset);
			return flag;
		}

		public bool CheckMapCollision(Game1 game, int xOffset, int yOffset)
		{
			if (game.CheckMapCollision(xOffset, yOffset, this)) return true;
			if (game.worldMap[game.currentMap].assets.Count > 0)
			{
				Spaceman offset = game.worldMap[game.currentMap].assets[0].offsetSpaceMan(this, xOffset, yOffset);
				foreach (MapAsset asset in game.worldMap[game.currentMap].assets)
				{
						if (asset.RectCollisionDetect(offset)) return true;
				}
			}
			return false;
		}

		public void UpdateKeys(KeyboardState keys)
		{
			oldkeys = newkeys;
			newkeys = keys;
		}

		public void StopMomentum()
		{
			this.SetXVel(0);
			this.yVel = 0;
		}

		public void UpdateSprite(Game1 game)
        {
			GravityUpdate(game);
			UpdateKeys(game.newkeys);
			if (game.worldMap[game.currentMap].wasJustActivated)
			{
				StopMomentum();
			}
			else
			{
				HandleKeys(game);
			}
			UpdateHead();
			UpdateBody(game);
			UpdateWorldCoords(game);
			CreateTexture(game);
			UpdateCoords(game.worldMap[game.currentMap].offset);
			if (status.state.Equals("hit"))
			{
				if (status.duration > 0) status.duration--;
			}
			if (newkeys.IsKeyDown(Game1.special1))
			{
				this.destRect.Height = 2;
			}
			else this.destRect.Height = this.texture.Height;
		}

		public void SetXVel(double xVel)
		{
			this.xVel = xVel;
		}

		public double GetYVel()
		{
			return this.yVel;
		}

		public void SetGunCooldown(int cooldown)
		{
			this.gunCooldown = cooldown;
		}
	}
}
