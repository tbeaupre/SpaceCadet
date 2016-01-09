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
		private int bodyFrames;
		private int currentBodyFrame;
		public Texture2D head;
		private int headFrames;
		private int currentHeadFrame;
		public Rectangle headSource;
		public Rectangle bodySource;
		public Directions direction; // 1 = left, 2 = upLeft, 3 = up, 4 = upRight, 5 = right, 6 = down
		private ActionStatus bodyStatus;
		private int runCycleStart = 3;
		private int jumpsRemaining;
		private int maxJumps;
        private double xAirMomentum;
        private double xGroundMomentum;
        private double xVel;
        private double yVel;
        KeyboardState newkeys;
		KeyboardState oldkeys;
		int gunCooldown;
        const int TURN_FRAMES = 10;
        const int SKID_FRAMES = 20;
        List<GunData> arsenal = new List<GunData>();
        GunOverlay guns;
        int currentGun = 0;
        bool hold;

        // Energy
        double maxEnergy = 100;
        double currentEnergy = 5;
        double energyRecoveryRate = 1 / 60;
        // Health
        double maxHealth = 100;
        double currentHealth = 50;

        public void SetEnergyRecoveryRate(double energyRecoveryRate)
        {
            this.energyRecoveryRate = energyRecoveryRate;
        }

        public double GetCurrentHealth()
        {
            return this.currentHealth;
        }

        public double GetMaxHealth()
        {
            return this.maxHealth;
        }

        public void SetMaxHealth(double max)
        {
            this.maxHealth = max;
        }

        public double GetCurrentEnergy()
        {
            return this.currentEnergy;
        }

        public double GetMaxEnergy()
        {
            return this.maxEnergy;
        }

        public void SetMaxEnergy(double max)
        {
            this.maxEnergy = max;
        }

        public GunOverlay GetGuns()
        {
            return this.guns;
        }

        public int GetCurrentGun()
        {
            return currentGun;
        }

        public void SetCurrentGun(int index)
        {
            this.currentGun = index;
            this.guns.SetFrameNum(index);
        }

        public void SetMaxJumps(int jumps)
		{
			this.maxJumps = jumps;
		}

        public void SetBodyStatus(ActionStatus status)
        {
            this.bodyStatus = status;
        }

        public void SetBodyStatusDuration(int dur)
        {
            this.bodyStatus.duration = dur;
        }

        public double GetYVel()
        {
            return this.yVel;
        }

        public void SetYVel(double val)
        {
            this.yVel = val;
        }

        public void SetXVel(double xVel)
        {
            this.xVel = xVel;
        }

        public void SetGunCooldown(int cooldown)
        {
            this.gunCooldown = cooldown;
        }

        public int GetCurrentBodyFrame()
        {
            return this.currentBodyFrame;
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
			this.direction = Directions.right;
			this.bodyStatus = new ActionStatus(ActionStates.Idle, 0);
			this.spriteWidth = head.Width / headFrames;
			this.spriteHeight = body.Height;
			this.sourceRect = new Rectangle(0,0,spriteWidth,spriteHeight);
            this.xVel = 0;
            this.yVel = 0;
			this.gunCooldown = 0;
		}

        public void InitializeArsenal(Texture2D pistolTexture, Texture2D shotgunTexture, Texture2D railgunTexture, Texture2D machinegunTexture, Texture2D bumblegunTexture)
        {
            arsenal.Add(
                new GunData(
                    "G-32_C Phazer Pistol",                                 // name
                    false,                                                  // unlocked
                    0,                                                      // cooldown
                    new StandardProjectile(pistolTexture, 5, -1, 10),       // projectile data
                    false,                                                  // automatic
                    13,                                                     // barrel X
                    8,                                                      // barrel Y
                    7,                                                      // angled barrel X
                    8)                                                      // angled barrel Y
                );

            arsenal.Add(
                new GunData("Flouroantimonic Shotgun", false, 15, new StandardProjectile(shotgunTexture, 5, 15, 20), false, 16, 8, 10, 6));

            arsenal.Add(
                new GunData("IT-6.7 Rail Gun", false, 5, new StandardProjectile(railgunTexture, 5, -1, 50), false, 14, 8, 9, 9));

            arsenal.Add(
                new GunData("Magmatorque Nail-Gun", false, 7, new StandardProjectile(machinegunTexture, 6, -1, 10), true, 18, 7, 10, 6));
             
            arsenal.Add(
                new GunData("Symbionic Hive-Oscilator", false, 10, new SinusoidalProjectile(bumblegunTexture, 2.2, -1, 20, 7, 2), true, 17, 9, 10, 5));
        }
        
        public void InitializeGunOverlay(Texture2D gunsAngleUpTexture, Texture2D gunsAngleDownTexture, Texture2D gunsTexture)
        {
            guns = new GunOverlay(this, gunsAngleUpTexture, gunsAngleDownTexture, gunsTexture,
                                new Vector2(Game1.spaceManX, Game1.spaceManY),
                                5,
                                0,
                                1,
                                false,
                                null);
        }

		public void UpdateHead()// 1 = left, 2 = upLeft, 3 = up, 4 = upRight, 5 = right, 6 = down, 7 = downRight, 8 = downLeft
		{
			if (direction.Equals(Directions.left) || direction.Equals(Directions.right) || direction.Equals(Directions.down) || direction.Equals(Directions.downRight) || direction.Equals(Directions.downLeft))
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
			if (bodyStatus.state == ActionStates.Idle)
			{
				currentBodyFrame = 1;
			}
			if (bodyStatus.state == ActionStates.Walk)
			{
				if (bodyStatus.duration % FRAME_OFFSET == 0)
				{
					currentBodyFrame = bodyStatus.duration / FRAME_OFFSET;
				}
			}
            if (bodyStatus.state == ActionStates.Crouch)
            {
                currentBodyFrame = 0;
            }
            if (bodyStatus.state == ActionStates.Fall)
            {
				if (bodyStatus.duration == 0)
				{
					game.boostJump.incrementTimer();
					currentBodyFrame = game.boostJump.getSpacemanFrame();
					if (currentBodyFrame == 7)
					{
						SetYVel(game.jumpSpeed);
					}
				}
				else
				{
					currentBodyFrame = 5;
				}
            }
            if (bodyStatus.state == ActionStates.Skid)
            {
                currentBodyFrame = 1;
            }
            if (bodyStatus.state == ActionStates.Turn)
            {
                currentBodyFrame = 3;
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

        // Checks to see if there is solid ground below the character and sets the character's state to "fall" if necessary.
        public void GravityUpdate(Game1 game)
        {
			if (!CheckMapCollision(game, 0, 1))
            {
                if (!(bodyStatus.state == ActionStates.Fall))
                {
                    if (yVel > 2) jumpsRemaining--;
                    SetBodyStatus(new ActionStatus(ActionStates.Fall, maxJumps - 1));
                }
				if (CheckMapCollision(game, 0, -1) && yVel < 0)
				{
                    SetYVel(0);
					game.worldMap[game.currentRoom].ChangeCoords(0, 1);
				}
                SetYVel(yVel + game.gravity);

			}
			else
			{
				ResetJump(game);
			}
			if (yVel > game.terminalVel) SetYVel(game.terminalVel);
        }

        // Resets the character to a grounded state
		public void ResetJump(Game1 game)
		{
            if (bodyStatus.state == ActionStates.Fall)
            {
                SetBodyStatus(new ActionStatus(ActionStates.Idle, 0));
                xAirMomentum = 0;
            }
            SetYVel(0);
			jumpsRemaining = maxJumps;
			game.boostJump.reset();
		}

        public Directions HandleDirection(ActionStatus status)
        {
            this.direction = LookDirection();
            switch (status.state)
            {
                case ActionStates.Crouch:
                    if (!hold) return (mirrorX ? Directions.left : Directions.right);
                    else if (direction == Directions.left || direction == Directions.downLeft || direction == Directions.upLeft)
                    {
                        mirrorX = true;
                    }
                    else if (direction == Directions.right || direction == Directions.downRight || direction == Directions.upRight)
                    {
                        mirrorX = false;
                    }
                    return direction;
                case ActionStates.Idle:
                    return direction;
                default:
                    if (mirrorX)
                    {
                        switch (direction)
                        {
                            case Directions.downRight:
                                return Directions.down;
                            case Directions.right:
                                return Directions.left;
                            case Directions.upRight:
                                return Directions.up;
                            default:
                                return direction;
                        }
                    }
                    else
                    {
                        switch (direction)
                        {
                            case Directions.downLeft:
                                return Directions.down;
                            case Directions.left:
                                return Directions.right;
                            case Directions.upLeft:
                                return Directions.up;
                            default:
                                return direction;
                        }
                    }
            }
        }

        // Handles peripheral keys which do not interact with the other movements
        public void HandleKeys(Game1 game)
        {
            if (bodyStatus.state == ActionStates.Fall)
            {
                if (IsKeyHeld(Game1.left))
                {
                    this.xVel -= game.getDirectionalInfluence();
                }
                if (IsKeyHeld(Game1.right))
                {
                    this.xVel += game.getDirectionalInfluence();
                }
                if (IsKeyHeld(Game1.down) && yVel > 0)
                {
                    SetYVel(yVel + game.gravity);
                }
            }
            else if (bodyStatus.state != ActionStates.Jump)
            {
                hold = IsKeyHeld(Game1.hold);
            }

            // firing logic
            if (IsKeyHeld(Game1.fire)
                && this.gunCooldown == 0
                && (arsenal[currentGun].automatic ? true : oldkeys.IsKeyUp(Game1.fire)))
            {
                CreateProjectile(game.worldMap[game.currentRoom]);
                RefreshGunCooldown();
            }
            else
            {
                if (gunCooldown > 0) gunCooldown--;
            }

            // Next Gun
            if (IsKeyPressed(Game1.nextGun))
            {
                NextGun();
            }

            //Update other sprites connected to this one.
            this.guns.mirrorX = mirrorX;
        }

        public ActionStatus HandleKeys(ActionStatus currentStatus)
        {
            ActionStatus result = new ActionStatus(ActionStates.Idle,0);
            bool jumping = (IsKeyPressed(Game1.jump) && jumpsRemaining > 0);
            bool holding = newkeys.IsKeyDown(Game1.hold);
            bool crouching = (newkeys.IsKeyDown(Game1.down) && bodyStatus.state != ActionStates.Fall && !holding);

            // If currently in the air, do certain things.
            if (currentStatus.state == ActionStates.Fall)
            {
                if (jumping)
                {
                    result = new ActionStatus(ActionStates.Jump, currentStatus.duration - 1);
                }
                else
                    result = currentStatus;
            }
            else // Otherwise, check if the character is jumping, crouching, running, turning around, etc.
            {
                if (jumping)
                {
                    result = new ActionStatus(ActionStates.Jump, maxJumps - 1);
                }
                else if (crouching) // If the character is crouching
                {
                    result = new ActionStatus(ActionStates.Crouch, 0);
                }
                else if (xGroundMomentum > 0) // If the character has been moving right
                {
                    if (IsKeyHeld(Game1.left))
                    {
                        if (currentStatus.state == ActionStates.Turn) // If the character is already turning left.
                            result = new ActionStatus(ActionStates.Turn, currentStatus.duration - 1);
                        else
                            result = new ActionStatus(ActionStates.Turn, TURN_FRAMES);
                    }
                    else if (IsKeyHeld(Game1.right))
                    {
                        // check to see if the character is currently running versus just sliding.
                        if (currentStatus.state != ActionStates.Walk)  // If the character was turning around or skidding.
                        {
                            result = new ActionStatus(ActionStates.Walk, FRAME_OFFSET);
                        }
                        else if (currentStatus.duration == (bodyFrames) * FRAME_OFFSET - 1) // check to see if the character has finished the run animation
                        {
                            result = new ActionStatus(ActionStates.Walk, runCycleStart * FRAME_OFFSET);
                        }
                        else
                        {
                            result = new ActionStatus(ActionStates.Walk, currentStatus.duration + 1);
                        }
                    }
                    else
                    {
                        if (currentStatus.state == ActionStates.Turn)
                            result = new ActionStatus(ActionStates.Turn, currentStatus.duration - 1);
                        else if (currentStatus.state == ActionStates.Skid)
                            result = new ActionStatus(ActionStates.Skid, currentStatus.duration - 1);
                        else
                            result = new ActionStatus(ActionStates.Skid, SKID_FRAMES);
                    }
                }
                else if (xGroundMomentum < 0) // If the character has been moving left
                {
                    if (IsKeyHeld(Game1.right))
                    {
                        if (currentStatus.state == ActionStates.Turn) // If the character is already turning right.
                            result = new ActionStatus(ActionStates.Turn, currentStatus.duration - 1);
                        else
                            result = new ActionStatus(ActionStates.Turn, TURN_FRAMES);
                    }
                    else if (IsKeyHeld(Game1.left))
                    {
                        // check to see if the character is currently running versus just sliding.
                        if (currentStatus.state != ActionStates.Walk)  // If the character was turning around or skidding.
                        {
                            result = new ActionStatus(ActionStates.Walk, FRAME_OFFSET);
                        }
                        else if (currentStatus.duration == (bodyFrames) * FRAME_OFFSET - 1) // Check to see if the character has finished the run animation
                        {
                            result = new ActionStatus(ActionStates.Walk, runCycleStart * FRAME_OFFSET);
                        }
                        else
                        {
                            result = new ActionStatus(ActionStates.Walk, currentStatus.duration + 1);
                        }
                    }
                    else // if the character was running a direction and then stopped, he should skid if he is not already turning.
                    {
                        if (currentStatus.state == ActionStates.Turn)
                            result = new ActionStatus(ActionStates.Turn, currentStatus.duration - 1);
                        else if (currentStatus.state == ActionStates.Skid)
                            result = new ActionStatus(ActionStates.Skid, currentStatus.duration - 1);
                        else
                            result = new ActionStatus(ActionStates.Skid, SKID_FRAMES);
                    }
                }
                else // If the character is still.
                {
                    if (IsKeyHeld(Game1.left) && IsKeyHeld(Game1.right)) result = new ActionStatus(ActionStates.Idle, 0);
                    else if (IsKeyHeld(Game1.left))
                    {
                        result = new ActionStatus(ActionStates.Walk, FRAME_OFFSET);
                        mirrorX = true;
                    }
                    else if (IsKeyHeld(Game1.right))
                    {
                        result = new ActionStatus(ActionStates.Walk, FRAME_OFFSET);
                        mirrorX = false;
                    }
                    else if (currentStatus.state == ActionStates.Skid)
                    {
                        result = new ActionStatus(ActionStates.Skid, currentStatus.duration - 1);
                    }
                }
            }
            return result;
        }

        // Checks the different body states and adjusts velocities and momentum appropriately.
        public void HandleStatus(ActionStatus status, Game1 game)
        {
            switch (status.state) {
                case ActionStates.Crouch:
                    xVel = 0;
                    xGroundMomentum = 0;
                    break;
                case ActionStates.Fall:
                    xVel = xAirMomentum;
                    break;
                case ActionStates.Idle:
                    xVel = 0;
                    xGroundMomentum = 0;
                    break;
                case ActionStates.Skid:
                    if (status.duration <= 0)
                    {
                        SetBodyStatus(new ActionStatus(ActionStates.Idle, 0));
                        xGroundMomentum = 0;
                    }
                    else if (mirrorX)
                        xGroundMomentum = (status.duration * -game.moveSpeed) / SKID_FRAMES;
                    else
                        xGroundMomentum = (status.duration * game.moveSpeed) / SKID_FRAMES;
                    xVel = xGroundMomentum;
                    break;
                case ActionStates.Turn:
                    if (status.duration == TURN_FRAMES)
                        mirrorX = !mirrorX;
                    else if (status.duration == 0)
                    {
                        SetBodyStatus(new ActionStatus(ActionStates.Idle, 0));
                        xGroundMomentum = 0;
                    }
                    else if (mirrorX)
                        xGroundMomentum = (status.duration * game.moveSpeed) / TURN_FRAMES;
                    else
                        xGroundMomentum = (status.duration * -game.moveSpeed) / TURN_FRAMES;
                    break;
                case ActionStates.Walk:
                    if (hold)
                    {
                        if (direction == Directions.down || direction == Directions.downLeft || direction == Directions.downRight)
                            SetBodyStatus(new ActionStatus(ActionStates.Crouch, 0));
                        else
                            SetBodyStatus(new ActionStatus(ActionStates.Idle, 0));
                        xGroundMomentum = 0;
                        xVel = 0;
                    }
                    else
                    {
                        if (mirrorX)
                            xGroundMomentum = -game.moveSpeed;
                        else
                            xGroundMomentum = game.moveSpeed;
                        xVel = xGroundMomentum;
                    }
                    break;
                case ActionStates.Jump:
                    SetYVel(game.jumpSpeed);
                    jumpsRemaining--;
                    SetBodyStatus(new ActionStatus(ActionStates.Fall, status.duration));

                    if (jumpsRemaining != 0)
                    {
                        if (xGroundMomentum > 0) xAirMomentum = game.moveSpeed;
                        if (xGroundMomentum < 0) xAirMomentum = -game.moveSpeed;
                        if (xGroundMomentum == 0) xAirMomentum = 0;
                        xGroundMomentum = 0;
                    }
                    break;
                default:
                    SetBodyStatus(new ActionStatus(ActionStates.Idle, 0));
                    xGroundMomentum = 0;
                    xVel = xGroundMomentum;
                    break;
                    }
        }

		public bool IsKeyPressed(Keys key)
		{
			return (newkeys.IsKeyDown(key) && oldkeys.IsKeyUp(key));
		}

        public bool IsKeyHeld(Keys key)
        {
            return newkeys.IsKeyDown(key);
        }

		// Returns a Direction representing which way the character should look based upon which keys are currently pressed.
		public Directions LookDirection()
		{
			Directions result;

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
				if (up) result = Directions.upLeft;
				else if (down) result = Directions.downLeft;
				else result = Directions.left;
			}
			else if (right)
			{
				if (up) result = Directions.upRight;
				else if (down) result = Directions.downRight;
				else result = Directions.right;
			}
			else
			{
				if (up) result = Directions.up;
				else if (down) result = Directions.down;
				else if (mirrorX) result = Directions.left;
				else result = Directions.right;
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
					game.worldMap[game.currentRoom].ChangeCoords(0, yOffset);
					if (yOffset > 0 && CheckMapCollision(game, 0, 1)) ResetJump(game);
				}
				if (!CheckMapCollision(game, xOffset, 0))
					game.worldMap[game.currentRoom].ChangeCoords(xOffset, 0);

				if (xOffset == 0 && !(bodyStatus.state == ActionStates.Fall) && Math.Abs(xVel) > 0)
				{
					CheckDiagonalUp(game);
				}
			}
			else
			{
				game.worldMap[game.currentRoom].ChangeCoords(0, -1);
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
                if (!CheckMapCollision(game, 1, -1) && CheckMapCollision(game, 1, 0))
                {
                    xOffset = 1;
                    yOffset = -1;
					ResetJump(game);
                }
            }
            else
            {
                if (!CheckMapCollision(game, -1, -1) && CheckMapCollision(game, -1, 0))
                {
                    xOffset = -1;
					yOffset = -1;
					ResetJump(game);
                }
            }
			game.worldMap[game.currentRoom].ChangeCoords(xOffset, yOffset);
        }

		// Checks to see if the character is attempting to travel down stairs and reacts accordingly, allowing the player faster travel down stairs
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
						game.worldMap[game.currentRoom].ChangeCoords(xOffset, yOffset);
					}
					else
					{
						if (i == 0)
						{
							if (!CheckMapCollision(game, 1, 0) && CheckMapCollision(game, 0, 1) && !CheckMapCollision(game, 2, 1) && CheckMapCollision(game, 2, 2) && CheckMapCollision(game, 1, 1))
							{
								game.worldMap[game.currentRoom].ChangeCoords(1, 0);
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
                            SetYVel(yVel - i);
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
						game.worldMap[game.currentRoom].ChangeCoords(xOffset, yOffset);
					}
					else
					{
						if (i == 0)
						{
							if (!CheckMapCollision(game, -1, 0) && CheckMapCollision(game, 0, 1) && !CheckMapCollision(game, -2, 1) && CheckMapCollision(game, -2, 2) && CheckMapCollision(game, -1, 1))
							{
								game.worldMap[game.currentRoom].ChangeCoords(-1, 0);
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
                            SetYVel(yVel - i);
							break;
						}
					}
				}
			}
			game.worldMap[game.currentRoom].ChangeCoords(xOffset, yOffset);
			return flag;
		}

		public bool CheckMapCollision(Game1 game, int xOffset, int yOffset)
		{
			if (game.CheckMapCollision(xOffset, yOffset, this)) return true;
			if (game.worldMap[game.currentRoom].assets.Count > 0)
			{
				Spaceman offset = game.worldMap[game.currentRoom].assets[0].offsetSpaceMan(this, xOffset, yOffset);
				foreach (MapAsset asset in game.worldMap[game.currentRoom].assets)
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

        public void RefreshGunCooldown()
        {
            SetGunCooldown(arsenal[currentGun].cooldown);
        }

        public void StopMomentum()
		{
			this.SetXVel(0);
            SetYVel(0);
            SetBodyStatus(new ActionStatus(ActionStates.Idle, 0));
		}

        public void NextGun()
        {
            do
            {
                HelpNext();
                guns.NextFrame(1);
            } while (arsenal[currentGun].unlocked == false);
        }

        void HelpNext()
        {
            currentGun++; // gets the next gun index
            if (currentGun == arsenal.Count)
            {
                currentGun = 0;
            }
        }

        public void UnlockGun(int index)
        {
            this.arsenal[index].unlocked = true;
        }

        public void TakeDamage(Projectile proj)
        {
            TakeDamage(proj.damage);
        }

        public void TakeDamage(int amount)
        {
            if (!status.state.Equals("hit") || (status.state.Equals("hit") && status.duration == 0))
            {
                status = new Status("hit", HIT_DURATION * FRAME_OFFSET);
                currentEnergy -= amount;
                if (currentEnergy < 0)
                {
                    currentHealth += currentEnergy;
                    currentEnergy = 0;
                }
            }
        }

        public void CreateProjectile(Map map)
        {
            map.AddProjectile(arsenal[currentGun].CreateProjectile(this,map.mapCoordinates,
                (int)map.mapCoordinates.X - map.offset.X + guns.GetDestRect().X + FindBulletX(direction, mirrorX),
                (int)map.mapCoordinates.Y - map.offset.Y + guns.GetDestRect().Y + FindBulletY(direction, mirrorX)), false);
        }

        public double FindBulletX(Directions dir, bool mirrorX)
        {
            GunData gun = arsenal[currentGun];
            int barrelX = gun.barrelX;
            int barrelY = gun.barrelY;
            int angledBarrelX = gun.angledBarrelX;
            int angledBarrelY = gun.angledBarrelY;
            switch (dir)
            {
                case Directions.left:
                    return guns.GetSpriteWidth() - barrelX;

                case Directions.upLeft:
                    return guns.GetSpriteWidth() - angledBarrelX - 4;

                case Directions.up:

                    if (mirrorX) return guns.GetSpriteHeight() - barrelY + 11;
                    else return -guns.GetSpriteHeight() + barrelY + 8;

                case Directions.upRight:
                    return angledBarrelX - 1;

                case Directions.right:
                    return barrelX - 6;

                case Directions.downRight:
                    return guns.GetSpriteHeight() - angledBarrelY + 7;

                case Directions.down:
                    if (mirrorX) return barrelY + 3;
                    else return guns.GetSpriteHeight() - barrelY + 1;

                default:
                    return angledBarrelY - 2;
            }
        }

        public double FindBulletY(Directions dir, bool mirrorX)
        {
            GunData gun = arsenal[currentGun];
            int barrelX = gun.barrelX;
            int barrelY = gun.barrelY;
            int angledBarrelX = gun.angledBarrelX;
            int angledBarrelY = gun.angledBarrelY;
            switch (dir)
            {
                case Directions.left:
                    return barrelY - 3;

                case Directions.upLeft:
                    return angledBarrelY - 8;

                case Directions.up:
                    if (mirrorX) return guns.GetSpriteWidth() - barrelX - 7;
                    else return guns.GetSpriteWidth() - barrelX - 1;

                case Directions.upRight:
                    return angledBarrelY - 8;

                case Directions.right:
                    return barrelY - 3;

                case Directions.downRight:
                    return angledBarrelX + 3;

                case Directions.down:
                    if (mirrorX) return guns.GetSpriteWidth() + barrelX - 15;
                    else return barrelX - 2;

                default:
                    return angledBarrelX + 9;
            }
        }

        public void UpdateEnergy()
        {
            if (!status.state.Equals("hit") || (status.state.Equals("hit") && status.duration == 0))
            {
                AddEnergy(energyRecoveryRate);
            }
        }

        public void AddEnergy(double amount)
        {
            currentEnergy += amount;
            if (GetCurrentEnergy() > maxEnergy) currentEnergy = maxEnergy;
            if (GetCurrentEnergy() < 0) currentEnergy = 0;
        }

        public void AddHealth(double amount)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            if (currentHealth < 0) currentHealth = 0;
        }


        public void UpdateSprite(Game1 game)
        {
            guns.UpdateSprite();
			GravityUpdate(game);
            UpdateKeys(game.newkeys);
			if (game.worldMap[game.currentRoom].GetWasJustActivated())
			{
				StopMomentum();
			}
			else
            {
                HandleKeys(game);
                SetBodyStatus(HandleKeys(bodyStatus));
                this.direction = HandleDirection(bodyStatus);
                HandleStatus(bodyStatus, game);
            }
			UpdateHead();
			UpdateBody(game);
			UpdateWorldCoords(game);
			CreateTexture(game);
			UpdateCoords(game.worldMap[game.currentRoom].offset);
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
	}
}
