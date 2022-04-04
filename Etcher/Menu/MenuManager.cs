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
            public const string START = "Start";
            public const string HIGH_SCORE_A = "High Score - A Type";
            public const string HIGH_SCORE_B = "High Score - B Type";
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
            
            List<DataString> dataStrings = new List<DataString>(); //Separate for A-Type and B-Type game modes later.
            for (int i = 0; i < 10; i++)
            {
                dataStrings.Add(new DataString("highScoreA" + i, "[blank]"));
                dataStrings.Add(new DataString("highScoreB" + i, "[blank]"));
            }

            List<SubMenu> subMenus = new List<SubMenu>();
            subMenus.Add(new SubMenu(MenuName.MAIN, wrapAround: true));
            subMenus.Add(new SubMenu(MenuName.START, wrapAround: true));
            subMenus.Add(new SubMenu(MenuName.HIGH_SCORE_A, wrapAround: true));
            subMenus.Add(new SubMenu(MenuName.HIGH_SCORE_B, wrapAround: true));
            subMenus.Add(new SubMenu(MenuName.SETTINGS, wrapAround: true));
            subMenus.Add(new SubMenu(MenuName.ABOUT, wrapAround: true));

            TitleMenu = new Menu(subMenus, dataBooleans, dataIntegers, dataDoubles, dataStrings);

            MenuItem[] mainItems = new MenuItem[6];
            mainItems[0] = new MILink("Start", TitleMenu.GetSubMenu(MenuName.START));
            mainItems[1] = new MICommand("Tutorial", Tutorial);
            mainItems[2] = new MILink("High Score", TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_A));
            mainItems[3] = new MILink("Settings", TitleMenu.GetSubMenu(MenuName.SETTINGS));
            mainItems[4] = new MILink("About", TitleMenu.GetSubMenu(MenuName.ABOUT));
            mainItems[5] = new MICommand("Quit", Quit);

            MenuItem[] startItems = new MenuItem[4];
            startItems[0] = new MICommand("Start A Type Game", StartA);
            startItems[1] = new MICommand("Start B Type Game", StartB);
            startItems[2] = new MISpace();
            startItems[3] = new MILink("Back", TitleMenu.GetSubMenu(MenuName.MAIN));

            MenuItem[] settingsItems = new MenuItem[8];
            settingsItems[0] = new MIDial(TitleMenu.GetInt("resolution"), "Screen Resolution: ", resolutions.ToArray());
            settingsItems[1] = new MIDial(TitleMenu.GetInt("filter"), "Screen Filter: ", filters);
            settingsItems[2] = new MISwitch(TitleMenu.GetBool("fullscreenOn"), "Fullscreen: ", "Yes", "No");
            settingsItems[3] = new MISlider(TitleMenu.GetDouble("brightness"), "Filter Brightness: ", 10, 1);
            settingsItems[4] = new MISlider(TitleMenu.GetDouble("soundVolume"), "Sound Volume: ", 10, 1);
            settingsItems[5] = new MICommand("Apply", Apply);
            settingsItems[6] = new MISpace();
            settingsItems[7] = new MILink("Back", TitleMenu.GetSubMenu(MenuName.MAIN));

            (settingsItems[0] as MIDial).Forward();
            (settingsItems[3] as MISlider).OutputValue = false;
            (settingsItems[4] as MISlider).OutputValue = false;

            MenuItem[] highScoreAItems = new MenuItem[14];
            highScoreAItems[0] = new MIHeadline(TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_A).Title);
            highScoreAItems[1] = new MISpace();
            for (int i = 2; i <= 11; i++)
                highScoreAItems[i] = new MIHeadline(TitleMenu.GetString("highScoreA" + (i - 2)).Value);
            highScoreAItems[12] = new MISpace();
            highScoreAItems[13] = new MILink("Next", TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_B));

            MenuItem[] highScoreBItems = new MenuItem[14];
            highScoreBItems[0] = new MIHeadline(TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_B).Title);
            highScoreBItems[1] = new MISpace();
            for (int i = 2; i <= 11; i++)
                highScoreBItems[i] = new MIHeadline(TitleMenu.GetString("highScoreB" + (i - 2)).Value);
            highScoreBItems[12] = new MISpace();
            highScoreBItems[13] = new MILink("Back to Main Menu", TitleMenu.GetSubMenu(MenuName.MAIN));

            MenuItem[] aboutItems = new MenuItem[5];
            aboutItems[0] = new MIHeadline("Music, sound, graphics,");
            aboutItems[1] = new MIHeadline("programming and design");
            aboutItems[2] = new MIHeadline("by Lenny Young.");
            aboutItems[3] = new MISpace();
            aboutItems[4] = new MILink("Back", TitleMenu.GetSubMenu(MenuName.MAIN));

            TitleMenu.GetSubMenu(MenuName.MAIN).SetItems(mainItems.ToList());
            TitleMenu.GetSubMenu(MenuName.START).SetItems(startItems.ToList());
            TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_A).SetItems(highScoreAItems.ToList());
            TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_B).SetItems(highScoreBItems.ToList());
            TitleMenu.GetSubMenu(MenuName.SETTINGS).SetItems(settingsItems.ToList());
            TitleMenu.GetSubMenu(MenuName.ABOUT).SetItems(aboutItems.ToList());
        }

        public static void ConstructPauseMenu()
        {
            List<SubMenu> subMenus = new List<SubMenu>();
            subMenus.Add(new SubMenu(MenuName.MAIN, wrapAround: true));

            PauseMenu = new Menu(subMenus);

            List <MenuItem> mainItems = new List<MenuItem>();
            mainItems.Add(new MICommand("Resume", Resume));
            mainItems.Add(new MICommand("Quit to Menu", QuitToMenu));

            PauseMenu.GetSubMenu(MenuName.MAIN).SetItems(mainItems);
        }

        public static void ConstructTextFields()
        {
            TheTextfields = new Textfield[NUM_FIELDS];
            for (int i = 0; i < TheTextfields.Length; i++)
                TheTextfields[i] = new Textfield(Game1.characterSet, "", 0, 0, 40 * Game1.TILE_SIZE);
        }

        public static void UpdateHighScore(string[] topScore, bool updateBType = false)
        {
            for (int i = 0; i < 10; i++)
                TitleMenu.GetString("highScore" + (updateBType ? "B" : "A") + i).Value = topScore[i];

            MenuItem[] highScoreItems = new MenuItem[14];

            if (updateBType)
            {               
                highScoreItems[0] = new MIHeadline(TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_B).Title);
                highScoreItems[1] = new MISpace();
                for (int i = 2; i <= 11; i++)
                    highScoreItems[i] = new MIHeadline(TitleMenu.GetString("highScoreB" + (i - 2)).Value);
                highScoreItems[12] = new MISpace();
                highScoreItems[13] = new MILink("Back to Main Menu", TitleMenu.GetSubMenu(MenuName.MAIN));

                TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_B).SetItems(highScoreItems.ToList());
            }
            else
            {
                highScoreItems[0] = new MIHeadline(TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_A).Title);
                highScoreItems[1] = new MISpace();
                for (int i = 2; i <= 11; i++)
                    highScoreItems[i] = new MIHeadline(TitleMenu.GetString("highScoreA" + (i - 2)).Value);
                highScoreItems[12] = new MISpace();
                highScoreItems[13] = new MILink("Next", TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_B));

                TitleMenu.GetSubMenu(MenuName.HIGH_SCORE_A).SetItems(highScoreItems.ToList());
            }
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

        public static void StartA(Data[] data)
        {
            Finished = FINISHED_DURATION;
            StartTutorial = false;
            Game1.bType = false;
            Game1.StopMusic();
            Game1.PlaySound(Sounds.SELECT);
        }
        public static void StartB(Data[] data)
        {
            StartA(null);
            Game1.message = 
                "In a type-B game, there are no " +
                "stages or exit gates. Try to last as " +
                "long as possible!";
            Game1.messageTimer = 5 * Game1.FRAME_RATE;
            Game1.bType = true;
        }
        public static void Tutorial(Data[] data)
        {
            Finished = FINISHED_DURATION;
            StartTutorial = true;
            Game1.bType = false;
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
