using QA.Library.Entities;

namespace QA.Library.Factories;

public static class StageFactory
{
    public static Stage Create<T>(Func<T, StageReturn> stageFunc) => new(r => stageFunc((T)r));
}