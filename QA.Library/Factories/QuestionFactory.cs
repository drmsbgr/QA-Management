using QA.Library.Entities;

namespace QA.Library.Factories;

/// <summary>
/// Soru nesneleri oluşturmak için fabrika sınıfı.
/// </summary>
public static class QuestionFactory
{
    /// <summary>
    /// Hiç aşaması olmayan yeni bir soru oluşturur.
    /// </summary>
    /// <param name="questionText">Soru içeriği</param>
    /// <returns></returns>
    public static Question Create(string questionText) => new(questionText, []);
}