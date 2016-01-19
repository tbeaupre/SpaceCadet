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
using System.IO;

namespace Spaceman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public int currentRoom;

        public List<Guns> unlockedGuns = new List<Guns>();
        public List<Song> MusicLibrary = new List<Song>();
        public int currentSong;
        public Map[] worldMap;

        const int fullScreenWidth = 1920;
        const int fullScreenHeight = 1080;
        const int SCREEN_MULTIPLIER = 6;
        public const int screenWidth = fullScreenWidth / SCREEN_MULTIPLIER;
        public const int screenHeight = fullScreenHeight / SCREEN_MULTIPLIER;

        public const int spaceManX = screenWidth / 2 - spaceManWidth / 2;
        public const int spaceManY = screenHeight / 2 - spaceManHeight / 2;

        const int FRAME_OFFSET = 5;
        public double moveSpeed;
        private double directionInfluence;
        public double gravity;
        public int terminalVel;
        public double jumpSpeed;
        const int RECOVERY_TIME = 10;

        public KeyboardState newkeys;
        public KeyboardState oldkeys;

        public RenderTarget2D lowRes;

        public Spaceman player;
        public Liquid liquidPlayer;
        Texture2D spaceManHeadTexture;
        Texture2D spaceManBodyTexture;

        Texture2D spaceManTexture;
        const int spaceManWidth = 11;
        const int spaceManHeight = 15;

        Texture2D gunsTexture;
        Texture2D gunsAngleUpTexture;
        Texture2D gunsAngleDownTexture;

        Texture2D PistolBulletTexture;
        Texture2D RailgunBulletTexture;
        Texture2D BumblegunBulletTexture;
        Texture2D MachinegunBulletTexture;
        Texture2D ShotgunBulletTexture;

        Texture2D batteryTexture;
        int[,] batteryLocations = new int[,] { { 383, 718 }, { 440, 718 }, { 1368, 971 } };

        Texture2D healthTexture;
        HealthPickupData[] healthLocations = new HealthPickupData[]
        {
            new HealthPickupData(130, 965, 1),
            new HealthPickupData(145, 965, 2),
            new HealthPickupData(160, 965, 3),
            new HealthPickupData(175, 965, 4),
            new HealthPickupData(190, 965, 5)
        };

        Sprite healthBarOverlay;
        Texture2D healthBarOverlayTexture;

        Sprite energyBar;
        Texture2D energyBarTexture;

        Sprite healthBar;
        Texture2D healthBarTexture;

        Texture2D bioSoldierTexture;

        Texture2D bioSnailProjectileTexture;
        public ProjectileData bioSnailProjectileData;
        public EnemyTextureSet bioSnailTexture;

        Texture2D doorTexture;
        Texture2D doorHitboxTexture;

        Texture2D spaceshipTexture;

        Texture2D saveStationTexture;

        Texture2D boostJumpTexture;
        Texture2D LiquidSpacemanTexture;
        public BoostJump boostJump;

        #region Map Resources

        public Vector2 initMapCoordinates = new Vector2(560, 100); // technically the world coordinates of the top left-hand corner of the screen

        #endregion

        #region Keys
        public const Keys jump = Keys.Z;
        public const Keys fire = Keys.X;
        public const Keys left = Keys.Left;
        public const Keys right = Keys.Right;
        public const Keys up = Keys.Up;
        public const Keys down = Keys.Down;
        public const Keys hold = Keys.LeftShift;
        public const Keys nextGun = Keys.C;
        public const Keys back = Keys.Back;
        public const Keys special1 = Keys.A;
        public const Keys special2 = Keys.S;
        public const Keys special3 = Keys.D;
        #endregion

        List<Portal> portals = new List<Portal>();

        #region Menus

        public Menu currentMenu;
        public Menu lastMenu;

        Menu mainMenu;
        List<IMenuItem> mainMenuItems;

        Menu startMenu;
        List<IMenuItem> startMenuItems;

        public Menu saveStationMenu;
        List<IMenuItem> saveStationMenuItems;

        #endregion

        PowerUpManager powerUpManager = new PowerUpManager();

        public string currentSaveFilepath;
        public StreamWriter currentSaveFile;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = fullScreenWidth;
            graphics.PreferredBackBufferHeight = fullScreenHeight;
            //graphics.IsFullScreen = true;

            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        #region Getters
        public double GetDirectionalInfluence()
        {
            return this.directionInfluence;
        }

        public PowerUpManager GetPowerUpManager()
        {
            return this.powerUpManager;
        }
        #endregion

        #region Setters
        public void setDirectionalInfluence(double di)
        {
            this.directionInfluence = di;
        }
        #endregion

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            powerUpManager.UnlockPowerUp(PowerUps.BoostJump);
            powerUpManager.UnlockPowerUp(PowerUps.Warp);
            powerUpManager.UnlockPowerUp(PowerUps.Liquid);
            powerUpManager.UpdateAbilities(PowerUps.BoostJump, PowerUps.Warp, PowerUps.Liquid, PowerUps.NULL);

            spaceshipTexture = this.Content.Load<Texture2D>("MapResources\\OtherAssets\\Spaceship");

            boostJumpTexture = this.Content.Load<Texture2D>("Boost Jump");

            PistolBulletTexture = this.Content.Load<Texture2D>("Bullets\\PistolBullet");
            MachinegunBulletTexture = this.Content.Load<Texture2D>("Bullets\\MachinegunBullet");
            ShotgunBulletTexture = this.Content.Load<Texture2D>("Bullets\\ShotgunBullet");
            RailgunBulletTexture = this.Content.Load<Texture2D>("Bullets\\RailgunBullet");
            BumblegunBulletTexture = this.Content.Load<Texture2D>("Bullets\\BumblegunBullet");

            gunsTexture = this.Content.Load<Texture2D>("Guns\\Guns");
            gunsAngleUpTexture = this.Content.Load<Texture2D>("Guns\\Guns Angle");
            gunsAngleDownTexture = this.Content.Load<Texture2D>("Guns\\Guns Angle2");

            spaceManTexture = this.Content.Load<Texture2D>("Spaceman");
            spaceManHeadTexture = this.Content.Load<Texture2D>("Spaceman Heads");
            spaceManBodyTexture = this.Content.Load<Texture2D>("Spaceman Body");
            LiquidSpacemanTexture = Content.Load<Texture2D>("LiquidSpaceman");

            player = new Spaceman(spaceManBodyTexture,
                spaceManHeadTexture,
                new Vector2(spaceManX, spaceManY),
                13,
                7,
                false);

            liquidPlayer = new Liquid(LiquidSpacemanTexture,
    spaceManX, spaceManY);



            player.InitializeArsenal(PistolBulletTexture, ShotgunBulletTexture, RailgunBulletTexture, MachinegunBulletTexture, BumblegunBulletTexture);// Placeholder Textures. Put bullet textures here.
            player.InitializeGunOverlay(gunsAngleUpTexture, gunsAngleDownTexture, gunsTexture);

            healthBarOverlayTexture = this.Content.Load<Texture2D>("HUD\\HealthBarOverlay2");
            healthBarOverlay = new Sprite(healthBarOverlayTexture, new Vector2(0, 0), 1, 0, false);

            energyBarTexture = this.Content.Load<Texture2D>("HUD\\EnergyBar");
            energyBar = new Sprite(energyBarTexture, new Vector2(21, 5), 1, 0, false);

            healthBarTexture = this.Content.Load<Texture2D>("HUD\\HealthBar");
            healthBar = new Sprite(healthBarTexture, new Vector2(21, 10), 1, 0, false);

            bioSoldierTexture = this.Content.Load<Texture2D>("Enemies\\BioSoldier");

            bioSnailTexture = new EnemyTextureSet(this.Content.Load<Texture2D>("Enemies\\Bio-Snail\\Bio-Snail"), this.Content.Load<Texture2D>("Enemies\\Bio-Snail\\Bio-Snail Hitbox"), this.Content.Load<Texture2D>("Enemies\\Bio-Snail\\Bio-Snail Vulnerable"));
            bioSnailProjectileTexture = this.Content.Load<Texture2D>("Enemies\\Bio-Snail\\Bio-Snail Projectile");
            bioSnailProjectileData = new ProjectileData(
                5,  // int damage
                2,  // double xVel;
                0,  // double yVel;
                0,  // double yAcc;
                bioSnailProjectileTexture,  // Texture2D texture;
                1,  // int numFrames;
                0,  // int frameNum;
                6,  // int xOffset;
                10  // int yOffset;
                );

            batteryTexture = this.Content.Load<Texture2D>("PickUps\\Battery");
            healthTexture = this.Content.Load<Texture2D>("PickUps\\HealthPickups");

            saveStationTexture = this.Content.Load<Texture2D>("SaveStation");

            #region Menu Setup

            startMenuItems = new List<IMenuItem>();
            startMenuItems.Add(new PortalMenuItem(this.Content.Load<Texture2D>("Menu\\NewMenuItem"), null));
            startMenuItems.Add(new ActionMenuItem(this.Content.Load<Texture2D>("Menu\\LoadMenuItem"), null, "loadMenuItem"));
            startMenu = new Menu(this.Content.Load<Texture2D>("Menu\\MainMenu"), startMenuItems, new Vector2(600, 200));

            mainMenuItems = new List<IMenuItem>();
            mainMenuItems.Add(new PortalMenuItem(this.Content.Load<Texture2D>("Menu\\StartMenuItem"), startMenu));
            mainMenuItems.Add(new PortalMenuItem(this.Content.Load<Texture2D>("Menu\\OptionsMenuItem"), startMenu));
            mainMenu = new Menu(this.Content.Load<Texture2D>("Menu\\MainMenu"), mainMenuItems, new Vector2(600, 200));
            currentMenu = mainMenu;

            saveStationMenuItems = new List<IMenuItem>();
            saveStationMenuItems.Add(new ActionMenuItem(this.Content.Load<Texture2D>("Menu\\SaveMenuItem"), null, "saveMenuItem"));
            saveStationMenuItems.Add(new PortalMenuItem(this.Content.Load<Texture2D>("Menu\\AlterSuitMenuItem"), null));
            saveStationMenu = new Menu(this.Content.Load<Texture2D>("Menu\\SaveStationMenu"), saveStationMenuItems, new Vector2(340, 200));

            #endregion

            #region Initialize Maps

            // Use Map Array
            worldMap = new Map[] {
                new Map(new MapResource(this, "1", true),5), // 1 X 4
				new Map(new MapResource(this, "2", false),5), // 3 X 3
				new Map(new MapResource(this, "3", false), 5), // 3 X 2
			};
            this.currentRoom = 1;
            List<IMapItem> items = new List<IMapItem>();
            items.Add(CreateSaveStation(500, 341));
            worldMap[currentRoom].InitializeMap(items);
            this.worldMap[currentRoom].active = true;
            this.worldMap[currentRoom].mapCoordinates = initMapCoordinates;

            doorTexture = this.Content.Load<Texture2D>("Doors\\Door2");
            doorHitboxTexture = this.Content.Load<Texture2D>("Doors\\DoorHitbox3");

            worldMap[2].spawns.Add(new Spawn(150, 115, "BioSnail"));
            worldMap[currentRoom].assets.Add(new MapAsset(141, 232, spaceshipTexture, worldMap[currentRoom].mapCoordinates, 1, 0, false));

            //Initialize Batteries
            for (int i = 0; i <= batteryLocations.GetUpperBound(0); i++)
            {
                worldMap[currentRoom].pickUps.Add(new Battery(worldMap[currentRoom].mapCoordinates, batteryLocations[i, 0], batteryLocations[i, 1], batteryTexture));
            }

            //Initialize Health
            for (int i = 0; i <= healthLocations.GetUpperBound(0); i++)
            {
                worldMap[currentRoom].pickUps.Add(new Health(worldMap[currentRoom].mapCoordinates, healthLocations[i].x, healthLocations[i].y, healthTexture, healthLocations[i].level));
            }

            //Initialize Doors
            //Door door1 = CreateDoor(1995, 1339, 1, true);
            Door door2_1L = CreateDoor(907, 361, 1, true);
            Door door3_1R = CreateDoor(4, 107, 1, false);
            Door door3_2L = CreateDoor(907, 227, 1, true);
            portals.Add(new Portal(worldMap[1], worldMap[2], door2_1L, door3_1R));
            //portals.Add(new Portal(map3, map3, door3_2L, door3_1R));
            #endregion

            #region Initialize Guns

            UnlockGun(Guns.Pistol);
            UnlockGun(Guns.Shotgun);
            UnlockGun(Guns.Railgun);
            UnlockGun(Guns.MachineGun);
            UnlockGun(Guns.BumbleGun);
            #endregion

            boostJump = new BoostJump(boostJumpTexture);

            InitializePortals(this.portals);

            InitializeMusic();

            SetStandardAttributes();

            UpdateAttributes(powerUpManager.GetCurrentPowerUps());
            base.Initialize();
        }
        public void InitializeMusic()
        {
            MusicLibrary.Add(new Song("demo", "keys", "drums", "bass"));
            currentSong = (int)Songs.demo;
            MusicLibrary[currentSong].initializeCue(Content.RootDirectory);
        }
        public void InitializePortals(List<Portal> portals)
        {
            foreach (Portal p in portals)
            {
                p.Initialize();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lowRes = new RenderTarget2D(graphics.GraphicsDevice, screenWidth, screenHeight);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            oldkeys = newkeys;
            newkeys = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (currentMenu == null) // If the game is in progress
            {
                if (newkeys.IsKeyDown(back) && oldkeys.IsKeyUp(back))
                    currentMenu = mainMenu;
                UpdateObjects();
                player.UpdateEnergy();
            }
            else
            {
                if (newkeys.IsKeyDown(back) && oldkeys.IsKeyUp(back) && currentMenu != mainMenu)
                    currentMenu = lastMenu;
                else
                {
                    currentMenu.UpdateMenu(this);
                }
            }

            base.Update(gameTime);
            MusicLibrary[currentSong].UpdateMusic();
            MusicLibrary[currentSong].SetMusicDynamic(player.GetCurrentGun());
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (currentMenu != this.mainMenu)
            {
                graphics.GraphicsDevice.SetRenderTarget(lowRes);
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                GraphicsDevice.Clear(new Color(0, 0, 0, 0));
                DrawMap(worldMap[currentRoom]);
                if (worldMap[currentRoom].GetWasJustActivated())
                {
                    this.worldMap[currentRoom].SetWasJustActivated(false);
                }
                else
                {
                    DrawSprite(player, 0.6f, player.GetColor(), player.GetDestRectXOffset());
                    for (int i = 0; i < 18; i++)
                    {
                        DrawSprite(liquidPlayer.Pixel(i), 0.6f);
                    }
                    DrawSprite(player.GetGuns(), 0.5f, player.GetColor());
                    DrawOverlay(boostJump, 0.5f, Color.White);

                }
                DrawObjects();
                DrawForeground(worldMap[currentRoom]);
                DrawSprite(healthBarOverlay, 0.1f);
                DrawEnergyBar();
                DrawHealthBar();
                base.Draw(gameTime);
                spriteBatch.End();

                graphics.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                DrawBackground(worldMap[currentRoom]);
                spriteBatch.Draw(lowRes, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                base.Draw(gameTime);
                spriteBatch.End();
            }
            if (currentMenu != null)
            {
                spriteBatch.Begin();
                DrawMenu(currentMenu);
                base.Draw(gameTime);
                spriteBatch.End();
            }
        }

        public void DrawMenu(Menu menu)
        {
            int rectX = (graphics.PreferredBackBufferWidth - menu.background.Width) / 2;
            int rectY = (graphics.PreferredBackBufferHeight - menu.background.Height) / 2;
            spriteBatch.Draw(menu.background,
                new Rectangle(
                    rectX,
                    rectY,
                    menu.background.Width,
                    menu.background.Height),
                null,
                Color.White);
            int deltaY = (int)menu.itemZone.Y / menu.numItems;
            for (int i = 0; i < menu.items.Count; i++)
            {
                IMenuItem item = menu.items[i];
                Rectangle sourceRect;
                if (item.GetIsHighlighted())
                {
                    sourceRect = new Rectangle(0, item.GetTexture().Height / 2, item.GetTexture().Width, item.GetTexture().Height / 2);
                }
                else
                {
                    sourceRect = new Rectangle(0, 0, item.GetTexture().Width, item.GetTexture().Height / 2);
                }
                spriteBatch.Draw(item.GetTexture(),
                    new Rectangle(
                        rectX + (menu.background.Width / 2) - (item.GetTexture().Width / 2),
                        rectY + (int)menu.itemZone.X + (deltaY * i),
                        item.GetTexture().Width,
                        item.GetTexture().Height / 2),
                    sourceRect,
                    Color.White);
            }
        }

        public void DrawMap(Map map)
        {
            spriteBatch.Draw(map.hitbox, new Rectangle((int)(-map.mapCoordinates.X + map.offset.X), (int)(-map.mapCoordinates.Y + map.offset.Y), map.hitbox.Width, map.hitbox.Height), null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);
        }

        public void DrawForeground(Map map)
        {
            if (map.foreground != null)
                spriteBatch.Draw(map.foreground, new Rectangle((int)(-map.mapCoordinates.X + map.offset.X), (int)(-map.mapCoordinates.Y + map.offset.Y), map.foreground.Width, map.foreground.Height), null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);
        }

        public void DrawBackground(Map map)
        {
            spriteBatch.Draw(
                map.background,
                new Rectangle(
                    (int)((-map.mapCoordinates.X + map.offset.X) * (6 / map.parallaxFactor) * ((double)graphics.PreferredBackBufferWidth / (double)fullScreenWidth)),
                    (int)((-map.mapCoordinates.Y + map.offset.Y) * (6 / map.parallaxFactor) * ((double)graphics.PreferredBackBufferHeight / (double)fullScreenHeight)),
                    (int)(map.background.Width * 6 * ((double)graphics.PreferredBackBufferWidth / (double)fullScreenWidth)),
                    (int)(map.background.Height * 6 * ((double)graphics.PreferredBackBufferHeight / (double)fullScreenHeight))),
                null,
                Color.White);
        }

        public void DrawSprite(ISprite sprite, float layer)
        {
            Texture2D texture;
            if (sprite.GetStatus().state.Equals("hit") && (sprite.GetStatus().duration / sprite.GetHitDuration()) % 2 == 1)
                texture = WhiteSilhouette(sprite.GetTexture(), sprite.GetSourceRect());
            else texture = sprite.GetTexture();

            if (sprite.GetMirrorX())
                spriteBatch.Draw(texture, sprite.GetDestRect().ToRectangle(), sprite.GetSourceRect(), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, layer);
            else
                spriteBatch.Draw(texture, sprite.GetDestRect().ToRectangle(), sprite.GetSourceRect(), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, layer);
        }

        public void DrawSprite(ISprite sprite, float layer, Color color, int xOffset)
        {
            Rectangle destRect = sprite.GetDestRect().ToRectangle();
            destRect.X += xOffset;

            Texture2D texture;
            if (sprite.GetStatus().state.Equals("hit") && (sprite.GetStatus().duration / sprite.GetHitDuration()) % 2 == 1)
                texture = WhiteSilhouette(sprite.GetTexture(), sprite.GetSourceRect());
            else texture = sprite.GetTexture();

            if (sprite.GetMirrorX())
                spriteBatch.Draw(texture, destRect, sprite.GetSourceRect(), color, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, layer);
            else
                spriteBatch.Draw(texture, destRect, sprite.GetSourceRect(), color, 0.0f, new Vector2(0, 0), SpriteEffects.None, layer);
        }

        public void DrawOverlay(CharOverlay sprite, float layer, Color color)
        {
            DRectangle originalDest = sprite.getDestRect(this);
            Rectangle newDest;
            if (player.GetMirrorX())
            {
                newDest = new Rectangle((int)originalDest.X, (int)(originalDest.Y + sprite.getYOffset()), originalDest.Width, originalDest.Height);
                spriteBatch.Draw(sprite.getTexture(), newDest, sprite.getSourceRect(this), color, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, layer);
            }
            else
            {
                newDest = new Rectangle((int)(originalDest.X + sprite.getXOffset()), (int)(originalDest.Y + sprite.getYOffset()), originalDest.Width, originalDest.Height);
                spriteBatch.Draw(sprite.getTexture(), newDest, sprite.getSourceRect(this), color, 0.0f, new Vector2(0, 0), SpriteEffects.None, layer);
            }
        }

        public void DrawProjectile(Projectile sprite, float layer)
        {
            Texture2D texture;
            texture = sprite.texture;

            if (sprite.mirrorX)
                spriteBatch.Draw(texture, sprite.destRect.ToRectangle(), sprite.sourceRect, Color.White, (float)FindBulletAngle(sprite.direction, sprite.mirrorX), new Vector2(0, 0), SpriteEffects.FlipHorizontally, layer);
            else
                spriteBatch.Draw(texture, sprite.destRect.ToRectangle(), sprite.sourceRect, Color.White, (float)FindBulletAngle(sprite.direction, sprite.mirrorX), new Vector2(0, 0), SpriteEffects.None, layer);
        }

        public void DrawAllyProjectile(Projectile sprite, float layer)
        {
            Texture2D texture;
            texture = sprite.texture;

            if (sprite.mirrorX)
                spriteBatch.Draw(texture, sprite.destRect.ToRectangle(), sprite.sourceRect, Color.White, (float)FindBulletAngle(sprite.direction, sprite.mirrorX), new Vector2(0, 0), SpriteEffects.FlipHorizontally, layer);
            else
                spriteBatch.Draw(texture, sprite.destRect.ToRectangle(), sprite.sourceRect, Color.White, (float)FindBulletAngle(sprite.direction, sprite.mirrorX), new Vector2(0, 0), SpriteEffects.None, layer);
        }

        public void DrawSprite(GunOverlay sprite, float layer, Color color)
        {
            Texture2D texture;
            if (sprite.GetStatus().state.Equals("hit") && (sprite.GetStatus().duration / sprite.GetHitDuration()) % 2 == 1)
                texture = WhiteSilhouette(sprite.GetTexture(), sprite.GetSourceRect());
            else texture = sprite.GetTexture();

            if (sprite.GetMirrorX())
                spriteBatch.Draw(texture, new Rectangle((int)(sprite.GetDestRect().X + sprite.xOffset), (int)(sprite.GetDestRect().Y + sprite.yOffset), sprite.GetDestRect().Width, sprite.GetDestRect().Height), sprite.GetSourceRect(), color, sprite.angle, new Vector2(0, 0), SpriteEffects.FlipHorizontally, layer);
            else
                spriteBatch.Draw(texture, new Rectangle((int)(sprite.GetDestRect().X + sprite.xOffset), (int)(sprite.GetDestRect().Y + sprite.yOffset), sprite.GetDestRect().Width, sprite.GetDestRect().Height), sprite.GetSourceRect(), color, -sprite.angle, new Vector2(0, 0), SpriteEffects.None, layer);
        }

        public Texture2D WhiteSilhouette(Texture2D texture, Rectangle source)
        {
            Texture2D result = new Texture2D(graphics.GraphicsDevice, texture.Width, texture.Height);
            Color[] pixels = new Color[source.Width * source.Height];
            Color[] newPixels = new Color[source.Width * source.Height];
            texture.GetData<Color>(0, source, pixels, 0, source.Width * source.Height);

            for (int y = source.Height - 1; y >= 0; y--)
            {
                for (int x = source.Width - 1; x >= 0; x--)
                {
                    newPixels[y * source.Width + x] = pixels[y * source.Width + x];
                    if (pixels[y * source.Width + x].A != 0)
                    {
                        newPixels[y * source.Width + x] = Color.White;
                    }
                }
            }

            result.SetData<Color>(0, source, newPixels, 0, source.Width * source.Height);
            return result;
        }

        public void ActivateMap(Map toActivate, Door door)
        {
            this.worldMap[currentRoom].DeactivateMap(this);
            this.worldMap[currentRoom] = toActivate;
            this.worldMap[currentRoom].ActivateMap(door, this);
        }


        #region CreateProjectile

        public double FindBulletXVel(Directions dir, double vel)
        {
            switch (dir)
            {
                case Directions.left:
                    return -vel;

                case Directions.upLeft:
                    return -Math.Cos(Math.PI / 4) * vel;

                case Directions.up:
                    return 0;

                case Directions.upRight:
                    return Math.Cos(Math.PI / 4) * vel;

                case Directions.right:
                    return vel;

                case Directions.downRight:
                    return Math.Cos(Math.PI / 4) * vel;

                case Directions.down:
                    return 0;

                default:
                    return -Math.Cos(Math.PI / 4) * vel;
            }
        }

        public double FindBulletYVel(Directions dir, double vel)
        {
            switch (dir)
            {
                case Directions.left:
                    return 0;

                case Directions.upLeft:
                    return -Math.Cos(Math.PI / 4) * vel;

                case Directions.up:
                    return -vel;

                case Directions.upRight:
                    return -Math.Cos(Math.PI / 4) * vel;

                case Directions.right:
                    return 0;

                case Directions.downRight:
                    return Math.Cos(Math.PI / 4) * vel;

                case Directions.down:
                    return vel;

                default:
                    return Math.Cos(Math.PI / 4) * vel;
            }
        }

        public double FindBulletAngle(Directions dir, bool mirrorX)
        {
            switch (dir)
            {
                case Directions.left:
                    return 0;

                case Directions.upLeft:
                    return 0;

                case Directions.up:
                    if (mirrorX) return Math.PI / 2;
                    else return -Math.PI / 2;

                case Directions.upRight:
                    return 0;

                case Directions.right:
                    return 0;

                case Directions.downRight:
                    return Math.PI / 2;

                case Directions.down:
                    if (mirrorX) return -Math.PI / 2;
                    else return Math.PI / 2;

                default:
                    return -Math.PI / 2;
            }
        }
        #endregion

        public void CreateProjectile(Enemy origin)
        {
            worldMap[currentRoom].enemyProjectiles.Add(
                new Projectile(
                    new StandardProjectile(bioSnailProjectileTexture, origin.projectileData.GetDamage(), -1, origin.projectileData.GetDamage()),
                    origin.mirrorX ? Directions.right : Directions.left,
                    origin,
                    worldMap[currentRoom].mapCoordinates,
                    (origin.mirrorX ? origin.worldX + origin.spriteWidth - origin.projectileData.xOffset : origin.worldX + origin.projectileData.xOffset),
                    origin.worldY + origin.projectileData.yOffset,
                    origin.projectileData.frameNum,
                    origin.mirrorX));
        }

        public void callMenuFunction(String function)
        {
            switch (function)
            {
                case "saveMenuItem": SaveGameData();
                    break;
                case "loadMenuItem": LoadGameData();
                    break;
            }
        }
        
        public void UpdateObjects()
        {
            player.UpdateSprite(this);
            liquidPlayer.UpdateLiquid( 0, player.destRect.Y);
            worldMap[currentRoom].UpdateMap(this);
            UpdateMapAssets();
            UpdateSpawns();
            UpdatePickUps();
            UpdateEnemies();
        }

        public void SpawnEnemy(Enemy enemy)
        {
            worldMap[currentRoom].enemies.Add(enemy);
        }

        public void UpdateMapAssets()
        {
            foreach (MapAsset asset in worldMap[currentRoom].assets)
            {
                asset.UpdateSprite(worldMap[currentRoom]);
                if (asset.onScreen)
                {
                    AddObjectToDraw(asset);
                }
                else RemoveObjectToDraw(asset);
            }
        }

        public void UpdateSpawns()
        {
            foreach (Spawn spawn in worldMap[currentRoom].spawns)
            {
                spawn.Update(this);
            }
        }

        public void UpdateEnemies()
        {
            List<Enemy> current = worldMap[currentRoom].enemies;
            for (int i = current.Count - 1; i >= 0; i--)
            {
                int existenceCheck = current.Count;

                current[i].UpdateSprite(this);

                if (current.Count == existenceCheck)
                {
                    if (current[i].onScreen)
                    {
                        AddObjectToDraw(current[i]);
                    }
                    else RemoveObjectToDraw(current[i]);

                    if (current[i].status.state.Equals("die") == false)
                    {
                        if (CollisionDetector.PerPixelSprite(current[i], player, graphics) > 0)
                        {
                            if (player.status.state != "hit")
                            {
                                player.status = new Status("hit", RECOVERY_TIME);
                                player.TakeDamage(5);
                            }
                        }
                        List<Projectile> currentProjectiles = worldMap[currentRoom].allyProjectiles;
                        for (int j = currentProjectiles.Count - 1; j >= 0; j--)
                        {
                            CollisionState result = CollisionDetector.PerPixelSprite(current[i], currentProjectiles[j], graphics);
                            if ((result == CollisionState.Hurtbox || result == CollisionState.Standard) && currentProjectiles[j].origin == player)
                            {
                                if (result == CollisionState.Hurtbox)
                                {
                                    current[i].TakeDamage(currentProjectiles[j].damage, this);
                                }
                                RemoveObjectToDraw(worldMap[currentRoom].allyProjectiles[j]);
                                worldMap[currentRoom].allyProjectiles.RemoveAt(j);
                            }
                        }
                    }
                }
            }
        }

        public void UpdatePickUps()
        {
            List<PickUp> current = worldMap[currentRoom].pickUps;
            for (int i = current.Count - 1; i >= 0; i--)
            {
                current[i].UpdateSprite(worldMap[currentRoom]);
                if (current[i].onScreen)
                {
                    CollisionState result = CollisionDetector.PerPixelSprite(current[i], player, graphics);
                    if (result == CollisionState.Hurtbox || result == CollisionState.Standard)
                    {
                        current[i].PickUpObj(this);
                        RemoveObjectToDraw(current[i]);
                        worldMap[currentRoom].pickUps.RemoveAt(i);
                    }
                    else
                        AddObjectToDraw(current[i]);
                }
                else RemoveObjectToDraw(current[i]);
            }
        }

        public void RemoveObjectToDraw(IObject obj)
        {
            if (worldMap[currentRoom].objectsToDraw.Contains(obj))
                worldMap[currentRoom].objectsToDraw.Remove(obj);
        }

        public void AddObjectToDraw(Object obj)
        {
            if (worldMap[currentRoom].objectsToDraw.Contains(obj) == false)
                worldMap[currentRoom].objectsToDraw.Add(obj);
        }

        public void UpdateAttributes(List<PowerUps> pUps)
        {
            SetStandardAttributes();
            if (pUps.Contains(PowerUps.BoostJump))
            {
                player.SetMaxJumps(2);
            }
            player.SetCooldown(0, GetPowerUpCooldown(pUps[0]));
            player.SetCooldown(1, GetPowerUpCooldown(pUps[1]));
            player.SetCooldown(2, GetPowerUpCooldown(pUps[2]));
        }

        // Returns the cooldown value of the input power up.
        public int GetPowerUpCooldown(PowerUps pu)
        {
            switch(pu)
            {
                case PowerUps.Warp:
                    return 40;
                default:
                    return 0;
            }
        }

        public void SetStandardAttributes()
        {

            moveSpeed = 2.4;
            directionInfluence = .9;
            gravity = .27;
            terminalVel = 9;
            jumpSpeed = -5;
            player.SetMaxJumps(1);
            player.SetMaxEnergy(100);
            player.SetEnergyRecoveryRate(.15);
            player.SetMaxHealth(100);
            player.SetColor(Color.White);
        }

        public void DrawObjects()
        {
            foreach (IObject obj in worldMap[currentRoom].objectsToDraw)
            {
                if (obj is Enemy)
                {
                    DrawSprite(obj, 0.8f);
                }
                else if (obj is MapAsset)
                {
                    DrawSprite(obj, 0.35f);
                }
                else if (obj is Projectile)
                {
                    Projectile projectile = (Projectile)obj;
                    if (projectile.origin == player)
                    {
                        DrawAllyProjectile(projectile, 0.7f);
                    }
                    else
                    {
                        DrawProjectile(projectile, 0.4f);
                    }
                }
                else DrawSprite(obj, 0.9f);
            }
        }

        public void DrawEnergyBar()
        {
            int x;
            int length;
            if (player.GetCurrentEnergy() == 0) length = 0;
            else
            {
                double ratio = player.GetCurrentEnergy() / player.GetMaxEnergy();
                length = (int)(ratio * (double)energyBarTexture.Width);
            }
            x = energyBarTexture.Width - length;
            Rectangle destRect = new Rectangle((int)energyBar.destRect.X + x, (int)energyBar.destRect.Y, length, energyBarTexture.Height);
            spriteBatch.Draw(energyBar.texture, destRect, energyBar.sourceRect, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);
        }

        public void DrawHealthBar()
        {
            int length;
            if (player.GetCurrentHealth() == 0) length = 0;
            else
            {
                double ratio = player.GetCurrentHealth() / player.GetMaxHealth();
                length = (int)(ratio * (double)healthBarTexture.Width);
            }
            Rectangle destRect = new Rectangle((int)healthBar.destRect.X, (int)healthBar.destRect.Y, length, healthBarTexture.Height);
            spriteBatch.Draw(healthBar.texture, destRect, healthBar.sourceRect, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);
        }

        public Door CreateDoor(double worldX, double worldY, int level, bool isLeft)
        {
            return new Door(worldX, worldY, doorTexture, doorHitboxTexture, this.worldMap[currentRoom].mapCoordinates, level, isLeft);
        }

        public SaveStation CreateSaveStation(double worldX, double worldY)
        {
            return new SaveStation(worldX, worldY, saveStationTexture, this.worldMap[currentRoom].mapCoordinates, 7, 0);
        }

        public void PickUpBattery()
        {
            player.AddEnergy(40);
        }

        public void PickUpHealth(int level)
        {
            switch (level)
            {
                case 1:
                    player.AddHealth(5);
                    break;
                case 2:
                    player.AddHealth(10);
                    break;
                case 3:
                    player.AddHealth(15);
                    break;
                case 4:
                    player.AddHealth(20);
                    break;
                case 5:
                    player.AddHealth(30);
                    break;
                case 6:
                    player.AddHealth(40);
                    break;
            }
        }

        // Unlocks a gun in the arsenal and adds it to the list of unlocked guns
        public void UnlockGun(Guns gun)
        {
            if (!unlockedGuns.Contains(gun))
            {
                unlockedGuns.Add(gun);
                switch (gun)
                {
                    case Guns.Pistol:
                        player.UnlockGun(0);
                        break;
                    case Guns.Shotgun:
                        player.UnlockGun(1);
                        break;
                    case Guns.Railgun:
                        player.UnlockGun(2);
                        break;
                    case Guns.MachineGun:
                        player.UnlockGun(3);
                        break;
                    case Guns.BumbleGun:
                        player.UnlockGun(4);
                        break;
                    default:
                        break;
                }
            }
        }

        // Saves Game Data by serializing it to XML and exporting it to a file
        public void SaveGameData()
        {
            // SaveData is created using current game information.
            SaveData s = new SaveData(currentRoom,
                powerUpManager.GetUnlockedPowerUps(),
                powerUpManager.GetCurrentPowerUps(),
                unlockedGuns,
                player.GetCurrentGun(),
                worldMap[currentRoom].mapCoordinates);

            currentSaveFilepath = "save1.sav";
            currentSaveFile = File.CreateText(currentSaveFilepath);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(s.GetType());
            x.Serialize(currentSaveFile, s);
            currentSaveFile.WriteLine();

            currentSaveFile.Close();
        }


        public void LoadGameData()
        {
            currentSaveFilepath = "save1.sav";
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));
            System.IO.StreamReader file = new System.IO.StreamReader(currentSaveFilepath);
            SaveData saveData = (SaveData)reader.Deserialize(file);
            file.Close();

            currentRoom = saveData.mapIndex;
            powerUpManager.unlockedPowerUps = saveData.unlockedPowerUps;
            powerUpManager.currentPowerUps = saveData.currentPowerUps;
            unlockedGuns = saveData.guns;
            player.SetCurrentGun(saveData.currentGun);
            worldMap[currentRoom].mapCoordinates = saveData.coordinates;
        }

        public void OpenSaveStationMenu()
        {
            this.currentMenu = saveStationMenu;
        }
    }
}

