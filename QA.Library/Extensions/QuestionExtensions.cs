using QA.Library.Entities;
using QA.Library.Factories;

namespace QA.Library.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Question"/> class.
/// </summary>
public static class QuestionExtensions
{
    extension(Question question)
    {
        /// <summary>
        /// Şart fonksiyonun sonucuna göre birer değer dönecek olan fonksiyonları çalıştırır.
        /// </summary>
        /// <typeparam name="T">Bu aşamaya gelen girdinin tipi</typeparam>
        /// <param name="conditionFunc">Şart fonksiyonu</param>
        /// <param name="thenReturn">Şart sağlanırsa değer dönen fonksiyon</param>
        /// <param name="otherwiseReturn">şart sağlanmazsa değer dönen fonksiyon</param>
        /// <returns>Mofidifiye edilmiş soru yapısını döner.</returns>
        public Question AddConditionalReturnStage<T>(Func<T, bool> conditionFunc, Func<T, object> thenReturn, Func<T, object> otherwiseReturn)
        {
            return question.AddOrdinaryStage<T>(r =>
            {
                if (conditionFunc(r))
                    return StageReturnFactory.CreateContinueExecution(thenReturn(r));
                else
                    return StageReturnFactory.CreateContinueExecution(otherwiseReturn(r));
            });
        }

        /// <summary>
        /// Şartlar sağlanırsa fonksiyon çalışır ve değer döner aksi durumda diğer aksiyon çalışır.
        /// </summary>
        /// <typeparam name="T">Bu aşamaya gelen girdinin tipi</typeparam>
        /// <param name="conditionFunc">Şart fonksiyonu</param>
        /// <param name="thenReturn">Şartlar sağlanırsa çalıştırılacak ve bir değer dönecek olan fonksiyon</param>
        /// <param name="otherwise">Şartlar sağlanmazsa çalıştırılacak aksiyon</param>
        /// <returns>Mofidifiye edilmiş soru yapısını döner.</returns>
        public Question AddConditionalReturnOrExecStage<T>(Func<T, bool> conditionFunc, Func<T, object>? thenReturn = null, Action<T>? otherwise = null)
        {
            return question.AddOrdinaryStage<T>(r =>
            {
                if (conditionFunc(r))
                {
                    if (thenReturn is not null)
                        return StageReturnFactory.CreateContinueExecution(thenReturn(r));
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

        /// <summary>
        /// Şartlar sağlanırsa bir aksiyon çalışır aksi durumda diğer fonksiyon çalışır ve bir değer döner.
        /// </summary>
        /// <typeparam name="T">Bu aşamaya gelen girdinin tipi</typeparam>
        /// <param name="conditionFunc">Şart fonksiyonu</param>
        /// <param name="then">Şartlar sağlanırsa çalıştırılacak aksiyon</param>
        /// <param name="otherwiseReturn">Şartlar sağlanmazsa çalıştırılacak ve bir değer dönecek olan fonksiyon</param>
        /// <returns>Mofidifiye edilmiş soru yapısını döner.</returns>
        public Question AddConditionalExecOrReturnStage<T>(Func<T, bool> conditionFunc, Action<T>? then = null, Func<T, object>? otherwiseReturn = null)
        {
            return question.AddOrdinaryStage<T>(r =>
            {
                if (conditionFunc(r))
                {
                    then?.Invoke(r);
                    return StageReturnFactory.CreateFinishExec();
                }
                else
                {
                    if (otherwiseReturn is not null)
                        return StageReturnFactory.CreateContinueExecution(otherwiseReturn(r));
                    else
                        return StageReturnFactory.CreateAskAgain();
                }
            });
        }

        /// <summary>
        /// Şart fonksiyonun sonucuna göre verilen mesajları yazar.
        /// </summary>
        /// <typeparam name="T">Bu aşamaya gelen girdinin tipi</typeparam>
        /// <param name="conditionFunc">Şart fonksiyonu</param>
        /// <param name="then">Şartlar sağlanırsa yazılacak mesaj</param>
        /// <param name="otherwiseReturn">Şartlar sağlanmazsa yazılacak mesaj</param>
        /// <returns>Mofidifiye edilmiş soru yapısını döner.</returns>
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

        /// <summary>
        /// Şartlara göre eylemleri çalıştır ve değer dönebilir.
        /// </summary>
        /// <typeparam name="T">Girdi olarak kullanılacak tip</typeparam>
        /// <param name="conditionFunc">Şart fonksiyonu</param>
        /// <param name="then">Şartlar sağlanırsa çalıştırılacak eylem</param>
        /// <param name="otherwise">Şartlar sağlanmazsa çalıştırılacak eylem</param>
        /// <param name="thenReturn">Şartlar sağlanırsa dönülecek değer</param>
        /// <returns>Modifiye edilmiş soru</returns>
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
                        return StageReturnFactory.CreateContinueExecution(thenReturn(r));
                }
                else
                {
                    otherwise?.Invoke(r);
                    return StageReturnFactory.CreateAskAgain();
                }
            }));

            return question;
        }

        /// <summary>
        /// Verilen eylemi çalıştırır ve soru mekanizmasını sonlandırır.
        /// </summary>
        /// <typeparam name="T">Bu aşamaya gelen girdinin tipi</typeparam>
        /// <param name="action">Bu aşamada çalıştırılacak olan aksiyon</param>
        /// <returns>Mofidifiye edilmiş soru yapısını döner.</returns>
        public Question AddExecAndFinishStage<T>(Action<T> action)
        {
            return question.AddOrdinaryStage<T>(r =>
            {
                action(r);
                return StageReturnFactory.CreateFinishExec();
            });
        }

        /// <summary>
        /// Verilen değer dönen eylemi çalıştırır ve bir sonraki aşamaya geçmeye izin verir.
        /// </summary>
        /// <typeparam name="T">Girdi olarak kullanılacak tip</typeparam>
        /// <param name="action">Çalıştırılacak aksiyon</param>
        /// <returns>Modifiye edilmiş soru</returns>
        public Question AddExecAndContinueStage<T>(Func<T, object> action)
        {
            return question.AddOrdinaryStage<T>(r =>
            {
                return StageReturnFactory.CreateContinueExecution(action(r));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stageFunc"></param>
        /// <returns></returns>
        public Question AddOrdinaryStage<T>(Func<T, StageReturn> stageFunc)
        {
            question.Stages.Add(StageFactory.Create(stageFunc));
            return question;
        }

        /// <summary>
        /// Soruyu ekrana yazdırır.
        /// </summary>
        private void AskQuestion()
        {
            Console.WriteLine(question.QuestionText);
            Console.Write("->");
        }

        /// <summary>
        /// Soru yapısını çalıştırır.
        /// </summary>
        public void Execute()
        {
            _ = Execute<object>(question);
        }

        /// <summary>
        /// Soru yapısını çalıştırır.
        /// </summary>
        /// <typeparam name="T">En son çalışmış olan değer döndüren eylemin döndürdüğü tip</typeparam>
        /// <returns>Eylemlerin son döndüğü değeri döndürür.</returns>
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