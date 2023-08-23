using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_List
{
    internal class Display
    {
        public enum Direction
        {
            Vertical,
            Horizontal
        }

        public static int Selector(string title, string[] options, int maxIndex, Direction direction, bool menuMode)
        {
            int _index = 0;
            ConsoleKey _key = ConsoleKey.Add;
            while (_key != ConsoleKey.Enter)
            {
                Console.Clear();
                Console.WriteLine(title);
                Console.WriteLine();

                if (direction == Direction.Vertical)
                {
                    ListVertical(options, _index);
                }
                else
                {
                    ListHorizontal(options, _index);
                }

                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                if (direction == Direction.Vertical)
                {
                    Console.WriteLine("Use arrow keys UP or DOWN to choose and press 'ENTER' to select.");
                }
                else if (menuMode)
                {
                    Console.WriteLine("Use arrow keys UP or DOWN to choose and press 'ENTER' to select.");
                    Console.WriteLine("Press 'C' to create a new task.");
                }
                else
                {
                    Console.WriteLine("Use arrow keys LEFT or RIGHT to choose and press 'ENTER' to select.");
                }
                Console.ResetColor();

                _key = Console.ReadKey().Key;
                if (menuMode && _key == ConsoleKey.C)
                {
                    return -1;
                }
                if (direction == Direction.Vertical)
                {
                    switch (_key)
                    {
                        case ConsoleKey.DownArrow:
                            if (_index != maxIndex - 1)
                                _index++;
                            else
                                _index = 0;
                            break;
                        case ConsoleKey.UpArrow:
                            if (_index != 0)
                                _index--;
                            else
                                _index = maxIndex - 1;
                            break;
                    }
                }
                else
                {
                    switch (_key)
                    {
                        case ConsoleKey.RightArrow:
                            if (_index != maxIndex - 1)
                                _index++;
                            else
                                _index = 0;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (_index != 0)
                                _index--;
                            else
                                _index = maxIndex - 1;
                            break;
                    }
                }
            }
            return _index;
        }

        private static void ListVertical(string[] options, int index)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (i == index)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"{options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"{options[i]}");
                }
            }
        }

        private static void ListHorizontal(string[] options, int index)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (i == index)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{options[i]}");
                    Console.ResetColor();
                    Console.Write($" ");
                }
                else
                {
                    Console.Write($"{options[i]} ");
                }
            }
        }

    }
}
