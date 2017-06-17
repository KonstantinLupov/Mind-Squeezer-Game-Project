﻿using System;
using System.Collections.Generic;
using System.Linq;
using MindSqueezer.Utilities;

namespace MindSqueezer
{
    public static class OptionsMenu
    {

        private static List<string> menu = Messages.MainMenu.Split('\n').ToList();

        public static void Menu()
        {
            int pointer = 2;
            var response = String.Empty;

            while (true)
            {
                DefaultColors();
                Console.Clear();
                int current = 1;

                foreach (var line in menu)
                {
                    DefaultColors();

                    if (current == 1)
                    {
                        ColorChanger.ChangeColor(ConsoleColor.Yellow, ConsoleColor.Black);
                    }
                    if (current == pointer)
                    {
                        if (current == 6)
                        {
                            ColorChanger.ChangeColor(ConsoleColor.Red, ConsoleColor.Black);
                            Writer.WriteMessageOnNewLine($"{line} <");
                        }
                        else
                        {
                            CurrentChoiceColors();
                            Writer.WriteMessageOnNewLine($"{line} <");
                        }
                    }
                    else
                    {
                        Writer.WriteMessageOnNewLine($"{line}");
                    }

                    current++;
                }

                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        pointer = 6;
                        break;
                    case ConsoleKey.UpArrow:
                        if (pointer > 2) pointer--;
                        else pointer = 6;
                        break;
                    case ConsoleKey.DownArrow:
                        if (pointer < menu.Count()) pointer++;
                        else pointer = 2;
                        break;
                    case ConsoleKey.Enter:
                        goto breakOut;
                }
            }

            breakOut:
            switch (pointer)
            {
                case 2:
                    Start();
                    break;
                case 3:
                    GameRules();
                    break;
                case 4:
                    HighScores();
                    break;
                case 5:
                    Credits();
                    break;
                case 6:
                    Quit();
                    break;
            }
        }

        private static void CurrentChoiceColors()
        {
            ColorChanger.ChangeColor(ConsoleColor.Green, ConsoleColor.Black);
        }

        private static void DefaultColors()
        {
            ColorChanger.ChangeColor(ConsoleColor.White, ConsoleColor.Black);
        }

        private static void ReturnButton()
        {
            CurrentChoiceColors();
            Writer.WriteMessageOnNewLine("\n Return <");
            ConsoleKeyInfo key = Console.ReadKey(true);

            while (key.Key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

        private static void Start()
        {
            var totalScore = 0;
            int seconds = 25000;

            while (true)
            {
                Console.Clear();

                Question quest =
                    Activator.CreateInstance(Type.GetType(Question.GetRandomQuestionType())) as Question;

                string name = string.Empty; 
                
                Writer.WriteMessageOnNewLine();
                Writer.WriteMessageOnNewLine(quest.QuestionText);

                quest.GenerateQuestion();
                Writer.WriteMessageOnNewLine();

                bool success = Timer.TryReadLine(out name, seconds);

                if (!success)
                {
                    ColorChanger.ChangeColor(ConsoleColor.Red, ConsoleColor.Black);
                    Writer.WriteMessageOnNewLine();
                    Writer.WriteMessageOnNewLine(Messages.TimeUp);
                    ColorChanger.DefaultColor();

                    break;
                }

                if (!quest.IsCorrectAnswer(name))
                {
                    ColorChanger.ChangeColor(ConsoleColor.Red, ConsoleColor.Black);
                    Writer.WriteMessageOnNewLine(Messages.EndMsg);
                    ColorChanger.DefaultColor();

                    break;
                }

                Writer.WriteMessageOnNewLine();
                totalScore = Score.Add(totalScore);
                Writer.WriteMessageOnNewLine($"Total score: {totalScore}");
                ColorChanger.ChangeColor(ConsoleColor.Green, ConsoleColor.Black);
                Writer.WriteMessageOnNewLine(Messages.RightInput);
                ColorChanger.DefaultColor();
                System.Threading.Thread.Sleep(1000);

                if (totalScore % 3 == 0 && seconds > 5000)
                {
                    seconds -= 2000;
                }
            }

            ColorChanger.ChangeColor(ConsoleColor.Yellow, ConsoleColor.Black);
            Writer.WriteMessageOnNewLine($"Your score: {totalScore}");
            ColorChanger.DefaultColor();
            System.Threading.Thread.Sleep(500);

            if (Score.CheckIfTopScore(totalScore))
            {
                Writer.WriteMessageOnNewLine();
                ColorChanger.ChangeColor(ConsoleColor.Green, ConsoleColor.Black);
                Writer.WriteMessageOnNewLine(Messages.EnterTopThree);
                ColorChanger.DefaultColor();
                Writer.WriteMessage(Messages.WriteYourName);

                Score.IsInTheTop(totalScore);
            }
            
            ReturnButton();
        }

        private static void GameRules()
        {
            Console.Clear();

            Writer.WriteMessageOnNewLine(Messages.GameRules);

            ReturnButton();
        }

        private static void HighScores()
        {
            Console.Clear();
                    
            Writer.WriteMessageOnNewLine(Messages.HighScores);          
            Writer.WriteMessageOnNewLine(Messages.LongLine);
            Writer.WriteMessageOnNewLine(Messages.HighScoresPanel);
            Writer.WriteMessageOnNewLine(Messages.LongLine);
            Score.CheckHighScores();
            Writer.WriteMessageOnNewLine(Messages.LongLine);
            
            ReturnButton();
        }

        private static void Credits()
        {
            Console.Clear();
            Writer.WriteMessageOnNewLine(Messages.Credits);
            ReturnButton();

        }

        private static void Quit()
        {
            ColorChanger.ChangeColor(ConsoleColor.White, ConsoleColor.Black);
            Environment.Exit(0);
        }
    }
}
