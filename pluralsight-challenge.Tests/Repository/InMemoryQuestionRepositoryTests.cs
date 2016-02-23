namespace pluralsight_challenge.Tests.Repository
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using pluralsight_challenge.Entity;
    using pluralsight_challenge.Repository;

    [TestClass]
    public class InMemoryQuestionRepositoryTests
    {
        private InMemoryQuestionRepository _repo;

        [TestInitialize]
        public void TestInitialize()
        {
            _repo = new InMemoryQuestionRepository();
        }

        [TestMethod]
        public void InMemoryQuestionRepository_Can_insert_and_id_gets_set()
        {
            Question testQuestion = new Question();
            _repo.Insert(testQuestion);
            Assert.AreEqual(1, testQuestion.QuestionId);
            _repo.Insert(testQuestion);
            Assert.AreEqual(2, testQuestion.QuestionId);
        }

        [TestMethod]
        public void InMemoryQuestionRepository_Can_insert_and_get_by_id()
        {
            Question testQuestion = new Question()
            {
                Text = "Test Question",
                Answer = "Answer",
                Distractors = new string[] { "1", "2", "3" }
            };
            _repo.Insert(testQuestion);

            Question fetchedQuestion = _repo.Get(testQuestion.QuestionId);
            Assert.IsNotNull(fetchedQuestion);
            Assert.AreEqual(testQuestion.Text, fetchedQuestion.Text);
            Assert.AreEqual(testQuestion.Answer, fetchedQuestion.Answer);
            Assert.AreEqual(testQuestion.Distractors.Length, fetchedQuestion.Distractors.Length);
            Assert.AreEqual(testQuestion.Distractors[0], fetchedQuestion.Distractors[0]);
            Assert.AreEqual(testQuestion.Distractors[1], fetchedQuestion.Distractors[1]);
            Assert.AreEqual(testQuestion.Distractors[2], fetchedQuestion.Distractors[2]);
        }

        [TestMethod]
        public void InMemoryQuestionRepository_returns_new_objects()
        {
            Question testQuestion = new Question()
            {
                Text = "Test Question",
                Answer = "Answer",
                Distractors = new string[] { "1", "2", "3" }
            };
            _repo.Insert(testQuestion);

            Question fetchedQuestion = _repo.Get(testQuestion.QuestionId);
            fetchedQuestion.Text = "Blah";
            fetchedQuestion.Answer = "Blah";
            fetchedQuestion.Distractors = new string[] { "5", "6" };

            fetchedQuestion = _repo.Get(testQuestion.QuestionId);
            Assert.IsNotNull(fetchedQuestion);
            Assert.AreEqual(testQuestion.Text, fetchedQuestion.Text);
            Assert.AreEqual(testQuestion.Answer, fetchedQuestion.Answer);
            Assert.AreEqual(testQuestion.Distractors.Length, fetchedQuestion.Distractors.Length);
            Assert.AreEqual(testQuestion.Distractors[0], fetchedQuestion.Distractors[0]);
            Assert.AreEqual(testQuestion.Distractors[1], fetchedQuestion.Distractors[1]);
            Assert.AreEqual(testQuestion.Distractors[2], fetchedQuestion.Distractors[2]);
        }
        [TestMethod]
        public void InMemoryQuestionRepository_can_update()
        {

            Question testQuestion = new Question()
            {
                Text = "Test Question",
                Answer = "Answer",
                Distractors = new string[] { "1", "2", "3" }
            };
            _repo.Insert(testQuestion);

            testQuestion.Text += "1";
            testQuestion.Answer += "1";
            testQuestion.Distractors = new string[] { "1", "3", "$" };
            _repo.Update(testQuestion);

            Question fetchedQuestion = _repo.Get(testQuestion.QuestionId);
            Assert.IsNotNull(fetchedQuestion);
            Assert.AreEqual(testQuestion.Text, fetchedQuestion.Text);
            Assert.AreEqual(testQuestion.Answer, fetchedQuestion.Answer);
            Assert.AreEqual(testQuestion.Distractors.Length, fetchedQuestion.Distractors.Length);
            Assert.AreEqual(testQuestion.Distractors[0], fetchedQuestion.Distractors[0]);
            Assert.AreEqual(testQuestion.Distractors[1], fetchedQuestion.Distractors[1]);
            Assert.AreEqual(testQuestion.Distractors[2], fetchedQuestion.Distractors[2]);
        }

        [TestMethod]
        public void InMemoryQuestionRepository_returns_null_on_unknown_entity()
        {
            Question fetchedQuestion = _repo.Get(-1);
            Assert.IsNull(fetchedQuestion);
        }

        [TestMethod]
        public void InMemoryQuestionRepository_update_throws_exception_on_unknown_entity()
        {
            try
            {
                _repo.Update(new Question() { QuestionId = -1 });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Unknown question id -1", ex.Message);
            }
        }
    }
}
