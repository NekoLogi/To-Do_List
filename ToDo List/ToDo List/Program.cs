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

                do
                {
                    if (_target == null)
                        hasTarget = false;

                    if (_tasks!.Length > 1 && !hasTarget && Task.CountTargets(_tasks) > 1)
                        _target = TargetSelection(_tasks);
                    else if (Task.CountTargets(_tasks) == 1)
                        _target = "All";

                    object[] _result = Task.GetTasksByTarget(_tasks, _target);
                    _tasks = (Task[])_result[0];
                    _target = (string)_result[1];

                } while (_target == null);

                if (_tasks == null)
                {
                    string[] _options = { "Yes", "No" };
                    int _index = Display.Selector("Task list is empty.\nDo you want to create a new task?", _options, _options.Count(), Display.Direction.Vertical, false, false);
                    Console.Clear();
                    if (_index == 1)
                    {
                        Environment.Exit(0);
                    }
                    CreateTask();
                }

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
            int _index = Display.Selector("Editor:", _options, _options.Count(), Display.Direction.Vertical, false, false);
            if (_index == 2)
            {
                _options = new string[] { "Yes", "No" };
                _index = Display.Selector("Are you sure:", _options, _options.Count(), Display.Direction.Vertical, false, false);
                if (_index == 0)
                    _task.Delete(Preset.Path);
            }
            else if (_index == 1)
                Editor(_task);
        }

        private static Task MainSelector(Task[] tasks)
        {
            string[] _taskList = Task.TasksToString(tasks!);
            int _taskIndex = Display.Selector("Tasks:", _taskList, _taskList.Count(), Display.Direction.Vertical, true, true);
            switch (_taskIndex)
            {
                case (int)Display.SpecialSelector.RETURN:
                    return null;
                case (int)Display.SpecialSelector.CREATE_NEW_TASK:
                    CreateTask();
                    return null;
                default:
                    return tasks![_taskIndex];
            }
        }

        private static string TargetSelection(Task[] tasks)
        {
            string[] _targets = Task.GetTargets(tasks);

            List<string> _options = new List<string>(_targets);
            _options.Insert(0, "All");
            int _index = Display.Selector("Applications:", _options.ToArray(), _options.Count(), Display.Direction.Vertical, false, false);

            return _options[_index];
        }

        private static string Prompt(string request)
        {
            string s = "";
            ConsoleKeyInfo _key = new ConsoleKeyInfo();
            while (true)
            {
                Console.WriteLine("Press 'ESC' to cancel.");
                Console.Write($"{request}{s}");
                _key = Console.ReadKey();
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

        private static void CreateTask()
        {
            // Target
            string _target = Prompt("Application: ");
            if (_target == null)
                return;
            // Topic
            string _topic = Prompt("Task Group: ");
            if (_topic == null)
                return;
            // Priority
            string[] _options = Enum.GetNames(typeof(Task.PriorityLevel));
            int _index = Display.Selector("Priority:", _options, _options.Count(), Display.Direction.Vertical, false, true);
            int _priority = _index;
            if (_priority == (int)Display.SpecialSelector.RETURN)
                return;
            // Title
            string _title = Prompt("Title: ");
            if (_title == null)
                return;
            // Description
            string _description = Prompt("Description: ");
            if (_description == null)
                return;
            // Version
            string _version = Prompt("Version: ");
            if (_version == null)
                return;
            // Date
            int _status = (int)Task.TaskStatus.Queued;
            string _date = DateTime.Now.Date.ToString("dd/MM/yyyy");


            Task.Create(Preset.Path, _target, _topic, _status, _priority, _title, _description, _version, _date);
        }
    }
}