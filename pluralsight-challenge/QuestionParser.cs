namespace pluralsight_challenge
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using pluralsight_challenge.Entity;

    /// <summary>
    /// Class for reading the stored line and parsing them into question objects
    /// </summary>
    public static class QuestionParser
    {
        /// <summary>
        /// Parts the lines into questions
        /// </summary>
        /// <param name="lines">Lines to parse</param>
        /// <returns>Questions from the lines</returns>
        public static Question[] Parse(string[] lines)
        {
            List<Question> questions = new List<Question>();
            foreach (string line in lines.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                string[] parts = line.Split('|');
                if (parts.Length != 3)
                {
                    throw new Exception(string.Format("Invalid line: {0}", line));
                }

                if (string.IsNullOrWhiteSpace(parts[0]))
                {
                    throw new Exception(string.Format("Missing question text: {0}", line));
                }

                if (string.IsNullOrWhiteSpace(parts[1]))
                {
                    throw new Exception(string.Format("Missing Answer: {0}", line));
                }

                string[] distractors = parts[2].Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToArray();
                if (distractors.Length == 0)
                {
                    throw new Exception(string.Format("Missing distractors: {0}", line));
                }

                questions.Add(new Question()
                {
                    Text = parts[0].Trim(),
                    Answer = parts[1].Trim(),
                    Distractors = distractors,
                });
            }

            return questions.ToArray();
        }
    }
}