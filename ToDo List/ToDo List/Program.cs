namespace ToDo_List
{
    internal class Program
    {
        /// <summary>
        /// 
        /// TODO:
        /// 
        /// </summary>


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

        private static string? Rename(string text)
        {
            Console.WriteLine("Current: {0}", text);
            Console.Write("New: ");
            string? _name = Console.ReadLine();
            Console.Clear();
            return _name;
        }

        private static Task Editor(Task task)
        {
            int _loop = 0;
            while (_loop == 0)
            {
                string[] _options = { "Edit", "Back" };

                string[] _taskOptions = List.GenerateTask(task);
                int _index = Display.Selector("Editor:", _taskOptions, 6, Display.Direction.Horizontal, false);
                switch (_index)
                {
                    case (int)Task.Label.Target:
                        _index = Display.Selector("Editor:", _options, _options.Length, Display.Direction.Vertical, false);
                        if (_index == 0)
                        {
                            task.Target = Rename(task.Target!);
                        }
                        break;
                    case (int)Task.Label.Topic:
                        _index = Display.Selector("Editor:", _options, _options.Length, Display.Direction.Vertical, false);
                        if (_index == 0)
                        {
                            task.Topic = Rename(task.Topic!);
                        }
                        break;
                    case (int)Task.Label.Status:
                        _index = Display.Selector("Editor:", _options, _options.Length, Display.Direction.Vertical, false);
                        if (_index == 0)
                        {
                            task.Status = Display.Selector($"Current: {Enum.GetNames(typeof(Task.TaskStatus))[task.Status]}",
                                Enum.GetNames(typeof(Task.TaskStatus)),
                                Enum.GetNames(typeof(Task.TaskStatus)).Length,
                                Display.Direction.Vertical,
                                false);
                        }
                        break;
                    case (int)Task.Label.Priority:
                        _index = Display.Selector("Editor:", _options, _options.Count(), Display.Direction.Vertical, false);
                        if (_index == 0)
                        {
                            task.Priority = Display.Selector($"Current: {Enum.GetNames(typeof(Task.PriorityLevel))[task.Priority]}",
                                Enum.GetNames(typeof(Task.PriorityLevel)),
                                Enum.GetNames(typeof(Task.PriorityLevel)).Length,
                                Display.Direction.Vertical,
                                false);
                        }
                        break;
                    case (int)Task.Label.Title:
                        _index = Display.Selector("Editor:", _options, _options.Length, Display.Direction.Vertical, false);
                        if (_index == 0)
                        {
                            task.Title = Rename(task.Title!);
                        }
                        break;
                    case (int)Task.Label.Description:
                        _index = Display.Selector("Editor:", _options, _options.Length, Display.Direction.Vertical, false);
                        if (_index == 0)
                        {
                            task.Description = Rename(task.Description!);
                        }
                        break;
                    case (int)Task.Label.Version:
                        _index = Display.Selector("Editor:", _options, _options.Length, Display.Direction.Vertical, false);
                        if (_index == 0)
                        {
                            task.Version = Rename(task.Version!);
                        }
                        break;
                }

                _options = new string[] { "Yes", "No" };
                _loop = Display.Selector("Do you want to edit more:", _options, _options.Count(), Display.Direction.Vertical, false);
            }
            return task;
        }

        private static void Start(int[]? sortFlags)
        {
            while (true)
            {
                Task[] _tasks = Task.LoadTasks(Preset.Path)!;
                Task[] _sortedTaskList = null!;
                if (sortFlags == null || sortFlags?.Length == 0)
                {
                    _sortedTaskList = _tasks;
                }
                else
                {
                    //_sortedTaskList = List.Sort();
                }
                string[] _taskList = List.GenerateTasks(_sortedTaskList!);
                int _taskIndex = Display.Selector("Tasks:", _taskList, _taskList.Count(), Display.Direction.Vertical, true);
                Task _task;
                switch (_taskIndex)
                {
                    case -1:
                        Task.Create(Preset.Path);
                        continue;
                    default:
                        _task = _sortedTaskList![_taskIndex];
                        break;
                }
                string[] _options = { "Back", "Edit", "Delete" };
                int _index = Display.Selector("Editor:", _options, _options.Count(), Display.Direction.Vertical, false);
                if (_index == 2)
                {
                    _options = new string[] { "Yes", "No" };
                    _index = Display.Selector("Are you sure:", _options, _options.Count(), Display.Direction.Vertical, false);
                    if (_index == 0)
                    {
                        Task.Delete(Preset.Path, _task);
                    }
                }
                else if (_index == 1)
                {
                    Task.SaveTask(Preset.Path, Editor(_task));
                }
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