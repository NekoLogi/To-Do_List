namespace ToDo_List
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"{Build.NAME}: {Build.BRANCH}-{Build.VERSION}";

            bool hasTarget = false;
            string _target = null;
            while (true)
            {
                Task[] _tasks = Task.GetTasks(Preset.Path);
                while (_tasks == null)
                {
                    if (_tasks == null)
                    {
                        string[] _options = { "Yes", "No" };
                        int _index = Display.Selector("Task list is empty.\nDo you want to create a new task?", _options, _options.Length, Display.Direction.Vertical, false, false);
                        Console.Clear();
                        if (_index == 1)
                        {
                            Environment.Exit(0);
                        }
                        if (CreateTask(_tasks))
                            _tasks = Task.GetTasks(Preset.Path);
                    }
                }


                do
                {
                    if (_target == null)
                        hasTarget = false;

                    int _targetCount = Task.CountTargets(_tasks);
                    if (_tasks!.Length > 1 && !hasTarget && _targetCount > 1)
                        _target = TargetSelection(out _tasks);
                    else if (_targetCount == 1)
                        _target = "All";

                    object[] _result = Task.GetTasksByTarget(_tasks, _target);
                    _tasks = (Task[])_result[0];
                    _target = (string)_result[1];

                } while (_target == null);

                hasTarget = Start(_tasks!);
            }
        }

        private static bool Start(Task[] tasks)
        {
            Task _task = MainSelector(tasks);
            if (_task != null)
            {
                EditorSelector(_task);
                return true;
            }
            return false;
        }

        private static string GetString(string text)
        {
            Console.WriteLine("Current: {0}", text);
            Console.Write("New: ");
            string _name = Console.ReadLine();
            Console.Clear();
            return _name;
        }

        private static void Editor(Task task)
        {
            while (true)
            {
                string _result;
                string[] _taskOptions = task.TaskToString();
                int _index = Display.Selector("Editor:", _taskOptions, 7, Display.Direction.Horizontal, false, true);
                switch (_index)
                {
                    case (int)Display.SpecialSelector.RETURN:
                        return;
                    case (int)Task.Label.Target:
                        _result = GetString(task.Target!);
                        task.SetTarget(_result);
                        break;
                    case (int)Task.Label.Topic:
                        _result = GetString(task.Topic!);
                        task.SetTopic(_result);
                        break;
                    case (int)Task.Label.Status:
                        int _result1 = Display.Selector($"Current: {Enum.GetNames(typeof(Task.TaskStatus))[task.Status]}",
                            Enum.GetNames(typeof(Task.TaskStatus)),
                            Enum.GetNames(typeof(Task.TaskStatus)).Length,
                            Display.Direction.Vertical,
                            false, false);
                        task.SetStatus(_result1);
                        break;
                    case (int)Task.Label.Priority:
                        int _result2 = Display.Selector($"Current: {Enum.GetNames(typeof(Task.PriorityLevel))[task.Priority]}",
                            Enum.GetNames(typeof(Task.PriorityLevel)),
                            Enum.GetNames(typeof(Task.PriorityLevel)).Length,
                            Display.Direction.Vertical,
                            false, false);
                        task.SetPriority(_result2);
                        break;
                    case (int)Task.Label.Title:
                        _result = GetString(task.Title!);
                        task.SetTitle(_result);
                        break;
                    case (int)Task.Label.Description:
                        _result = GetString(task.Description!);
                        task.SetDescription(_result);
                        break;
                    case (int)Task.Label.Version:
                        _result = GetString(task.Version!);
                        task.SetVersion(_result);
                        break;
                }
                task.Save(Preset.Path);
            }
        }

        private static void EditorSelector(Task _task)
        {
            string[] _options = { "Back", "Edit", "Delete" };
            int _index = Display.Selector("Editor:", _options, _options.Length, Display.Direction.Vertical, false, false);
            if (_index == 2)
            {
                _options = new string[] { "Yes", "No" };
                _index = Display.Selector("Are you sure:", _options, _options.Length, Display.Direction.Vertical, false, false);
                if (_index == 0)
                    _task.Delete(Preset.Path);
            }
            else if (_index == 1)
                Editor(_task);
        }

        private static Task MainSelector(Task[] tasks)
        {
            string[] _taskList = Task.TasksToString(tasks);
            int _taskIndex = Display.Selector("Tasks:", _taskList, _taskList.Length, Display.Direction.Vertical, true, true);
            switch (_taskIndex)
            {
                case (int)Display.SpecialSelector.RETURN:
                    return null;
                case (int)Display.SpecialSelector.CREATE_NEW_TASK:
                    CreateTask(tasks);
                    return null;
                default:
                    return tasks[_taskIndex];
            }
        }

        private static string TargetSelection(out Task[] tasks)
        {
            int _index = 0;
            List<string> _options = new();
            tasks = null;
            while (_index == 0)
            {
                tasks = Task.GetTasks(Preset.Path);
                string[] _targets = Task.GetTargets(tasks);
                _options = new(_targets);
                _options.Insert(0, "All");
                _options.Insert(0, "Create new Task");
                _index = Display.Selector("Applications:", _options.ToArray(), _options.Count, Display.Direction.Vertical, false, false);
                if (_index == 0)
                    CreateTask(tasks);
            }
            return _options[_index];
        }

        private static string Prompt(string request)
        {
            string s = "";
            while (true)
            {
                Console.WriteLine("Press 'ESC' to cancel.");
                Console.Write($"{request}{s}");
                ConsoleKeyInfo _key = Console.ReadKey();
                Console.Clear();

                switch (_key.Key)
                {
                    case ConsoleKey.Enter:
                        return s;
                    case ConsoleKey.Escape:
                        return null;
                    case ConsoleKey.Backspace:
                        if (s.Length != 0)
                            s = s.Remove(s.Length - 1);
                        break;
                    default:
                        s += _key.KeyChar;
                        break;
                }
            }
        }

        private static bool CreateTask(Task[] tasks)
        {
            // Target
            string _target = "";
            if (tasks != null && tasks.Length != 0)
            {
                List<string> _targets = new()
                {
                    "New Application"
                };
                _targets.AddRange(from _task in Task.GetTargets(tasks)
                                  select _task);

                int _result = Display.Selector("Existing Applications:", _targets.ToArray(), _targets.Count, Display.Direction.Vertical, false, true);
                if (_result == (int)Display.SpecialSelector.RETURN)
                    return false;
                else if (_result != 0)
                    _target = tasks[_result - 1].Target;

            }
            if (string.IsNullOrEmpty(_target))
            {
                _target = Prompt("Application: ");
                if (_target == null)
                    return false;
            }
            // Topic
            string _topic = "";
            string[] _options = new string[] { "New Group", "Feature", "Bug" };
            int _index = Display.Selector("Groups:", _options, _options.Length, Display.Direction.Vertical, false, true);
            if (_index == (int)Display.SpecialSelector.RETURN)
                return false;
            else if (_index != 0)
                _topic = _options[_index];
            if (string.IsNullOrEmpty(_topic))
            {
                _topic = Prompt("Task Group: ");
                if (_topic == null)
                    return false;
            }
            // Priority
            _options = Enum.GetNames(typeof(Task.PriorityLevel));
            _index = Display.Selector("Priority:", _options, _options.Length, Display.Direction.Vertical, false, true);
            int _priority = _index;
            if (_priority == (int)Display.SpecialSelector.RETURN)
                return false;
            // Title
            string _title = Prompt("Title: ");
            if (_title == null)
                return false;
            // Description
            string _description = Prompt("Description: ");
            if (_description == null)
                return false;
            // Version
            string _version = Prompt("Version: ");
            if (_version == null)
                return false;
            // Date
            int _status = (int)Task.TaskStatus.Queued;
            string _date = DateTime.Now.Date.ToString("dd/MM/yyyy");


            Task.Create(Preset.Path, _target, _topic, _status, _priority, _title, _description, _version, _date);
            return true;
        }
    }
}