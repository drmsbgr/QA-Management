using QA.Library.Entities;

namespace QA.Library.Factories;

/// <summary>
/// Aşama nesneleri oluşturmak için fabrika sınıfı.
/// </summary>
public static class StageFactory
{
    /// <summary>
    /// Yeni bir aşama oluşturur.
    /// </summary>
    /// <typeparam name="T">Geri dönüş değeri tipi</typeparam>
    /// <param name="stageFunc">Aşama fonksiyonu</param>
    /// <returns>Oluşturulan aşama nesnesini döner.</returns>
    public static Stage Create<T>(Func<T, StageReturn> stageFunc) => new(r => stageFunc((T)r));
}