using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public static bool ButtonPressed(GamePadState gamepad, GamePadState gamepadPrev)
        {
            return
                (gamepad.Buttons.A == ButtonState.Pressed && gamepadPrev.Buttons.A == ButtonState.Released) ||
                (gamepad.Buttons.B == ButtonState.Pressed && gamepadPrev.Buttons.B == ButtonState.Released) ||
                (gamepad.Buttons.X == ButtonState.Pressed && gamepadPrev.Buttons.X == ButtonState.Released) ||
                (gamepad.Buttons.Y == ButtonState.Pressed && gamepadPrev.Buttons.Y == ButtonState.Released);
        }

        public static bool ButtonReleased(GamePadState gamepad, GamePadState gamepadPrev)
        {
            return
                (gamepad.Buttons.A == ButtonState.Released && gamepadPrev.Buttons.A == ButtonState.Pressed) ||
                (gamepad.Buttons.B == ButtonState.Released && gamepadPrev.Buttons.B == ButtonState.Pressed) ||
                (gamepad.Buttons.X == ButtonState.Released && gamepadPrev.Buttons.X == ButtonState.Pressed) ||
                (gamepad.Buttons.Y == ButtonState.Released && gamepadPrev.Buttons.Y == ButtonState.Pressed);
        }

        public static bool DPadLeftPressed(GamePadState gamepad, GamePadState gamepadPrev)
        {
            return gamepad.DPad.Left == ButtonState.Pressed && gamepadPrev.DPad.Left == ButtonState.Released;
        }

        public static bool DPadRightPressed(GamePadState gamepad, GamePadState gamepadPrev)
        {
            return gamepad.DPad.Right == ButtonState.Pressed && gamepadPrev.DPad.Right == ButtonState.Released;
        }

        public static bool DPadUpPressed(GamePadState gamepad, GamePadState gamepadPrev)
        {
            return gamepad.DPad.Up == ButtonState.Pressed && gamepadPrev.DPad.Up == ButtonState.Released;
        }

        public static bool DPadDownPressed(GamePadState gamepad, GamePadState gamepadPrev)
        {
            return gamepad.DPad.Down == ButtonState.Pressed && gamepadPrev.DPad.Down == ButtonState.Released;
        }

        public static bool KeyPressed(KeyboardState keys, KeyboardState keysPrev, Keys key)
        {
            return keys.IsKeyDown(key) && keysPrev.IsKeyUp(key);
        }

        public static bool KeyReleased(KeyboardState keys, KeyboardState keysPrev,Keys key)
        {
            return keys.IsKeyUp(key) && keysPrev.IsKeyDown(key);
        }

        public static bool PointToRectCollision(Vector2 point, Rectangle rect)
        {
            return point.X >= rect.Left && point.X < rect.Right && point.Y >= rect.Top && point.Y < rect.Bottom;
        }

        public static void LoadLevel(uint level)
        {
            trail.Clear();
            spriteSet.Clear();
            spriteSet.AddRange(Map.Load(level));
            spriteSet.Add(player);

            ResetTimers();
            startTimer = START_DURATION;
            gateTimer = FIRST_LEVEL_TIME + (level * TIME_INCREMENT);
            arpeggioIndex = 0;
            message = "Ready?";
            player.Reset();
            Vortex.Reset();
            Vortex.DeterminePresence(spriteSet);
        }

        public static void LoadTutorialStep(uint level)
        {
            uint? gateTimerPreserve = gateTimer;

            trail.Clear();
            spriteSet.Clear();
            spriteSet.AddRange(Map.Load(level, tutorialOn: true));
            spriteSet.Add(player);

            ResetTimers();
            gateTimer = gateTimerPreserve; //Preserve the original gate timer value from before ResetTimers() was invoked.
            startTimer = START_DURATION;
            player.Reset();
        }

        public static bool ProcessActionDelay()
        {
            if (startTimer == null || (clockTimer != null && clockTimer % 2 == 0))

                return true;

            else if (--startTimer == 0)
            {
                startTimer = null;
                message = null;
            }
            else message = "Ready? " + ((int)(3.0f * (startTimer / (float)START_DURATION)) + 1) + "...";

            return false;
        }

        public static void ResetTimers()
        {
            gateTimer = null;
            clockTimer = null;
            startTimer = null;
            deathTimer = null;
            clearTimer = null;
            orbTimer = 0;
        }

        public static bool InSession()
        {
            return gameMode == GameModes.Action && startTimer == null && deathTimer == null && clearTimer == null;
        }

        public static Texture2D DetermineNoise()
        {
            switch (rand.Next(5))
            {
                case 0: return Images.Noise.NOISE_A;
                case 1: return Images.Noise.NOISE_B;
                case 2: return Images.Noise.NOISE_C;
                case 3: return Images.Noise.NOISE_D;
                case 4: return Images.Noise.NOISE_E;
            }

            return Images.Noise.NOISE_A;
        }

        public static void PlayMusic(Song song, bool repeat = false)
        {
            if (currentSong != song)
            {
                if (currentSound != null) currentSound.Stop();
                if (currentArpeggioNote != null) currentArpeggioNote.Stop();

                MediaPlayer.Stop();
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = repeat;
                currentSong = song;
            }
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = false;
            currentSong = null;
        }

        public static void PlaySound(SoundEffectInstance sound)
        {
            if (gameMode == GameModes.Tutorial)

                return;

            else if (currentSound != null && currentSound.State == SoundState.Playing)
            {
                Sounds.SoundCharacteristics targetCharacteristics = Sounds.GetCharacteristics(sound);
                Sounds.SoundCharacteristics currentCharacteristics = Sounds.GetCharacteristics(currentSound);

                if ((targetCharacteristics.interrupts && targetCharacteristics.priority < currentCharacteristics.priority) ||
                    (!targetCharacteristics.interrupts && targetCharacteristics.priority <= currentCharacteristics.priority))

                    return;
            }

            if (currentSound != null)
                currentSound.Stop();

            if (currentSound == sound)
            {
                SoundEffectInstance duplicateSound = Sounds.RetrieveDuplicate(sound);
                currentSound = duplicateSound == null ? sound : duplicateSound;
            }
            else currentSound = sound;

            currentSound.Volume = MediaPlayer.Volume; //Set sound effect volume equal to music volume.
            currentSound.Play();
        }

        public static void PlayArpeggio()
        {
            const uint INTERVAL_SLOW = 20;
            const uint INTERVAL_MEDIUM = 10;
            const uint INTERVAL_FAST = 5;
            const uint INTERVAL_CLOCK = 12;

            if (gameMode == GameModes.Tutorial)

                return;

            else if (!(deathTimer == null && clearTimer == null && startTimer == null))

                return;

            else if (clockTimer != null)
            {
                const uint HALF_TIME = INTERVAL_CLOCK / 2;

                uint elapsedTime = universalTimer % INTERVAL_CLOCK;

                //Accounting for the fact that the universal timer may be landing on odd numbers.

                if (elapsedTime == 0 || elapsedTime == 1)
                    Sounds.Pitches.TICK.Play();
                else if (elapsedTime == HALF_TIME || elapsedTime == HALF_TIME + 1)
                    Sounds.Pitches.TOCK.Play();

                return;
            }

            uint arpeggioInterval = INTERVAL_SLOW;

            switch (player.CurrentSpeed)
            {
                case Player.Speeds.Slow:
                    arpeggioInterval = INTERVAL_SLOW;
                    break;
                case Player.Speeds.Medium:
                    arpeggioInterval = INTERVAL_MEDIUM;
                    break;
                case Player.Speeds.Fast:
                    arpeggioInterval = INTERVAL_FAST;
                    break;
            }

            if (universalTimer % arpeggioInterval == 0)
            {
                int i = (int)level / 2;
                int j = (int)arpeggioIndex % Sounds.Pitches.Arpeggio[i].Length;

                Sounds.Pitches.Arpeggio[i][j].Play();

                arpeggioIndex++;
            }
        }

        public static string FilterName(RenderEffects filter)
        {
            string s = filter.ToString();

            for (int i = s.Length - 1; i > 0; i--)
                if (Char.IsUpper(s[i]))
                    s = s.Insert(i, " ");

            return s;
        }
    }
}
