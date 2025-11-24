using QA.Library.Entities;

namespace QA.Library.Factories;

public static class QuestionFactory
{
    public static Question Create(string questionText) => new(questionText, []);
}