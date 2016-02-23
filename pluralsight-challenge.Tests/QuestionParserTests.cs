namespace pluralsight_challenge.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using pluralsight_challenge.Entity;

    [TestClass]
    public class QuestionParserTests
    {
        [TestMethod]
        public void Test_single_line()
        {
            string[] lines = new string[]
            {
                "What is 1754 - 3936?|-2182|3176, 6529, 6903"
            };

            Question[] questions = QuestionParser.Parse(lines);
            Assert.AreEqual(1, questions.Length);
            Assert.AreEqual("What is 1754 - 3936?", questions[0].Text);
            Assert.AreEqual("-2182", questions[0].Answer);
            Assert.AreEqual(3, questions[0].Distractors.Length);
            Assert.AreEqual("3176", questions[0].Distractors[0]);
            Assert.AreEqual("6529", questions[0].Distractors[1]);
            Assert.AreEqual("6903", questions[0].Distractors[2]);
        }

        [TestMethod]
        public void Test_multiple_lines()
        {
            string[] lines = new string[]
            {
                "What is 1754 - 3936?|-2182|3176, 6529, 6903",
                "What is 7269 * 2771?|20142399|874",
            };

            Question[] questions = QuestionParser.Parse(lines);
            Assert.AreEqual(2, questions.Length);
            Assert.AreEqual("What is 7269 * 2771?", questions[1].Text);
            Assert.AreEqual("20142399", questions[1].Answer);
            Assert.AreEqual(1, questions[1].Distractors.Length);
            Assert.AreEqual("874", questions[1].Distractors[0]);
        }

        [TestMethod]
        public void Test_blank_question_throws_exception()
        {
            string[] lines = new string[]
            {
                "|20142399|874",
            };

            try
            {
                Question[] questions = QuestionParser.Parse(lines);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Missing question text: |20142399|874", e.Message);
            }
        }

        [TestMethod]
        public void Test_blank_answer_throws_exception()
        {
            string[] lines = new string[]
            {
                "What is 7269 * 2771?||874",
            };

            try
            {
                Question[] questions = QuestionParser.Parse(lines);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Missing Answer: What is 7269 * 2771?||874", e.Message);
            }
        }

        [TestMethod]
        public void Test_blank_distractors_throws_exception()
        {
            string[] lines = new string[]
            {
                "What is 7269 * 2771?|20142399|",
            };

            try
            {
                Question[] questions = QuestionParser.Parse(lines);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Missing distractors: What is 7269 * 2771?|20142399|", e.Message);
            }
        }

        [TestMethod]
        public void Test_too_many_tabs_question_throws_exception()
        {
            string[] lines = new string[]
            {
                "What is 7269 * 2771?|20142399|874|asdsd",
            };

            try
            {
                Question[] questions = QuestionParser.Parse(lines);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Invalid line: What is 7269 * 2771?|20142399|874|asdsd", e.Message);
            }
        }

        [TestMethod]
        public void Test_too_few_tabs_question_throws_exception()
        {
            string[] lines = new string[]
            {
                "What is 7269 * 2771?|20142399",
            };

            try
            {
                Question[] questions = QuestionParser.Parse(lines);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Invalid line: What is 7269 * 2771?|20142399", e.Message);
            }
        }

        [TestMethod]
        public void Test_parse_sample_file()
        {
            string[] lines = Resource1.code_challenge_question_dump.Split('\n').Skip(1).Select(x => x.Trim()).ToArray();
            Question[] questions = QuestionParser.Parse(lines);
            Assert.AreEqual(4000, questions.Length);
        }
    }
}
