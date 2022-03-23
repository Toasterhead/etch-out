using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Etcher
{
    public static class MenuManager
    {
        public static class MenuName
        {
            public const string MAIN = "Main";
            public const string HIGH_SCORE = "High Score";
            public const string SETTINGS = "Settings";
            public const string ABOUT = "About";
        }

        public static class MenuItemIndices
        {
            public const int FILTER = 1;
            public const int FILTER_BRIGHTNESS = 3;
        }

        private const int NUM_FIELDS = 14;
        private const uint FINISHED_DURATION = 60;

        public static Textfield[] TheTextfields;
        public static Menu TitleMenu;
        public static Menu WaypointMenu;
        public static Menu PauseMenu;

        public static uint? Finished { get; private set; } = null;
        public static bool StartTutorial { get; private set; } = false;

        public static void ConstructTitleMenu()
        {
            //Obtain supported screen resolutions...

            Point currentResolution = Master.CurrentResolution;
            Point[] gameResolutions = new Point[8];

            for (int i = 0; i < gameResolutions.Length; i++)
                gameResolutions[i] = new Point((i + 1) * Game1.FULLFIELD_WIDTH, (i + 1) * Game1.FULLFIELD_HEIGHT);

            List<string> resolutions = new List<string>();

            foreach (Point i in gameResolutions)
            
                if (currentResolution.X > i.X && currentResolution.Y > i.Y)
                    resolutions.Add(i.X + " x " + i.Y);

            string[] filters = new string[(int)Game1.RenderEffects.EnumSize];

            for (int i = 0; i < (int)Game1.RenderEffects.EnumSize; i++)
                filters[i] = Game1.FilterName((Game1.RenderEffects)i);

            //Construct menus...

            List<DataBool> dataBooleans = new List<DataBool>();
            dataBooleans.Add(new DataBool("fullscreenOn", false));

            List<DataInt> dataIntegers = new List<DataInt>();
            dataIntegers.Add(new DataInt("resolution", 0));
            dataIntegers.Add(new DataInt("filter", 0));

            List<DataDouble> dataDoubles = new List<DataDouble>();
            dataDoubles.Add(new DataDouble("soundVolume", 0.3));
            dataDoubles.Add(new DataDouble("brightness", 1.0));
            
            List<DataString> dataStrings = new List<DataString>();
            for (int i = 0; i < 10; i++)
                dataStrings.Add(new DataString("highScore" + i, "[blank]"));

            List<SubMenu> subMenus = new List<SubMenu>();
            subMenus.Add(new SubMenu(MenuName.MAIN, wrapAround: true));
            subMenus.Add(new SubMenu(MenuName.HIGH_SCORE, wrapAround: true));
            subMenus.Add(new SubMenu(MenuName.SETTINGS, wrapAround: true));
            subMenus.Add(new SubMenu(MenuName.ABOUT, wrapAround: true));

            TitleMenu = new Menu(subMenus, dataBooleans, dataIntegers, dataDoubles, dataStrings);

            MenuItem[] mainItems = new MenuItem[6];
            mainItems[0] = new MICommand("Start", Start);
            mainItems[1] = new MICommand("Tutorial", Tutorial);
            mainItems[2] = new MILink("High Score", TitleMenu.GetSubMenu("High Score"));
            mainItems[3] = new MILink("Settings", TitleMenu.GetSubMenu("Settings"));
            mainItems[4] = new MILink("About", TitleMenu.GetSubMenu("About"));
            mainItems[5] = new MICommand("Quit", Quit);

            MenuItem[] settingsItems = new MenuItem[8];
            settingsItems[0] = new MIDial(TitleMenu.GetInt("resolution"), "Screen Resolution: ", resolutions.ToArray());
            settingsItems[1] = new MIDial(TitleMenu.GetInt("filter"), "Screen Filter: ", filters);
            settingsItems[2] = new MISwitch(TitleMenu.GetBool("fullscreenOn"), "Fullscreen: ", "Yes", "No");
            settingsItems[3] = new MISlider(TitleMenu.GetDouble("brightness"), "Filter Brightness: ", 10, 1);
            settingsItems[4] = new MISlider(TitleMenu.GetDouble("soundVolume"), "Sound Volume: ", 10, 1);
            settingsItems[5] = new MICommand("Apply", Apply);
            settingsItems[6] = new MISpace();
            settingsItems[7] = new MILink("Back", TitleMenu.GetSubMenu("Main"));

            (settingsItems[0] as MIDial).Forward();
            (settingsItems[3] as MISlider).OutputValue = false;
            (settingsItems[4] as MISlider).OutputValue = false;

            MenuItem[] highScoreItems = new MenuItem[14];
            highScoreItems[0] = new MIHeadline(TitleMenu.GetSubMenu(MenuName.HIGH_SCORE).Title);
            highScoreItems[1] = new MISpace();
            for (int i = 2; i <= 11; i++)
                highScoreItems[i] = new MIHeadline(TitleMenu.GetString("highScore" + (i - 2)).Value);
            highScoreItems[12] = new MISpace();
            highScoreItems[13] = new MILink("Back to Main Menu", TitleMenu.GetSubMenu("Main"));

            MenuItem[] aboutItems = new MenuItem[5];
            aboutItems[0] = new MIHeadline("Music, sound, graphics,");
            aboutItems[1] = new MIHeadline("programming and design");
            aboutItems[2] = new MIHeadline("by Lenny Young.");
            aboutItems[3] = new MISpace();
            aboutItems[4] = new MILink("Back", TitleMenu.GetSubMenu("Main"));

            TitleMenu.GetSubMenu("Main").SetItems(mainItems.ToList());
            TitleMenu.GetSubMenu("High Score").SetItems(highScoreItems.ToList());
            TitleMenu.GetSubMenu("Settings").SetItems(settingsItems.ToList());
            TitleMenu.GetSubMenu("About").SetItems(aboutItems.ToList());
        }

        public static void ConstructPauseMenu()
        {
            List<SubMenu> subMenus = new List<SubMenu>();
            subMenus.Add(new SubMenu("Main", wrapAround: true));

            PauseMenu = new Menu(subMenus);

            List <MenuItem> mainItems = new List<MenuItem>();
            mainItems.Add(new MICommand("Resume", Resume));
            mainItems.Add(new MICommand("Quit to Menu", QuitToMenu));

            PauseMenu.GetSubMenu("Main").SetItems(mainItems);
        }

        public static void ConstructTextFields()
        {
            TheTextfields = new Textfield[NUM_FIELDS];
            for (int i = 0; i < TheTextfields.Length; i++)
                TheTextfields[i] = new Textfield(Game1.characterSet, "", 0, 0, 40 * Game1.TILE_SIZE);
        }

        public static void UpdateHighScore(string[] topScore)
        {
            for (int i = 0; i < 10; i++)
                TitleMenu.GetString("highScore" + i).Value = topScore[i];

            MenuItem[] highScoreItems = new MenuItem[14];
            highScoreItems[0] = new MIHeadline(TitleMenu.GetSubMenu(MenuName.HIGH_SCORE).Title);
            highScoreItems[1] = new MISpace();
            for (int i = 2; i <= 11; i++)
                highScoreItems[i] = new MIHeadline(TitleMenu.GetString("highScore" + (i - 2)).Value);
            highScoreItems[12] = new MISpace();
            highScoreItems[13] = new MILink("Back to Main Menu", TitleMenu.GetSubMenu("Main"));

            TitleMenu.GetSubMenu("High Score").SetItems(highScoreItems.ToList());
        }

        public static bool CheckFinished()
        {
            if (Finished != null && --Finished == 0)
            {
                Finished = null;
                return true;
            }

            return false;
        }

        //Delegate Methods

        public static void Start(Data[] data)
        {
            Finished = FINISHED_DURATION;
            StartTutorial = false;
            Game1.StopMusic();
            Game1.PlaySound(Sounds.SELECT);
        }
        public static void Tutorial(Data[] data)
        {
            Finished = FINISHED_DURATION;
            StartTutorial = true;
            Game1.StopMusic();
            Game1.PlaySound(Sounds.SELECT);
        }
        public static void Apply(Data[] data)
        {
            int selected = TitleMenu.GetInt("resolution").Value;

            if (TitleMenu.GetBool("fullscreenOn").Value == false)
            {
                //Note: the following requests to change the window size may not succeed.
                Windows.Foundation.Size size = new Windows.Foundation.Size(
                        (selected + 1) * Game1.FULLFIELD_WIDTH,
                        (selected + 1) * Game1.FULLFIELD_HEIGHT);
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryResizeView(size);
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetPreferredMinSize(size);
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetDesiredBoundsMode(
                    Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);

                Game1.graphics.IsFullScreen = false;
                Game1.canvasMultiplier = selected + 1;
            }
            else
            {
                Point currentResolution = Master.CurrentResolution;
                int selectedWidth = (selected + 1) * Game1.FULLFIELD_WIDTH;
                int selectedHeight = (selected + 1) * Game1.FULLFIELD_HEIGHT;

                if (currentResolution.X >= selectedWidth &&
                    currentResolution.Y >= selectedHeight &&
                    (double)currentResolution.Y / currentResolution.X < 0.75)
                {
                    Game1.canvasMultiplier = selected + 1;
                    Game1.graphics.IsFullScreen = true;
                }
            }

            Game1.graphics.ApplyChanges();

            if ((TitleMenu.GetSubMenu(MenuName.SETTINGS).GetAtIndex(MenuItemIndices.FILTER) as ISelectable).Muted)
            {
                Game1.renderEffect = Game1.RenderEffects.None;
                Game1.effectBrightness = 1.0f;
            }
            else
            {
                Game1.renderEffect = (Game1.RenderEffects)TitleMenu.GetInt("filter").Value;
                Game1.effectBrightness = (float)TitleMenu.GetDouble("brightness").Value;
            }
            MediaPlayer.Volume = (float)TitleMenu.GetDouble("soundVolume").Value;
        }
        public static void Quit(Data[] data) { Game1.quit = true; }
        public static void Resume(Data[] data) { Game1.gameMode = Game1.GameModes.Action; }
        public static void QuitToMenu(Data[] data)
        {
            Game1.gameMode = Game1.GameModes.Title;
            Game1.PlayMusic(Sounds.Music.TITLE, repeat: true);
        }
    }
}
