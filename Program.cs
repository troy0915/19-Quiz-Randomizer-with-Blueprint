using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Question
{
    public string Topic { get; }
    public string Difficulty { get; }
    public string Text { get; }

    public Question(string topic, string difficulty, string text)
    {
        Topic = topic;
        Difficulty = difficulty;
        Text = text;
    }

    public override string ToString()
    {
        return $"[{Topic} | {Difficulty}] {Text}";
    }
}

public class QuizBuilder
{
    private List<Question> _questionBank;
    private Random _random;

    public QuizBuilder(List<Question> questionBank, int seed)
    {
        _questionBank = questionBank;
        _random = new Random(seed); 
    }

    public List<Question> BuildQuiz(Dictionary<(string Topic, string Difficulty), int> blueprint)
    {
        var selectedQuestions = new List<Question>();

        foreach (var req in blueprint)
        {
            string topic = req.Key.Topic;
            string difficulty = req.Key.Difficulty;
            int count = req.Value;

            var available = _questionBank
                .Where(q => q.Topic.Equals(topic, StringComparison.OrdinalIgnoreCase) &&
                            q.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (available.Count == 0)
            {
                Console.WriteLine($"No questions available for {topic}-{difficulty}, skipping...");
                continue;
            }

            if (available.Count < count)
            {
                Console.WriteLine($"Only {available.Count} questions available for {topic}-{difficulty}, using all.");
                count = available.Count;
            }

            var chosen = available
                .OrderBy(q => _random.Next())
                .Take(count)
                .ToList();

            selectedQuestions.AddRange(chosen);
        }

        return selectedQuestions;
    }
}

namespace _19__Quiz_Randomizer_with_Blueprint
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var questionBank = new List<Question>
        {
            new Question("Math", "Easy", "What is 2 + 2?"),
            new Question("Math", "Medium", "What is the square root of 144?"),
            new Question("Math", "Hard", "Prove Fermat's Last Theorem."),
            new Question("Science", "Easy", "What planet is known as the Red Planet?"),
            new Question("Science", "Medium", "What is the chemical symbol for gold?"),
            new Question("History", "Easy", "Who was the first president of the USA?"),
            new Question("History", "Medium", "In what year did WW2 end?"),
            new Question("History", "Hard", "Explain the causes of the Cold War.")
        };

            var blueprint = new Dictionary<(string Topic, string Difficulty), int>
        {
            { ("Math", "Easy"), 1 },
            { ("Math", "Hard"), 1 },
            { ("Science", "Medium"), 1 },
            { ("History", "Easy"), 1 },
            { ("History", "Hard"), 1 }
        };

            int seed = 12345; 
            var builder = new QuizBuilder(questionBank, seed);

            var quiz = builder.BuildQuiz(blueprint);

            Console.WriteLine("\nGenerated Quiz:");
            foreach (var q in quiz)
                Console.WriteLine(q);
        }
    }
}




