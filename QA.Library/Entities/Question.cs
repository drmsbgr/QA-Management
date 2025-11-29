namespace QA.Library.Entities;

/// <summary>
/// Kullanıcıya sorulacak soruyu ve bu soruya verilecek cevaplara göre çalışacak aşamaları tutar.
/// </summary>
/// <param name="QuestionText">Kullanıcıya gösterilecek olan soru içeriği</param>
/// <param name="Stages">Cevaplandıktan sonra çalışacak aşamalar</param>
public sealed record Question(string QuestionText, List<Stage> Stages);