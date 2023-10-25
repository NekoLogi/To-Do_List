using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ToDo_List
{
    public class Task
    {
        [JsonProperty] public int Id { get; private set; }
        [JsonProperty] public string Target { get; private set; }
        [JsonProperty] public string Topic { get; private set; }
        [JsonProperty] public int Status { get; private set; }
        [JsonProperty] public int Priority { get; private set; }
        [JsonProperty] public string Title { get; private set; }
        [JsonProperty] public string Description { get; private set; }
        [JsonProperty] public string Version { get; private set; }
        [JsonProperty] public string Date { get; private set; }


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


        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetTopic(string topic)
        {
            Topic = topic;
        }

        public void SetTarget(string target)
        {
            Target = target;
        }

        public void SetStatus(int status)
        {
            Status = status;
        }

        public void SetPriority(int priority)
        {
            Priority = priority;
        }

        public void SetVersion(string version)
        {
            Version = version;
        }


        public static string[] TasksToString(Task[] tasks)
        {
            List<string> _tasks = new();
            for (int i = 0; i < tasks.Length; i++)
            {
                string _task = $"{tasks[i].Id}~~{tasks[i].Target}~~{tasks[i].Topic}~~{Enum.GetNames(typeof(TaskStatus))[tasks[i].Status]}~~{Enum.GetNames(typeof(Task.PriorityLevel))[tasks[i].Priority]}~~{tasks[i].Title}~~{tasks[i].Description}~~{tasks[i].Version}~~{tasks[i].Date}";
                _tasks.Add(_task);
            }
            return _tasks.ToArray();
        }

        public string[] TaskToString()
        {
            string[] _tasks = new string[]
            {
                $"{Target}",
                $"{Topic}",
                $"{Enum.GetNames(typeof(TaskStatus))[Status]}",
                $"{Enum.GetNames(typeof(PriorityLevel))[Priority]}",
                $"{Title}",
                $"{Description}",
                $"{Version}",
                $"{Date}"
            };
            return _tasks;
        }


        public static string[] GetTargets(Task[] tasks)
        {
            List<string> _targets = new();
            foreach (var task in tasks)
            {
                if (!string.IsNullOrEmpty(task.Target))
                {
                    if (!_targets.Contains(task.Target!))
                    {
                        _targets.Add(task.Target!);
                    }
                }
            }
            return _targets.ToArray();
        }

        public static string[] GetTopics(Task[] tasks, string target)
        {
            List<string> _topics = new();

            foreach (var task in tasks)
            {
                if (!string.IsNullOrEmpty(task.Target) && !string.IsNullOrEmpty(task.Topic))
                {
                    if (target == task.Target! && !_topics.Contains(task.Topic!))
                    {
                        _topics.Add(task.Topic!);
                    }
                }
            }
            return _topics.ToArray();
        }


        public static object[] GetTasksByTarget(Task[] tasks, string target)
        {
            if (target == null || target == "All")
                return new object[] { tasks, target };

            List<Task> _tasks = new();
            foreach (var task in tasks!)
            {
                if (task.Target == target)
                    _tasks.Add(task);
            }

            return _tasks.Count == 0 ? new object[] { tasks, null } : new object[] { _tasks.ToArray(), target };
        }

        public static int CountTargets(Task[] tasks)
        {
            if (tasks == null)
                return 0;
            if (tasks.Length == 0)
                return 0;

            List<string> _targets = new();
            foreach (var _task in tasks)
            {
                if (!_targets.Contains(_task.Target))
                    _targets.Add(_task.Target);
            }
            return _targets.Count;
        }


        public static bool Create(string path, string target, string topic, int status, int priority, string title, string description, string version, string date)
        {
            Task _task = new();
            int _largestId = 0;
            foreach (var _item in Directory.GetFiles(path))
            {
                int _currentId = int.Parse(Path.GetFileNameWithoutExtension(_item));
                if (_largestId < _currentId)
                    _largestId = _currentId;
            }
            _task.Id = _largestId + 1;
            _task.Target = ReplaceEmpty(target);
            _task.Topic = ReplaceEmpty(topic);
            _task.Priority = priority;
            _task.Title = ReplaceEmpty(title);
            _task.Description = ReplaceEmpty(description);
            _task.Version = ReplaceEmpty(version);
            _task.Status = status;
            _task.Date = date;

            string _path = $"{path}/{_task.Id}.json";
            string _json = JsonConvert.SerializeObject(_task, Formatting.Indented);
            File.WriteAllText(_path, _json);

            return true;
        }

        public bool Delete(string path)
        {
            string _fileName = $"{Id}.json";
            string _path = $"{path}/{_fileName}";
            if (File.Exists(_path))
            {
                File.Delete(_path);
                return true;
            }
            return false;
        }

        public static Task[] GetTasks(string path)
        {
            if (!Directory.Exists(Preset.Path))
            {
                Directory.CreateDirectory(Preset.Path);
                return null;
            }

            string[] _files = Directory.GetFiles(path);
            List<Task> _tasks = new();
            foreach (string _file in _files)
            {
                string _json = File.ReadAllText(_file);
                Task _task = JsonConvert.DeserializeObject<Task>(_json)!;
                if (_task != null)
                    _tasks.Add(_task!);
            }
            return _tasks.Count == 0 ? null : _tasks.ToArray();
        }

        public bool Save(string path)
        {
            string _path = $"{path}/{Id}.json";
            string _json = JsonConvert.SerializeObject(this, Formatting.Indented);
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
