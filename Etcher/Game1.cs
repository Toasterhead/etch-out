using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace Etcher
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public partial class Game1 : Game
    {
        public enum GameModes { Title, Message, Action, Tutorial, GameOver, GameComplete, HighScore, Pause }
        public enum Layers { Front = 0, Middle, Back, EnumSize }
        public enum RenderEffects { None = 0, Scanline, PhosphorDot, Static, PhaseShift, EnumSize }

        //Width and height (in pixels) for tiles.
        public const int TILE_SIZE = 8;
        //The split between the action and scoreboard sections of the screen occures at this horizontal tile.
        public const int SCREEN_DIVIDER = 40;
        //Game field dimensions.
        public const int FULLFIELD_WIDTH = 400;
        public const int FULLFIELD_HEIGHT = 240;
        //Other size and positional data.
        public const int MESSAGE_WIDTH = 36 * TILE_SIZE;
        //Timer durations.
        public const uint FRAME_RATE = 30;
        public const uint FIRST_LEVEL_TIME = 10 * FRAME_RATE;
        public const uint TIME_INCREMENT = 2 * FRAME_RATE;
        public const uint CLOCK_DURATION = 12 * FRAME_RATE;
        public const uint START_DURATION = 60;
        public const uint DEATH_DURATION = 120;
        public const uint CLEAR_DURATION = 90;
        public const uint FIRST_START_DURATION = 3 * START_DURATION;
        public const uint ITEM_APPEARANCE_PROBABILITY = 12 * FRAME_RATE; //B-type only.

        //Note: Rectangular play area within border is 38 x 26 tiles.

        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static KeyboardState keys;
        public static KeyboardState keysPrev;
        public static GamePadState gamepad;
        public static GamePadState gamepadPrev;

        public static RenderEffects renderEffect;
        public static RenderTarget2D canvasRaw;
        public static RenderTarget2D vortexSurface;
        public static Texture2D noise;
        public static int canvasMultiplier;
        public static float effectBrightness;

        public static bool quit;
        public static bool bType;
        public static uint level;
        public static uint points;
        public static uint? gateTimer;
        public static uint? clockTimer;
        public static uint? startTimer;
        public static uint? deathTimer;
        public static uint? clearTimer;
        public static uint? messageTimer;
        public static uint orbTimer;
        public static uint universalTimer;
        public static uint universalTimeStamp;
        public static GameModes gameMode;
        public static Random rand;
        public static Color borderColor;

        public static Player player;
        public static List<Vector2> trail;

        public static List<IGameObject> spriteSet;
        public static Stack<IGameObject> removalSet;
        public static Stack<IGameObject> pendingSet;
        public static Stack<Pickup> pickups;
        public static Stack<Debris> debris;

        public static CharacterSet characterSet;
        public static Textfield textLives;
        public static Textfield textScore;
        public static Textfield textTime;
        public static Textfield textLabelScore;
        public static Textfield textLabelTime;
        public static Textfield textMessage;
        public static string message;

        public static Song currentSong;
        public static SoundEffectInstance currentSound;
        public static SoundEffectInstance currentArpeggioNote;
        public static uint arpeggioIndex;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f); // --- Adjust framerate.

            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "Etch-Out";

            canvasMultiplier = 2;

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = canvasMultiplier * FULLFIELD_WIDTH;
            graphics.PreferredBackBufferHeight = canvasMultiplier * FULLFIELD_HEIGHT;
            graphics.ApplyChanges();

            canvasRaw = new RenderTarget2D(
                GraphicsDevice,
                FULLFIELD_WIDTH,
                FULLFIELD_HEIGHT,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            vortexSurface = new RenderTarget2D(
                GraphicsDevice,
                2 * (int)Vortex.RADIUS_LARGE,
                2 * (int)Vortex.RADIUS_LARGE,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            Master.TileSize = TILE_SIZE;
            Master.DefaultImage = Images.Characters.HAPPY;
            Master.TheGraphics = graphics;
            Master.TheGraphicsDevice = GraphicsDevice;
            Master.TheSpriteBatch = spriteBatch;

            rand = new Random();
            borderColor = Color.Blue;

            characterSet = new CharacterSet(TILE_SIZE, TILE_SIZE, 0);
            LoadCharacterSet(characterSet);

            Sounds.Pitches.InitializeArpeggios();

            textLives = new Textfield(characterSet, "", 0, 0, 10 * TILE_SIZE, layer: (int)Layers.Back);
            textScore = new Textfield(characterSet, "", 0, 0, 10 * TILE_SIZE, layer: (int)Layers.Back);
            textTime = new Textfield(characterSet, "", 0, 0, 10 * TILE_SIZE, layer: (int)Layers.Back);
            textLabelScore = new Textfield(characterSet, "Score", 0, 0, "Score".Length * TILE_SIZE, layer: (int)Layers.Back);
            textLabelTime = new Textfield(characterSet, "Time", 0, 0, "Time".Length * TILE_SIZE, layer: (int)Layers.Back);
            textMessage = new Textfield(characterSet, "", 0, 0, 40 * TILE_SIZE, layer: (int)Layers.Front);
            
            textLabelScore.Draw();
            textLabelTime.Draw();

            MenuManager.ConstructTitleMenu();
            MenuManager.ConstructPauseMenu();
            MenuManager.ConstructTextFields();
            MenuManager.Apply(null);

            bType = false;

            HighScore.LoadFromFileAsync(bType: false);
            HighScore.LoadFromFileAsync(bType: true);

            gameMode = GameModes.Title;
            PlayMusic(Sounds.Music.TITLE, repeat: true);

            //base.Initialize();
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
            #region Read Input

            switch (gameMode)
            {
                case GameModes.Title: CheckTitleInput();
                    break;
                case GameModes.Message: CheckMessageInput();
                    break;
                case GameModes.Action: CheckActionInput();
                    break;
                case GameModes.Tutorial: CheckTutorialInput();
                    break;
                case GameModes.Pause: CheckPauseInput();
                    break;
                case GameModes.GameOver:
                    break;
                case GameModes.GameComplete:
                    break;
                case GameModes.HighScore: CheckHighScoreInput();
                    break;
                default: throw new Exception("Error - Unable to recognize game mode.");
            }

            #endregion

            #region Update Game State

            switch (gameMode)
            {
                case GameModes.Title: UpdateTitleMode();
                    break;
                case GameModes.Message: UpdateMessageeMode();
                    break;
                case GameModes.Action: if (ProcessActionDelay()) UpdateActionMode();
                    break;
                case GameModes.Tutorial: if (ProcessActionDelay()) UpdateActionMode();
                    break;
                case GameModes.Pause:
                    break;
                case GameModes.GameOver: UpdateGameOverMode();
                    break;
                case GameModes.GameComplete: UpdateGameOverMode();
                    break;
                case GameModes.HighScore: UpdateHighScoreMode();
                    break; 
                default: throw new Exception("Error - Unable to recognize game mode.");
            }

            #endregion

            universalTimer++;

            if (quit) Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            #region Draw Screen

            switch (gameMode)
            {
                case GameModes.Title: DrawTitleMode();
                    break;
                case GameModes.Message: DrawMessageMode();
                    break;
                case GameModes.Action: DrawActionMode();
                    break;
                case GameModes.Tutorial: DrawTutorialMode();
                    break;
                case GameModes.Pause: DrawPauseMode();
                    break;
                case GameModes.GameOver: DrawGameOverMode();
                    break;
                case GameModes.GameComplete: DrawGameOverMode();
                    break;
                case GameModes.HighScore: DrawHighScoreMode();
                    break;
                default: throw new Exception("Error - Unable to recognize game mode.");
            }

            #endregion

            base.Draw(gameTime);
        }
    }
}
