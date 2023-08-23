using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ToDo_List
{
    internal class Task
    {
        public string? Target;
        public string? Topic;
        public int Status;
        public int Priority;
        public string? Title;
        public string? Description;
        public string? Version;
        public string? Date;

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
                Console.Write("Application: ");
                _task.Target = Console.ReadLine();
                Console.Clear();

                Console.Write("Task Group: ");
                _task.Topic = Console.ReadLine();
                Console.Clear();

                string[] _options = (string[])Enum.GetValues(typeof(PriorityLevel));
                int _index = Display.Selector("Priority:", _options, _options.Count(), Display.Direction.Vertical, false);
                _task.Priority = _index;
                Console.Clear();

                Console.Write("Title: ");
                _task.Title = Console.ReadLine();
                Console.Clear();

                Console.Write("Description: ");
                _task.Description = Console.ReadLine();
                Console.Clear();

                Console.Write("Version: ");
                _task.Version = Console.ReadLine();
                Console.Clear();

                _task.Status = (int)TaskStatus.Queued;
                _task.Date = DateTime.Now.Date.ToString();
            }
            string _path = $"{path}/{_task.Target} {_task.Topic} {_task.Title}.json";
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
                Task _task = JsonConvert.DeserializeObject<Task>(_file)!;
                _tasks.Add(_task);
            }
            return _tasks.ToArray();
        }
    }
}
