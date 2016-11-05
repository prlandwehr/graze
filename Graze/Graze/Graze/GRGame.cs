using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Graze
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GRGame : Microsoft.Xna.Framework.Game
    {
        ////
        //FIELDS
        ////

        //XNA OBJECTS
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //"STATIC" MEDIA
        private Texture2D triTx;
        private Texture2D whitepx;
        private Texture2D gameareaborder;
        private SpriteFont pixelfont24;
        private SpriteFont pixelfontlarge;
        private SoundEffect grazesfx;
        private SoundEffect explosionsfx;
        private SoundEffect slowmosfx;
        private SoundEffect newwavesfx;
        private Song grazebgm;
        //GAME STATUS FIELDS
        private enum GRGameState
        {
            mainmenu = 0,
            ingame = 1,
            gameover = 2
        };
        private GRGameState gamestate;
        private float score;
        private float grazemultiplier;
        private float gamespeed;
        private int wavenum;
        private bool inSlowMode;
        private float slowmodetimer;
        private float grazelosstimer;
        private float colorlerptimer;
        //COLOR VARS
        private Color lastBGcolor;
        private Color lastFGcolor;
        private Color cBGcolor;
        private Color cFGcolor;
        private Color targetBGcolor;
        private Color targetFGcolor;
        private bool usecolorlerping = false;
        //GAME CONSTANTS
        private static readonly Color DEFAULT_BGCOLOR = Color.DarkGray;
        private static readonly Color DEFAULT_FGCOLOR = Color.White;
        private static readonly Color DEFAULT_GUITEXTCOLOR = Color.Black;
        private static readonly Color DEFAULT_BORDERCOLOR = Color.White;
        private static readonly Color DEFAULT_SCORETEXTCOLOR = Color.LawnGreen;
        private static readonly float MAX_GRAZE_MULTIPLIER = 15;
        private static readonly float GRAZE_LOSS_INTERVAL = 3.0f;
        private static readonly float SLOW_MODE_DURATION = 4.0f;
        private static readonly float SLOW_MODE_SPEED = 3.0f;
        private static readonly float WAVE_SPEED_INCREASE = 0.225f; //.25
        private static readonly float WAVE_SCORE_VALUE = 1000;
        private static readonly float GRAZE_SCORE_VALUE = 100;
        private static readonly float COLOR_LERP_DURATION = 20.0f; //60.0f
        private static readonly float BGM_VOLUME = 0;//0.2f;
        //GAME OBJECTS+COLLECTIONS
        private ArrayList bulletTextures50;
        private ArrayList bulletTextures25;
        private GRPlayer player;
        private Rectangle screenarea;
        private Rectangle gamearea;
        private Rectangle guiarea;
        private GRWave currentwave;
        private GRScoreIndicators pscoreIndicator;
        private GRColors colorgen;
        private Random rand;

        ////
        //CONSTRUCTOR
        ////

        public GRGame()
        {
            //XNA+SYS INIT
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rand = new Random();

            //SCREEN INIT
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;

            //TIMER INIT + other
            initVarValues();
            //COLOR INIT
            initLerpColors();
            
            //RECT INIT
            screenarea = gamearea = new Rectangle(0, 0, 1280, 720);
            gamearea = new Rectangle(36, 36, 875, 648);
            //guiarea = new Rectangle(911, 36, 333, 648);
            guiarea = new Rectangle(911, 36, 369, 648);

            //OBJECT INIT
            bulletTextures50 = new ArrayList();
            bulletTextures25 = new ArrayList();
            colorgen = new GRColors();
            gamestate = GRGameState.mainmenu;
            //player = new GRPlayer(gamearea);
            //player.position = new Vector2(gamearea.Center.X, gamearea.Center.Y);
        }

        //constructor to respond to command line lerp setting
        public GRGame(bool colorlerp) 
            : this()
        {
            usecolorlerping = colorlerp;
            initLerpColors();
        }

        ////
        //METHODS
        ////

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Own Init

            //XNA Init
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

            // TODO: use this.Content to load your game content here
            triTx = Content.Load<Texture2D>("power35");
            gameareaborder = Content.Load<Texture2D>("border");
            bulletTextures50.Add(Content.Load<Texture2D>("Shapes/square50"));
            bulletTextures50.Add(Content.Load<Texture2D>("Shapes/pent50"));
            bulletTextures50.Add(Content.Load<Texture2D>("Shapes/hex50"));
            bulletTextures50.Add(Content.Load<Texture2D>("Shapes/circle50"));
            bulletTextures25.Add(Content.Load<Texture2D>("Shapes/square25"));
            bulletTextures25.Add(Content.Load<Texture2D>("Shapes/pent25"));
            bulletTextures25.Add(Content.Load<Texture2D>("Shapes/hex25"));
            bulletTextures25.Add(Content.Load<Texture2D>("Shapes/circle25"));

            whitepx = Content.Load<Texture2D>("whitepx");
            pixelfont24 = Content.Load<SpriteFont>("04b03_24");
            pixelfontlarge = Content.Load<SpriteFont>("04b03_large");
            grazesfx = Content.Load<SoundEffect>("grazewav");
            explosionsfx = Content.Load<SoundEffect>("rad");
            slowmosfx = Content.Load<SoundEffect>("slowmo");
            newwavesfx = Content.Load<SoundEffect>("newwave");
            grazebgm = Content.Load<Song>("Space Fighter Loop");

            //set player Tx
            player = new GRPlayer(gamearea, Content);
            player.position = new Vector2(gamearea.Center.X, gamearea.Center.Y);
            player.setTex(triTx);

            //construct first wave
            setWave();

            MediaPlayer.Volume = BGM_VOLUME;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(grazebgm);
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
            //XNA Update
            base.Update(gameTime);

            //Own Updates

            //allow exiting the game
            KeyboardState keystate = Keyboard.GetState();
            GamePadState p1gp = GamePad.GetState(PlayerIndex.One);
            if (p1gp.Buttons.Back == ButtonState.Pressed || keystate.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //state select
            if (gamestate == GRGameState.mainmenu)
            {
                mainMenuUpdate(gameTime);
            }
            else if (gamestate == GRGameState.ingame)
            {
                inGameUpdate(gameTime);
            }
            else if (gamestate == GRGameState.gameover)
            {
                gameOverUpdate(gameTime);
            }

        }

        //Update loop for when game is in progress
        private void inGameUpdate(GameTime gameTime)
        {
            //input processing
            processInGameInput();
            //collision processing
            processCollisions();
            //object and field updating
            //COLOR
            if (usecolorlerping)
            {
                colorlerptimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (colorlerptimer < COLOR_LERP_DURATION)
                {
                    cFGcolor = Color.Lerp(lastFGcolor, targetFGcolor, colorlerptimer / COLOR_LERP_DURATION);
                    cBGcolor = Color.Lerp(lastBGcolor, targetBGcolor, colorlerptimer / COLOR_LERP_DURATION);
                }
                else
                {
                    colorlerptimer = 0;
                    lastFGcolor = cFGcolor;
                    lastBGcolor = cBGcolor;
                    targetFGcolor = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
                    targetBGcolor = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
                }
            }
            //TIMER BEHAVIOR
            grazelosstimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (grazelosstimer > GRAZE_LOSS_INTERVAL)
            {
                if (grazemultiplier > 0)
                {
                    grazemultiplier -= 1;
                }
                grazelosstimer = 0;
            }
            //if in slow mode
            if (inSlowMode && slowmodetimer < SLOW_MODE_DURATION)
            {
                slowmodetimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            //if exiting slow mode
            if (inSlowMode && slowmodetimer >= SLOW_MODE_DURATION)
            {
                inSlowMode = false;
                grazemultiplier = 0;
                gamespeed *= SLOW_MODE_SPEED;
                for (int index = 0; index < currentwave.bullets.Count; index++)
                {
                    GRBullet cbullet = (GRBullet)currentwave.bullets[index];
                    cbullet.rotation *= SLOW_MODE_SPEED;
                }
                slowmodetimer = 0;
            }
            //PLAYER + WAVE UPDATE
            player.Update(gameTime, gamespeed);
            currentwave.Update(gameTime, gamespeed, inSlowMode, cFGcolor);
            pscoreIndicator.Update(gameTime, player);
            //NEW WAVE
            if (currentwave.isWaveComplete())
            {
                newwavesfx.Play();
                score += WAVE_SCORE_VALUE;
                pscoreIndicator.addScore(WAVE_SCORE_VALUE, player);
                wavenum++;
                if (inSlowMode)
                {
                    gamespeed += WAVE_SPEED_INCREASE / SLOW_MODE_SPEED;
                }
                else
                {
                    gamespeed += WAVE_SPEED_INCREASE;
                }
                //set random new wave type
                setWave();
            }
            //game over
            if (player.lives <= 0)
            {
                gamestate = GRGameState.gameover;
                player.setInvincibleOff();
            }
        }

        //Update loop for when game over occurs
        private void gameOverUpdate(GameTime gameTime)
        {
            // Input Polling Section
            KeyboardState keystate = Keyboard.GetState();
            GamePadState p1gp = GamePad.GetState(PlayerIndex.One);

            if (p1gp.Buttons.Start == ButtonState.Pressed || keystate.IsKeyDown(Keys.Space))
            {
                this.newGame();
            }
        }

        //Update loop for main menu
        private void mainMenuUpdate(GameTime gameTime)
        {
            // Input Polling Section
            KeyboardState keystate = Keyboard.GetState();
            GamePadState p1gp = GamePad.GetState(PlayerIndex.One);

            if (p1gp.Buttons.Start == ButtonState.Pressed || keystate.IsKeyDown(Keys.Space))
            {
                gamestate = GRGameState.ingame;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //BEGIN DRAW
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            //state select
            if (gamestate == GRGameState.mainmenu)
            {
                menuDraw();
            }
            else if (gamestate == GRGameState.ingame)
            {
                gameDraw();
            }
            else if (gamestate == GRGameState.gameover)
            {
                gameoverDraw();
            }

            //END DRAW
            spriteBatch.End();

            //XNA Draw call
            base.Draw(gameTime);
        }

        private void menuDraw()
        {
            //BG DRAW
            spriteBatch.Draw(whitepx, screenarea, Color.White);
            spriteBatch.Draw(whitepx, new Rectangle(36, 36, 1280-72, 720-72), cBGcolor);

            //GAME ELEMENTS
            

            //GUI elements
            //spriteBatch.Draw(gameareaborder, Vector2.Zero, DEFAULT_BORDERCOLOR);
            //text rendering
            string titlestring = "graze.type";
            Vector2 titlestringsize = pixelfontlarge.MeasureString(titlestring);
            spriteBatch.DrawString(pixelfontlarge, titlestring, new Vector2(screenarea.Center.X - titlestringsize.X / 2, screenarea.Center.Y - titlestringsize.Y / 2 - 100), Color.Black);

            string pressstart = "press space / start";
            Vector2 pressstartsize = pixelfont24.MeasureString(pressstart);
            spriteBatch.DrawString(pixelfont24, pressstart, new Vector2(screenarea.Center.X - pressstartsize.X / 2, screenarea.Center.Y - pressstartsize.Y / 2 + 100), Color.Black);

        }

        private void gameDraw()
        {
            //BG DRAW
            if (usecolorlerping)
            {
                spriteBatch.Draw(whitepx, screenarea, cFGcolor);
            }
            else
            {
                spriteBatch.Draw(whitepx, screenarea, Color.White);
            }
            spriteBatch.Draw(whitepx, gamearea, cBGcolor);

            //GAME ELEMENTS
            player.Draw(spriteBatch);
            for (int index = 0; index < currentwave.bullets.Count; index++)
            {
                GRBullet cbullet = (GRBullet)currentwave.bullets[index];
                cbullet.Draw(spriteBatch);
            }
            pscoreIndicator.Draw(spriteBatch, pixelfont24, DEFAULT_SCORETEXTCOLOR);

            //GUI elements
            spriteBatch.Draw(gameareaborder, Vector2.Zero, DEFAULT_BORDERCOLOR);
            //text rendering
            string gametitle = "graze.type";
            Vector2 gametitlesize = pixelfont24.MeasureString(gametitle);
            string livesgui = "Lives: " + player.lives;
            Vector2 livessize = pixelfont24.MeasureString(livesgui);
            string wavegui = "Wave: " + wavenum;
            Vector2 wavesize = pixelfont24.MeasureString(wavegui);
            string grazegui = "Graze: " + grazemultiplier;
            Vector2 grazesize = pixelfont24.MeasureString(grazegui);
            string scoregui = "Score: " + score;
            Vector2 scoresize = pixelfont24.MeasureString(scoregui);
            spriteBatch.DrawString(pixelfont24, gametitle, new Vector2(guiarea.Center.X - gametitlesize.X / 2, guiarea.Top), DEFAULT_GUITEXTCOLOR);
            spriteBatch.DrawString(pixelfont24, livesgui, new Vector2(guiarea.Center.X - livessize.X / 2, guiarea.Center.Y), DEFAULT_GUITEXTCOLOR);
            spriteBatch.DrawString(pixelfont24, wavegui, new Vector2(guiarea.Center.X - wavesize.X / 2, guiarea.Center.Y + 50), DEFAULT_GUITEXTCOLOR);
            spriteBatch.DrawString(pixelfont24, grazegui, new Vector2(guiarea.Center.X - grazesize.X / 2, guiarea.Center.Y - 50), DEFAULT_GUITEXTCOLOR);
            spriteBatch.DrawString(pixelfont24, scoregui, new Vector2(guiarea.Center.X - scoresize.X / 2, guiarea.Center.Y - 100), DEFAULT_GUITEXTCOLOR);
        }

        private void gameoverDraw()
        {
            //BG DRAW
            if (usecolorlerping)
            {
                spriteBatch.Draw(whitepx, screenarea, cFGcolor);
            }
            else
            {
                spriteBatch.Draw(whitepx, screenarea, Color.White);
            }
            spriteBatch.Draw(whitepx, gamearea, cBGcolor);

            //GAME ELEMENTS
            player.Draw(spriteBatch);
            for (int index = 0; index < currentwave.bullets.Count; index++)
            {
                GRBullet cbullet = (GRBullet)currentwave.bullets[index];
                cbullet.Draw(spriteBatch);
            }
            pscoreIndicator.Draw(spriteBatch, pixelfont24, Color.LawnGreen);

            //GUI elements
            spriteBatch.Draw(gameareaborder, Vector2.Zero, DEFAULT_BORDERCOLOR);
            //text rendering
            string gameoverstring = "game.over";
            Vector2 gameoversize = pixelfontlarge.MeasureString(gameoverstring);
            spriteBatch.DrawString(pixelfontlarge, gameoverstring, new Vector2(gamearea.Center.X - gameoversize.X / 2, gamearea.Center.Y - gameoversize.Y / 2 - 50), Color.Black);
            string pressstart = "press space / start";
            Vector2 pressstartsize = pixelfont24.MeasureString(pressstart);
            spriteBatch.DrawString(pixelfont24, pressstart, new Vector2(gamearea.Center.X - pressstartsize.X / 2, gamearea.Center.Y - pressstartsize.Y / 2 + 50), Color.Black);

            string gametitle = "graze.type";
            Vector2 gametitlesize = pixelfont24.MeasureString(gametitle);
            string livesgui = "Lives: " + player.lives;
            Vector2 livessize = pixelfont24.MeasureString(livesgui);
            string wavegui = "Wave: " + wavenum;
            Vector2 wavesize = pixelfont24.MeasureString(wavegui);
            string grazegui = "Graze: " + grazemultiplier;
            Vector2 grazesize = pixelfont24.MeasureString(grazegui);
            string scoregui = "Score: " + score;
            Vector2 scoresize = pixelfont24.MeasureString(scoregui);
            spriteBatch.DrawString(pixelfont24, gametitle, new Vector2(guiarea.Center.X - gametitlesize.X / 2, guiarea.Top), DEFAULT_GUITEXTCOLOR);
            spriteBatch.DrawString(pixelfont24, livesgui, new Vector2(guiarea.Center.X - livessize.X / 2, guiarea.Center.Y), DEFAULT_GUITEXTCOLOR);
            spriteBatch.DrawString(pixelfont24, wavegui, new Vector2(guiarea.Center.X - wavesize.X / 2, guiarea.Center.Y + 50), DEFAULT_GUITEXTCOLOR);
            spriteBatch.DrawString(pixelfont24, grazegui, new Vector2(guiarea.Center.X - grazesize.X / 2, guiarea.Center.Y - 50), DEFAULT_GUITEXTCOLOR);
            spriteBatch.DrawString(pixelfont24, scoregui, new Vector2(guiarea.Center.X - scoresize.X / 2, guiarea.Center.Y - 100), DEFAULT_GUITEXTCOLOR);
        }

        //Check for hits and grazes
        private void processCollisions()
        {
            for (int index = 0; index < currentwave.bullets.Count; index++)
            {
                GRBullet cbullet = (GRBullet)currentwave.bullets[index];
                if (player.collidesWithBullet(cbullet))
                {
                    player.lives -= 1;
                    grazemultiplier = 0;
                    player.setInvincible();
                    explosionsfx.Play(0.25f,0f,0f);
                }
                if (player.grazesBullet(cbullet) /*&& !inSlowMode*/)
                {
                    if (!cbullet.grazeCooldown)
                    {
                        if (grazemultiplier < MAX_GRAZE_MULTIPLIER)
                        {
                            grazemultiplier += 1;
                        }
                        //varies pitch from -.5 to 1.5
                        float grazepercent = grazemultiplier / MAX_GRAZE_MULTIPLIER * 1.0f;
                        grazesfx.Play(1.0f, grazepercent-0.5f ,0);
                        grazelosstimer = 0;
                        cbullet.grazeCooldown = true;
                        score += GRAZE_SCORE_VALUE * grazemultiplier;
                        pscoreIndicator.addScore(GRAZE_SCORE_VALUE * grazemultiplier, player);
                    }
                }
            }
        }

        //Handles input from the user, called in update
        private void processInGameInput()
        {
            // Input Polling Section
            KeyboardState keystate = Keyboard.GetState();
            GamePadState p1gp = GamePad.GetState(PlayerIndex.One);
            Vector2 lstick = p1gp.ThumbSticks.Left;

            //player control

            //movement
            player.velocity = Vector2.Zero;
            if (keystate.IsKeyDown(Keys.Down) || p1gp.DPad.Down == ButtonState.Pressed)
            {
                player.velocity += Vector2.UnitY;
            }
            if (keystate.IsKeyDown(Keys.Up) || p1gp.DPad.Up == ButtonState.Pressed)
            {
                player.velocity -= Vector2.UnitY;
            }
            if (keystate.IsKeyDown(Keys.Left) || p1gp.DPad.Left == ButtonState.Pressed)
            {
                player.velocity -= Vector2.UnitX;
            }
            if (keystate.IsKeyDown(Keys.Right) || p1gp.DPad.Right == ButtonState.Pressed)
            {
                player.velocity += Vector2.UnitX;
            }
            if (player.velocity != Vector2.Zero)
            {
                player.velocity.Normalize();
            }
            //stick
            if (Math.Abs(lstick.X) > 0.1f || Math.Abs(lstick.Y) >0.1f)
            {
                player.velocity.X = lstick.X;
                player.velocity.Y = -lstick.Y;
                if ((float)Math.Sqrt(player.velocity.X * player.velocity.X + player.velocity.Y * player.velocity.Y) > 1.0f)
                {
                    player.velocity.Normalize();
                }
            }
            //have slowMo effect player too
            if (inSlowMode)
            {
                player.velocity /= SLOW_MODE_SPEED;
            }
            //focus button
            if (keystate.IsKeyDown(Keys.A) || p1gp.Buttons.A == ButtonState.Pressed)
            {
                player.focusOn = true;
            }
            else
            {
                player.focusOn = false;
            }
            //slow mode
            if (keystate.IsKeyDown(Keys.S) || p1gp.Buttons.X == ButtonState.Pressed)
            {
                if (!inSlowMode)
                {
                    slowmosfx.Play(0.3f,0f,0f);
                    inSlowMode = true;
                    gamespeed /= SLOW_MODE_SPEED;
                    for (int index = 0; index < currentwave.bullets.Count; index++)
                    {
                        GRBullet cbullet = (GRBullet) currentwave.bullets[index];
                        cbullet.rotation /= SLOW_MODE_SPEED;
                    }
                }
            }
        }

        //set a random new wave type
        private void setWave()
        {
            //set wave color if not lerping
            if (!usecolorlerping)
            {
                Color newcolor = colorgen.getColor();
                lastFGcolor = newcolor;
                cFGcolor = newcolor;
                targetFGcolor = newcolor;
            }

            int wavetype = rand.Next() % 5;
            //wavetype = 1;
            if (wavetype == 0)
            {
                Texture2D waveTx = (Texture2D)bulletTextures25[rand.Next(0, bulletTextures25.Count)];
                currentwave = new GRWaveSwirl(wavenum, gamearea, waveTx, cFGcolor);
            }
            else if (wavetype == 1)
            {
                Texture2D waveTx = (Texture2D)bulletTextures50[rand.Next(0, bulletTextures50.Count)];
                currentwave = new GRWaveBounce(wavenum, gamearea, waveTx, cFGcolor);
            }
            else if (wavetype == 2)
            {
                Texture2D waveTx = (Texture2D)bulletTextures25[rand.Next(0, bulletTextures25.Count)];
                currentwave = new GRWaveBarrage(wavenum, gamearea, waveTx, cFGcolor);
            }
            else if (wavetype == 3)
            {
                Texture2D waveTx = (Texture2D)bulletTextures25[rand.Next(0, bulletTextures25.Count)];
                currentwave = new GRWaveHoming(wavenum, gamearea, waveTx, cFGcolor, player);
            }
            else if (wavetype == 4)
            {
                //either target player or spiral in for type 4
                if ((rand.Next() % 2) == 1)
                {
                    Texture2D waveTx = (Texture2D)bulletTextures25[rand.Next(0, bulletTextures25.Count)];
                    currentwave = new GRWaveSpiralIn(wavenum, gamearea, waveTx, cFGcolor);
                }
                else
                {
                    Texture2D waveTx = (Texture2D)bulletTextures25[rand.Next(0, bulletTextures25.Count)];
                    currentwave = new GRWaveTargetPlayer(wavenum, gamearea, waveTx, cFGcolor, player);
                }
            }
        }

        //Start a new game
        private void newGame()
        {
            //TIMER INIT + other
            initVarValues();
            initLerpColors();
            gamestate = GRGameState.ingame;

            //OBJECT INIT
            player.position = new Vector2(gamearea.Center.X, gamearea.Center.Y);
            player.lives = GRPlayer.STARTING_LIVES;

            //START WAVE
            setWave();
        }

        //consolidation of variable initalization
        private void initVarValues()
        {
            //TIMER INIT + other
            grazelosstimer = 0;
            colorlerptimer = 0;
            slowmodetimer = 0;
            gamespeed = 1.0f;
            wavenum = 1;
            inSlowMode = false;
            grazemultiplier = 0;
            score = 0;
            initLerpColors();
            pscoreIndicator = new GRScoreIndicators();
        }

        //consolidation of color initalization
        private void initLerpColors()
        {
            //COLOR INIT
            cFGcolor = DEFAULT_FGCOLOR;
            cBGcolor = DEFAULT_BGCOLOR;
            lastFGcolor = DEFAULT_FGCOLOR;
            lastBGcolor = DEFAULT_BGCOLOR;
            if (usecolorlerping)
            {
                targetFGcolor = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
                targetBGcolor = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
                targetBGcolor = DEFAULT_BGCOLOR;
            }
            else
            {
                targetFGcolor = DEFAULT_FGCOLOR;
                targetBGcolor = DEFAULT_BGCOLOR;
            }
        }

    }
}