using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void UpdateHighScoreMode()
        {
            if (HighScore.CheckFinished())
            {
                HighScore.SubmitScore(new HighScore.FieldEntry(HighScore.NameEntry, (int)points), bType);
                HighScore.ResetFields();
                MenuManager.UpdateHighScore(HighScore.TopScoresAsString(bType), bType);
                HighScore.WriteToFileAsync(bType);

                gameMode = GameModes.Title;
                PlayMusic(Sounds.Music.TITLE, repeat: true);
            }
        }
    }
}
