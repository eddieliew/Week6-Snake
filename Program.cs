using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Media;
using System.IO;

namespace Snake
{
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
    class Program
    {
        // When snake eat food score will updated - Brandon
        static int score = 0;
        // Score reach to win the game - Brandon
        static int winScore = 10050;

        // Updating the score - Brandon
        static void UpdateScore()
        {
            score = score + 150;
            Console.ForegroundColor = ConsoleColor.Yellow;
            string scoretxt = "Score: " + score;
            int setscoreheight = (Console.WindowTop);
            int setscorewidth = ((Console.WindowWidth / 2) - 5);
            Console.SetCursorPosition(setscorewidth, setscoreheight);
            Console.Write(scoretxt);
        }

        // ------------Add background music and die music Lee Jun Yee-----------------------------------
        public static void overMusic()
        {
            SoundPlayer gameover = new SoundPlayer();
            gameover.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\die.wav";
            gameover.PlayLooping();
        }

        public static void backMusic()
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\snake.wav";
            player.Play();
        }
        //----------------- END---------------------------------------

        // Main
        static void Main(string[] args)
        {
            backMusic();
            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            int lastFoodTime = 0;
            int foodDissapearTime = 15000;
            int negativePoints = 0;

            Position[] directions = new Position[] {
                new Position(0, 1), // right
                new Position(0, -1), // left
                new Position(1, 0), // down
                new Position(-1, 0), // up
            };

            double sleepTime = 100;
            int direction = right;
            Random randomNumbersGenerator = new Random();
            Random rand = new Random();
            Console.BufferHeight = Console.WindowHeight;
            lastFoodTime = Environment.TickCount;
            
            // Set Score to the top right
            Console.ForegroundColor = ConsoleColor.Yellow;
            string scoretxt = "Score: 0";
            int setscoreheight = (Console.WindowTop);
            int setscorewidth = ((Console.WindowWidth / 2) - 5);
            Console.SetCursorPosition(setscorewidth, setscoreheight);
            Console.Write(scoretxt);

            List<Position> obstacles = new List<Position>() {
                new Position(rand.Next(0, Console.WindowHeight),randomNumbersGenerator.Next(0,Console.WindowWidth)),
                new Position(rand.Next(0, Console.WindowHeight),randomNumbersGenerator.Next(0,Console.WindowWidth)),
                new Position(rand.Next(0, Console.WindowHeight),randomNumbersGenerator.Next(0,Console.WindowWidth)),
                new Position(rand.Next(0, Console.WindowHeight),randomNumbersGenerator.Next(0,Console.WindowWidth)),
                new Position(rand.Next(0, Console.WindowHeight),randomNumbersGenerator.Next(0,Console.WindowWidth)),
            };

            foreach (Position obstacle in obstacles) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstacle.col, obstacle.row);
                Console.Write("=");
            }

            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 3; i++) {
                snakeElements.Enqueue(new Position(0, i));
                
            }

            Position food;
            do {
                food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth));
            }
            while (snakeElements.Contains(food) || obstacles.Contains(food));
            Console.SetCursorPosition(food.col, food.row);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@");

            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");
            }

            while (true)
            {
                negativePoints++;
              
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right) direction = left;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != left) direction = right;
                    }
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != down) direction = up;
                    }
                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != up) direction = down;
                    }
                }

                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[direction];

                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
                    snakeHead.col + nextDirection.col);

                if (snakeNewHead.col < 0) snakeNewHead.col = Console.WindowWidth - 1;
                if (snakeNewHead.row < 0) snakeNewHead.row = Console.WindowHeight - 1;
                if (snakeNewHead.row >= Console.WindowHeight) snakeNewHead.row = 0;
                if (snakeNewHead.col >= Console.WindowWidth) snakeNewHead.col = 0;

                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    //--------------------Game over text----------------
                    Console.ForegroundColor = ConsoleColor.Red;
                    //------------first line--------------
                    string gameovertxt = "Gameover!";
                    int setheight1 = (Console.WindowHeight / 2) - 1;
                    int setwidth1 = ((Console.WindowWidth - gameovertxt.Length) / 2);
                    Console.SetCursorPosition(setwidth1, setheight1);
                    Console.WriteLine(gameovertxt);
                    overMusic();

                    int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
                    //if (userPoints < 0) userPoints = 0;
                    userPoints = Math.Max(userPoints, 0);

                    //------------second line--------------
                    string pointtxt = "Your points are: {0}";
                    int setheight2 = (Console.WindowHeight / 2);
                    int setwidth2 = ((Console.WindowWidth - pointtxt.Length) / 2);
                    Console.SetCursorPosition(setwidth2, setheight2);
                    Console.WriteLine(pointtxt, score);


                    //------------third line--------------
                    string exittxt = "Press Enter to Exit";
                    int setheight3 = (Console.WindowHeight / 2) + 1;
                    int setwidth3 = ((Console.WindowWidth - exittxt.Length) / 2);
                    Console.SetCursorPosition(setwidth3, setheight3);
                    Console.WriteLine(exittxt);

                    ////------------exit line--------------
                    string continuetxt = "Press any key to continue . . .";
                    int setheight4 = (Console.WindowHeight / 2) + 2;
                    int setwidth4 = ((Console.WindowWidth - continuetxt.Length) / 2);
                    Console.SetCursorPosition(setwidth4, setheight4);
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                    System.Environment.Exit(0);
                }

                Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");

                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                Console.ForegroundColor = ConsoleColor.Gray;
                if (direction == right) Console.Write(">");
                if (direction == left) Console.Write("<");
                if (direction == up) Console.Write("^");
                if (direction == down) Console.Write("v");


                if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
                {
                    // feeding the snake
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("@");
                    sleepTime--;
                    UpdateScore();
                    Position obstacle = new Position();

                    // If reach score == winScore(1500), WIN - Brandon
                    if (score == winScore)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        //------------first line--------------
                        string wintxt = "You Won!";
                        int setheight1 = (Console.WindowHeight / 2) - 1;
                        int setwidth1 = ((Console.WindowWidth - wintxt.Length) / 2);
                        Console.SetCursorPosition(setwidth1, setheight1);
                        Console.WriteLine(wintxt);
                        int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
                        //if (userPoints < 0) userPoints = 0;
                        userPoints = Math.Max(userPoints, 0);
                        //------------second line--------------
                        string pointtxt = "Your points are: {0}";
                        int setheight2 = (Console.WindowHeight / 2);
                        int setwidth2 = ((Console.WindowWidth - pointtxt.Length) / 2);
                        Console.SetCursorPosition(setwidth2, setheight2);
                        Console.WriteLine(pointtxt, score);
                        //------------third line--------------
                        string exittxt = "Press Enter to Exit";
                        int setheight3 = (Console.WindowHeight / 2) + 1;
                        int setwidth3 = ((Console.WindowWidth - exittxt.Length) / 2);
                        Console.SetCursorPosition(setwidth3, setheight3);
                        Console.WriteLine(exittxt);
                        ////------------exit line--------------
                        string continuetxt = "Press any key to continue . . .";
                        int setheight4 = (Console.WindowHeight / 2) + 2;
                        int setwidth4 = ((Console.WindowWidth - continuetxt.Length) / 2);
                        Console.SetCursorPosition(setwidth4, setheight4);
                        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                        System.Environment.Exit(0);
                    }

                    do
                    {
                        obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(obstacle) ||
                        obstacles.Contains(obstacle) ||
                        (food.row != obstacle.row && food.col != obstacle.row));
                    obstacles.Add(obstacle);
                    Console.SetCursorPosition(obstacle.col, obstacle.row);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("=");
                }
                else
                {
                    // moving...
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }

                if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
                {
                    negativePoints = negativePoints + 50;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                }

                Console.SetCursorPosition(food.col, food.row);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("@");

                sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);

                // Write Player Detail into Plain Text, Located at (Esurient-Snake/bin/debug) - Brandon
                int snakePlayer = 0;
                snakePlayer++;
                string playerText = "Player " + snakePlayer.ToString();
                string scoreText = "Your points: " + score.ToString();
                System.IO.File.WriteAllText("Score.txt", playerText + "\n" + scoreText);
            }
        }
    }
}
