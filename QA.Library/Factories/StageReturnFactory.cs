using QA.Library.Entities;

namespace QA.Library.Factories;

public static class StageReturnFactory
{
    public static StageReturn CreateContinueExec(object _return) => new(StageTasks.ContinueExec, _return);
    public static StageReturn CreateAskAgain() => new(StageTasks.AskAgain);
    public static StageReturn CreateFinishExec() => new(StageTasks.FinishExec);
}