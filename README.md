# TaskAssignmentProgPOE3
assignment

This WPF application, TaskManagementApp, was built to help users manage their daily cybersecurity-related tasks while providing a fun and interactive way to learn about online safety through a built-in quiz system. The project is composed of several main components: MainWindow.xaml defines the user interface layout using XAML, while MainWindow.xaml.cs contains all the application logic, including task management, reminder setting, NLP command interpretation, and the cybersecurity quiz system. A separate model class file, CyberTask.cs, defines the structure of individual tasks or reminders, encapsulating properties like title, description, due date, and completion status.

⚙️ How It Was Built
The application was developed using C# and WPF (Windows Presentation Foundation) within Visual Studio. I started by designing the interface in XAML, creating sections for task input, task listing, an activity log, and a chatbot/quiz panel. Logic for adding, completing, deleting tasks and parsing user input via simple NLP was implemented in MainWindow.xaml.cs. I also added an interactive quiz system using a list of predefined QuizQuestion objects. To keep the code organized, each major feature (task handling, NLP, quiz logic, activity logging) is modularized into its own method, making it easy to maintain and expand. The project demonstrates core principles of event-driven programming, data binding, and basic natural language processing within a desktop application context.

