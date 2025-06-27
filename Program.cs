using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessChatbot
{
    class Program
    {
        static string userName;
        static Dictionary<string, string> userMemory = new Dictionary<string, string>();
        static Random random = new Random();

        // Define keywords and random responses
        static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
        {
            { "password", new List<string>
                {
                    "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                    "A password manager can help you create and store strong, random passwords for each of your accounts.",
                    "Don’t reuse passwords across different websites. It's crucial to create a new one each time!"
                }
            },
            { "scam", new List<string>
                {
                    "Be cautious when sharing personal information online, especially with unsolicited emails or phone calls.",
                    "Scammers often impersonate trusted organizations, so verify their identity before giving any details.",
                    "If an offer sounds too good to be true, it probably is. Always be suspicious of unexpected deals."
                }
            },
            { "privacy", new List<string>
                {
                    "Review your privacy settings regularly on social media to ensure you're sharing only what you're comfortable with.",
                    "Use two-factor authentication to add an extra layer of privacy and security to your accounts.",
                    "Be mindful of the personal information you share online, as it can be used to steal your identity."
                }
            },
            { "phishing", new List<string>
                {
                    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                    "Always hover over links in emails to check if they lead to a legitimate website before clicking on them.",
                    "Phishing emails often create a sense of urgency. Don't be pressured into providing information too quickly."
                }
            },
            {"interest", new List<string>()
                {
                    "As you mentioned before, you're interested in. Let's explore that further!"
                }
            },
            {"worried", new List<string>()
                {
                    "It's completely understandable to feel that way. Let me share some tips to help you stay safe."
                }
            },
            { "curious", new List<string>()
                {
                    "I'm glad you're curious! Let me give you some tips on staying safe online."
                }
            },
             { "how are you", new List<string>()
                {
                    "I am doing great and yourself.",
                    "I am fine thank you."
                }
             },
             { "great", new List<string>()
                {
                    "I am pleased to hear that",
                }
             },
             { "good", new List<string>()
                {
                    "How can i help you today?",
                }
             }
        };

        static void Main(string[] args)
        {
            PlayGreeting();
            if (OperatingSystem.IsWindows())
            {
                SoundPlayer spookyPlayer = new SoundPlayer("voice.text.wav");
                spookyPlayer.Load();
                spookyPlayer.Play();
            }

            // ASCII Art Logo
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ___________");
            Console.WriteLine(" \\ /");
            Console.WriteLine(" W E L C O M E");
            Console.WriteLine(" / \\");
            Console.WriteLine(" ___________");
            Console.WriteLine("\n");
            Console.WriteLine(" _____");
            Console.WriteLine(" | |");
            Console.WriteLine(" | T O |");
            Console.WriteLine(" | |");
            Console.WriteLine(" _____");
            Console.WriteLine(" ______________CyberSentinel______________");
            Console.WriteLine(" | Protecting Your Digital World |");
            Console.WriteLine(" |______________CyberSentinel______________|");

            // Get user's name
            Console.Write("Hello! What is your name ? ");
            Console.ForegroundColor = ConsoleColor.Green;
            userName = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = "User";
            }

            // Display welcome message
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Welcome, ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{userName}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("! I'm here to help you stay safe online.\n");

            // Start conversation loop
            StartConversation();
        }

        static void StartConversation()
        {
            bool continueChat = true;

            while (continueChat)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\nAsk me anything about cybersecurity: ");
                string userInput = Console.ReadLine()?.ToLower().Trim();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("I didn't quite catch that. Can you rephrase?");
                    continue;
                }

                // Handle specific topics based on keywords
                if (CheckKeyword(userInput, out string response))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(response);
                    continue;
                }

                // Check for sentiment in the user's input
                if (CheckSentiment(userInput, out string sentimentResponse))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(sentimentResponse);
                    continue;
                }

                // Memory recall
                if (userMemory.ContainsKey("interest"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"As you mentioned before, you're interested in {userMemory["interest"]}. Let's explore that further!");
                    continue;
                }

                // Default response if no keywords matched
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("I'm not sure I understand. Can you try rephrasing?");
            }
        }

        // Check if the user's input contains any cybersecurity-related keyword
        static bool CheckKeyword(string userInput, out string response)
        {
            response = null;

            foreach (var keyword in keywordResponses.Keys)
            {
                if (userInput.Contains(keyword))
                {
                    response = GetRandomResponse(keyword);
                    return true;
                }
            }
            return false;
        }

        // Randomly select a response from predefined list of responses
        static string GetRandomResponse(string keyword)
        {
            var responses = keywordResponses[keyword];
            return responses[random.Next(responses.Count)];
        }

        // Check the sentiment of the user's input
        static bool CheckSentiment(string userInput, out string response)
        {
            response = null;

            if (userInput.Contains("worried") || userInput.Contains("scared"))
            {
                response = "It's completely understandable to feel that way. Let me share some tips to help you stay safe.";
                return true;
            }
            if (userInput.Contains("curious"))
            {
                response = "I'm glad you're curious! Let me give you some tips on staying safe online.";
                return true;
            }

            return false;
        }

        static void PlayGreeting()
        {
            // ASCII art or greeting can be added here
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Welcome to CyberSentinel! Your personal cybersecurity assistant.");
        }
    }
}               