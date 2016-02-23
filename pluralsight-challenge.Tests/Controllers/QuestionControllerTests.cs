using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pluralsight_challenge.Controllers.api;
using Moq;
using pluralsight_challenge.Repository;
using pluralsight_challenge.Entity;
using pluralsight_challenge.Models;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Collections.Generic;

namespace pluralsight_challenge.Tests.Controllers
{
    [TestClass]
    public class QuestionControllerTests
    {
        private QuestionController _controller;
        private Mock<IQuestionRepository> _questionRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _questionRepository = new Mock<IQuestionRepository>(MockBehavior.Strict);
            _controller = new QuestionController(_questionRepository.Object);
        }

        [TestMethod]
        public void QuestionControllerTests_get_by_id()
        {
            _questionRepository.Setup(x => x.Get(1)).Returns(new Question() { QuestionId = 1, Text = "Test", Answer = "Foo", Distractors = new string[] { "Foo", "Baz", "Bar" } });
            QuestionModel question = _controller.Get(1);
            Assert.IsNotNull(question);
            Assert.AreEqual(1, question.QuestionId);
            Assert.AreEqual("Test", question.Text);
            Assert.AreEqual("Foo", question.Answer);
            Assert.AreEqual(3, question.Distractors.Length);
            Assert.IsTrue(question.Distractors.Contains("Foo"));
            Assert.IsTrue(question.Distractors.Contains("Bar"));
            Assert.IsTrue(question.Distractors.Contains("Baz"));

            _questionRepository.Verify(x => x.Get(1), Times.Once());
        }

        [TestMethod]
        public void QuestionControllerTests_throws_exception_on_unknown_questionId()
        {
            _questionRepository.Setup(x => x.Get(1)).Returns<Question>(null);
            try
            {
                QuestionModel question = _controller.Get(1);
                Assert.Fail();
            }
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
                _questionRepository.Verify(x => x.Get(1), Times.Once());
            }
        }

        [TestMethod]
        public void QuestionControllerTests_returns_empty_when_zero_questions_found()
        {
            _questionRepository.Setup(x => x.Get()).Returns(new Question[0]);
            IEnumerable<QuestionModel> questions = _controller.Get();
            Assert.IsNotNull(questions);
            Assert.AreEqual(0, questions.Count());
            _questionRepository.Verify(x => x.Get(), Times.Once());
        }

        [TestMethod]
        public void QuestionControllerTests_returns_all_questions_found()
        {
            Question[] questions = new Question[]
            {
                new Question() { QuestionId = 1, Text = "1", Answer = "@", Distractors = new string[] { "3", "4", "%" } },
                new Question() { QuestionId = 2, Text = "3", Answer = "f", Distractors = new string[] { "w", "b", "s" } },
            };
            _questionRepository.Setup(x => x.Get()).Returns(questions);
            IEnumerable<QuestionModel> returnedQuestions = _controller.Get();
            Assert.IsNotNull(returnedQuestions);
            Assert.AreEqual(2, returnedQuestions.Count());
            foreach (Question question in questions)
            {
                Assert.IsTrue(returnedQuestions.Any(x => x.Answer == question.Answer && x.QuestionId == question.QuestionId && x.Text == question.Text));
            }
            _questionRepository.Verify(x => x.Get(), Times.Once());
        }

        [TestMethod]
        public void QuestionControllerTests_post_valid_question()
        {
            _questionRepository.Setup(x => x.Insert(It.IsAny<Question>())).Callback<Question>(x => x.QuestionId = 6);
            QuestionModel question = _controller.Post(new QuestionModel() { QuestionId = 4, Text = "Test", Answer = "1244", Distractors = new string[] { "1", "3", "4" } });
            Assert.IsNotNull(question);
            Assert.AreEqual(6, question.QuestionId); // Make sure we ignored the passed in question id
            Assert.AreEqual("Test", question.Text);
            Assert.AreEqual("1244", question.Answer);
            Assert.AreEqual(3, question.Distractors.Length);
            _questionRepository.Verify(x => x.Insert(It.Is<Question>(q => q.Answer == "1244" && q.Text == "Test")), Times.Once());
        }

        [TestMethod]
        public void QuestionControllerTests_throws_exception_on_invalid_post()
        {
            try
            {
                QuestionModel question = _controller.Post(new QuestionModel() { QuestionId = 4, Text = "", Answer = "", Distractors = new string[] { } });
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                _questionRepository.Verify(x => x.Insert(It.IsAny<Question>()), Times.Never());
            }
        }

        [TestMethod]
        public void QuestionControllerTests_put_valid_question()
        {
            _questionRepository.Setup(x => x.Get(1)).Returns(new Question() { QuestionId = 1, Text = "Test", Answer = "Foo", Distractors = new string[] { "Foo", "Baz", "Bar" } });
            _questionRepository.Setup(x => x.Update(It.IsAny<Question>()));
            QuestionModel question = _controller.Put(1, new QuestionModel() { QuestionId = 4, Text = "Test", Answer = "1244", Distractors = new string[] { "1", "3", "4" } });
            Assert.IsNotNull(question);
            Assert.AreEqual(1, question.QuestionId); // Make sure we ignored the passed in question id
            Assert.AreEqual("Test", question.Text);
            Assert.AreEqual("1244", question.Answer);
            Assert.AreEqual(3, question.Distractors.Length);
            _questionRepository.Verify(x => x.Update(It.Is<Question>(q => q.QuestionId == 1 && q.Answer == "1244" && q.Text == "Test")), Times.Once());
            _questionRepository.Verify(x => x.Get(1), Times.Once());
        }

        [TestMethod]
        public void QuestionControllerTests_throws_exception_on_invalid_question_id_put()
        {
            _questionRepository.Setup(x => x.Update(It.IsAny<Question>()));
            _questionRepository.Setup(x => x.Get(1)).Returns<Question>(null);
            try
            {
                QuestionModel question = _controller.Put(1, new QuestionModel() { QuestionId = 4, Text = "", Answer = "", Distractors = new string[] { } });
                Assert.Fail();
            }
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
                _questionRepository.Verify(x => x.Update(It.IsAny<Question>()), Times.Never());
                _questionRepository.Verify(x => x.Get(1), Times.Once());
            }
        }

        [TestMethod]
        public void QuestionControllerTests_throws_exception_on_invalid_data_put()
        {
            _questionRepository.Setup(x => x.Update(It.IsAny<Question>()));
            _questionRepository.Setup(x => x.Get(1)).Returns(new Question());
            try
            {
                QuestionModel question = _controller.Put(1, new QuestionModel() { QuestionId = 4, Text = "", Answer = "", Distractors = new string[] { } });
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                _questionRepository.Verify(x => x.Update(It.IsAny<Question>()), Times.Never());
                _questionRepository.Verify(x => x.Get(1), Times.Once());
            }
        }
    }
}
