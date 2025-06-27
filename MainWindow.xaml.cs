using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TaskManagementApp
{
    public partial class MainWindow : Window
    {
        private List<CyberTask> tasks = new List<CyberTask>();
        private List<CyberTask> reminders = new List<CyberTask>();
        private List<string> activityLog = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            ReminderCheckBox.Checked += (s, e) => ReminderDatePicker.IsEnabled = true;
            ReminderCheckBox.Unchecked += (s, e) => ReminderDatePicker.IsEnabled = false;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text.Trim();
            string description = TaskDescriptionBox.Text.Trim();
            DateTime? reminderDate = ReminderCheckBox.IsChecked == true ? ReminderDatePicker.SelectedDate : null;

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            CyberTask newTask = new CyberTask
            {
                Title = title,
                Description = description,
                ReminderDate = reminderDate
            };

            tasks.Add(newTask);
            TaskListBox.ItemsSource = null;
            TaskListBox.ItemsSource = tasks;

            activityLog.Add($"Task added: '{title}' at {DateTime.Now}");
            LimitLogLength();

            TaskTitleBox.Clear();
            TaskDescriptionBox.Clear();
            ReminderCheckBox.IsChecked = false;
            ReminderDatePicker.SelectedDate = null;
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CyberTask selectedTask)
            {
                selectedTask.IsCompleted = true;
                TaskListBox.ItemsSource = null;
                TaskListBox.ItemsSource = tasks;
                activityLog.Add($"Task completed: '{selectedTask.Title}' at {DateTime.Now}");
                LimitLogLength();
                MessageBox.Show("Task marked as completed.");
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CyberTask selectedTask)
            {
                tasks.Remove(selectedTask);
                TaskListBox.ItemsSource = null;
                TaskListBox.ItemsSource = tasks;
                activityLog.Add($"Task deleted: '{selectedTask.Title}' at {DateTime.Now}");
                LimitLogLength();
                MessageBox.Show("Task deleted.");
            }
        }

        private void ShowActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogListBox.Items.Clear();
            foreach (string log in activityLog)
            {
                ActivityLogListBox.Items.Add(log);
            }
        }

        private void LimitLogLength()
        {
            if (activityLog.Count > 10)
                activityLog.RemoveAt(0);
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string input = UserInputTextBox.Text.ToLower();
                UserInputTextBox.Clear();

                if (input.Contains("task") || input.Contains("reminder"))
                {
                    if (input.Contains("add") || input.Contains("create"))
                        AddTaskOrReminder(input);
                    else if (input.Contains("view") || input.Contains("show"))
                        ViewTasksOrReminders(input);
                }
                else if (input.Contains("quiz"))
                {
                    StartQuiz();
                }
                else if (input.Contains("activity log") || input.Contains("what have you done"))
                {
                    ShowActivityLogButton_Click(null, null);
                }
            }
        }

        private void AddTaskOrReminder(string input)
        {
            if (input.Contains("task"))
            {
                string taskDesc = GetTaskDescription(input);
                tasks.Add(new CyberTask { Title = taskDesc, Description = "" });
                ChatbotOutputTextBlock.Text = $"Task added: '{taskDesc}'";
            }
            else if (input.Contains("reminder"))
            {
                string reminderDesc = GetReminderDescription(input);
                string reminderDate = GetReminderDate(input);
                reminders.Add(new CyberTask { Title = reminderDesc, ReminderDate = DateTime.Parse(reminderDate) });
                ChatbotOutputTextBlock.Text = $"Reminder set: '{reminderDesc}' on {reminderDate}";
            }
        }

        private void ViewTasksOrReminders(string input)
        {
            ChatbotOutputTextBlock.Text = input.Contains("task") ? "Tasks:" : "Reminders:";
            var list = input.Contains("task") ? tasks : reminders;
            foreach (var item in list)
            {
                ChatbotOutputTextBlock.Text += $"\n{item.Title} {(item.ReminderDate.HasValue ? " - " + item.ReminderDate.Value.ToShortDateString() : "")}";
            }
        }

        private string GetTaskDescription(string input)
        {
            return ExtractAfterKeyword(input, "task");
        }

        private string GetReminderDescription(string input)
        {
            return ExtractAfterKeyword(input, "reminder");
        }

        private string GetReminderDate(string input)
        {
            foreach (var word in input.Split(' '))
            {
                if (DateTime.TryParse(word, out DateTime parsed))
                    return parsed.ToString("yyyy-MM-dd");
                if (word == "tomorrow")
                    return DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        private string ExtractAfterKeyword(string input, string keyword)
        {
            string[] words = input.Split(' ');
            bool found = false;
            string result = "";
            foreach (string word in words)
            {
                if (found)
                    result += word + " ";
                if (word == keyword)
                    found = true;
            }
            return result.Trim();
        }

        // --- QUIZ SYSTEM ---
        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>
        {
            new QuizQuestion("What is phishing?", new[] { "A type of malware", "A type of social engineering attack", "A type of virus", "A type of firewall" }, 1),
            new QuizQuestion("What is a strong password?", new[] { "A short password with only letters", "A long password with a mix of letters, numbers, and special characters", "A password that is easy to remember", "A password that is used for multiple accounts" }, 1),
            new QuizQuestion("Is it safe to click on links from unknown emails?", new[] { "True", "False" }, 1),
            new QuizQuestion("What is social engineering?", new[] { "A type of malware", "A type of attack that exploits human psychology", "A type of virus", "A type of firewall" }, 1),
            new QuizQuestion("Why is two-factor authentication important?", new[] { "It makes your password stronger", "It adds an extra layer of security to your account", "It makes it easier to reset your password", "It is not important" }, 1),
        };

        private int currentQuestionIndex = 0;
        private int score = 0;

        private void StartQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            DisplayQuestion();
            activityLog.Add($"Quiz started at {DateTime.Now}");
            LimitLogLength();
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e) => StartQuiz();

        private void DisplayQuestion()
        {
            if (currentQuestionIndex < quizQuestions.Count)
            {
                var q = quizQuestions[currentQuestionIndex];
                QuestionTextBlock.Text = q.QuestionText;
                OptionButtonsStackPanel.Children.Clear();
                for (int i = 0; i < q.Options.Length; i++)
                {
                    Button b = new Button { Content = q.Options[i], Tag = i };
                    b.Click += OptionButton_Click;
                    OptionButtonsStackPanel.Children.Add(b);
                }
            }
            else
            {
                ResultTextBlock.Text = $"Quiz completed! Your score is {score}/{quizQuestions.Count}.";
            }
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            int selected = (int)((Button)sender).Tag;
            if (selected == quizQuestions[currentQuestionIndex].CorrectAnswer)
                score++;
            currentQuestionIndex++;
            DisplayQuestion();
        }

        public class QuizQuestion
        {
            public string QuestionText { get; }
            public string[] Options { get; }
            public int CorrectAnswer { get; }

            public QuizQuestion(string questionText, string[] options, int correctAnswer)
            {
                QuestionText = questionText;
                Options = options;
                CorrectAnswer = correctAnswer;
            }
        }
    }
}
