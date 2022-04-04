using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void UpdateTitleMode()
        {
            if (MenuManager.TitleMenu.CurrentSubMenu.Title == MenuManager.MenuName.SETTINGS)
            {
                SubMenu settingsMenu = MenuManager.TitleMenu.GetSubMenu(MenuManager.MenuName.SETTINGS);

                if (MenuManager.TitleMenu.GetInt("resolution").Value % 2 == 0)
                {
                    (settingsMenu.GetAtIndex(MenuManager.MenuItemIndices.FILTER) as ISelectable).Muted = true;
                    (settingsMenu.GetAtIndex(MenuManager.MenuItemIndices.FILTER_BRIGHTNESS) as ISelectable).Muted = true;
                }
                else
                {
                    (settingsMenu.GetAtIndex(MenuManager.MenuItemIndices.FILTER) as ISelectable).Muted = false;
                    (settingsMenu.GetAtIndex(MenuManager.MenuItemIndices.FILTER_BRIGHTNESS) as ISelectable).Muted = false;
                }
            }

            if (MenuManager.CheckFinished())
            {
                gameMode = GameModes.Message;
            }
        }
    }
}
