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
                HighScore.SubmitScore(new HighScore.FieldEntry(HighScore.NameEntry, (int)points));
                HighScore.ResetFields();
                MenuManager.UpdateHighScore(HighScore.TopScoresAsString());
                HighScore.WriteToFileAsync();

                gameMode = GameModes.Title;
                PlayMusic(Sounds.Music.TITLE, repeat: true);
            }
        }
    }
}
