using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Media;
using System.IO;
using System.Reflection;

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

        /// draw the food
        /// </summary>

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
        // Bonus food Score ---------- week 7 bonus food
        static void UpdateBonusScore()
        {
            score = score + 300;
            Console.ForegroundColor = ConsoleColor.Yellow;
            string scoretxt = "Score: " + score;
            int setscoreheight = (Console.WindowTop);
            int setscorewidth = ((Console.WindowWidth / 2) - 5);
            Console.SetCursorPosition(setscorewidth, setscoreheight);
            Console.Write(scoretxt);
        }

        // Addbackground music and die music ??
        public static void GameOverMusic()
        {
            SoundPlayer gameover = new SoundPlayer();
            gameover.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\die.wav";
            gameover.Play();
        }

        public void BackMusic()
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\snake.wav";
            player.PlayLooping();
        }

        // draw the food function
        public static void DrawFood()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");
        }

        public static void DrawBonusFood()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("$");
        }

        // draw the obstacles 
        public static void DrawObs()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("=");
        }

        // draw the snake body 
        public void SnakeBody()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("*");
        }

        // the direction of the snake in an array
        public void Dir(Position[] directions)
        {
            directions[0] = new Position(0, 1);
            directions[1] = new Position(0, -1);
            directions[2] = new Position(1, 0);
            directions[3] = new Position(-1, 0);
        }

        // generate the obstacle 
        public void RandomObstacles(List<Position> obstacles)
        {
            Random rand = new Random();
            obstacles.Add(new Position(rand.Next(0, Console.WindowHeight), rand.Next(0, Console.WindowWidth)));
            obstacles.Add(new Position(rand.Next(0, Console.WindowHeight), rand.Next(0, Console.WindowWidth)));
            obstacles.Add(new Position(rand.Next(0, Console.WindowHeight), rand.Next(0, Console.WindowWidth)));
            obstacles.Add(new Position(rand.Next(0, Console.WindowHeight), rand.Next(0, Console.WindowWidth)));
            obstacles.Add(new Position(rand.Next(0, Console.WindowHeight), rand.Next(0, Console.WindowWidth)));

            foreach (Position obstacle in obstacles)
            {

                Console.SetCursorPosition(obstacle.col, obstacle.row);
                DrawObs();
            }
        }

        // indicate the user input 
        public void UserInput(ref int direction, byte right, byte left, byte down, byte up)
        {

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
        }

        public void GenFood(ref Position food, Queue<Position> snakeElements, List<Position> obstacles)
        {
            Random randomNumbersGenerator = new Random();
            do
            {
                food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth));
            }

            while (snakeElements.Contains(food) || obstacles.Contains(food));
            Console.SetCursorPosition(food.col, food.row);
            DrawFood();
        }

        public void GenBonusFood(ref Position food, Queue<Position> snakeElements, List<Position> obstacles)
        {

            Random randomNumbersGenerator = new Random();
            do
            {
                food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth));
            }

            while (snakeElements.Contains(food) || obstacles.Contains(food));
            Console.SetCursorPosition(food.col, food.row);
            DrawBonusFood();
        }

        public void NewObstacle(ref Position food, Queue<Position> snakeElements, List<Position> obstacles)
        {
            Random randomNumbersGenerator = new Random();

            Position obstacle = new Position();
            do
            {
                obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth));
            }

            while (snakeElements.Contains(obstacle) || obstacles.Contains(obstacle) || (food.row == obstacle.row && food.col == obstacle.col));
            obstacles.Add(obstacle);
            Console.SetCursorPosition(obstacle.col, obstacle.row);
            DrawObs();
        }

        // Top center score
        public void ScoreText(string scoretxt, int x)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            int setheight = Console.WindowTop;
            int setwidth = ((Console.WindowWidth - scoretxt.Length) / 2);
            Console.SetCursorPosition(setwidth, setheight);
            Console.WriteLine(scoretxt);
        }

        // Game over text at center of screen
        public void GameOverText(string text, int x)
        {
            int setheight = ((Console.WindowHeight / 2) - x);
            int setwidth = ((Console.WindowWidth - text.Length) / 2);
            Console.SetCursorPosition(setwidth, setheight);
            Console.WriteLine(text);
        }

        // only exit when enter
        public void EnterExit(string exittxt, int x)
        {
            int setheight = ((Console.WindowHeight / 2) + 2);
            int setwidth = ((Console.WindowWidth - exittxt.Length) / 2);
            Console.SetCursorPosition(setwidth, setheight);
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            System.Environment.Exit(0);
        }
        //Bonus food here ------ week 7 task JY
        // Main
        static void Main(string[] args)
        {

            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            int lastFoodTime = 0;
            int foodDissapearTime = 15000;
            int BonusFoodDisappearTime = 10000;
            int negativePoints = 0;
            Position[] directions = new Position[4];

            Program snake = new Program();
            // play background music
            snake.BackMusic();
            // indicate direction with the index of array
            snake.Dir(directions);
            // reset the obstacle posion
            List<Position> obstacles = new List<Position>();
            snake.RandomObstacles(obstacles);

            double sleepTime = 100;
            int direction = right;
            Random randomNumbersGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;
            lastFoodTime = Environment.TickCount;

            // Set Score to the top right
            string scoretxt = "Score: 0";
            snake.ScoreText(scoretxt, -5);


            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 3; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            Position food = new Position();
            snake.GenFood(ref food, snakeElements, obstacles);

            
            lastFoodTime = Environment.TickCount;

            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                snake.SnakeBody();
            }

            while (true)
            {
                negativePoints++;

                // old check user input
                snake.UserInput(ref direction, right, left, down, up);

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
                    //----------Play Game Over Music------------
                    GameOverMusic();

                    //--------------------Game over text in red color----------------
                    Console.ForegroundColor = ConsoleColor.Red;

                    //------------first line--------------
                    string gameovertxt = "Gameover!";
                    snake.GameOverText(gameovertxt, 1);

                    int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
                    //if (userPoints < 0) userPoints = 0;
                    userPoints = Math.Max(userPoints, 0);

                    //------------second line--------------
                    string pointtxt = "Your points are: " + score;
                    snake.GameOverText(pointtxt, 0);

                    //------------third line--------------
                    string exittxt = "Press Enter to Exit";
                    snake.GameOverText(exittxt, -1);

                    ////------------exit line--------------
                    string continuetxt = "Press any key to continue . . .";
                    snake.EnterExit(continuetxt, -2);
                }

                Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                snake.SnakeBody();

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
                    DrawFood();
                    sleepTime--;
                    UpdateScore();
                    Position obstacle = new Position();

                    // If reach score == winScore(1500), WIN - Brandon
                    if (score == winScore)
                    {
                        // won text in green color
                        Console.ForegroundColor = ConsoleColor.Green;

                        //------------first line--------------
                        string wintxt = "You Won!";
                        snake.GameOverText(wintxt, 1);

                        int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
                        //if (userPoints < 0) userPoints = 0;
                        userPoints = Math.Max(userPoints, 0);

                        //------------second line--------------
                        string pointtxt = "Your points are: " + score;
                        snake.GameOverText(pointtxt, 0);

                        //------------third line--------------
                        string exittxt = "Press Enter to Exit";
                        snake.GameOverText(exittxt, -1);

                        ////------------exit line--------------
                        string continuetxt = "Press any key to continue . . .";
                        snake.EnterExit(continuetxt, 0);
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
                    DrawObs();
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
                    snake.GenFood(ref food, snakeElements, obstacles);
                    lastFoodTime = Environment.TickCount;
                }
             

                Console.SetCursorPosition(food.col, food.row);
                DrawFood();

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
