using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace SuperNova
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Declarations
        #region graphics declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice details;
        Vector2 graphicsInfo;
        #endregion
        #region player declarations
        Player player = new Player();
        Texture2D playerTexture;
        float scale = 1f;
        int lives = 0;
        bool dead = false;
        #endregion
        #region background declarations
        parallaxBackground bgLayer1, bgLayer2, mainMenuBackground1, mainMenuBackground2, NebulaBG1, NebulaBG2, ScrapBG1, ScrapBG2;
        Texture2D mainBackground;
        Rectangle rectBackgound;
        #endregion
        #region enemy declarations
        Texture2D [] enemyTexture = new Texture2D[2];
        EnemyManager enemyManager = new EnemyManager();
        #endregion
        #region laser declarations
        Texture2D[] laserTexture = new Texture2D[2];
        LaserManager LaserBeans = new LaserManager();
        LaserManager EnemyLaser = new LaserManager();
        SoundEffect laserSound;
        #endregion
        #region sound declarations
        Song Level1Theme;
        Song mainMenuTheme;
        Sound SND = new Sound();
        Sound exSND = new Sound();
        Sound puSND = new Sound();
        Sound slSND = new Sound();
        Sound enSND = new Sound();
        SoundEffect exSound;
        SoundEffect puSound;
        SoundEffect slSound;
        SoundEffect enSound;
        enum musicState
        {
            Playing,
            NotPlaying,
        }
        musicState MusicState = musicState.NotPlaying;
        #endregion
        #region effects declarations
        Texture2D explosionVFX;
        ExplosionManager VFX = new ExplosionManager();
        #endregion
        #region pause declarations
        int elapsedTime;
        bool paused = false;
        const int DELAY = 200;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;
        #endregion
        #region GUI declarations
        GUI gui;
        #endregion
        #region background items declarations
        BackgroundItemsManager BGItems = new BackgroundItemsManager();
        float bGMoveSpeed;
        #endregion
        #region powerup declarations
        PowerupManager powerMan = new PowerupManager();
        #endregion
        #region gameState declarations
        enum GameState
        {
            MainMenu,
            Death,
            Win,
            Credits,
            Tutorial,
            LevelOne,
            LevelTwo,
            LevelThree,
        }
        GameState CurrentGameState = GameState.MainMenu;
        GameState lastGameState;
        #endregion
        #region tutorialState
        enum TutState
        {
            Welcome,
            Move,
            Shoot,
            Score,
            Power,
        }
        TutState currentTutState = TutState.Welcome;
        TutState lastTutState;
        bool tutFinish;
        #endregion
        #region button declarations
        Button buttonPlay = new Button();
        Button buttonEndless = new Button();
        Button buttonTut = new Button();
        Button buttonExit = new Button();
        Button buttonMenu = new Button();
        Button buttonCredit = new Button();
        Texture2D playButtonTexture;
        Texture2D endlessButtonTexture;
        Texture2D playButtonTextureOver;
        Texture2D endlessButtonTextureOver;
        Texture2D tutButtonTexture;
        Texture2D tutButtonTextureOver;
        Texture2D exitButtonTexture;
        Texture2D exitButtonTextureOver;
        Texture2D menuButtonTexture;
        Texture2D menuButtonTextureOver;
        Texture2D creditButtonTexture;
        Texture2D creditButtonTextureOver;
        Vector2 playButtonPos;
        Vector2 endlessButtonPos;
        Vector2 tutButtonPos;
        Vector2 exitButtonPos;
        Vector2 menuButtonPos;
        Vector2 creditButtonPos;
        #endregion
        #region logo declrations
        Rectangle logoRect = new Rectangle(0, 0, 400, 109);
        Vector2 logoPos;
        Texture2D logoTexture;
        #endregion
        #region notifications declarations
        int count = 0;
        int delayLevel = 100;
        int tutDelayLevel = 800;
        int tutDelayFinal = 1000;
        SpriteFont scoreFont, labelFont, alertFont;
        #endregion
        #region mouse declarations
        Pointer pointer = new Pointer();
        #endregion
        #region endless variables
        bool endless = false;
        int finalScoreBoundary;
        #endregion
        Texture2D backDrop;
        bool devMode = false;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = false;
            //Make game full screen.
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            
            gui = new GUI();
            Debug.WriteLine("viewport width is " + GraphicsDevice.Viewport.Width);
            Debug.WriteLine("viewport height is " + GraphicsDevice.Viewport.Height);
            bgLayer1 = new parallaxBackground();
            bgLayer2 = new parallaxBackground();
            mainMenuBackground1 = new parallaxBackground();
            mainMenuBackground2 = new parallaxBackground();
            NebulaBG1 = new parallaxBackground();
            NebulaBG2 = new parallaxBackground();
            ScrapBG1 = new parallaxBackground();
            ScrapBG2 = new parallaxBackground();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Below This
            #region graphics stuff
            // Graphics Info Load
            graphicsInfo.X = GraphicsDevice.Viewport.Width;
            graphicsInfo.Y = GraphicsDevice.Viewport.Height;
            details = GraphicsDevice;
            #endregion
            #region Player Content
            Animation playerAnimation = new Animation();
            playerTexture = Content.Load<Texture2D>("FullAnimation");
            playerAnimation.Intialize(playerTexture, Vector2.Zero, 154, 51, 8, 100, Color.White, scale, true);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.initialize(playerAnimation, playerPosition, graphicsInfo);
            gui.Initialize(0, 100, 100, lives, 1, powerMan, enemyManager, LaserBeans); //UPDATE WITH VARIABLES WHEN POSSIBLE
            #endregion
            #region background content
            logoTexture = Content.Load<Texture2D>("SuperNovaLogo");
            mainBackground = Content.Load<Texture2D>("mainbackground(1)");
            bgLayer1.initialize(Content, "stars1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 12);
            bgLayer2.initialize(Content, "stars2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 10);
            mainMenuBackground1.initialize(Content, "stars1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 12);
            mainMenuBackground2.initialize(Content, "stars2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 10);
            NebulaBG1.initialize(Content, "NebulaBG1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 5);
            NebulaBG2.initialize(Content, "NebulaBG2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 3);
            ScrapBG1.initialize(Content, "ScrapBG1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 1f);
            ScrapBG2.initialize(Content, "ScrapBG2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 2);
            logoPos = new Vector2 (215, GraphicsDevice.Viewport.Height/4);
            #endregion
            #region powerup content
            powerMan.loadItems(Content);
            powerMan.Intiialize(details);
            #endregion
            #region laser content
            laserTexture[0] = Content.Load<Texture2D>("laser");
            laserTexture[1] = Content.Load<Texture2D>("EnemyLaserOrb");
            laserSound = Content.Load<SoundEffect>("Laser_Shoot");
            LaserBeans.Initialize(laserTexture[0], details);
            SND.Initialize(laserSound);
            #endregion
            #region enemy content
            enemyTexture[0] = Content.Load<Texture2D>("enemy01Animation");
            enemyTexture[1] = Content.Load<Texture2D>("enemy02Animation");
            enemyManager.Initialize(enemyTexture, details, laserTexture[1]);
            enemyManager.Load(Content);
            #endregion
            #region visual effects content
            explosionVFX = Content.Load<Texture2D>("explosionSpriteSheet");
            VFX.Initialize(explosionVFX, details);
            VFX.Load(Content);
            #endregion
            #region sound content
            // Sound Load
            Level1Theme = Content.Load<Song>("SuperNovaTheme1");
            mainMenuTheme = Content.Load<Song>("mainMenuTheme");
            exSound = Content.Load<SoundEffect>("SNExplosion3");
            puSound = Content.Load<SoundEffect>("SNPowerup2");
            slSound = Content.Load<SoundEffect>("SNSelect1");
            enSound = Content.Load<SoundEffect>("SNLazerEnemy");
            exSND.Initialize(exSound);
            puSND.Initialize(puSound);
            slSND.Initialize(slSound);
            enSND.Initialize(enSound);
            #endregion
            #region GUI declarations
            gui.LoadContent(Content);
            #endregion
            #region background content
            BGItems.loadItems(Content);
            BGItems.Intiialize(details);
            #endregion
            #region button load
            playButtonTexture = Content.Load<Texture2D>("playButton");
            endlessButtonTexture = Content.Load<Texture2D>("endlessButton");
            playButtonTextureOver = Content.Load<Texture2D>("playOver");
            endlessButtonTextureOver = Content.Load<Texture2D>("endOver");
            tutButtonTexture = Content.Load<Texture2D>("tutButton");
            tutButtonTextureOver = Content.Load<Texture2D>("tutOver");
            exitButtonTexture = Content.Load<Texture2D>("exitButton");
            exitButtonTextureOver = Content.Load<Texture2D>("exitOver");
            menuButtonTexture = Content.Load<Texture2D>("menuButton");
            menuButtonTextureOver = Content.Load<Texture2D>("menuOver");
            creditButtonTexture = Content.Load<Texture2D>("creditsButton");
            creditButtonTextureOver = Content.Load<Texture2D>("creditsOver");
            playButtonPos = new Vector2(350, 270);
            endlessButtonPos = new Vector2(350, 310);
            tutButtonPos = new Vector2(350, 350);
            exitButtonPos = new Vector2(350, 390);
            menuButtonPos = new Vector2(350, 430);
            creditButtonPos = new Vector2(600, 430);
            buttonPlay.initialzie(Content, details, playButtonPos, playButtonTexture, playButtonTextureOver);
            buttonEndless.initialzie(Content, details, endlessButtonPos, endlessButtonTexture, endlessButtonTextureOver);
            buttonTut.initialzie(Content, details, tutButtonPos, tutButtonTexture, tutButtonTextureOver);
            buttonExit.initialzie(Content, details, exitButtonPos, exitButtonTexture, exitButtonTextureOver);
            buttonMenu.initialzie(Content, details, menuButtonPos, menuButtonTexture, menuButtonTextureOver);
            buttonCredit.initialzie(Content, details, creditButtonPos, creditButtonTexture, creditButtonTextureOver);
            #endregion
            #region cursor load
            pointer.LoadContent(Content);
            #endregion
            #region notification content
            scoreFont = Content.Load<SpriteFont>("scoreFont");
            labelFont = Content.Load<SpriteFont>("labelFont");
            alertFont = Content.Load<SpriteFont>("alertFont");
            #endregion
            backDrop = Content.Load<Texture2D>("pauseBackdrop");
            CurrentGameState = GameState.MainMenu;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            MouseState mouse = Mouse.GetState();
            #region GameState
            switch (CurrentGameState)
            {
                #region MainMenuState
                case GameState.MainMenu:

                    #region buttonUpdates
                    if (buttonPlay.isClicked == true)
                    {
                        buttonPlay.isClicked = false;
                        endless = false;
                        gui.LIVES = 3;
                        gui.SCORE = 0;
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        CurrentGameState = GameState.LevelOne;
                        break;
                    }

                    if (buttonEndless.isClicked == true)
                    {
                        buttonEndless.isClicked = false;
                        endless = true;
                        gui.LIVES = 3;
                        gui.SCORE = 0;
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        CurrentGameState = GameState.LevelOne;
                        break;
                    }

                    if (buttonTut.isClicked == true)
                    {
                        buttonTut.isClicked = false;
                        gui.LIVES = 3;
                        gui.SCORE = 0;
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        CurrentGameState = GameState.Tutorial;
                        break;
                    }

                    if (buttonExit.isClicked == true)
                    {
                        buttonExit.isClicked = false;
                        Exit();
                    }

                    if (buttonCredit.isClicked == true)
                    {
                        buttonCredit.isClicked = false;
                        CurrentGameState = GameState.Credits;
                        break;
                    }

                    #endregion

                    buttonPlay.Update(pointer, slSND);
                    buttonEndless.Update(pointer, slSND);
                    buttonTut.Update(pointer, slSND);
                    buttonExit.Update(pointer, slSND);
                    buttonCredit.Update(pointer, slSND);
                    mainMenuBackground1.update();
                    mainMenuBackground2.update();
                    logoRect.X = (int)logoPos.X;
                    logoRect.Y = (int)logoPos.Y;
                    pointer.Update();
                    if (MusicState == musicState.Playing && CurrentGameState != lastGameState && lastGameState != GameState.Credits)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }
                    if (MusicState == musicState.NotPlaying)
                    {
                        MediaPlayer.Play(mainMenuTheme);
                        MusicState = musicState.Playing;
                    }
                    if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
                    lastGameState = CurrentGameState;
                    break;
                #endregion
                #region TutorialState
                case GameState.Tutorial:

                    switch (currentTutState)
                    {
                        case TutState.Welcome:
                            #region pause and main updates
                            #region control state updates
                            previousKeyboardState = currentKeyboardState;
                            currentKeyboardState = Keyboard.GetState();
                            previousGamePadState = currentGamePadState;
                            currentGamePadState = GamePad.GetState(PlayerIndex.One);
                            #endregion
                            #region pause toggle
                            if (elapsedTime >= DELAY)
                            {
                                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && currentGamePadState != previousGamePadState || Keyboard.GetState().IsKeyDown(Keys.Enter) && currentKeyboardState != previousKeyboardState)
                                {
                                    if (paused == false)
                                    {
                                        paused = true;
                                    }
                                    else
                                    {
                                        paused = false;
                                    }
                                    elapsedTime = 0;
                                }
                            }
                            #endregion
                            #region timer for pause delay
                            //timer used for delay between each pause cycle
                            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                            #endregion
                            #region pausable updates
                            //if the game is paused then prevent updates to the apropriate methods to freeze the game
                            //all updates to the game world MUST be put in here for pause to function effectively
                            if (paused == false)
                            {
                                if (tutDelayLevel != count)
                                {
                                    count++;
                                }
                                else
                                {
                                    currentTutState = TutState.Move;
                                    count = 0;
                                }

                                bgLayer1.update();
                                bgLayer2.update();
                                player.update(gameTime, gui);
                                LaserBeans.UpdateManagerLaser(gameTime, player, SND, exSND,  VFX, gui);
                                enemyManager.UpdateEnemies(gameTime, player, VFX, exSND, spriteBatch);
                                enemyManager.UpdateEnemyLaser(gameTime, player, enSND, exSND, VFX, gui);
                                VFX.updateExplosions(gameTime);
                                BGItems.UpdatePlanets(gameTime);
                                powerMan.UpdatePowerups(gameTime, player, LaserBeans, puSND, gui);
                            }
                            #endregion
                            #endregion
                            #region non pausable updates
                            if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }
                            if (MusicState == musicState.NotPlaying)
                            {
                                MediaPlayer.Play(Level1Theme);
                                MusicState = musicState.Playing;
                            }
                            if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }

                            if (devMode == true)
                            {
                                if (currentKeyboardState.IsKeyDown(Keys.P))
                                    player.Health = -98989;
                            }

                            if (player.Health <= 0)
                            {
                                
                            }
                            #endregion
                            lastTutState = currentTutState;
                            break;

                        case TutState.Move:
                            #region pause and main updates
                            #region control state updates
                            previousKeyboardState = currentKeyboardState;
                            currentKeyboardState = Keyboard.GetState();
                            previousGamePadState = currentGamePadState;
                            currentGamePadState = GamePad.GetState(PlayerIndex.One);
                            #endregion
                            #region pause toggle
                            if (elapsedTime >= DELAY)
                            {
                                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && currentGamePadState != previousGamePadState || Keyboard.GetState().IsKeyDown(Keys.Enter) && currentKeyboardState != previousKeyboardState)
                                {
                                    if (paused == false)
                                    {
                                        paused = true;
                                    }
                                    else
                                    {
                                        paused = false;
                                    }
                                    elapsedTime = 0;
                                }
                            }
                            #endregion
                            #region timer for pause delay
                            //timer used for delay between each pause cycle
                            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                            #endregion
                            #region pausable updates
                            //if the game is paused then prevent updates to the apropriate methods to freeze the game
                            //all updates to the game world MUST be put in here for pause to function effectively
                            if (paused == false)
                            {

                                if (tutDelayLevel != count)
                                {
                                    count++;
                                }
                                else
                                {
                                    currentTutState = TutState.Shoot;
                                    count = 0;
                                }
                                

                                bgLayer1.update();
                                bgLayer2.update();
                                player.update(gameTime, gui);
                                LaserBeans.UpdateManagerLaser(gameTime, player, SND, exSND, VFX, gui);
                                enemyManager.UpdateEnemies(gameTime, player, VFX, exSND, spriteBatch);
                                enemyManager.UpdateEnemyLaser(gameTime, player, enSND, exSND, VFX, gui);
                                VFX.updateExplosions(gameTime);
                                BGItems.UpdatePlanets(gameTime);
                                powerMan.UpdatePowerups(gameTime, player, LaserBeans, puSND, gui);
                            }
                            #endregion
                            #endregion
                            #region non pausable updates
                            if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }
                            if (MusicState == musicState.NotPlaying)
                            {
                                MediaPlayer.Play(Level1Theme);
                                MusicState = musicState.Playing;
                            }
                            if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }

                            if (devMode == true)
                            {
                                if (currentKeyboardState.IsKeyDown(Keys.P))
                                    player.Health = -98989;
                            }

                            if (player.Health <= 0)
                            {
                               
                            }
                            #endregion
                            lastTutState = currentTutState;
                            break;

                        case TutState.Shoot:
                            #region pause and main updates
                            #region control state updates
                            previousKeyboardState = currentKeyboardState;
                            currentKeyboardState = Keyboard.GetState();
                            previousGamePadState = currentGamePadState;
                            currentGamePadState = GamePad.GetState(PlayerIndex.One);
                            #endregion
                            #region pause toggle
                            if (elapsedTime >= DELAY)
                            {
                                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && currentGamePadState != previousGamePadState || Keyboard.GetState().IsKeyDown(Keys.Enter) && currentKeyboardState != previousKeyboardState)
                                {
                                    if (paused == false)
                                    {
                                        paused = true;
                                    }
                                    else
                                    {
                                        paused = false;
                                    }
                                    elapsedTime = 0;
                                }
                            }
                            #endregion
                            #region timer for pause delay
                            //timer used for delay between each pause cycle
                            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                            #endregion
                            #region pausable updates
                            //if the game is paused then prevent updates to the apropriate methods to freeze the game
                            //all updates to the game world MUST be put in here for pause to function effectively
                            if (paused == false)
                            {

                               
                                if (tutDelayLevel != count)
                                {
                                    count++;
                                }
                                else
                                {
                                    currentTutState = TutState.Score;
                                    count = 0;
                                }
                                

                                bgLayer1.update();
                                bgLayer2.update();
                                player.update(gameTime, gui);
                                LaserBeans.UpdateManagerLaser(gameTime, player, SND, exSND, VFX, gui);
                                enemyManager.UpdateEnemies(gameTime, player, VFX, exSND, spriteBatch);
                                enemyManager.UpdateEnemyLaser(gameTime, player, enSND, exSND, VFX, gui);
                                VFX.updateExplosions(gameTime);
                                BGItems.UpdatePlanets(gameTime);
                                powerMan.UpdatePowerups(gameTime, player, LaserBeans, puSND, gui);
                            }
                            #endregion
                            #endregion
                            #region non pausable updates
                            if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }
                            if (MusicState == musicState.NotPlaying)
                            {
                                MediaPlayer.Play(Level1Theme);
                                MusicState = musicState.Playing;
                            }
                            if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }

                            if (devMode == true)
                            {
                                if (currentKeyboardState.IsKeyDown(Keys.P))
                                    player.Health = -98989;
                            }

                            if (player.Health <= 0)
                            {
                                
                            }
                            #endregion
                            lastTutState = currentTutState;
                            break;

                        case TutState.Score:
                            #region pause and main updates
                            #region control state updates
                            previousKeyboardState = currentKeyboardState;
                            currentKeyboardState = Keyboard.GetState();
                            previousGamePadState = currentGamePadState;
                            currentGamePadState = GamePad.GetState(PlayerIndex.One);
                            #endregion
                            #region pause toggle
                            if (elapsedTime >= DELAY)
                            {
                                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && currentGamePadState != previousGamePadState || Keyboard.GetState().IsKeyDown(Keys.Enter) && currentKeyboardState != previousKeyboardState)
                                {
                                    if (paused == false)
                                    {
                                        paused = true;
                                    }
                                    else
                                    {
                                        paused = false;
                                    }
                                    elapsedTime = 0;
                                }
                            }
                            #endregion
                            #region timer for pause delay
                            //timer used for delay between each pause cycle
                            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                            #endregion
                            #region pausable updates
                            //if the game is paused then prevent updates to the apropriate methods to freeze the game
                            //all updates to the game world MUST be put in here for pause to function effectively
                            if (paused == false)
                            {

                               
                                if (tutDelayLevel != count)
                                {
                                    count++;
                                }
                                else
                                {
                                    currentTutState = TutState.Power;
                                    count = 0;
                                }

                                bgLayer1.update();
                                bgLayer2.update();
                                player.update(gameTime, gui);
                                LaserBeans.UpdateManagerLaser(gameTime, player, SND, exSND, VFX, gui);
                                enemyManager.UpdateEnemies(gameTime, player, VFX, exSND, spriteBatch);
                                enemyManager.UpdateEnemyLaser(gameTime, player, enSND, exSND, VFX, gui);
                                VFX.updateExplosions(gameTime);
                                BGItems.UpdatePlanets(gameTime);
                                powerMan.UpdatePowerups(gameTime, player, LaserBeans, puSND, gui);
                            }
                            #endregion
                            #endregion
                            #region non pausable updates
                            if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }
                            if (MusicState == musicState.NotPlaying)
                            {
                                MediaPlayer.Play(Level1Theme);
                                MusicState = musicState.Playing;
                            }
                            if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }

                            if (devMode == true)
                            {
                                if (currentKeyboardState.IsKeyDown(Keys.P))
                                    player.Health = -98989;
                            }

                            if (player.Health <= 0)
                            {
                                
                            }
                            #endregion
                            lastTutState = currentTutState;
                            break;

                        case TutState.Power:
                            #region pause and main updates
                            #region control state updates
                            previousKeyboardState = currentKeyboardState;
                            currentKeyboardState = Keyboard.GetState();
                            previousGamePadState = currentGamePadState;
                            currentGamePadState = GamePad.GetState(PlayerIndex.One);
                            #endregion
                            #region pause toggle
                            if (elapsedTime >= DELAY)
                            {
                                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && currentGamePadState != previousGamePadState || Keyboard.GetState().IsKeyDown(Keys.Enter) && currentKeyboardState != previousKeyboardState)
                                {
                                    if (paused == false)
                                    {
                                        paused = true;
                                    }
                                    else
                                    {
                                        paused = false;
                                    }
                                    elapsedTime = 0;
                                }
                            }
                            #endregion
                            #region timer for pause delay
                            //timer used for delay between each pause cycle
                            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                            #endregion
                            #region pausable updates
                            //if the game is paused then prevent updates to the apropriate methods to freeze the game
                            //all updates to the game world MUST be put in here for pause to function effectively
                            if (paused == false)
                            {

                                if (tutDelayFinal != count)
                                {
                                    count++;
                                    tutFinish = false;
                                }
                                else
                                {
                                    tutFinish = true;
                                    if (tutFinish == true && GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.M))
                                    {
                                        player.initialize();
                                        enemyManager.clean();
                                        LaserBeans.clean();
                                        powerMan.clean();
                                        paused = false;
                                        CurrentGameState = GameState.MainMenu;
                                        count = 0;
                                    }

                                    if (tutFinish == true && GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                                    {
                                        player.initialize();
                                        enemyManager.clean();
                                        LaserBeans.clean();
                                        powerMan.clean();
                                        paused = false;
                                        currentTutState = TutState.Welcome;
                                        count = 0;
                                    }
                                }
                                
                                bgLayer1.update();
                                bgLayer2.update();
                                player.update(gameTime, gui);
                                LaserBeans.UpdateManagerLaser(gameTime, player, SND, exSND, VFX, gui);
                                enemyManager.UpdateEnemies(gameTime, player, VFX, exSND, spriteBatch);
                                enemyManager.UpdateEnemyLaser(gameTime, player, enSND, exSND, VFX, gui);
                                VFX.updateExplosions(gameTime);
                                BGItems.UpdatePlanets(gameTime);
                                powerMan.UpdatePowerups(gameTime, player, LaserBeans, puSND, gui);
                            }
                            #endregion
                            #endregion
                            #region non pausable updates
                            if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }
                            if (MusicState == musicState.NotPlaying)
                            {
                                MediaPlayer.Play(Level1Theme);
                                MusicState = musicState.Playing;
                            }
                            if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                            {
                                MediaPlayer.Stop();
                                MusicState = musicState.NotPlaying;
                            }

                            if (devMode == true)
                            {
                                if (currentKeyboardState.IsKeyDown(Keys.P))
                                    player.Health = -98989;
                            }

                            if (player.Health <= 0)
                            {
                                
                            }
                            #endregion
                            lastTutState = currentTutState;
                            break;
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        gui.SCORE = 0;
                        paused = false;
                        CurrentGameState = GameState.MainMenu;
                        MusicState = musicState.NotPlaying;
                    }
                    lastGameState = CurrentGameState;
                    break;
                #endregion
                #region LevelOneState
                case GameState.LevelOne:
                    #region endless boundary setting
                    if (endless == true)
                    {
                        finalScoreBoundary = 2147483647;
                    }
                    else
                    {
                        finalScoreBoundary = 75000;
                    }
                    #endregion
                    if (lastGameState != CurrentGameState)
                    {
                        count = 0;
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        VFX.clean();
                    }
                    if (gui.SCORE >= 15000)
                    {
                        CurrentGameState = GameState.LevelTwo;
                        break;
                    }

                    #region pause and main updates
                    #region control state updates
                    previousKeyboardState = currentKeyboardState;
                    currentKeyboardState = Keyboard.GetState();
                    previousGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(PlayerIndex.One);
                    #endregion
                    #region pause toggle
                    if (elapsedTime >= DELAY)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && currentGamePadState != previousGamePadState || Keyboard.GetState().IsKeyDown(Keys.Enter) && currentKeyboardState != previousKeyboardState)
                        {
                            if (paused == false)
                            {
                                paused = true;
                            }
                            else
                            {
                                paused = false;
                            }
                            elapsedTime = 0;
                        }
                    }
                    #endregion
                    #region timer for pause delay
                    //timer used for delay between each pause cycle
                    elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    #endregion
                    #region pausable updates
                    //if the game is paused then prevent updates to the apropriate methods to freeze the game
                    //all updates to the game world MUST be put in here for pause to function effectively
                    if (paused == false)
                    {

                        if (delayLevel != count)
                        {
                            count++;
                        }
                        else
                        {
                            enemyManager.UpdateEnemies(gameTime, player, VFX, exSND, spriteBatch);
                        }

                        bgLayer1.update();
                        bgLayer2.update();
                        player.update(gameTime, gui);
                        LaserBeans.UpdateManagerLaser(gameTime, player, SND, exSND, VFX, gui);
                        enemyManager.UpdateEnemyLaser(gameTime, player, enSND, exSND, VFX, gui);
                        VFX.updateExplosions(gameTime);
                        BGItems.UpdatePlanets(gameTime);
                        powerMan.UpdatePowerups(gameTime, player, LaserBeans, puSND, gui);
                    }
                    #endregion
                    #endregion
                    #region non pausable updates
                    if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }
                    if (MusicState == musicState.NotPlaying)
                    {
                        MediaPlayer.Play(Level1Theme);
                        MusicState = musicState.Playing;
                    }
                    if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }

                    if (devMode == true)
                    {
                        if (currentKeyboardState.IsKeyDown(Keys.P))
                            player.Health = -98989;
                    }

                    if (player.Health <= 0)
                    {
                        if (gui.LIVES > 0)
                        {
                            if (buttonPlay.clicked(slSND))
                                buttonPlay.isClicked = false;
                            Debug.WriteLine("subtract lives");
                            gui.LIVES--;
                            player.initialize();
                            Debug.WriteLine(gui.LIVES);
                            break;
                        }
                        else
                        {
                            dead = true;
                            Debug.WriteLine("dead");
                            CurrentGameState = GameState.Death;
                            break;
                        }
                    }
                    #endregion

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        VFX.clean();
                        MusicState = musicState.NotPlaying;
                        paused = false;
                        CurrentGameState = GameState.MainMenu;
                    }
                    lastGameState = CurrentGameState;
                    break;
                #endregion
                #region LevelTwoState
                case GameState.LevelTwo:
                    if (lastGameState != CurrentGameState)
                    {
                        count = 0;
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        VFX.clean();
                    }

                    if (gui.SCORE >= 45000)
                    {
                        CurrentGameState = GameState.LevelThree;
                        MusicState = musicState.NotPlaying;
                        break;
                    }
                    #region pause and main updates
                    #region control state updates
                    previousKeyboardState = currentKeyboardState;
                    currentKeyboardState = Keyboard.GetState();
                    previousGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(PlayerIndex.One);
                    #endregion
                    #region pause toggle
                    if (elapsedTime >= DELAY)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && currentGamePadState != previousGamePadState || Keyboard.GetState().IsKeyDown(Keys.Enter) && currentKeyboardState != previousKeyboardState)
                        {
                            if (paused == false)
                            {
                                paused = true;
                            }
                            else
                            {
                                paused = false;
                            }
                            elapsedTime = 0;
                        }
                    }
                    #endregion
                    #region timer for pause delay
                    //timer used for delay between each pause cycle
                    elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    #endregion
                    #region pausable updates
                    //if the game is paused then prevent updates to the apropriate methods to freeze the game
                    //all updates to the game world MUST be put in here for pause to function effectively
                    if (paused == false)
                    {

                            if (delayLevel != count)
                            {
                                count++;
                            }
                            else
                            {
                                enemyManager.UpdateEnemies(gameTime, player, VFX, exSND, spriteBatch);
                                LaserBeans.UpdateManagerLaser(gameTime, player, SND, exSND, VFX, gui);
                                VFX.updateExplosions(gameTime);
                                powerMan.UpdatePowerups(gameTime, player, LaserBeans, puSND, gui);
                            }
                        mainMenuBackground1.update();
                        mainMenuBackground2.update();
                        NebulaBG1.update();
                        NebulaBG2.update();
                        enemyManager.UpdateEnemyLaser(gameTime, player, enSND, exSND, VFX, gui);
                        player.update(gameTime, gui);
                        BGItems.UpdatePlanets(gameTime);
                    }
                    #endregion
                    #endregion
                    #region non pausable updates
                    if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }
                    if (MusicState == musicState.NotPlaying)
                    {
                        MediaPlayer.Play(Level1Theme);
                        MusicState = musicState.Playing;
                    }
                    if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }


                    if (player.Health <= 0)
                    {
                        if (gui.LIVES > 0)
                        {
                            if (buttonPlay.clicked(slSND))
                                buttonPlay.isClicked = false;

                            Debug.WriteLine("subtract lives");
                            gui.LIVES--;
                            player.initialize();
                            Debug.WriteLine(gui.LIVES);
                            break;
                        }
                        else
                        {
                            dead = true;
                            Debug.WriteLine("dead");
                            CurrentGameState = GameState.Death;
                            break;
                        }
                    }
                    #endregion
                    //exit code will be added to pause menu when available
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        VFX.clean();
                        MusicState = musicState.NotPlaying;
                        paused = false;
                        CurrentGameState = GameState.MainMenu;
                    }
                    lastGameState = CurrentGameState;
                    buttonPlay.isClicked = false;
                    break;
                #endregion
                #region LevelThreeState
                case GameState.LevelThree:
                    if (lastGameState != CurrentGameState)
                    {
                        count = 0;
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        VFX.clean();
                    }

                    if (gui.SCORE >= finalScoreBoundary)
                    {
                        CurrentGameState = GameState.Win;
                        MusicState = musicState.NotPlaying;
                        break;
                    }
                    #region pause and main updates
                    #region control state updates
                    previousKeyboardState = currentKeyboardState;
                    currentKeyboardState = Keyboard.GetState();
                    previousGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(PlayerIndex.One);
                    #endregion
                    #region pause toggle
                    if (elapsedTime >= DELAY)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && currentGamePadState != previousGamePadState || Keyboard.GetState().IsKeyDown(Keys.Enter) && currentKeyboardState != previousKeyboardState)
                        {
                            if (paused == false)
                            {
                                paused = true;
                            }
                            else
                            {
                                paused = false;
                            }
                            elapsedTime = 0;
                        }
                    }
                    #endregion
                    #region timer for pause delay
                    //timer used for delay between each pause cycle
                    elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    #endregion
                    #region pausable updates
                    //if the game is paused then prevent updates to the apropriate methods to freeze the game
                    //all updates to the game world MUST be put in here for pause to function effectively
                    if (paused == false)
                    {

                        if (delayLevel != count)
                        {
                            count++;
                        }
                        else
                        {
                            enemyManager.UpdateEnemies(gameTime, player, VFX, exSND, spriteBatch);
                            LaserBeans.UpdateManagerLaser(gameTime, player, SND, exSND, VFX, gui);
                            VFX.updateExplosions(gameTime);
                            powerMan.UpdatePowerups(gameTime, player, LaserBeans, puSND, gui);
                        }
                        mainMenuBackground1.update();
                        mainMenuBackground2.update();
                        ScrapBG1.update();
                        ScrapBG2.update();
                        enemyManager.UpdateEnemyLaser(gameTime, player, enSND, exSND, VFX, gui);
                        player.update(gameTime, gui);
                        BGItems.UpdatePlanets(gameTime);
                    }
                    #endregion
                    #endregion
                    #region non pausable updates
                    if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }
                    if (MusicState == musicState.NotPlaying)
                    {
                        MediaPlayer.Play(Level1Theme);
                        MusicState = musicState.Playing;
                    }
                    if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }


                    if (player.Health <= 0)
                    {
                        if (gui.LIVES > 0)
                        {
                            if (buttonPlay.clicked(slSND))
                                buttonPlay.isClicked = false;

                            Debug.WriteLine("subtract lives");
                            gui.LIVES--;
                            player.initialize();
                            Debug.WriteLine(gui.LIVES);
                            break;
                        }
                        else
                        {
                            dead = true;
                            Debug.WriteLine("dead");
                            CurrentGameState = GameState.Death;
                            break;
                        }
                    }
                    #endregion
                    //exit code will be added to pause menu when available
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        VFX.clean();
                        MusicState = musicState.NotPlaying;
                        paused = false;
                        CurrentGameState = GameState.MainMenu;
                    }
                    lastGameState = CurrentGameState;
                    buttonPlay.isClicked = false;
                    break;
                #endregion
                #region DeathState
                case GameState.Death:
                    gui.SCORE = 0;
                    if (gui.LIVES > 0)
                    {
                        gui.LIVES--;
                        break;
                    }
                    else
                    {
                        player.initialize();
                        player.Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
                        if (buttonMenu.isClicked == true)
                        {
                            buttonMenu.isClicked = false;
                            CurrentGameState = GameState.MainMenu;
                            LaserBeans.enemiesDestroyed = 0;
                            powerMan.numOfPower = 0;
                            enemyManager.enemiesDestroyed = 0;
                            break;
                        }

                        pointer.Update();
                        buttonMenu.Update(pointer, slSND);
                        //mainMenuBackground1.update();
                        //mainMenuBackground2.update();
                        
                        logoRect.X = (int)logoPos.X;
                        logoRect.Y = (int)logoPos.Y;
                        if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                        {
                            MediaPlayer.Stop();
                            MusicState = musicState.NotPlaying;
                        }
                        if (MusicState == musicState.NotPlaying)
                        {
                            MediaPlayer.Play(mainMenuTheme);
                            MusicState = musicState.Playing;
                        }
                        if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                        {
                            MediaPlayer.Stop();
                            MusicState = musicState.NotPlaying;
                        }
                        lastGameState = CurrentGameState;
                    }
                    break;
                #endregion
                #region WinState
                case GameState.Win:
                    gui.SCORE = 0;
                    if (gui.LIVES > 0)
                    {
                        gui.LIVES--;
                        break;
                    }
                    else
                    {
                        player.initialize();
                        player.Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
                        if (buttonMenu.isClicked == true)
                        {
                            buttonMenu.isClicked = false;
                            CurrentGameState = GameState.MainMenu;
                            break;
                        }

                        pointer.Update();
                        buttonMenu.Update(pointer, slSND);
                        //mainMenuBackground1.update();
                        //mainMenuBackground2.update();

                        logoRect.X = (int)logoPos.X;
                        logoRect.Y = (int)logoPos.Y;
                        if (MusicState == musicState.Playing && CurrentGameState != lastGameState)
                        {
                            MediaPlayer.Stop();
                            MusicState = musicState.NotPlaying;
                        }
                        if (MusicState == musicState.NotPlaying)
                        {
                            MediaPlayer.Play(mainMenuTheme);
                            MusicState = musicState.Playing;
                        }
                        if (MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                        {
                            MediaPlayer.Stop();
                            MusicState = musicState.NotPlaying;
                        }
                        lastGameState = CurrentGameState;
                    }
                    break;
                #endregion
                #region CreditsState
                case GameState.Credits:
                        if (buttonMenu.isClicked == true)
                        {
                            buttonMenu.isClicked = false;
                            CurrentGameState = GameState.MainMenu;
                            break;
                        }

                    if (MusicState == musicState.Playing && CurrentGameState != lastGameState && lastGameState != GameState.MainMenu)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }
                    if (MusicState == musicState.NotPlaying)
                    {
                        MediaPlayer.Play(mainMenuTheme);
                        MusicState = musicState.Playing;
                    }
                    if(MediaPlayer.State == MediaState.Stopped || MediaPlayer.State == MediaState.Paused)
                    {
                        MediaPlayer.Stop();
                        MusicState = musicState.NotPlaying;
                    }

                    buttonMenu.Update(pointer, slSND);
                        mainMenuBackground1.update();
                        mainMenuBackground2.update();
                        pointer.Update();
                        lastGameState = CurrentGameState;
                    break;
                    #endregion
            }
            #endregion
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            #region GameStateDraw
            switch (CurrentGameState)
            {
                #region DrawMainMenuState
                case GameState.MainMenu:
                    spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                    
                    mainMenuBackground1.Draw(spriteBatch);
                    mainMenuBackground2.Draw(spriteBatch);
                    spriteBatch.Draw(logoTexture, logoRect, Color.White);
                    MouseState mouse = Mouse.GetState();
                    buttonPlay.Draw(spriteBatch);
                    buttonEndless.Draw(spriteBatch);
                    buttonTut.Draw(spriteBatch);
                    buttonExit.Draw(spriteBatch);
                    buttonCredit.Draw(spriteBatch);
                    pointer.Draw(spriteBatch, mouse, details);
                    break;
                #endregion              
                #region DrawTutorialState
                case GameState.Tutorial:
                    switch (currentTutState)
                    {
                        case TutState.Welcome:
                            #region draw code
                            #region background
                            spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                            BGItems.DrawPlanets(spriteBatch);
                            bgLayer1.Draw(spriteBatch);
                            bgLayer2.Draw(spriteBatch);
                            #endregion
                            #region main content
                            enemyManager.DrawEnemies(spriteBatch);
                            LaserBeans.DrawLasers(spriteBatch);
                            VFX.DrawExplosions(spriteBatch);
                            player.draw(spriteBatch);
                            #endregion
                            #region powerups
                            powerMan.DrawPowerups(spriteBatch);
                            #endregion
                            #region GUI
                            gui.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, paused);
                            #endregion
                            #endregion
                            if (tutDelayLevel != count && paused == false)
                            {
                                spriteBatch.DrawString(alertFont, "Welcome To SuperNova", new Vector2(10, 320), Color.Red);
                                spriteBatch.DrawString(labelFont, "Want to learn how to play?", new Vector2(10, 390), Color.Red);
                                spriteBatch.DrawString(labelFont, "You're in the right Place.", new Vector2(10, 430), Color.Red);
                            }
                            break;

                        case TutState.Move:
                            #region draw code
                            #region background
                            spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                            BGItems.DrawPlanets(spriteBatch);
                            bgLayer1.Draw(spriteBatch);
                            bgLayer2.Draw(spriteBatch);
                            #endregion
                            #region main content
                            enemyManager.DrawEnemies(spriteBatch);
                            LaserBeans.DrawLasers(spriteBatch);
                            VFX.DrawExplosions(spriteBatch);
                            player.draw(spriteBatch);
                            #endregion
                            #region powerups
                            powerMan.DrawPowerups(spriteBatch);
                            #endregion
                            #region GUI
                            gui.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, paused);
                            #endregion
                            #endregion
                            if (tutDelayLevel != count && paused == false)
                            {
                                spriteBatch.DrawString(alertFont, "Movement, Huh?", new Vector2(10, 320), Color.Red);
                                spriteBatch.DrawString(labelFont, "Use W,A,S,D on your Keyboard.", new Vector2(10, 390), Color.Red);
                                spriteBatch.DrawString(labelFont, "You can also use the analogs on your controller.", new Vector2(10, 430), Color.Red);
                            }
                            break;

                        case TutState.Shoot:
                            #region draw code
                            #region background
                            spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                            BGItems.DrawPlanets(spriteBatch);
                            bgLayer1.Draw(spriteBatch);
                            bgLayer2.Draw(spriteBatch);
                            #endregion
                            #region main content
                            enemyManager.DrawEnemies(spriteBatch);
                            LaserBeans.DrawLasers(spriteBatch);
                            VFX.DrawExplosions(spriteBatch);
                            player.draw(spriteBatch);
                            #endregion
                            #region powerups
                            powerMan.DrawPowerups(spriteBatch);
                            #endregion
                            #region GUI
                            gui.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, paused);
                            #endregion
                            #endregion
                            if (tutDelayLevel != count && paused == false)
                            {
                                spriteBatch.DrawString(alertFont, "Shooting's easy too.", new Vector2(10, 320), Color.Red);
                                spriteBatch.DrawString(labelFont, "Press the Space Bar to shoot.", new Vector2(10, 390), Color.Red);
                                spriteBatch.DrawString(labelFont, "You can also use your X Button or Right Trigger.", new Vector2(10, 430), Color.Red);
                            }
                            break;

                        case TutState.Score:
                            #region draw code
                            #region background
                            spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                            BGItems.DrawPlanets(spriteBatch);
                            bgLayer1.Draw(spriteBatch);
                            bgLayer2.Draw(spriteBatch);
                            #endregion
                            #region main content
                            enemyManager.DrawEnemies(spriteBatch);
                            LaserBeans.DrawLasers(spriteBatch);
                            VFX.DrawExplosions(spriteBatch);
                            player.draw(spriteBatch);
                            #endregion
                            #region powerups
                            powerMan.DrawPowerups(spriteBatch);
                            #endregion
                            #region GUI
                            gui.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, paused);
                            #endregion
                            #endregion
                            if (tutDelayLevel != count && paused == false)
                            {
                                spriteBatch.DrawString(alertFont, "Your Score is up there ^.", new Vector2(10, 320), Color.Red);
                                spriteBatch.DrawString(labelFont, "It goes up depending on how many enemies you kill.", new Vector2(10, 390), Color.Red);
                                spriteBatch.DrawString(labelFont, "What else could there possibly be?", new Vector2(10, 430), Color.Red);
                            }
                            break;

                        case TutState.Power:
                            #region draw code
                            #region background
                            spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                            BGItems.DrawPlanets(spriteBatch);
                            bgLayer1.Draw(spriteBatch);
                            bgLayer2.Draw(spriteBatch);
                            #endregion
                            #region main content
                            enemyManager.DrawEnemies(spriteBatch);
                            LaserBeans.DrawLasers(spriteBatch);
                            VFX.DrawExplosions(spriteBatch);
                            player.draw(spriteBatch);
                            #endregion
                            #region powerups
                            powerMan.DrawPowerups(spriteBatch);
                            #endregion
                            #region GUI
                            gui.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, paused);
                            #endregion
                            #endregion
                            if (tutDelayLevel != count && paused == false)
                            {
                                if (tutFinish == true && paused == false)
                                {
                                    spriteBatch.DrawString(alertFont, "You're all set.", new Vector2(10, 320), Color.Red);
                                    spriteBatch.DrawString(labelFont, "You can go through this tutorial again by pressing Y or Enter.", new Vector2(10, 390), Color.Red);
                                    spriteBatch.DrawString(labelFont, "Or you can return to the main menu by pressing Back or M", new Vector2(10, 430), Color.Red);
                                    break;
                                }
                            
                                spriteBatch.DrawString(alertFont, "I'm glad you asked.", new Vector2(10, 320), Color.Red);
                                spriteBatch.DrawString(labelFont, "These powerups will increase the score you get.", new Vector2(10, 390), Color.Red);
                                spriteBatch.DrawString(labelFont, "That is, if you shoot an enemy while it's active.", new Vector2(10, 430), Color.Red);
                                
                            }
                            break;
                    }
                    break;
                #endregion
                #region DrawLevelOneState
                case GameState.LevelOne:
                    #region draw code
                    #region background
                    spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                    BGItems.DrawPlanets(spriteBatch);
                    bgLayer1.Draw(spriteBatch);
                    bgLayer2.Draw(spriteBatch);
                    #endregion
                    #region main content
                    enemyManager.DrawEnemies(spriteBatch);
                    LaserBeans.DrawLasers(spriteBatch);
                    VFX.DrawExplosions(spriteBatch);
                    player.draw(spriteBatch);
                    #endregion
                    #region powerups
                    powerMan.DrawPowerups(spriteBatch);
                    #endregion
                    #region GUI
                    gui.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, paused);
                    #endregion
                    #endregion
                    if (delayLevel != count && paused == false)
                    {
                        spriteBatch.DrawString(alertFont, "Level 1", new Vector2(300, 120), Color.Red);
                        spriteBatch.DrawString(labelFont, "Star Field", new Vector2(300, 180), Color.Red);
                        spriteBatch.DrawString(labelFont, "Get 15000 points to continue", new Vector2(300, 200), Color.Red);
                    }
                    break;
                #endregion
                #region DrawLevelTwoState
                case GameState.LevelTwo:
                    #region draw code
                    
                    #region background
                    spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                    BGItems.DrawPlanets(spriteBatch);
                    mainMenuBackground1.Draw(spriteBatch);
                    mainMenuBackground2.Draw(spriteBatch);
                    NebulaBG1.Draw(spriteBatch);
                    NebulaBG2.Draw(spriteBatch);
                    
                    #endregion
                    #region main content
                    enemyManager.DrawEnemies(spriteBatch);
                    LaserBeans.DrawLasers(spriteBatch);
                    VFX.DrawExplosions(spriteBatch);
                    player.draw(spriteBatch);
                    #endregion
                    #region powerups
                    powerMan.DrawPowerups(spriteBatch);
                    #endregion
                    #region GUI
                    gui.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, paused);
                    #endregion
                    if (delayLevel != count)
                    {
                        spriteBatch.DrawString(alertFont, "Level 2", new Vector2(300, 120), Color.Red);
                        spriteBatch.DrawString(labelFont, "Nebula", new Vector2(300, 180), Color.Red);
                        spriteBatch.DrawString(labelFont, "Get 45000 points to continue", new Vector2(300, 200), Color.Red);
                    }
                    #endregion
                    break;
                #endregion
                #region DrawLevelThreeState
                case GameState.LevelThree:
                    #region draw code

                    #region background
                    spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                    BGItems.DrawPlanets(spriteBatch);
                    mainMenuBackground1.Draw(spriteBatch);
                    mainMenuBackground2.Draw(spriteBatch);
                    ScrapBG2.Draw(spriteBatch);
                    ScrapBG1.Draw(spriteBatch);
                    #endregion
                    #region main content
                    enemyManager.DrawEnemies(spriteBatch);
                    LaserBeans.DrawLasers(spriteBatch);
                    VFX.DrawExplosions(spriteBatch);
                    player.draw(spriteBatch);
                    #endregion
                    #region powerups
                    powerMan.DrawPowerups(spriteBatch);
                    #endregion
                    #region GUI
                    gui.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, paused);
                    #endregion
                    if (delayLevel != count)
                    {
                        spriteBatch.DrawString(alertFont, "Level 3", new Vector2(300, 120), Color.Red);
                        spriteBatch.DrawString(labelFont, "Star Ship Graveyard", new Vector2(300, 180), Color.Red);
                        spriteBatch.DrawString(labelFont, "Get 75000 points to continue", new Vector2(300, 200), Color.Red);
                    }
                    #endregion
                    break;
                #endregion
                #region DrawDeathState
                case GameState.Death:
                    if(dead)
                    {
                        spriteBatch.Draw(backDrop, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(alertFont, "Game Over", new Vector2(200, 150), Color.Red);
                        spriteBatch.DrawString(labelFont, "Press M to return to the main menu", new Vector2(200, 240), Color.Red);
                        spriteBatch.DrawString(labelFont, "Press R to restart the game", new Vector2(200, 260), Color.Red);
                        spriteBatch.DrawString(labelFont, "press Escape to exit", new Vector2(200, 280), Color.Red);

                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            Exit();
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.M))
                        {
                            player.initialize();
                            enemyManager.clean();
                            LaserBeans.clean();
                            powerMan.clean();
                            VFX.clean();
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    MouseState mouse2 = Mouse.GetState();
                    break;
                #endregion
                #region DrawWinState
                case GameState.Win:
                        spriteBatch.Draw(backDrop, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(alertFont, "You Won!", new Vector2(200, 150), Color.Red);
                        spriteBatch.DrawString(labelFont, "Press M or Y to return to the main menu.", new Vector2(200, 220), Color.Red);
                        spriteBatch.DrawString(labelFont, "Press R or X to restart the game.", new Vector2(200, 240), Color.Red);
                        spriteBatch.DrawString(labelFont, "Press C or B to view the credits.", new Vector2(200, 260), Color.Red);
                        spriteBatch.DrawString(labelFont, "press Escape to exit.", new Vector2(200, 280), Color.Red);

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            Exit();
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.M))
                        {
                            player.initialize();
                            enemyManager.clean();
                            LaserBeans.clean();
                            powerMan.clean();
                            VFX.clean();
                            CurrentGameState = GameState.MainMenu;
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.R))
                        {
                            player.initialize();
                            enemyManager.clean();
                            LaserBeans.clean();
                            powerMan.clean();
                            VFX.clean();
                            CurrentGameState = GameState.LevelOne;
                        }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        player.initialize();
                        enemyManager.clean();
                        LaserBeans.clean();
                        powerMan.clean();
                        VFX.clean();
                        CurrentGameState = GameState.Credits;
                    }
                    MouseState mouse3 = Mouse.GetState();
                    break;
                #endregion
                #region  DrawCredits
                case GameState.Credits:
                    MouseState mouse4 = Mouse.GetState();
                    spriteBatch.Draw(mainBackground, rectBackgound, Color.White);
                    mainMenuBackground1.Draw(spriteBatch);
                    mainMenuBackground2.Draw(spriteBatch);
                    buttonMenu.Draw(spriteBatch);
                    pointer.Draw(spriteBatch, mouse4, details);
                    spriteBatch.DrawString(alertFont, "JAMES FURLONG", new Vector2(170, 30), Color.Red);
                    spriteBatch.DrawString(labelFont, "Sound // Programming // Art", new Vector2(310, 85), Color.Red);
                    spriteBatch.DrawString(alertFont, "STEFAN SMITH-BOARD", new Vector2(100, 115), Color.Red);
                    spriteBatch.DrawString(labelFont, "Progamming // Art", new Vector2(340, 170), Color.Red);
                    spriteBatch.DrawString(alertFont, "UMAIR KHALIQ", new Vector2(200, 200), Color.Red);
                    spriteBatch.DrawString(labelFont, "Progamming // Art", new Vector2(340, 255), Color.Red);
                    spriteBatch.DrawString(alertFont, "GEORGY KARAKOZOV", new Vector2(110, 285), Color.Red);
                    spriteBatch.DrawString(labelFont, "Progamming // Art", new Vector2(340, 340), Color.Red);
                    break;
                    #endregion
            }
            #endregion
            spriteBatch.End();
            base.Draw(gameTime);
        }       
    }
}

