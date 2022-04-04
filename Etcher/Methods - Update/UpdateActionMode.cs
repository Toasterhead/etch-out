using System;
using Microsoft.Xna.Framework;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void UpdateActionMode()
        {
            if (clockTimer != null && clockTimer % 2 == 0 && clockTimer > 0)
            {
                if (player.CurrentSpeed == Player.Speeds.Fast)
                    clockTimer = null;

                clockTimer--;

                return;
            }

            if (Orb.Appears(orbTimer, level) && gameMode != GameModes.Tutorial)
            {
                bool clockwise = rand.Next(2) == 0;
                bool isFast = Orb.AppearsAsFast(level);
                int positionX = rand.Next(rand.Next(2, 40) * Game1.TILE_SIZE);
                int positionY = rand.Next(rand.Next(2, 28) * Game1.TILE_SIZE);
                Orb orb = isFast ? new Orb(clockwise, Orb.Speeds.Fast) : new Orb(clockwise, Orb.Speeds.Slow);

                if (clockwise && !isFast)
                    PlaySound(Sounds.ORB_APPEARS_A);
                else if (!clockwise && !isFast)
                    PlaySound(Sounds.ORB_APPEARS_B);
                else if (clockwise && isFast)
                    PlaySound(Sounds.ORB_APPEARS_C);
                else PlaySound(Sounds.ORB_APPEARS_D);

                pendingSet.Push(new OrbAppearing(positionX, positionY, orb));
            }

            if (Game1.bType) ProcessDropItem(pendingSet);

            foreach (IGameObject i in spriteSet)
            {               
                i.Update();

                if (i.Remove)

                    removalSet.Push(i as IGameObject);

                if (i is Player && !i.Remove)
                {
                    bool playerCrashed = false;

                    //Player hit the trail?
                    foreach (Vector2 j in trail)

                        if (PointToRectCollision(j, (i as Player).GetHitBox(0)))
                        {
                            playerCrashed = true;
                            break;
                        }

                    //Player hit a hazard or picked up an item?
                    if (!playerCrashed)

                        foreach (IGameObject j in spriteSet)
                        {
                            bool isHazardous = j is Vortex || j is Block || j is Orb || j is Rod || (j is Zapper && (j as Zapper).Potent);
                            bool hazardCollidesWithPlayer = isHazardous && j.GetHitBox(0).Intersects(i.GetHitBox(0));
                            bool pickupCollidesWithPlayer = j is Pickup && j.Rect.Intersects(i.Rect);

                            if (hazardCollidesWithPlayer)
                            {
                                if (j is Vortex)
                                    playerCrashed = (j as Vortex).Collides(player);
                                else playerCrashed = true;

                                break;
                            }
                            else if (pickupCollidesWithPlayer)
                            {
                                if (j is Points)
                                {
                                    points += (j as Points).Value;
                                    PlaySound(Sounds.PICKUP_A);
                                }
                                else if (j is ExtraLife)
                                {
                                    (i as Player).IncreaseLives();
                                    PlaySound(Sounds.PICKUP_B);
                                }
                                else if (j is HyperSpeed)
                                {
                                    (i as Player).InitiateHyper();
                                    PlaySound(Sounds.PICKUP_B);
                                }
                                else if (j is Clock)
                                {
                                    clockTimer = CLOCK_DURATION;
                                    player.CancelHyper();
                                    player.MoveSlow();
                                }
                                else throw new Exception("Error - No defined action for object type: " + j);

                                removalSet.Push(j);

                                break;
                            }
                        }

                    //Player hit the scoreboard wall?
                    if (!playerCrashed && (i as Player).GetHitBox(0).Right > SCREEN_DIVIDER * TILE_SIZE)
                        playerCrashed = true;

                    //Process death if player crashed...
                    if (playerCrashed)
                    {
                        player.Remove = true;
                        player.Render = false;
                        player.DecreaseLives();
                        deathTimer = DEATH_DURATION;

                        int debrisCount = debris.Count;

                        for (int k = 0; k < debrisCount; k++)
                        {
                            Debris piece = debris.Pop();
                            piece.SetPosition(i as Player);
                            pendingSet.Push(piece);
                        }

                        for (int k = 0; k < debrisCount; k++)
                            debris.Push(new Debris());

                        if (clockTimer != null)
                            clockTimer = null;

                        PlaySound(Sounds.CRASH);
                    }

                    bool playerOffScreen = i.Center.X <= 0 || i.Center.Y <= 0 || i.Center.Y > FULLFIELD_HEIGHT;

                    //The player clears the stage...
                    if (playerOffScreen && gameMode != GameModes.Tutorial)
                    {
                        const uint LIVES_BONUS = 200;

                        points += (uint)player.Lives * LIVES_BONUS;
                        ResetTimers();
                        clearTimer = CLEAR_DURATION;
                        clockTimer = null;
                        player.Remove = true;
                        message = "Stage Clear!";
                        PlayMusic(Sounds.Music.STAGE_CLEAR);
                        break;
                    }
                }
                else if (i is Rod)
                    (i as Rod).ProcessCollision(spriteSet);
            }

            //Add Pending Game Objects

            for (int i = 0; i < pendingSet.Count; i++)
                spriteSet.Add(pendingSet.Pop());

            //Game Object Removal

            for (int i = 0; i < removalSet.Count; i++)
            {
                IGameObject gameObject = removalSet.Pop();

                spriteSet.Remove(gameObject);

                if (gameObject is Orb)
                    spriteSet.Add(new OrbAppearing(gameObject.X, gameObject.Y, materializingOrb: null));
                else if (gameObject is OrbAppearing && (gameObject as OrbAppearing).MaterializingOrb != null)
                    spriteSet.Add((gameObject as OrbAppearing).MaterializingOrb);
            }

            removalSet.Clear();

            //Process Trail

            Vector2? trailPoint = player.ObtainTrailPoint();
            if (trailPoint != null &&!player.Remove)
                trail.Add((Vector2)trailPoint);

            //Timer Adjustment

            Zapper.UpdateTimer();

            if (!Game1.bType && gateTimer != null && --gateTimer == 0)
            {
                gateTimer = null;

                foreach (IGameObject i in spriteSet)
                    if (i is Gate)
                        (i as Gate).InitiateOpening();

                if (gameMode != GameModes.Tutorial)

                    while (pickups.Count > 0)
                        pendingSet.Push(pickups.Pop());

                PlaySound(Sounds.GATES_OPEN);
            }

            if (deathTimer != null && --deathTimer == 0)
            {
                deathTimer = null;

                if (gameMode != GameModes.Tutorial)
                    universalTimeStamp = universalTimer;

                if (player.Lives > 0)
                {
                    foreach (IGameObject i in spriteSet)
                        if (i is Pickup)
                            pickups.Push(i as Pickup);

                    if (gameMode != GameModes.Tutorial)
                        LoadLevel(level);
                    else LoadTutorialStep(++level);
                }
                else
                {
                    gameMode = GameModes.GameOver;
                    universalTimeStamp = universalTimer;
                    message = "G A M E   O V E R";
                    PlayMusic(Sounds.Music.GAME_OVER);
                }
            }

            if (clearTimer != null && --clearTimer == 0)
            {
                clearTimer = null;
                LoadLevel(level);

                if (level + 1 < Map.Levels.Length)
                {
                    level++;

                    LoadLevel(level);
                    pickups = Map.LoadPickups(level);
                    StopMusic();
                }
                else
                {
                    spriteSet.Clear();
                    removalSet.Clear();

                    gameMode = GameModes.GameComplete;
                    universalTimeStamp = universalTimer;
                    message = "G A M E   C O M P L E T E !";
                    PlayMusic(Sounds.Music.VICTORY);
                }
            }

            Vortex.UpdateRadius();

            orbTimer++;

            //Play Arpeggio

            PlayArpeggio();

            //Increase Points

            if (clockTimer != null && --clockTimer == 0)

                clockTimer = null;

            else if (!player.Remove)

                switch (player.CurrentSpeed)
                {
                    case Player.Speeds.Slow:
                        points += (uint)(clockTimer != null ? 4 : 1);
                        break;
                    case Player.Speeds.Medium:
                        points += 3;
                        break;
                    case Player.Speeds.Fast:
                        points += 8;
                        break;
                }
        }
    }
}
