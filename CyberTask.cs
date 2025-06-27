using System;

namespace TaskManagementApp
{
    public class CyberTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public string Display => $"{Title} {(IsCompleted ? "(Completed)" : "")}";
    }
}

