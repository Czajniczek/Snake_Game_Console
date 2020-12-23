using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    class Menu
    {
        public Menu() { }

        #region MENU

        #region VARIABLES AND OBJECTS
        //THREADS
        Thread music;

        //POSITION
        private static readonly int middleWindowWidth = Convert.ToInt32(0.5 * Console.WindowWidth);
        private static readonly int middleWindowHeight = Convert.ToInt32(0.5 * Console.WindowHeight);
        private readonly int startDravingHeaderXPosition = 13;
        private readonly int startDravingHeaderYPosition = 9;
        private readonly int middleHeightWithHeader = 21;

        //HEADER
        private static readonly List<string> headerList = new List<string>();

        //MUSIC
        private bool musicOn = true;
        private static SoundPlayer soundPlayer;

        //GAME
        Snake snake;
        private int level = 0;
        private bool ownSettings = false;
        private int additionalFruitLevel = 5000;
        #endregion VARIABLES AND OBJECTS

        #region NAVIGATION
        public void Navigation()
        {
            music = new Thread(PlayMusic);
            music.Start();
            LoadLogo();
            ShowFrame();
            ShowAnimatedLogo();
            MainMenu();
        }
        #endregion NAVIGATION

        #region PLAY MUSIC
        public void PlayMusic()
        {
            soundPlayer = new SoundPlayer("Music/Snake_Song.wav");
            soundPlayer.PlayLooping();
        }
        #endregion PLAY MUSIC

        #region LOADING LOGO
        private void LoadLogo()
        {
            StreamReader header = new StreamReader("ASCII Arts/Logo_Varsity.txt");

            string line;
            while ((line = header.ReadLine()) != null) headerList.Add(line);

            header.Close();
        }
        #endregion LOADING LOGO

        #region SHOW FRAME
        public void ShowFrame()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Console.ForegroundColor = ConsoleColor.DarkGray;

            for (int i = 4; i <= width - 3; i++)
            {
                for (int j = 2; j <= height - 2; j++)
                {
                    Console.SetCursorPosition(i, j);

                    #region FRAME 1
                    //if (i == 4 && j == 2) Console.Write("█▀\n");
                    //else if (i == 4 && j == height - 2) Console.Write("█▄\n");
                    //else if (i == width - 4 && j == 2) Console.Write("▀█\n");
                    //else if (i == width - 4 && j == height - 2) Console.Write("▄█\n");
                    //else if (i == 4 || i == width - 3) Console.Write("█\n");
                    //else if (j == 2) Console.Write("▀\n");
                    //else if (j == height - 2) Console.Write("▄\n");
                    //else Console.Write(".");
                    #endregion

                    #region FRAME 2 - BETTER
                    if (i == 4 && j == 2) Console.Write("▄\n"); //LEFT TOP CORNER
                    else if (i == 4 && j == height - 2) Console.Write("▀\n"); //LEFT BOTTOM CORNER
                    else if (i == width - 3 && j == 2) Console.Write("▄\n"); //RIGHT TOP CORNER
                    else if (i == width - 3 && j == height - 2) Console.Write("▀\n"); //RIGHT BOTTOM CORNER
                    else if (i == 4 || i == width - 3) Console.Write("█\n"); //RIGHT AND LEFT WALL
                    else if (j == 2) Console.Write("▄\n"); //TOP WALL
                    else if (j == height - 2) Console.Write("▀\n"); //BOTTOM WALL
                    #endregion
                }
            }

            Console.ResetColor();
        }
        #endregion SHOW FRAME

        #region SHOW LOGO
        private void ShowAnimatedLogo()
        {
            #region ANIMATION 1
            //int x = startDravingHeaderXPosition;
            //int y = startDravingHeaderYPosition;

            //for (int i = headerList.Count - 1; i >= 0; i--)
            //{
            //    Console.SetCursorPosition(x, y--);
            //    Console.ForegroundColor = ConsoleColor.Green;
            //    Console.Write(headerList[i]);
            //    Thread.Sleep(500);
            //}

            //Console.ResetColor();
            #endregion ANIMATION 1

            #region ANIMATION 2
            int x1 = startDravingHeaderXPosition;
            int y1 = startDravingHeaderYPosition;
            int x2 = x1;
            int y2 = 4;
            int height = headerList.Count - 1;
            int height2 = 0;

            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 3; i > 0; i--)
            {
                Console.SetCursorPosition(x1, y1--);
                Console.Write(headerList[height--]);
                Console.SetCursorPosition(x2, y2++);
                Console.Write(headerList[height2++]);
                Thread.Sleep(1000);
            }

            Console.ResetColor();
            #endregion ANIMATION 2
        }

        public void ShowLogo()
        {
            int x = startDravingHeaderXPosition;
            int y = startDravingHeaderYPosition;

            for (int i = headerList.Count - 1; i >= 0; i--)
            {
                Console.SetCursorPosition(x, y--);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(headerList[i]);
            }

            Console.ResetColor();
        }
        #endregion

        #region MAIN MENU
        public void MainMenu()
        {
            string[] availableOptions =
            {
                "            NEW GAME            ",
                "          BEST RESULTS          ",
                "            SETTINGS            ",
                "             AUTHOR             ",
                "              EXIT              "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 4) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 4;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 4) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 4;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        QuitQuestion();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    InitSnake();
                    break;

                case 1:
                    ShowBestResults(GetBestResult());
                    break;

                case 2:
                    Settings();
                    break;

                case 3:
                    ShowAuthor();
                    break;

                case 4:
                    QuitQuestion();
                    break;

                default: break;
            }
        }
        #endregion MAIN MENU

        #region PRINT MENU
        public void PrintMenu(int currentOption, string[] availableOptions, bool withHeader)
        {
            int numberOfavailableOptions = availableOptions.Length / 2;
            int midMenu;

            if (withHeader) midMenu = middleHeightWithHeader - numberOfavailableOptions; //MENU CENTRING WITH HEADER 
            else midMenu = middleWindowHeight - numberOfavailableOptions; //MENU CENTRING WITHOUT HEADER 

            for (int i = 0; i < availableOptions.Length; i++)
            {
                if (i == currentOption)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(middleWindowWidth - availableOptions[i].Length / 2, midMenu + i);
                    Console.WriteLine(availableOptions[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.SetCursorPosition(middleWindowWidth - availableOptions[i].Length / 2, midMenu + i);
                    Console.WriteLine(availableOptions[i]);
                }
            }
        }
        #endregion PRINT MENU

        #region INIT SNAKE
        public void InitSnake()
        {
            snake = new Snake
            {
                MiddleWindowWidth = middleWindowWidth,
                MiddleWindowHeight = middleWindowHeight,
                MiddleHeightWithHeader = middleHeightWithHeader,
                Level = level,
                OwnSettings = ownSettings,
                AdditionalFruitLevel = additionalFruitLevel
            };

            snake.NewGame();
        }
        #endregion INIT SNAKE

        #region BEST RESULTS
        private void ShowBestResults(List<string> results)
        {
            CleanTheScreenWithoutLogo();

            int lineLength = 56 / 2;
            int rowsHeight = 10 / 2;

            Console.SetCursorPosition(middleWindowWidth - lineLength, middleHeightWithHeader - rowsHeight - 2);
            Console.WriteLine("LP.   NICKNAME         DATE      SCORE    DIFFICULTY LEVEL");

            for (int i = 0; i < results.Count; i++)
            {
                string[] row = results[i].Split(' ');

                if (i > 9) continue;
                else if (i < 9)
                {
                    Console.SetCursorPosition(middleWindowWidth - lineLength, middleHeightWithHeader + i - rowsHeight);
                    Console.WriteLine(((i + 1).ToString() + ": ").PadRight(5) + row[1].PadRight(14) + " " + row[2].PadRight(12) + " " + row[0].PadRight(12) + " " + row[3]);
                }
                else if (i == 9)
                {
                    Console.SetCursorPosition(middleWindowWidth - lineLength, middleHeightWithHeader + i - rowsHeight);
                    Console.WriteLine(((i + 1).ToString() + ":").PadRight(5) + row[1].PadRight(14) + " " + row[2].PadRight(12) + " " + row[0].PadRight(12) + " " + row[3]);
                }
            }

            string backInfo = "PRESS ANY KEY TO RETURN";

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(middleWindowWidth - backInfo.Length / 2, middleHeightWithHeader + 10);
            Console.Write(backInfo);
            Console.ResetColor();

            Console.ReadKey();
            CleanTheScreenWithoutLogo();
            MainMenu();
        }

        public List<string> GetBestResult()
        {
            List<string> results = new List<string>();
            StreamReader streamReader = new StreamReader("Results.txt");

            string line;

            while ((line = streamReader.ReadLine()) != null) results.Add(line);

            streamReader.Close();

            results.Sort();
            results.Reverse();

            return results;
        }
        #endregion  BEST RESULTS

        #region SETTINGS

        #region BASIC SETTINGS
        private void Settings()
        {
            CleanTheScreenWithoutLogo();
            string[] availableOptions =
            {
                "            GAMEPLAY            ",
                "             MUSIC              ",
                "             BACK               "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        MainMenu();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    GameplayMenu();
                    break;

                case 1:
                    MusicMenu();
                    break;

                case 2:
                    MainMenu();
                    break;

                default: break;
            }
        }
        #endregion BASIC SETTINGS

        #region GAMEPLAY SETTINGS
        private void GameplayMenu()
        {
            CleanTheScreenWithoutLogo();
            string[] availableOptions =
            {
                "        DIFFICULTY LEVEL        ",
                "         OWN SETTINGS           ",
                "             BACK               "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        Settings();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    DifficultyMenu();
                    break;

                case 1:
                    OwnSettings();
                    break;

                case 2:
                    Settings();
                    break;

                default: break;
            }
        }

        #region DIFFICULTY SETTINGS
        public void DifficultyMenu()
        {
            string[] availableOptions =
            {
                "              EASY              ",
                "             MEDIUM             ",
                "              HARD              ",
                "              BACK              "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 3) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 3;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 3) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 3;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        GameplayMenu();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    level = 0;
                    ownSettings = false;
                    additionalFruitLevel = 5000;
                    GameplayMenu();
                    break;

                case 1:
                    level = 1;
                    ownSettings = false;
                    additionalFruitLevel = 7500;
                    GameplayMenu();
                    break;

                case 2:
                    level = 2;
                    ownSettings = false;
                    additionalFruitLevel = 10000;
                    GameplayMenu();
                    break;

                case 3:
                    GameplayMenu();
                    break;

                default: break;
            }
        }
        #endregion DIFFICULTY SETTINGS

        #region OWN SETTINGS
        private void OwnSettings()
        {
            CleanTheScreenWithoutLogo();
            string[] availableOptions =
            {
                "          SNAKE SPEED           ",
                "          BONUS FRUIT           ",
                "             BACK               "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        GameplayMenu();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    SnakeSpeed();
                    break;

                case 1:
                    BonusFruit();
                    break;

                case 2:
                    GameplayMenu();
                    break;

                default: break;
            }
        }

        #region SNAKE SPEED
        private void SnakeSpeed()
        {
            CleanTheScreenWithoutLogo();
            string[] availableOptions =
            {
                "             SLOW               ",
                "            NORMAL              ",
                "             FAST               ",
                "           VERY FAST            ",
                "             BACK               "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 4) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 4;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 4) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 4;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        OwnSettings();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    level = 0;
                    ownSettings = true;
                    OwnSettings();
                    break;

                case 1:
                    level = 1;
                    ownSettings = true;
                    OwnSettings();
                    break;

                case 2:
                    level = 2;
                    ownSettings = true;
                    OwnSettings();
                    break;

                case 3:
                    level = 3;
                    ownSettings = true;
                    OwnSettings();
                    break;

                case 4:
                    OwnSettings();
                    break;

                default: break;
            }
        }
        #endregion SNAKE SPEED

        #region BONUS FRUIT
        private void BonusFruit()
        {
            CleanTheScreenWithoutLogo();
            string[] availableOptions =
            {
                "             RARELY             ",
                "            NORMALLY            ",
                "             OFTEN              ",
                "           VERY OFTEN           ",
                "              BACK              "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 4) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 4;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 4) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 4;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        OwnSettings();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    additionalFruitLevel = 12500;
                    ownSettings = true;
                    OwnSettings();
                    break;

                case 1:
                    additionalFruitLevel = 10000;
                    ownSettings = true;
                    OwnSettings();
                    break;

                case 2:
                    additionalFruitLevel = 7500;
                    ownSettings = true;
                    OwnSettings();
                    break;

                case 3:
                    additionalFruitLevel = 5000;
                    ownSettings = true;
                    OwnSettings();
                    break;

                case 4:
                    OwnSettings();
                    break;

                default: break;
            }
        }
        #endregion BONUS FRUIT

        #endregion OWN SETTINGS

        #endregion GAMEPLAY SETTINGS

        #region MUSIC SETTINGS
        private void MusicMenu()
        {
            string[] availableOptions;
            if (musicOn)
            {
                availableOptions = new string[3]
                {
                    "        CHANGE THE MUSIC        ",
                    "       TURN OFF THE MUSIC       ",
                    "              BACK              "
                };
            }
            else
            {
                availableOptions = new string[3]
                {
                    "       CHANGE THE MUSIC         ",
                    "       TURN ON THE MUSIC        ",
                    "              BACK              "
                };
            }

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        Settings();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    ChooseSong();
                    break;

                case 1:
                    if (musicOn)
                    {
                        soundPlayer.Stop();
                        musicOn = false;
                    }
                    else
                    {
                        soundPlayer.PlayLooping();
                        musicOn = true;
                    }
                    MusicMenu();
                    break;

                case 2:
                    Settings();
                    break;

                default: break;
            }
        }

        #region CHOOSE SONG
        private void ChooseSong()
        {
            CleanTheScreenWithoutLogo();

            string[] availableOptions =
            {
                "           SNAKE SONG           ",
                "    BENNY HILL - YAKETY SAX     ",
                "              BACK              "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: true);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 2) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 2;
                        PrintMenu(currentOption, availableOptions, withHeader: true);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        MusicMenu();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    soundPlayer = new SoundPlayer("Music/Snake_Song.wav");
                    soundPlayer.PlayLooping();
                    musicOn = true;
                    MusicMenu();
                    break;

                case 1:
                    soundPlayer = new SoundPlayer("Music/Benny_Hill_Yakety_Sax.wav");
                    soundPlayer.PlayLooping();
                    musicOn = true;
                    MusicMenu();
                    break;

                case 2:
                    CleanTheScreenWithoutLogo();
                    MusicMenu();
                    break;

                default: break;
            }
        }
        #endregion CHOOSE SONG

        #endregion MUSIC SETTINGS

        #endregion SETTINGS

        #region AUTHOR
        private void ShowAuthor()
        {
            CleanTheScreenWithoutLogo();

            string authorHeader = "CREATED BY:";
            string[] authorName = File.ReadAllLines("ASCII Arts/Author/Author name2.txt");
            string[] authorSurname = File.ReadAllLines("ASCII Arts/Author/Author surname2.txt");

            Console.SetCursorPosition(middleWindowWidth - (authorHeader.Length / 2), middleHeightWithHeader - 7);
            Console.WriteLine(authorHeader);

            Console.ForegroundColor = ConsoleColor.Magenta;
            for (int i = 0; i < authorName.Length; i++)
            {
                Console.SetCursorPosition(middleWindowWidth - (authorName[0].Length / 2), middleHeightWithHeader - 3 + i);
                Console.Write(authorName[i]);
            }

            for (int i = 0; i < authorSurname.Length; i++)
            {
                Console.SetCursorPosition(middleWindowWidth - (authorSurname[0].Length / 2), middleHeightWithHeader + i);
                Console.Write(authorSurname[i]);
            }

            string backInfo = "PRESS ANY KEY TO RETURN";

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(middleWindowWidth - backInfo.Length / 2, middleHeightWithHeader + 10);
            Console.Write(backInfo);
            Console.ResetColor();

            Console.ReadKey();
            CleanTheScreen();
            ShowLogo();
            MainMenu();
        }
        #endregion AUTHOR

        #region EXIT FROM THE GAME
        private void QuitQuestion()
        {
            CleanTheScreen();

            string exitQuestion = "   EXIT NOW?  ";
            string[] availableOptions =
            {
                "      NO      ",
                "      YES     "
            };
            int numberOfavailableOptions = availableOptions.Count() / 2;

            Console.SetCursorPosition(middleWindowWidth - exitQuestion.Length / 2, middleWindowHeight - numberOfavailableOptions - 1);
            Console.WriteLine(exitQuestion);

            PrintMenu(currentOption: 0, availableOptions: availableOptions, withHeader: false);

            int currentOption = 0;
            ConsoleKey option;

            do
            {
                option = Console.ReadKey(true).Key;
                switch (option)
                {
                    //ARROWS
                    case ConsoleKey.DownArrow:
                        currentOption++;
                        if (currentOption > 1) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: false);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 1;
                        PrintMenu(currentOption, availableOptions, withHeader: false);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 1) currentOption = 0;
                        PrintMenu(currentOption, availableOptions, withHeader: false);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 1;
                        PrintMenu(currentOption, availableOptions, withHeader: false);
                        break;

                    //ESC BUTTON
                    case ConsoleKey.Escape:
                        CleanTheScreen();
                        ShowLogo();
                        MainMenu();
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    CleanTheScreen();
                    ShowLogo();
                    MainMenu();
                    break;

                case 1:
                    Farewell();
                    break;

                default: break;
            }
        }

        private static void Farewell()
        {
            soundPlayer.Stop();
            Console.Clear();

            string farewell1 = "I hope you enjoyed the game.";
            string farewell2 = "See you in the future :)";

            Console.SetCursorPosition(middleWindowWidth - (farewell1.Length / 2), middleWindowHeight - 1);
            Console.WriteLine(farewell1);
            Console.SetCursorPosition(middleWindowWidth - (farewell2.Length / 2), middleWindowHeight);
            Console.WriteLine(farewell2);

            Console.ReadKey(true);
            Environment.Exit(1);
        }
        #endregion

        #region CLEAN THE SCREEN
        public void CleanTheScreen()
        {
            for (int i = 0; i <= 29; i++)
            {
                Console.SetCursorPosition(5, 3 + i);
                Console.Write("                                                                             ");
            }
        }

        public void CleanTheScreenWithoutLogo()
        {
            for (int i = 0; i <= 22; i++)
            {
                Console.SetCursorPosition(5, 10 + i);
                Console.Write("                                                                             ");
            }
        }

        public void CleanTopAndBottom()
        {
            Console.SetCursorPosition(0, Console.WindowTop);
            Console.Write("                                                                                   ");
            Console.SetCursorPosition(0, Console.WindowTop + 1);
            Console.Write("                                                                                   ");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("                                                                                   ");
        }
        #endregion

        #endregion MENU
    }
}