namespace pluralsight_challenge.Repository
{
    using pluralsight_challenge.Entity;

    /// <summary>
    /// Provides access to questions
    /// </summary>
    public interface IQuestionRepository
    {
        /// <summary>
        /// Deletes a question from the repository
        /// </summary>
        /// <param name="questionId">ID of the question to delete</param>
        void Delete(int questionId);

        /// <summary>
        /// Gets all questions in the repository
        /// </summary>
        /// <returns>Array of questions</returns>
        Question[] Get();

        /// <summary>
        /// Gets a single question by id
        /// </summary>
        /// <param name="questionId">Question Id to load</param>
        /// <returns>Question or null</returns>
        Question Get(int questionId);

        /// <summary>
        /// Inserts a new question into the repository
        /// </summary>
        /// <param name="question">Question to insert</param>
        void Insert(Question question);

        /// <summary>
        /// Updates an existing question in the repository
        /// </summary>
        /// <param name="question">Question to update</param>
        void Update(Question question);
    }
}