using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Etcher
{
    public partial class Game1 : Game
    {
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            FX.VORTEX = Content.Load<Effect>("effects/vortex");
            FX.SCANLINE = Content.Load<Effect>("effects/scanline");
            FX.PHOSPHOR_DOT = Content.Load<Effect>("effects/phosphor_dot");
            FX.STATIC = Content.Load<Effect>("effects/static");
            FX.PHASE_SHIFT = Content.Load<Effect>("effects/phase_shift");

            Sounds.PICKUP_A = Content.Load<SoundEffect>("sound/pickup_a").CreateInstance();
            Sounds.PICKUP_B = Content.Load<SoundEffect>("sound/pickup_b").CreateInstance();
            Sounds.ORB_APPEARS_A = Content.Load<SoundEffect>("sound/orb_appears_a").CreateInstance();
            Sounds.ORB_APPEARS_B = Content.Load<SoundEffect>("sound/orb_appears_b").CreateInstance();
            Sounds.ORB_APPEARS_C = Content.Load<SoundEffect>("sound/orb_appears_c").CreateInstance();
            Sounds.ORB_APPEARS_D = Content.Load<SoundEffect>("sound/orb_appears_d").CreateInstance();
            Sounds.SELECT = Content.Load<SoundEffect>("sound/select").CreateInstance();
            Sounds.PAUSE = Content.Load<SoundEffect>("sound/pause").CreateInstance();
            Sounds.GATES_OPEN = Content.Load<SoundEffect>("sound/gates_open").CreateInstance();
            Sounds.CRASH = Content.Load<SoundEffect>("sound/crash").CreateInstance();
            Sounds.FIREWORK = Content.Load<SoundEffect>("sound/firework").CreateInstance();
            Sounds.NAME_INPUT = Content.Load<SoundEffect>("sound/name_input").CreateInstance();

            Sounds.DUPLICATE_PICKUP_A = Content.Load<SoundEffect>("sound/pickup_a").CreateInstance();
            Sounds.DUPLICATE_PICKUP_B = Content.Load<SoundEffect>("sound/pickup_b").CreateInstance();
            Sounds.DUPLICATE_ORB_APPEARS_A = Content.Load<SoundEffect>("sound/orb_appears_a").CreateInstance();
            Sounds.DUPLICATE_ORB_APPEARS_B = Content.Load<SoundEffect>("sound/orb_appears_b").CreateInstance();
            Sounds.DUPLICATE_ORB_APPEARS_C = Content.Load<SoundEffect>("sound/orb_appears_c").CreateInstance();
            Sounds.DUPLICATE_ORB_APPEARS_D = Content.Load<SoundEffect>("sound/orb_appears_d").CreateInstance();
            Sounds.DUPLICATE_FIREWORK = Content.Load<SoundEffect>("sound/firework").CreateInstance();
            Sounds.DUPLICATE_NAME_INPUT = Content.Load<SoundEffect>("sound/name_input").CreateInstance();

            Sounds.Music.TITLE = Content.Load<Song>("sound/music/title");
            Sounds.Music.TUTORIAL = Content.Load<Song>("sound/music/tutorial");
            Sounds.Music.START = Content.Load<Song>("sound/music/start");
            Sounds.Music.STAGE_CLEAR = Content.Load<Song>("sound/music/stage_clear");
            Sounds.Music.GAME_OVER = Content.Load<Song>("sound/music/game_over");
            Sounds.Music.HIGH_SCORE = Content.Load<Song>("sound/music/high_score");
            Sounds.Music.VICTORY = Content.Load<Song>("sound/music/victory");

            Sounds.Pitches.B1 = Content.Load<SoundEffect>("sound/notes/B1").CreateInstance();
            Sounds.Pitches.E2 = Content.Load<SoundEffect>("sound/notes/E2").CreateInstance();
            Sounds.Pitches.GSharp2 = Content.Load<SoundEffect>("sound/notes/GSharp2").CreateInstance();
            Sounds.Pitches.B2 = Content.Load<SoundEffect>("sound/notes/B2").CreateInstance();
            Sounds.Pitches.DSharp3 = Content.Load<SoundEffect>("sound/notes/DSharp3").CreateInstance();
            Sounds.Pitches.E3 = Content.Load<SoundEffect>("sound/notes/E3").CreateInstance();
            Sounds.Pitches.GSharp3 = Content.Load<SoundEffect>("sound/notes/GSharp3").CreateInstance();
            Sounds.Pitches.TICK = Content.Load<SoundEffect>("sound/notes/tick").CreateInstance();
            Sounds.Pitches.TOCK = Content.Load<SoundEffect>("sound/notes/tock").CreateInstance();

            Images.BALL = Content.Load<Texture2D>("images/ball");
            Images.BLOCK = Content.Load<Texture2D>("images/block");
            Images.BORDER = Content.Load<Texture2D>("images/border");
            Images.BORDER_BOTTOM = Content.Load<Texture2D>("images/border_bottom");
            Images.BORDER_BOTTOM_LEFT = Content.Load<Texture2D>("images/border_bottom_left");
            Images.BORDER_BOTTOM_RIGHT = Content.Load<Texture2D>("images/border_bottom_right");
            Images.BORDER_LEFT = Content.Load<Texture2D>("images/border_left");
            Images.BORDER_RIGHT = Content.Load<Texture2D>("images/border_right");
            Images.BORDER_TOP = Content.Load<Texture2D>("images/border_top");
            Images.BORDER_TOP_LEFT = Content.Load<Texture2D>("images/border_top_left");
            Images.BORDER_TOP_RIGHT = Content.Load<Texture2D>("images/border_top_right");
            Images.CLOCK = Content.Load<Texture2D>("images/clock");
            Images.CURSOR = Content.Load<Texture2D>("images/cursor");
            Images.DOT = Content.Load<Texture2D>("images/dot");
            Images.END = Content.Load<Texture2D>("images/end");
            Images.EXCLAMATION = Content.Load<Texture2D>("images/exclamation");
            Images.EXTRA_LIFE = Content.Load<Texture2D>("images/extra_life");
            Images.ICON_PLAYER = Content.Load<Texture2D>("images/icon_player");
            Images.OFF = Content.Load<Texture2D>("images/off");
            Images.ORB = Content.Load<Texture2D>("images/orb");
            Images.ORB_APPEARING = Content.Load<Texture2D>("images/orb_appearing");
            Images.PLAYER = Content.Load<Texture2D>("images/player");
            Images.POINTS_100 = Content.Load<Texture2D>("images/points_100");
            Images.POINTS_250 = Content.Load<Texture2D>("images/points_250");
            Images.POINTS_500 = Content.Load<Texture2D>("images/points_500");
            Images.QUANTITY = Content.Load<Texture2D>("images/quantity");
            Images.RIBBON = Content.Load<Texture2D>("images/ribbon");
            Images.ROD = Content.Load<Texture2D>("images/rod");
            Images.SLIDER_KNOB = Content.Load<Texture2D>("images/slider_knob");
            Images.SLIDER_RAIL = Content.Load<Texture2D>("images/slider_rail");
            Images.STAR = Content.Load<Texture2D>("images/star");
            Images.TITLE = Content.Load<Texture2D>("images/title");
            Images.ZAPPER = Content.Load<Texture2D>("images/zapper");

            //Characters
            Images.Characters.A = Content.Load<Texture2D>("images/characters/A");
            Images.Characters.B = Content.Load<Texture2D>("images/characters/B");
            Images.Characters.C = Content.Load<Texture2D>("images/characters/C");
            Images.Characters.D = Content.Load<Texture2D>("images/characters/D");
            Images.Characters.E = Content.Load<Texture2D>("images/characters/E");
            Images.Characters.F = Content.Load<Texture2D>("images/characters/F");
            Images.Characters.G = Content.Load<Texture2D>("images/characters/G");
            Images.Characters.H = Content.Load<Texture2D>("images/characters/H");
            Images.Characters.I = Content.Load<Texture2D>("images/characters/I");
            Images.Characters.J = Content.Load<Texture2D>("images/characters/J");
            Images.Characters.K = Content.Load<Texture2D>("images/characters/K");
            Images.Characters.L = Content.Load<Texture2D>("images/characters/L");
            Images.Characters.M = Content.Load<Texture2D>("images/characters/M");
            Images.Characters.N = Content.Load<Texture2D>("images/characters/N");
            Images.Characters.O = Content.Load<Texture2D>("images/characters/O");
            Images.Characters.P = Content.Load<Texture2D>("images/characters/P");
            Images.Characters.Q = Content.Load<Texture2D>("images/characters/Q");
            Images.Characters.R = Content.Load<Texture2D>("images/characters/R");
            Images.Characters.S = Content.Load<Texture2D>("images/characters/S");
            Images.Characters.T = Content.Load<Texture2D>("images/characters/T");
            Images.Characters.U = Content.Load<Texture2D>("images/characters/U");
            Images.Characters.V = Content.Load<Texture2D>("images/characters/V");
            Images.Characters.W = Content.Load<Texture2D>("images/characters/W");
            Images.Characters.X = Content.Load<Texture2D>("images/characters/X");
            Images.Characters.Y = Content.Load<Texture2D>("images/characters/Y");
            Images.Characters.Z = Content.Load<Texture2D>("images/characters/Z");
            Images.Characters.NUM_0 = Content.Load<Texture2D>("images/characters/0");
            Images.Characters.NUM_1 = Content.Load<Texture2D>("images/characters/1");
            Images.Characters.NUM_2 = Content.Load<Texture2D>("images/characters/2");
            Images.Characters.NUM_3 = Content.Load<Texture2D>("images/characters/3");
            Images.Characters.NUM_4 = Content.Load<Texture2D>("images/characters/4");
            Images.Characters.NUM_5 = Content.Load<Texture2D>("images/characters/5");
            Images.Characters.NUM_6 = Content.Load<Texture2D>("images/characters/6");
            Images.Characters.NUM_7 = Content.Load<Texture2D>("images/characters/7");
            Images.Characters.NUM_8 = Content.Load<Texture2D>("images/characters/8");
            Images.Characters.NUM_9 = Content.Load<Texture2D>("images/characters/9");
            Images.Characters.PERIOD = Content.Load<Texture2D>("images/characters/period");
            Images.Characters.QUESTION = Content.Load<Texture2D>("images/characters/question");
            Images.Characters.EXCLAMATION = Content.Load<Texture2D>("images/characters/exclamation");
            Images.Characters.COMMA = Content.Load<Texture2D>("images/characters/comma");
            Images.Characters.COLON = Content.Load<Texture2D>("images/characters/colon");
            Images.Characters.APOSTROPHE = Content.Load<Texture2D>("images/characters/apostrophe");
            Images.Characters.LEFT_PARENTHESIS = Content.Load<Texture2D>("images/characters/left_parenthesis");
            Images.Characters.RIGHT_PARENTHESIS = Content.Load<Texture2D>("images/characters/right_parenthesis");
            Images.Characters.LEFT_BRACKET = Content.Load<Texture2D>("images/characters/left_bracket");
            Images.Characters.RIGHT_BRACKET = Content.Load<Texture2D>("images/characters/right_bracket");
            Images.Characters.MINUS = Content.Load<Texture2D>("images/characters/minus");
            Images.Characters.POUND = Content.Load<Texture2D>("images/characters/pound");
            Images.Characters.SPACE = Content.Load<Texture2D>("images/characters/space");
            Images.Characters.LEFT = Content.Load<Texture2D>("images/characters/left");
            Images.Characters.RIGHT = Content.Load<Texture2D>("images/characters/right");
            Images.Characters.UP = Content.Load<Texture2D>("images/characters/up");
            Images.Characters.DOWN = Content.Load<Texture2D>("images/characters/down");
            Images.Characters.CURSOR = Content.Load<Texture2D>("images/characters/cursor");
            Images.Characters.HAPPY = Content.Load<Texture2D>("images/characters/happy");
            Images.Characters.EMPTY = Content.Load<Texture2D>("images/characters/empty");

            Images.Noise.NOISE_A = Content.Load<Texture2D>("images/noise/noise_a");
            Images.Noise.NOISE_B = Content.Load<Texture2D>("images/noise/noise_b");
            Images.Noise.NOISE_C = Content.Load<Texture2D>("images/noise/noise_c");
            Images.Noise.NOISE_D = Content.Load<Texture2D>("images/noise/noise_d");
            Images.Noise.NOISE_E = Content.Load<Texture2D>("images/noise/noise_e");
        }

        public static void LoadCharacterSet(CharacterSet characterSet)
        {
            characterSet.A = Images.Characters.A;
            characterSet.B = Images.Characters.B;
            characterSet.C = Images.Characters.C;
            characterSet.D = Images.Characters.D;
            characterSet.E = Images.Characters.E;
            characterSet.F = Images.Characters.F;
            characterSet.G = Images.Characters.G;
            characterSet.H = Images.Characters.H;
            characterSet.I = Images.Characters.I;
            characterSet.J = Images.Characters.J;
            characterSet.K = Images.Characters.K;
            characterSet.L = Images.Characters.L;
            characterSet.M = Images.Characters.M;
            characterSet.N = Images.Characters.N;
            characterSet.O = Images.Characters.O;
            characterSet.P = Images.Characters.P;
            characterSet.Q = Images.Characters.Q;
            characterSet.R = Images.Characters.R;
            characterSet.S = Images.Characters.S;
            characterSet.T = Images.Characters.T;
            characterSet.U = Images.Characters.U;
            characterSet.V = Images.Characters.V;
            characterSet.W = Images.Characters.W;
            characterSet.X = Images.Characters.X;
            characterSet.Y = Images.Characters.Y;
            characterSet.Z = Images.Characters.Z;
            characterSet.a = characterSet.A;
            characterSet.b = characterSet.B;
            characterSet.c = characterSet.C;
            characterSet.d = characterSet.D;
            characterSet.e = characterSet.E;
            characterSet.f = characterSet.F;
            characterSet.g = characterSet.G;
            characterSet.h = characterSet.H;
            characterSet.i = characterSet.I;
            characterSet.j = characterSet.J;
            characterSet.k = characterSet.K;
            characterSet.l = characterSet.L;
            characterSet.m = characterSet.M;
            characterSet.n = characterSet.N;
            characterSet.o = characterSet.O;
            characterSet.p = characterSet.P;
            characterSet.q = characterSet.Q;
            characterSet.r = characterSet.R;
            characterSet.s = characterSet.S;
            characterSet.t = characterSet.T;
            characterSet.u = characterSet.U;
            characterSet.v = characterSet.V;
            characterSet.w = characterSet.W;
            characterSet.x = characterSet.X;
            characterSet.y = characterSet.Y;
            characterSet.z = characterSet.Z;
            characterSet.Zero = Images.Characters.NUM_0;
            characterSet.One = Images.Characters.NUM_1;
            characterSet.Two = Images.Characters.NUM_2;
            characterSet.Three = Images.Characters.NUM_3;
            characterSet.Four = Images.Characters.NUM_4;
            characterSet.Five = Images.Characters.NUM_5;
            characterSet.Six = Images.Characters.NUM_6;
            characterSet.Seven = Images.Characters.NUM_7;
            characterSet.Eight = Images.Characters.NUM_8;
            characterSet.Nine = Images.Characters.NUM_9;
            characterSet.Period = Images.Characters.PERIOD;
            characterSet.Exclamation = Images.Characters.EXCLAMATION;
            characterSet.Question = Images.Characters.QUESTION;
            characterSet.Comma = Images.Characters.COMMA;
            characterSet.QuoteDouble = Images.Characters.HAPPY;
            characterSet.QuoteSingle = Images.Characters.APOSTROPHE;
            characterSet.Colon = Images.Characters.COLON;
            characterSet.ParenthesisLeft = Images.Characters.LEFT_PARENTHESIS;
            characterSet.ParenthesisRight = Images.Characters.RIGHT_PARENTHESIS;
            characterSet.BracketLeft = Images.Characters.LEFT_BRACKET;
            characterSet.BracketRight = Images.Characters.RIGHT_BRACKET;
            characterSet.Minus = Images.Characters.MINUS;
            characterSet.Pound = Images.Characters.POUND;
            characterSet.Dollars = Images.Characters.HAPPY;
            characterSet.Space = Images.Characters.SPACE;
            characterSet.LessThan = Images.Characters.LEFT;
            characterSet.GreaterThan = Images.Characters.RIGHT;
            characterSet.Carot = Images.Characters.UP;
            characterSet.Tilde = Images.Characters.DOWN;
            characterSet.Asterisk = Images.Characters.CURSOR;
            characterSet.At = Images.Characters.EMPTY;
        }
    }
}
