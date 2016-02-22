namespace pluralsight_challenge.Models
{
    /// <summary>
    /// Question DTO exposed via the API
    /// </summary>
    public class QuestionModel
    {
        /// <summary>
        /// ID of the question
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Question Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Answer to the question
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Wrong answers to show with the question
        /// </summary>
        public string[] Distractors { get; set; }
    }
}