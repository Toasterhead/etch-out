using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Etcher
{
    public static class Sounds
    {
        public struct SoundCharacteristics
        {
            public uint priority;
            public bool interrupts;

            public SoundCharacteristics(uint priority, bool interrupts)
            {
                this.priority = priority;
                this.interrupts = interrupts;
            }
        }

        public static SoundEffectInstance PICKUP_A;
        public static SoundEffectInstance PICKUP_B;
        public static SoundEffectInstance ORB_APPEARS_A;
        public static SoundEffectInstance ORB_APPEARS_B;
        public static SoundEffectInstance ORB_APPEARS_C;
        public static SoundEffectInstance ORB_APPEARS_D;
        public static SoundEffectInstance SELECT;
        public static SoundEffectInstance PAUSE;
        public static SoundEffectInstance GATES_OPEN;
        public static SoundEffectInstance CRASH;
        public static SoundEffectInstance FIREWORK;
        public static SoundEffectInstance NAME_INPUT;

        public static SoundEffectInstance DUPLICATE_PICKUP_A;
        public static SoundEffectInstance DUPLICATE_PICKUP_B;
        public static SoundEffectInstance DUPLICATE_ORB_APPEARS_A;
        public static SoundEffectInstance DUPLICATE_ORB_APPEARS_B;
        public static SoundEffectInstance DUPLICATE_ORB_APPEARS_C;
        public static SoundEffectInstance DUPLICATE_ORB_APPEARS_D;
        public static SoundEffectInstance DUPLICATE_FIREWORK;
        public static SoundEffectInstance DUPLICATE_NAME_INPUT;

        public static class Pitches
        {
            public static SoundEffectInstance B1;
            public static SoundEffectInstance E2;
            public static SoundEffectInstance GSharp2;
            public static SoundEffectInstance B2;
            public static SoundEffectInstance DSharp3;
            public static SoundEffectInstance E3;
            public static SoundEffectInstance GSharp3;
            public static SoundEffectInstance TICK;
            public static SoundEffectInstance TOCK;

            public static List<SoundEffectInstance[]> Arpeggio { get; private set; } 

            public static void InitializeArpeggios()
            {
                Arpeggio = new List<SoundEffectInstance[]>()
                {
                    new SoundEffectInstance[2]  { E2, B2 },
                    new SoundEffectInstance[3]  { E2, B2, E3 },
                    new SoundEffectInstance[4]  { E2, B2, E3, B2 },
                    new SoundEffectInstance[6]  { E2, GSharp2, B2, E3, B2, GSharp2 },
                    new SoundEffectInstance[8]  { E2, GSharp2, B2, E3, DSharp3, E3, B2, GSharp2 },
                    new SoundEffectInstance[12] { E2, GSharp2, B2, E3, DSharp3, E3, GSharp3, E3, DSharp3, E3, B2, GSharp2 },
                    new SoundEffectInstance[16] { E2, B1, GSharp2, E2, B2, GSharp2, E3, B2, DSharp3, B2, E3, GSharp2, B2, E2, GSharp2, B1 },
                    new SoundEffectInstance[24]
                    {
                        E2, B1, E2, GSharp2, E2, GSharp2, B2, GSharp2, B2, E3, B2, E3,
                        DSharp3, B2, DSharp3, E3, B2, E3, B2, GSharp2, B2, GSharp2, E2, GSharp2
                    }
                };
            }
        }

        public static class Music
        {
            public static Song TITLE;
            public static Song TUTORIAL;
            public static Song START;
            public static Song STAGE_CLEAR;
            public static Song GAME_OVER;
            public static Song HIGH_SCORE;
            public static Song VICTORY;
        }

        public static SoundCharacteristics GetCharacteristics(SoundEffectInstance sound)
        {
            if (sound == ORB_APPEARS_A || sound == ORB_APPEARS_B || sound == ORB_APPEARS_C || sound == ORB_APPEARS_C)
                return new SoundCharacteristics(0, false);
            else if (sound == GATES_OPEN)
                return new SoundCharacteristics(1, false);
            else if (sound == PICKUP_A)
                return new SoundCharacteristics(2, true);
            else if (sound == PICKUP_B)
                return new SoundCharacteristics(3, true);
            else if (sound == CRASH)
                return new SoundCharacteristics(4, true);

            return new SoundCharacteristics(0, true);
        }

        public static SoundEffectInstance RetrieveDuplicate(SoundEffectInstance sound)
        {
            if (sound == PICKUP_A)
                return DUPLICATE_PICKUP_A;
            else if (sound == PICKUP_B)
                return DUPLICATE_PICKUP_B;
            else if (sound == ORB_APPEARS_A)
                return DUPLICATE_ORB_APPEARS_A;
            else if (sound == ORB_APPEARS_B)
                return DUPLICATE_ORB_APPEARS_B;
            else if (sound == ORB_APPEARS_C)
                return DUPLICATE_ORB_APPEARS_C;
            else if (sound == ORB_APPEARS_D)
                return DUPLICATE_ORB_APPEARS_D;
            else if (sound == FIREWORK)
                return DUPLICATE_FIREWORK;
            else if (sound == NAME_INPUT)
                return DUPLICATE_NAME_INPUT;

            return null;
        }
    }
}
