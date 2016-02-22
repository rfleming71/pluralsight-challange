namespace pluralsight_challenge.App_Start
{
    using Entity;
    using Repository;
    using StructureMap;
    using System.Linq;

    /// <summary>
    /// Dependency registry
    /// </summary>
    public class DependencyRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyRegistry" /> class.
        /// </summary>
        public DependencyRegistry()
        {
            string[] questionLines = Resource1.code_challenge_question_dump.Split('\n').Skip(1).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            Question[] questionsToLoad = QuestionParser.Parse(questionLines);
            InMemoryQuestionRepository questionRepository = new InMemoryQuestionRepository();
            foreach (Question question in questionsToLoad)
            {
                questionRepository.Insert(question);
            }

            For<IQuestionRepository>().Singleton().Use(questionRepository);
        }
    }
}