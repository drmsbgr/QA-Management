using QA.Library.Entities;
using QA.Library.Factories;

namespace QA.Library.Extensions;

public static class QuestionExtensions
{
    extension(Question question)
    {
        public Question AddConditionalReturnStage<T>(Func<T, bool> conditionFunc, Func<T, object> thenReturn, Func<T, object> otherwiseReturn)
        {
            return question.AddStage<T>(r =>
            {
                if (conditionFunc(r))
                    return StageReturnFactory.CreateContinueExec(thenReturn(r));
                else
                    return StageReturnFactory.CreateContinueExec(otherwiseReturn(r));
            });
        }

        public Question AddConditionStage<T>(Func<T, bool> conditionFunc, Func<T, object>? thenReturn = null, Action<T>? otherwise = null)
        {
            return question.AddStage<T>(r =>
            {
                if (conditionFunc(r))
                {
                    if (thenReturn is not null)
                        return StageReturnFactory.CreateContinueExec(thenReturn(r));
                    else
                        return StageReturnFactory.CreateFinishExec();
                }
                else
                {
                    otherwise?.Invoke(r);
                    return StageReturnFactory.CreateAskAgain();
                }
            });
        }

        public Question AddConditionalResponseStage<T>(Func<T, bool> conditionFunc, string then, string otherwise)
        {
            question.Stages.Add(StageFactory.Create<T>(r =>
            {
                if (conditionFunc(r))
                {
                    Console.WriteLine(then);
                    return StageReturnFactory.CreateFinishExec();
                }
                else
                {
                    Console.WriteLine(otherwise);
                    return StageReturnFactory.CreateAskAgain();
                }
            }));
            return question;
        }

        public Question AddFinishExecIfTrue<T>(Func<T, bool> conditionFunc, Action<T>? then = null, Func<T, object>? otherwiseReturn = null)
        {
            question.Stages.Add(StageFactory.Create<T>(r =>
            {
                if (conditionFunc(r))
                {
                    then?.Invoke(r);
                    return StageReturnFactory.CreateFinishExec();
                }
                else
                {
                    if (otherwiseReturn is not null)
                        return StageReturnFactory.CreateContinueExec(otherwiseReturn(r));
                    else
                        return StageReturnFactory.CreateAskAgain();
                }
            }));

            return question;
        }

        public Question AddConditionalActionStage<T>(Func<T, bool> conditionFunc, Action<T>? then = null, Action<T>? otherwise = null, Func<T, object>? thenReturn = null)
        {
            question.Stages.Add(StageFactory.Create<T>(r =>
            {
                if (conditionFunc(r))
                {
                    then?.Invoke(r);
                    if (thenReturn is null)
                        return StageReturnFactory.CreateFinishExec();
                    else
                        return StageReturnFactory.CreateContinueExec(thenReturn(r));
                }
                else
                {
                    otherwise?.Invoke(r);
                    return StageReturnFactory.CreateAskAgain();
                }
            }));

            return question;
        }

        public Question AddExecAndFinishStage<T>(Action<T> action)
        {
            return question.AddStage<T>(r =>
            {
                action(r);
                return StageReturnFactory.CreateFinishExec();
            });
        }

        public Question AddStage<T>(Func<T, StageReturn> stageFunc)
        {
            question.Stages.Add(StageFactory.Create(stageFunc));
            return question;
        }

        private void AskQuestion()
        {
            Console.WriteLine(question.QuestionText);
            Console.Write("->");
        }

        public void Execute()
        {
            _ = Execute<object>(question);
        }

        public T Execute<T>()
        {
            question.AskQuestion();

            object input = Console.ReadLine()!;

            foreach (var stage in question.Stages)
            {
                var returns = stage.StageFunc(input!);
                switch (returns.Task)
                {
                    case StageTasks.ContinueExec:
                        input = returns.Return!;
                        break;
                    case StageTasks.AskAgain:
                        Console.WriteLine("Tekrar denemek için bir tuşa basın...");
                        Console.ReadKey();
                        Console.Clear();
                        return Execute<T>(question);
                    case StageTasks.FinishExec:
                        return (T)input;
                }
            }

            return (T)input;
        }
    }
}