namespace QA.Library.Entities;

public sealed record StageReturn(StageTasks Task, object? Return = null);