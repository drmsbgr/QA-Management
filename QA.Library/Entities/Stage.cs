namespace QA.Library.Entities;

public sealed record Stage(Func<object, StageReturn> StageFunc);
