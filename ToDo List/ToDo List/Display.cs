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
            int[] _rows = GetLongestStrings(options, menuMode, direction);
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


            if (direction == Direction.Horizontal)
            {
                List<string> strings = _topStrings.ToList();
                strings.RemoveAt(0);
                _topStrings = strings.ToArray();
            }

            while (_key != ConsoleKey.Enter && _index > -1)
            {
                Console.Clear();
                Console.WriteLine(title);

                List<string> _options = new List<string>();

                if (menuMode)
                {
                    DisplayTop(_rows, _topStrings);
                    MainMenu(options, _rows, _topStrings, _options);
                }
                else
                    _options = options.ToList();

                Console.WriteLine();
                if (direction == Direction.Vertical)
                {
                    ListVertical(_options.ToArray(), _index, maxIndex);
                    Console.WriteLine();
                    Console.WriteLine("Use arrow keys UP or DOWN to choose and press 'ENTER' to select.");
                }
                else
                {
                    DisplayTop(_rows, _topStrings);
                    ListHorizontal(_options.ToArray(), _index, maxIndex, _rows, _topStrings);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Use arrow keys LEFT or RIGHT to choose and press 'ENTER' to select.");
                }
                if (menuMode)
                    Console.WriteLine("Press 'End' to create a new task.");
                else
                    Console.WriteLine("Press 'End' to go back.");

                _key = Console.ReadKey().Key;
                _index = Controls(_key, _index, maxIndex, direction, menuMode);
            }
            Console.Clear();
            return _index;
        }

        private static void MainMenu(string[] options, int[] _rows, string[] _topStrings, List<string> _options)
        {
            List<string> _optionColumn;
            for (int j = 0; j < options.Length; j++)
            {
                string[] _args = options[j].Split("~~");
                _optionColumn = FormatOptions(_rows, _topStrings, _args);

                string _newOption = "";
                foreach (var _option in _optionColumn)
                {
                    _newOption += _option;
                }
                _options.Add(_newOption);
                _optionColumn.Clear();
            }
        }

        private static List<string> FormatOptions(int[] rows, string[] topStrings, string[] strings)
        {
            List<string> _optionColumn = new List<string>();
            for (int i = 0; i < topStrings.Length; i++)
            {
                if (rows[i] >= topStrings[i].Length && rows[i] <= strings[i].Length)
                {
                    _optionColumn.Add($"{strings[i]} |");
                }
                else
                    _optionColumn.Add($"{AddSpaceToString(strings[i], topStrings[i].Length)} |");
            }
            return _optionColumn;
        }

        private static void DisplayTop(int[] _rows, string[] _topStrings)
        {
            for (int i = 0; i < _topStrings.Length; i++)
            {
                if (_rows[i] > _topStrings[i].Length)
                {
                    _topStrings[i] = AddSpaceToString(_topStrings[i], _rows[i]);
                }
            }
            foreach (var _text in _topStrings)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Write(_text + " |");
                Console.ResetColor();
            }
        }

        private static int Controls(ConsoleKey key, int index, int maxIndex, Direction direction, bool menuMode)
        {
            if (menuMode && key == ConsoleKey.End)
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

        private static void ListHorizontal(string[] options, int index, int maxIndex, int[] rows, string[] topStrings)
        {
            Console.WriteLine();
            string[] _options = FormatOptions(rows, topStrings, options).ToArray();
            for (int i = 0; i < options.Length; i++)
            {
                if (i == index && maxIndex > 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(_options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(_options[i]);
                }
            }
        }

        private static int[] StringsToInts(string[] strings)
        {
            List<int> _stringLength = new List<int>();

            foreach (var _string in strings)
            {
                _stringLength.Add(_string.Length);
            }
            return _stringLength.ToArray();
        }

        private static int[] GetLongestStrings(string[] options, bool menuMode, Direction direction)
        {
            List<int[]> _optionLength = new List<int[]>();
            if (direction == Direction.Vertical)
                foreach (var _option in options)
                    _optionLength.Add(StringsToInts(_option.Split("~~")));
            else
                _optionLength.Add(StringsToInts(options));

            List<int> _longestRowInts = new List<int>();
            for (int i = 0; i < _optionLength[0].Length; i++)
            {
                int[] _rows = GetRows(_optionLength.ToArray(), i);
                _longestRowInts.Add(GetLargestInt(_rows));
            }
            return _longestRowInts.ToArray();
        }

        private static int[] GetRows(int[][] options, int index)
        {
            List<int> _rowIndex = new List<int>();
            foreach (var _option in options)
            {
                _rowIndex.Add(_option[index]);
            }
            return _rowIndex.ToArray();
        }

        private static int GetLargestInt(int[] lengths)
        {
            int _longestlength = 0;
            foreach (var _length in lengths)
            {
                if (_longestlength < _length)
                    _longestlength = _length;
            }
            return _longestlength;
        }

        private static string AddSpaceToString(string text, int length)
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
