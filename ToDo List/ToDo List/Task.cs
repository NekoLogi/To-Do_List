using Newtonsoft.Json;


namespace ToDo_List
{
    internal class Task
    {
        public int Id;
        public string? Target;
        public string? Topic;
        public int Status;
        public int Priority;
        public string? Title;
        public string? Description;
        public string? Version;
        public string? Date;

        public enum Label
        {
            Target,
            Topic,
            Status,
            Priority,
            Title,
            Description,
            Version,
            Date
        }

        public enum PriorityLevel
        {
            Critical,
            Major,
            Moderate,
            Minor,
            Cosmetic
        }

        public enum TaskStatus
        {
            Queued,
            In_Progress,
            Done
        }

        public static bool Create(string path)
        {
            Task _task = new Task();
            if (!Preset.UseGUI)
            {
                // ID
                int _largestId = 0;
                foreach (var _item in Directory.GetFiles(path))
                {
                    int _currentId = int.Parse(Path.GetFileNameWithoutExtension(_item));
                    if (_largestId < _currentId)
                        _largestId = _currentId;
                }
                _task.Id = _largestId + 1;

                // Target
                Console.Write("Application: ");
                _task.Target = ReplaceEmpty(Console.ReadLine()!);
                Console.Clear();

                // Topic
                Console.Write("Task Group: ");
                _task.Topic = ReplaceEmpty(Console.ReadLine()!);
                Console.Clear();

                // Priority
                string[] _options = Enum.GetNames(typeof(PriorityLevel));
                int _index = Display.Selector("Priority:", _options, _options.Count(), Display.Direction.Vertical, false);
                _task.Priority = _index;
                Console.Clear();

                // Title
                Console.Write("Title: ");
                _task.Title = ReplaceEmpty(Console.ReadLine()!);
                Console.Clear();

                // Description
                Console.Write("Description: ");
                _task.Description = ReplaceEmpty(Console.ReadLine()!);
                Console.Clear();

                // Version
                Console.Write("Version: ");
                _task.Version = ReplaceEmpty(Console.ReadLine()!);
                Console.Clear();

                _task.Status = (int)TaskStatus.Queued;
                _task.Date = DateTime.Now.Date.ToString("dd/MM/yyyy");
            }
            string _path = $"{path}/{_task.Id}.json";
            string _json = JsonConvert.SerializeObject(_task, Formatting.Indented);
            File.WriteAllText(_path, _json);

            return true;
        }

        public static bool Delete(string path, Task task)
        {
            string _fileName = $"{task.Target} {task.Topic} {task.Title}.json";
            string _path = $"{path}/{_fileName}";
            if (File.Exists(_path))
            {
                File.Delete(_path);
                return true;
            }
            return false;
        }

        public static Task[]? LoadTasks(string path)
        {
            string[] _files = Directory.GetFiles(path);
            List<Task> _tasks = new List<Task>();
            foreach (string _file in _files)
            {
                string _json = File.ReadAllText(_file);
                Task _task = JsonConvert.DeserializeObject<Task>(_json)!;
                _tasks.Add(_task);
            }
            return _tasks.ToArray();
        }

        public static bool SaveTask(string path, Task task)
        {
            string _path = $"{path}/{task.Id}.json";
            string _json = JsonConvert.SerializeObject(task, Formatting.Indented);
            File.WriteAllText(_path, _json);

            return true;
        }

        private static string ReplaceEmpty(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return " ";
            }
            return data;
        }
    }
}
