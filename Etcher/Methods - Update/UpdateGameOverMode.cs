using Microsoft.Xna.Framework;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void UpdateGameOverMode()
        {
            int gameOverDelay = gameMode == GameModes.GameOver ? 9 : 18;
            uint elapsedTime = universalTimer - universalTimeStamp;

            bool halfwayElapsed = elapsedTime >= (gameOverDelay / 2) * FRAME_RATE;
            bool beforeFinalSeconds = elapsedTime < (gameOverDelay - 2) * FRAME_RATE;

            if (elapsedTime > gameOverDelay * FRAME_RATE)
            {
                HighScore.FieldEntry[] topScore = bType ? HighScore.TopScoreB : HighScore.TopScoreA;
                int? newHighScoreIndex = null;

                for (int i = 0; i < topScore.Length; i++)
                
                    if (points > topScore[i].score)
                    {
                        newHighScoreIndex = i;
                        break;
                    }

                if (newHighScoreIndex == null)
                {
                    gameMode = GameModes.Title;
                    PlayMusic(Sounds.Music.TITLE, repeat: true);
                }
                else
                {
                    gameMode = GameModes.HighScore;
                    PlayMusic(Sounds.Music.HIGH_SCORE);
                    HighScore.ResetFields();
                }
            }

            if (gameMode == GameModes.GameComplete && halfwayElapsed && beforeFinalSeconds)
            {
                if (elapsedTime == (gameOverDelay / 2) * FRAME_RATE || rand.Next((int)((0.2 / 0.3) * FRAME_RATE)) == 0)
                {
                    const int X_MIN = FULLFIELD_WIDTH / 6;
                    const int X_MAX = FULLFIELD_WIDTH - (FULLFIELD_WIDTH / 6);
                    const int Y_MIN = FULLFIELD_HEIGHT / 5;
                    const int Y_MAX = FULLFIELD_HEIGHT - (FULLFIELD_HEIGHT / 5);

                    int numCinders = rand.Next(15, 40);
                    Point position = new Point(rand.Next(X_MIN, X_MAX), rand.Next(Y_MIN, Y_MAX));
                    Color color = Color.Red;
                    switch (rand.Next(6))
                    {
                        case 0: color = Color.Red;
                            break;
                        case 1: color = Color.Orange;
                            break;
                        case 2: color = Color.Yellow;
                            break;
                        case 3: color = Color.Green;
                            break;
                        case 4: color = Color.Blue;
                            break;
                        case 5: color = Color.Purple;
                            break;
                    }

                    for (int i = 0; i < numCinders; i++)
                    {
                        Debris cinder = new Debris();
                        cinder.SetPosition(position);
                        cinder.SetColor(color);
                        spriteSet.Add(cinder);
                    }

                    PlaySound(Sounds.FIREWORK);
                }
            }

            foreach (IGameObject i in spriteSet)

                if (i is Debris)
                {
                    i.Update();

                    if (i.Remove)
                        removalSet.Push(i);
                }

            while (removalSet.Count > 0)
                spriteSet.Remove(removalSet.Pop());
        }
    }
}
