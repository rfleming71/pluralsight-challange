namespace pluralsight_challenge.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using pluralsight_challenge.Entity;

    /// <summary>
    /// Provides access to questions
    /// </summary>
    public class InMemoryQuestionRepository : IQuestionRepository
    {
        private object _lock = new object();
        private int _nextId = 1;
        private Dictionary<int, Question> _questions = new Dictionary<int, Question>();

        /// <summary>
        /// Inserts a new question into the repository
        /// </summary>
        /// <param name="question">Question to insert</param>
        public void Insert(Question question)
        {
            Question insertedQuestion = Clone(question);
            lock (_lock)
            {
                insertedQuestion.QuestionId = _nextId++;
                _questions[insertedQuestion.QuestionId] = insertedQuestion;
            }
            question.QuestionId = insertedQuestion.QuestionId;
        }

        /// <summary>
        /// Updates an existing question in the repository
        /// </summary>
        /// <param name="question">Question to update</param>
        public void Update(Question question)
        {
            lock (_lock)
            {
                if (!_questions.ContainsKey(question.QuestionId))
                {
                    throw new Exception(string.Format("Unknown question id {0}", question.QuestionId));
                }

                _questions[question.QuestionId] = Clone(question);
            }
        }

        /// <summary>
        /// Deletes a question from the repository
        /// </summary>
        /// <param name="questionId">ID of the question to delete</param>
        public void Delete(int questionId)
        {
            lock (_lock)
            {
                if (_questions.ContainsKey(questionId))
                {
                    _questions.Remove(questionId);
                }
            }
        }

        /// <summary>
        /// Gets a single question by id
        /// </summary>
        /// <param name="questionId">Question Id to load</param>
        /// <returns>Question or null</returns>
        public Question Get(int questionId)
        {
            lock (_lock)
            {
                if (!_questions.ContainsKey(questionId))
                {
                    return null;
                }

                return Clone(_questions[questionId]);
            }
        }

        /// <summary>
        /// Gets all questions in the repository
        /// </summary>
        /// <returns>Array of questions</returns>
        public Question[] Get()
        {
            lock (_lock)
            {
                return _questions.Values.Select(Clone).ToArray();
            }
        }

        /// <summary>
        /// Helper method for cloning question objects
        /// </summary>
        /// <param name="question">Question to clone</param>
        /// <returns>Cloned question</returns>
        private Question Clone(Question question)
        {
            return new Question()
            {
                QuestionId = question.QuestionId,
                Text = question.Text,
                Answer = question.Answer,
                Distractors = question.Distractors.ToArray(),
            };
        }
    }
}