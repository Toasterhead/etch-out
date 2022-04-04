using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Etcher
{
    public partial class Game1 : Game 
    {
        public static void NewGame(bool tutorial = false)
        {
            gameMode = tutorial ? GameModes.Tutorial : GameModes.Action;

            //Ensure that the game returns to the main menu after the play session.
            MenuManager.TitleMenu.CurrentSubMenu = MenuManager.TitleMenu.GetSubMenu(MenuManager.MenuName.MAIN);

            gateTimer = FIRST_LEVEL_TIME;
            level = 0;
            points = 0;
            message = null;

            player = new Player(tutorial);
            trail = new List<Vector2>();
            pendingSet = new Stack<IGameObject>();
            removalSet = new Stack<IGameObject>();

            const int NUM_DEBRIS_PIECES = 50;
            debris = new Stack<Debris>();
            for (int i = 0; i < NUM_DEBRIS_PIECES; i++)
                debris.Push(new Debris());

            spriteSet = new List<IGameObject>();
            pickups = Map.LoadPickups(0);

            if (!tutorial)
            {
                borderColor = Color.Blue;
                LoadLevel(level);
                startTimer = FIRST_START_DURATION; //Overrides initialization in LoadLevel();
                PlayMusic(Sounds.Music.START);
            }
            else
            {
                borderColor = Color.Yellow;
                LoadTutorialStep(level);
                Tutorial.CommandIndex = 0;
                gateTimer = 34 * FRAME_RATE;
                PlayMusic(Sounds.Music.TUTORIAL, repeat: true);
            }

            universalTimeStamp = universalTimer;
        }
    }
}
