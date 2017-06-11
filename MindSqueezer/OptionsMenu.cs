﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSqueezer
{
    public static class OptionsMenu
    {
        public static void Menu()
        {
            List<string> menu = Messages.MainMenu.Split('\n').ToList();
            int pointer = 2;

            var response = String.Empty;

            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                int current = 1;

                foreach (var line in menu)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;

                    if (current == pointer)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;                       
                        Writer.WriteMessageOnNewLine($">{line}<");
                    }
                    else
                    {
                        Writer.WriteMessageOnNewLine($" {line}");
                    }

                    current++;
                }

                var key = Console.ReadKey();
                switch (key.Key.ToString())
                {
                    case "UpArrow":
                        if (pointer > 2) pointer--;
                        break;
                    case "DownArrow":
                        if (pointer < menu.Count()) pointer++;
                        break;
                    case "Enter":
                        goto breakOut;
                    case "Escape":
                        return;
                }
                
            }
            breakOut:
            switch (pointer)
            {
                case 2:
                    response = "Start";
                    break;
                case 3:
                    response = "GameRules";
                    break;
                case 4:
                    response = "HighScores";
                    break;
                case 5:
                    response = "Credits";
                    break;
                case 6:
                    response = "Quit";
                    break;
            }

            
            if (response == "Start")
            {
                var totalScore = 0;
                while (true)
                {
                    Console.Clear();

                    Question quest =
                        Activator.CreateInstance(Type.GetType(Question.GetRandomQuestionType())) as Question;

                    Writer.WriteMessageOnNewLine(quest.QuestionText);

                    quest.GenerateQuestion();

                    if (!quest.IsCorrectAnswer(Console.ReadLine()))
                    {
                        Console.WriteLine("wrong");
                        break;
                    }

                    totalScore = Score.Add(totalScore);
                    Writer.WriteMessageOnNewLine($"Total score: {totalScore}");
                    Writer.WriteMessageOnNewLine(Messages.RightInput);
                    System.Threading.Thread.Sleep(1000);
                }

                Writer.WriteMessageOnNewLine($"Your score: {totalScore}");
                Writer.WriteMessageOnNewLine(Messages.EndMsg);
                if (Score.CheckIfTopScore(totalScore))
                {
                    Writer.WriteMessageOnNewLine("Congratulations! You've made it to the top 3!");
                    Writer.WriteMessage("Write your name: ");
                    Score.IsInTheTop(totalScore);
                }
                
                System.Threading.Thread.Sleep(1000);
            }
            else if (response == "GameRules")
            {
                Console.Clear();              
                Writer.WriteMessageOnNewLine(Messages.GameRules);
                ReturnButton();
                
            }
            else if (response == "HighScores")
            {
                //TODO
            }
            if (response == "Credits")
            {
                Console.Clear();              
                Writer.WriteMessageOnNewLine(Messages.Credits);
                ReturnButton();
            }
            else if (response == "Quit")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(0);
            }
        }

        private static void ReturnButton()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Writer.WriteMessageOnNewLine($">return<");
            var key = Console.ReadKey();
        }
    }
}