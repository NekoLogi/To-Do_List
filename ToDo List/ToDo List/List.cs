namespace ToDo_List
{
    internal class List
    {
        public enum SortFlag
        {
            sortdate,
            sortid,
            sortprio,
            sorttarget,
            sorttitle,
            sortversion,
            sortsubtasks
        }

        public enum Order
        {
            Ascending,
            Descending
        }

        public static string[] GenerateTasks(Task[] tasks)
        {
            List<string> _tasks = new List<string>();
            string _task = "";
            for (int i = 0; i < tasks.Count(); i++)
            {
                _task = $"{tasks[i].Id}~~{tasks[i].Target}~~{tasks[i].Topic}~~{Enum.GetNames(typeof(Task.TaskStatus))[tasks[i].Status]}~~{Enum.GetNames(typeof(Task.PriorityLevel))[tasks[i].Priority]}~~{tasks[i].Title}~~{tasks[i].Description}~~{tasks[i].Version}~~{tasks[i].Date}";
                _tasks.Add(_task);
            }
            return _tasks.ToArray();
        }

        public static string[] GenerateTask(Task task)
        {
            string[] _tasks = new string[]
            {
                $"{task.Target}",
                $"{task.Topic}",
                $"{Enum.GetNames(typeof(Task.TaskStatus))[task.Status]}",
                $"{Enum.GetNames(typeof(Task.PriorityLevel))[task.Priority]}",
                $"{task.Title}",
                $"{task.Description}",
                $"{task.Version}",
                $"{task.Date}"
            };
            return _tasks;
        }

        public static string[] Sort(string[] tasks, int[] sortFlags)
        {

            foreach (int _flag in sortFlags)
            {
                string[]? _sortedTasks = null;

                switch ((SortFlag)_flag)
                {
                    case SortFlag.sortdate:
                        break;
                    case SortFlag.sortid:
                        break;
                    case SortFlag.sortprio:
                        break;
                    case SortFlag.sorttarget:
                        break;
                    case SortFlag.sorttitle:
                        break;
                    case SortFlag.sortversion:
                        break;
                    case SortFlag.sortsubtasks:
                        break;
                }
            }
            return null!;
        }

        private static string[] GetTargets(Task[] tasks)
        {
            List<string> _targets = new List<string>();

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

        private static string[] GetTopics(Task[] tasks, string target)
        {
            List<string> _topics = new List<string>();

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
    }
}
