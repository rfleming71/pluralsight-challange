namespace pluralsight_challenge.Controllers.api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using pluralsight_challenge.Entity;
    using pluralsight_challenge.Models;
    using pluralsight_challenge.Repository;

    /// <summary>
    /// Provides the API for Questions
    /// </summary>
    public class QuestionController : ApiController
    {
        private IQuestionRepository _questionRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="questionRepository">Instance of the question repository</param>
        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        
        /// <summary>
        /// Gets all questions in the system
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionModel> Get()
        {
            return _questionRepository.Get().Select(MapQuestionToModel);
        }

        /// <summary>
        /// Gets a given question by id
        /// </summary>
        /// <param name="id">Question id to fetch</param>
        /// <returns></returns>
        public QuestionModel Get(int id)
        {
            Question question = _questionRepository.Get(id);
            if (question == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return MapQuestionToModel(question);
        }

        /// <summary>
        /// Creates a new question
        /// </summary>
        /// <param name="value">Question information</param>
        public QuestionModel Post([FromBody]QuestionModel value)
        {
            Question newQuestion = MapModelToQuestion(value);
            ValidateQuestion(newQuestion);
            _questionRepository.Insert(newQuestion);
            return MapQuestionToModel(newQuestion);
        }

        /// <summary>
        /// Updates an existing question
        /// </summary>
        /// <param name="id">Question id to update</param>
        /// <param name="value">Question information</param>
        public QuestionModel Put(int id, [FromBody]QuestionModel value)
        {
            Question existingQuestion = _questionRepository.Get(id);
            if (existingQuestion == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            existingQuestion = MapModelToQuestion(value);
            existingQuestion.QuestionId = id;
            ValidateQuestion(existingQuestion);
            _questionRepository.Update(existingQuestion);

            return MapQuestionToModel(existingQuestion);
        }

        /// <summary>
        /// Deletes a given question by id
        /// </summary>
        /// <param name="id">Question ID to delete</param>
        /* Not required by the project
        public void Delete(int id)
        {
            Question existingQuestion = _questionRepository.Get(id);
            if (existingQuestion != null)
            {
                _questionRepository.Delete(id);
            }
            else
            {
                // ToDo: Log deleting unknown question id 
            }
        }*/

        private QuestionModel MapQuestionToModel(Question question)
        {
            return new QuestionModel()
            {
                QuestionId = question.QuestionId,
                Text = question.Text,
                Answer = question.Answer,
                Distractors = question.Distractors,
            };
        }

        private Question MapModelToQuestion(QuestionModel model)
        {
            return new Question()
            {
                Text = model.Text,
                Answer = model.Answer,
                Distractors = model.Distractors,
            };
        }

        private void ValidateQuestion(Question question)
        {
            if (string.IsNullOrWhiteSpace(question.Text))
            {
                throw new ArgumentException("Question text must not be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(question.Answer))
            {
                throw new ArgumentException("Answer must not be null or whitespace");
            }

            if (question.Distractors == null || question.Distractors.Length == 0)
            {
                throw new ArgumentException("A minimum of 1 distractor is required");
            }
        }
    }
}
