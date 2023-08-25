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

                int[] _rows = GetLongestStrings(options, menuMode);
                string[] _topStrings = new string[] {
                    "ID",
                    "Application",
                    "Group",
                    "Status",
                    "Priority",
                    "Title",
                    "Description",
                    "Version",
                    "Date"
                };
                int _rowIndex = 0;
                if (_rows[_rowIndex] > _topStrings[_rowIndex].Length)
                    _topStrings[_rowIndex] = AddWhitespaces(_topStrings[_rowIndex], _rows[_rowIndex]);
                else
                {
                    _rows[_rowIndex] = _topStrings[_rowIndex].Length;
                }
                foreach (var _text in _topStrings)
                {
                    Console.Write(_text + " |");
                }
                Console.WriteLine();
                if (direction == Direction.Vertical)
                {
                    ListVertical(options, _index, maxIndex);
                }
                else
                {
                    ListHorizontal(options, _index, maxIndex);
                    Console.WriteLine();
                }

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
                _index = Controls(_key, _index, maxIndex, direction, menuMode);
            }
            Console.Clear();
            return _index;
        }

        private static int Controls(ConsoleKey key, int index, int maxIndex, Direction direction, bool menuMode)
        {
            if (menuMode && key == ConsoleKey.C)
            {
                return -1;
            }
            if (direction == Direction.Vertical)
            {
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (index != maxIndex - 1)
                            index++;
                        else
                            index = 0;
                        break;
                    case ConsoleKey.UpArrow:
                        if (index != 0)
                            index--;
                        else
                            index = maxIndex - 1;
                        break;
                }
            }
            else
            {
                switch (key)
                {
                    case ConsoleKey.RightArrow:
                        if (index != maxIndex - 1)
                            index++;
                        else
                            index = 0;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (index != 0)
                            index--;
                        else
                            index = maxIndex - 1;
                        break;
                }
            }
            return index;
        }

        private static void ListVertical(string[] options, int index, int maxIndex)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (i == index && maxIndex > 1)
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

        private static void ListHorizontal(string[] options, int index, int maxIndex)
        {
            Console.Write($"| ");
            for (int i = 0; i < options.Length; i++)
            {
                if (i == index && maxIndex > 1)
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

        private static int[] GetLengthOfStrings(string[] strings)
        {
            List<int> _stringLength = new List<int>();

            foreach (var _string in strings)
            {
                _stringLength.Add(_string.Length);
            }
            return _stringLength.ToArray();
        }

        private static int[] GetLongestStrings(string[] options, bool menuMode)
        {
            List<int[]> _optionLength = new List<int[]>();
            foreach (var _option in options)
            {
                if (menuMode)
                    _optionLength.Add(GetLengthOfStrings(_option.Split(" | ")));
                else
                    _optionLength.Add(GetLengthOfStrings(_option.Split(" ")));
            }

            List <int> _longestRowInts = new List<int>();
            for (int i = 0; i < _optionLength[0].Length; i++)
            {
                int[] _rows = GetRows(_optionLength.ToArray(), i);
                _longestRowInts.Add(GetLongestString(_rows));
            }
            return _longestRowInts.ToArray();
        }

        private static int[] GetRows(int[][] options, int index)
        {
            List <int> _rowIndex = new List<int>();
            foreach (var _option in options)
            {
                _rowIndex.Add(_option[index]);
            }
            return _rowIndex.ToArray();
        }

        private static int GetLongestString(int[] lengths)
        {
            int _longestlength = 0;
            foreach (var _length in lengths)
            {
                if (_longestlength < _length)
                    _longestlength = _length;
            }
            return _longestlength;
        }

        private static string AddWhitespaces(string text, int length)
        {
            int _textLength = text.Length;
            for (int i = 0; i < length + 1; i++)
            {
                if (_textLength < i)
                {
                    text += ' ';
                }
            }
            return text;
        }
    }
}
