using QA.Library.Entities;

namespace QA.Library.Factories;

/// <summary>
///  StageReturn nesneleri oluşturmak için fabrika sınıfı.
/// </summary>
public static class StageReturnFactory
{
    /// <summary>
    /// Bir sonraki aşamaya geçmek için StageReturn nesnesi oluşturur.
    /// </summary>
    /// <param name="_return">Bir sonraki aşama için döndürülecek değer</param>
    /// <returns>Soru mekanizmasının durumunu yönetecek nesneyi döndürür.</returns>
    public static StageReturn CreateContinueExecution(object _return) => new(StageTasks.ContinueExec, _return);
    /// <summary>
    /// Soruyu tekrar sormak için StageReturn nesnesi oluşturur.
    /// </summary>
    /// <returns>Soru mekanizmasının durumunu yönetecek nesneyi döndürür.</returns>
    public static StageReturn CreateAskAgain() => new(StageTasks.AskAgain);
    /// <summary>
    /// Soruyu sonlandırmak için StageReturn nesnesi oluşturur.
    /// </summary>
    /// <returns>Soru mekanizmasının durumunu yönetecek nesneyi döndürür.</returns>
    public static StageReturn CreateFinishExec() => new(StageTasks.FinishExec);
}