namespace QA.Library.Entities;

public sealed record Question(string QuestionText, List<Stage> Stages);