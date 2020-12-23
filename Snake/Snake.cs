using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    class Snake
    {
        public Snake() { }

        #region GAME

        #region POINT
        private class Point
        {
            public int x;
            public int y;
            private bool red = true;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public void Write()
            {
                Console.SetCursorPosition(x, y);
                if (red)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("O");
                    red = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("o");
                    red = true;
                }
            }
        }
        #endregion POINT

        #region VARIABLES AND OBJECTS
        readonly Menu menu = new Menu();

        //THREADS
        Thread fruitGenerator;
        Thread fruitAnimator;

        //MUTEX
        public object writer;
        public object listManipulator;

        //REQUEST TO END THE THREAD
        private CancellationTokenSource token;

        //FRUIT LIST
        List<Point> fruitList;

        //GETTERS AND SETTERS
        public int MiddleWindowWidth { get; set; }
        public int MiddleWindowHeight { get; set; }
        public int MiddleHeightWithHeader { get; set; }
        public int Level { get; set; }
        public bool OwnSettings { get; set; }
        public int AdditionalFruitLevel { get; set; }

        //GAME
        private bool gameOver = false;
        private int score = 0;
        private int delay = 150;
        private int scoreFactor = 0;
        private string nickname;
        private readonly Random rand = new Random();
        private int itemX;
        private int itemY;
        private int[] snakeX;
        private int[] snakeY;
        private int snakeLength;
        private string bestResult;
        private bool setNewRecord = false;
        #endregion VARIABLES AND OBJECTS

        #region NEW GAME
        public void NewGame()
        {
            fruitList = new List<Point>();
            token = new CancellationTokenSource();
            fruitGenerator = new Thread(() => PutExtraFruit(ref token));
            fruitAnimator = new Thread(() => FruitAnimation(ref token));
            writer = new object();
            listManipulator = new object();

            menu.CleanTheScreenWithoutLogo();
            gameOver = false;
            score = 0;

            switch (Level)
            {
                case 0:
                    scoreFactor = 5;
                    delay = 125;
                    break;
                case 1:
                    scoreFactor = 10;
                    delay = 100;
                    break;
                case 2:
                    scoreFactor = 20;
                    delay = 75;
                    break;
                case 3:
                    scoreFactor = 30;
                    delay = 50;
                    break;

                default:
                    break;
            }

            CheckNickname();
            CountdownToGameStart();
            ShowPlayingBoard();
            InitSnake();
            PlaySnake();
        }
        #endregion NEW GAME

        #region CHECK NICKNAME
        private void CheckNickname()
        {
            if (nickname == null)
            {
                string enterTheNickname = "ENTER A NICKNAME:";
                string enterTheCorrectNickname = "ENTER THE CORRECT NICKNAME:";
                string incorrectNicknameLength = "THE NICKNAME MUST BE BETWEEN 3 AND 12 CHARACTERS";

                Console.SetCursorPosition(MiddleWindowWidth - (enterTheNickname.Length / 2), MiddleWindowHeight - 1);
                Console.WriteLine(enterTheNickname);
                Console.SetCursorPosition(MiddleWindowWidth - 6, MiddleWindowHeight);
                Console.CursorVisible = true;
                nickname = Console.ReadLine();

                while (nickname.Length > 12 || nickname.Length < 3)
                {
                    if (nickname.Length > 47)
                    {
                        Console.Clear();
                        menu.ShowFrame();
                        menu.ShowLogo();
                    }
                    else
                    {
                        Console.SetCursorPosition(5, MiddleWindowHeight);
                        Console.Write("                                                                             ");
                    }

                    Console.SetCursorPosition(MiddleWindowWidth - (incorrectNicknameLength.Length / 2), MiddleWindowHeight - 2);
                    Console.WriteLine(incorrectNicknameLength);
                    Console.SetCursorPosition(MiddleWindowWidth - (enterTheCorrectNickname.Length / 2), MiddleWindowHeight - 1);
                    Console.WriteLine(enterTheCorrectNickname);
                    Console.SetCursorPosition(MiddleWindowWidth - 7, MiddleWindowHeight);
                    nickname = Console.ReadLine();
                }

                Console.CursorVisible = false;

                menu.CleanTheScreen();
            }
        }
        #endregion CHECK NICKNAME

        #region COUNTDOWN TO GAME START
        private void CountdownToGameStart()
        {
            menu.CleanTheScreen();
            string[] _3 = File.ReadAllLines("ASCII Arts/Coundown to game start/3.txt");
            string[] _2 = File.ReadAllLines("ASCII Arts/Coundown to game start/2.txt");
            string[] _1 = File.ReadAllLines("ASCII Arts/Coundown to game start/1.txt");
            string[] start = File.ReadAllLines("ASCII Arts/Coundown to game start/Start.txt");

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < _3.Length; i++)
            {
                Console.SetCursorPosition(MiddleWindowWidth - (_3[0].Length / 2), MiddleWindowHeight - 4 + i);
                Console.Write(_3[i]);
            }

            Thread.Sleep(1000);
            menu.CleanTheScreenWithoutLogo();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < _2.Length; i++)
            {
                Console.SetCursorPosition(MiddleWindowWidth - (_2[0].Length / 2), MiddleWindowHeight - 4 + i);
                Console.Write(_2[i]);
            }

            Thread.Sleep(1000);
            menu.CleanTheScreenWithoutLogo();

            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < _1.Length; i++)
            {
                Console.SetCursorPosition(MiddleWindowWidth - (_1[0].Length / 2), MiddleWindowHeight - 4 + i);
                Console.Write(_1[i]);
            }

            Thread.Sleep(1000);
            menu.CleanTheScreenWithoutLogo();

            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < start.Length; i++)
            {
                Console.SetCursorPosition(MiddleWindowWidth - (start[0].Length / 2), MiddleWindowHeight - 4 + i);
                Console.Write(start[i]);
            }

            Console.ResetColor();
            Thread.Sleep(1000);
            menu.CleanTheScreen();
        }
        #endregion COUNTDOWN TO GAME START

        #region SHOW PLAYING BOARD
        private void ShowPlayingBoard()
        {
            string player = "PLAYER:";
            string difficultyLevel = "DIFFICULTY LEVEL: ";
            string points = "POINTS: ";

            Console.SetCursorPosition(MiddleWindowWidth - (player.Length / 2), Console.WindowTop);
            Console.WriteLine(player);
            Console.SetCursorPosition(MiddleWindowWidth - (nickname.Length / 2), Console.WindowTop + 1);
            Console.WriteLine(nickname);

            Console.SetCursorPosition(4, Console.WindowHeight - 1);
            Console.Write(difficultyLevel);
            Console.SetCursorPosition(difficultyLevel.Length + 4, Console.WindowHeight - 1);

            if (OwnSettings) Console.Write("OWN SETTINGS");
            else
            {
                switch (Level)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("EASY");
                        break;

                    case 1:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("MEDIUM");
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("HARD");
                        break;
                }
            }

            Console.ResetColor();
            Console.SetCursorPosition(Console.WindowWidth - (points.Length / 2) - 10, Console.WindowHeight - 1);
            Console.Write(points);
            Console.SetCursorPosition(Console.WindowWidth - (points.Length / 2) - 2, Console.WindowHeight - 1);
            Console.Write(score);
        }
        #endregion SHOW PLAYING BOARD

        #region INIT SNAKE
        private void InitSnake()
        {
            snakeX = new int[90];
            snakeY = new int[90];
            snakeX[0] = MiddleWindowWidth;
            snakeY[0] = MiddleWindowHeight;
            snakeX[1] = MiddleWindowWidth - 1;
            snakeY[1] = MiddleWindowHeight;
            snakeLength = 3;

            PrintSnake();
        }
        #endregion INIT SNAKE

        #region PRINT SNAKE
        private void PrintSnake()
        {
            //HEAD
            Console.SetCursorPosition(snakeX[0], snakeY[0]);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write(" ");

            //BODY
            for (int i = 1; i < snakeLength; i++)
            {
                Console.SetCursorPosition(snakeX[i], snakeY[i]);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write(" ");
            }

            //ERASE THE LAST PART OF THE BODY
            Console.SetCursorPosition(snakeX[snakeLength - 1], snakeY[snakeLength - 1]);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" ");

            //UPDATE POSITIONS
            for (int i = snakeLength - 1; i > 0; i--)
            {
                snakeX[i] = snakeX[i - 1];
                snakeY[i] = snakeY[i - 1];
            }

            Console.ResetColor();
        }
        #endregion PRINT SNAKE

        #region PLAY THE GAME
        private void PlaySnake()
        {
            ConsoleKey command = ConsoleKey.RightArrow;
            PutFruit();
            fruitGenerator.Start();
            fruitAnimator.Start();

            do
            {
                if (Console.KeyAvailable) command = Console.ReadKey(true).Key;

                switch (command)
                {
                    //ARROWS
                    case ConsoleKey.UpArrow:
                        snakeY[0]--;
                        break;

                    case ConsoleKey.DownArrow:
                        snakeY[0]++;
                        break;

                    case ConsoleKey.LeftArrow:
                        snakeX[0]--;
                        break;

                    case ConsoleKey.RightArrow:
                        snakeX[0]++;
                        break;

                    //W S A D BUTTONS
                    case ConsoleKey.W:
                        snakeY[0]--;
                        break;

                    case ConsoleKey.S:
                        snakeY[0]++;
                        break;

                    case ConsoleKey.A:
                        snakeX[0]--;
                        break;

                    case ConsoleKey.D:
                        snakeX[0]++;
                        break;

                    default: continue;
                }

                if (WallHitted() || TailHitted()) gameOver = true;

                lock (writer) PrintSnake();

                if (FruitEated())
                {
                    //SHOW NEW ITEM
                    PutFruit();

                    //UPDATE POINTS
                    score += scoreFactor;

                    //HOSE LENGTHENING
                    snakeLength++;

                    //HOSE ACCELERATION
                    if (!OwnSettings) if (delay >= 35) delay -= 3;

                    //BLOCKS THE THREAD FOR THE TIME OF WRITING AND THEN AUTOMATICALLY UNLOCKS IT
                    lock (writer)
                    {
                        Console.ResetColor();
                        Console.SetCursorPosition(Console.WindowWidth - 4 - 2, Console.WindowHeight - 1);
                        Console.Write(score);
                    }
                }

                if (ExtraFruitEated())
                {
                    score += 50;
                    snakeLength += 2;

                    if (!OwnSettings) if (delay >= 35) delay -= 6;

                    lock (writer)
                    {
                        Console.ResetColor();
                        Console.SetCursorPosition(Console.WindowWidth - 4 - 2, Console.WindowHeight - 1);
                        Console.Write(score);
                    }
                }

                Thread.Sleep(delay);

            } while (!gameOver);

            token.Cancel();

            GameOver();
        }

        #region PUT FRUIT
        private void PutFruit()
        {
            itemX = rand.Next(5, 81);
            itemY = rand.Next(3, 32);

            //THE FRUIT IS NOT ON THE SNAKE?
            if (snakeX.Contains(itemX) && snakeY.Contains(itemY)) PutFruit();

            lock (writer)
            {
                Console.SetCursorPosition(itemX, itemY);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("■");
                Console.ResetColor();
            }
        }

        //DISPLAYING EXTRA FRUIT
        public void PutExtraFruit(ref CancellationTokenSource token)
        {
            while (true)
            {
                Thread.Sleep(AdditionalFruitLevel);

                int X = rand.Next(5, 81);
                int Y = rand.Next(3, 32);

                if (snakeX.Contains(X) && snakeY.Contains(Y)) PutExtraFruit(ref token);

                //CALLING FOR THE END OF THE THREAD
                if (token.IsCancellationRequested) break;

                Point p = new Point(X, Y);

                lock (listManipulator) fruitList.Add(p);

                lock (writer)
                {
                    Console.SetCursorPosition(X, Y);
                    p.Write();
                    Console.ResetColor();
                }
            }
        }
        #endregion PUT FRUIT

        #region EXTRA FRUIT ANIMATION
        public void FruitAnimation(ref CancellationTokenSource token)
        {
            int i;
            while (true)
            {
                lock (listManipulator)
                {
                    if (fruitList.Count != 0)
                    {
                        i = rand.Next(0, fruitList.Count);
                        lock (writer)
                        {
                            if (token.IsCancellationRequested) break;
                            fruitList[i].Write();
                        }
                    }
                }

                //FLASHING EXTRA FRUIT
                Thread.Sleep(300);
            }
        }
        #endregion EXTRA FRUIT ANIMATION

        #region WALL HITTED
        private bool WallHitted()
        {
            int x = snakeX[0];
            int y = snakeY[0];

            if (x == 4 || x == 82 || y == 2 || y == 33) return true;
            else return false;
        }
        #endregion WALL HITTED

        #region TAIL HITTED
        private bool TailHitted()
        {
            int x = snakeX[0];
            int y = snakeY[0];

            for (int i = 1; i < snakeLength; i++) if (x == snakeX[i] && y == snakeY[i]) return true;
            return false;
        }
        #endregion TAIL HITTED

        #region FRUIT EATED
        private bool FruitEated()
        {
            if (snakeX[0] == itemX && snakeY[0] == itemY) return true;
            else return false;
        }

        private bool ExtraFruitEated()
        {
            Point p = fruitList.FirstOrDefault(x => x.x == snakeX[0] && x.y == snakeY[0]);

            if (p != null)
            {
                lock (listManipulator) fruitList.Remove(p);
                return true;
            }
            else return false;
        }
        #endregion FRUIT EATED

        #endregion PLAY THE GAME

        #region GAME OVER
        private void GameOver()
        {
            fruitAnimator.Abort();
            fruitGenerator.Abort();
            lock (writer) Console.Clear();

            menu.ShowFrame();

            string[] game = File.ReadAllLines("ASCII Arts/Game over/Game.txt");
            string[] over = File.ReadAllLines("ASCII Arts/Game over/Over.txt");

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < game.Length; i++)
            {
                Console.SetCursorPosition(MiddleWindowWidth - (game[0].Length / 2), MiddleWindowHeight - 14 + i);
                Console.Write(game[i]);
            }

            for (int i = 0; i < over.Length; i++)
            {
                Console.SetCursorPosition(MiddleWindowWidth - (over[0].Length / 2), MiddleWindowHeight - 7 + i);
                Console.Write(over[i]);
            }

            Console.ResetColor();

            int best_result = 0;
            if (menu.GetBestResult().Count != 0)
            {
                bestResult = menu.GetBestResult()[0];
                best_result = Convert.ToInt32(bestResult.Split(' ')[0]);
            }

            string newRecord = "A NEW RECORD HAS BEEN SET!";
            string pointsScored = "POINTS SCORED: ";

            if (score > best_result)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.SetCursorPosition(MiddleWindowWidth - (newRecord.Length / 2), MiddleWindowHeight + 1);
                Console.Write(newRecord);
                Console.ResetColor();

                Console.SetCursorPosition(MiddleWindowWidth - ((pointsScored.Length + 4) / 2), MiddleWindowHeight + 2);
                Console.Write(pointsScored);
                Console.SetCursorPosition(MiddleWindowWidth + 6, MiddleWindowHeight + 2);
                Console.Write(score);
                setNewRecord = true;
            }
            else
            {
                Console.SetCursorPosition(MiddleWindowWidth - ((pointsScored.Length + 4) / 2), MiddleWindowHeight + 1);
                Console.Write(pointsScored);
                Console.SetCursorPosition(MiddleWindowWidth + 6, MiddleWindowHeight + 1);
                Console.Write(score);
                setNewRecord = false;
            }

            if (score > 0) SaveResult();

            string[] availableOptions =
            {
                "            NEW GAME            ",
                "              EXIT              "
            };

            PrintMenu(currentOption: 0, availableOptions: availableOptions);

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
                        PrintMenu(currentOption, availableOptions);
                        break;

                    case ConsoleKey.UpArrow:
                        currentOption--;
                        if (currentOption < 0) currentOption = 1;
                        PrintMenu(currentOption, availableOptions);
                        break;

                    //S AND W BUTTONS
                    case ConsoleKey.S:
                        currentOption++;
                        if (currentOption > 1) currentOption = 0;
                        PrintMenu(currentOption, availableOptions);
                        break;

                    case ConsoleKey.W:
                        currentOption--;
                        if (currentOption < 0) currentOption = 1;
                        PrintMenu(currentOption, availableOptions);
                        break;

                    default: break;
                }
            } while (option != ConsoleKey.Enter);

            switch (currentOption)
            {
                case 0:
                    NewGame();
                    break;

                case 1:
                    menu.CleanTheScreen();
                    menu.ShowLogo();
                    menu.MainMenu();
                    break;

                default: break;
            }
        }

        #region GAME OVER MENU
        private void PrintMenu(int currentOption, string[] availableOptions)
        {
            if (setNewRecord)
            {
                for (int i = 0; i < availableOptions.Length; i++)
                {
                    if (i == currentOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(MiddleWindowWidth - (availableOptions[i].Length) / 2, MiddleWindowHeight + 5 + i);
                        Console.WriteLine(availableOptions[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.SetCursorPosition(MiddleWindowWidth - (availableOptions[i].Length) / 2, MiddleWindowHeight + 5 + i);
                        Console.WriteLine(availableOptions[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < availableOptions.Length; i++)
                {
                    if (i == currentOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(MiddleWindowWidth - (availableOptions[i].Length) / 2, MiddleWindowHeight + 4 + i);
                        Console.WriteLine(availableOptions[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.SetCursorPosition(MiddleWindowWidth - (availableOptions[i].Length) / 2, MiddleWindowHeight + 4 + i);
                        Console.WriteLine(availableOptions[i]);
                    }
                }
            }
        }
        #endregion GAME OVER MENU

        #region SAVE RESULT
        private void SaveResult()
        {
            StreamWriter SW = File.AppendText("Results.txt");
            if (OwnSettings) SW.WriteLine(score.ToString().PadLeft(4, '0') + " " + nickname + " " + DateTime.Today.ToString("dd/MM/yyyy") + " " + "OWN");
            else
                switch (Level)
                {
                    case 0:
                        SW.WriteLine(score.ToString().PadLeft(4, '0') + " " + nickname + " " + DateTime.Today.ToString("dd/MM/yyyy") + " " + "EASY");
                        break;
                    case 1:
                        SW.WriteLine(score.ToString().PadLeft(4, '0') + " " + nickname + " " + DateTime.Today.ToString("dd/MM/yyyy") + " " + "MEDIUM");
                        break;
                    case 2:
                        SW.WriteLine(score.ToString().PadLeft(4, '0') + " " + nickname + " " + DateTime.Today.ToString("dd/MM/yyyy") + " " + "HARD");
                        break;
                    default:
                        break;
                }

            SW.Close();
        }
        #endregion SAVE RESULT

        #endregion GAME OVER

        #endregion GAME
    }
}