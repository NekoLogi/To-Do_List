using System;
using System.Reflection;

namespace ToDo_List
{
    internal class Program
    {
        private enum Flag
        {
            gui
        }

        private static int[] GetSortFlags(string[] args)
        {
            List<int> _flags = new List<int>();
            foreach (string arg in args)
            {
                foreach (var flag in Enum.GetValues(typeof(List.SortFlag)))
                {
                    if (arg.Remove(0).ToLower() == flag.ToString())
                    {
                        _flags.Add((int)flag);
                    }
                }
            }
            return _flags.ToArray();
        }

        private static Flag[] GetFlags(string[] args)
        {
            List<Flag> _flags = new List<Flag>();
            foreach (string arg in args)
            {
                foreach (var flag in Enum.GetValues(typeof(List.SortFlag)))
                {
                    if (arg.Remove(0).ToLower() == flag.ToString())
                    {
                        _flags.Add((Flag)flag);
                    }
                }
            }
            return _flags.ToArray();
        }

        private static void Start(int[]? sortFlags)
        {
            while (true)
            {
                Task[] _tasks = Task.LoadTasks(Preset.Path)!;
                Task[] _sortedTaskList = null!;
                if (sortFlags == null || sortFlags?.Length == 0)
                {
                    //_sortedTaskList = List.Sort();
                }
                else
                {
                    //_sortedTaskList = List.Sort();
                }
                string[] _taskList = List.GenerateTasks(_sortedTaskList!);
                int _taskIndex = Display.Selector("Tasks:", _taskList, _taskList.Count(), Display.Direction.Vertical, true);
                Task _task = _sortedTaskList![_taskIndex];

                string[] _options = {  "Back", "Edit", "Delete" };
                int _index = Display.Selector("Editor:", _options, _options.Count(), Display.Direction.Horizontal, false);
                if (_index == 1)
                {
                    _options = new string[] { "Yes", "No" };
                    _index = Display.Selector("Are you sure:", _options, _options.Count(), Display.Direction.Horizontal, false);
                    if (_index == 2)
                    {
                        Task.Delete(Preset.Path, _task);
                    }
                }
                else if (_index == 1)
                {
                    Editor(_taskIndex, _task);
                }
            }
        }

        private static void Editor(int index, Task task)
        {
            string[] _taskOptions = List.GenerateTask(task);
            index = Display.Selector("Editor:", _taskOptions, 6, Display.Direction.Horizontal, false);

            string[] _options = { "Edit", "Back" };
            switch (index)
            {
                case 0:
                    index = Display.Selector("Editor:", _options, _options.Count(), Display.Direction.Horizontal, false);
                    break;
                case 1:
                    index = Display.Selector("Editor:", _taskOptions, _options.Count(), Display.Direction.Horizontal, false);
                    break;
                case 2:
                    index = Display.Selector("Editor:", _taskOptions, _options.Count(), Display.Direction.Horizontal, false);
                    break;
                case 3:
                    index = Display.Selector("Editor:", _taskOptions, _options.Count(), Display.Direction.Horizontal, false);
                    break;
                case 4:
                    index = Display.Selector("Editor:", _taskOptions, _options.Count(), Display.Direction.Horizontal, false);
                    break;
                case 5:
                    index = Display.Selector("Editor:", _taskOptions, _options.Count(), Display.Direction.Horizontal, false);
                    break;
                case 6:
                    index = Display.Selector("Editor:", _taskOptions, _options.Count(), Display.Direction.Horizontal, false);
                    break;
            }
        }

        static void Main(string[] args)
        {
            int[]? _sortFlags = null;
            Flag[]? _flags = null;

            if (!Directory.Exists(Preset.Path))
            {
                Directory.CreateDirectory(Preset.Path);
            }
            if (args.Length != 0)
            {
                _sortFlags = GetSortFlags(args);
                _flags = GetFlags(args);

                if (_flags.Contains(Flag.gui))
                    Preset.UseGUI = true;
            }
            if (!Preset.UseGUI)
            {
                Console.Title = $"{Build.Name} {Build.Version}";
            }
            if (Directory.GetFiles(Preset.Path).Length == 0)
            {
                string[] _options = { "Yes", "No" };
                int _index = Display.Selector("Task list is empty.\nDo you want to create a new task?", _options, _options.Count(), Display.Direction.Vertical, false);
                Console.Clear();
                if (_index == 1)
                {
                    Environment.Exit(0);
                }
                Task.Create(Preset.Path);
            }
            Start(_sortFlags);
        }
    }
}