using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void UpdateMessageeMode()
        {
            if (messageTimer == null)

                NewGame(MenuManager.StartTutorial);

            else
            {
                messageTimer--;

                if (messageTimer <= 0)
                    messageTimer = null;
            }
        }
    }
}
