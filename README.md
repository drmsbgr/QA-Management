# QA-Management

**Geliştirici:** Buğra DURMUŞ

`QA-Management`, .NET konsol uygulamaları için akıcı (fluent) bir arayüz kullanarak etkileşimli soru-cevap akışları oluşturmayı sağlayan basit ve hafif bir kütüphanedir. Kullanıcıdan girdi almayı, bu girdiyi doğrulamayı, şarta bağlı adımlar çalıştırmayı ve adımlar arasında veri aktarmayı kolaylaştırır.

## ✨ Özellikler

*   **Akıcı Arayüz (Fluent API):** Zincirleme metotlarla kolayca karmaşık soru akışları oluşturun.
*   **Aşamalı Yapı (Staging):** Soruları, her biri kendi mantığına sahip olan birden çok aşamaya bölün.
*   **Şartlı Mantık:** Kullanıcının girdisine göre farklı aksiyonlar veya yanıtlar tetikleyin.
*   **Girdi Doğrulama:** Geçersiz girdi durumunda soruyu otomatik olarak yeniden sorun.
*   **Aşamalar Arası Veri Aktarımı:** Bir aşamanın çıktısını bir sonraki aşamaya girdi olarak kullanın.
*   **Genişletilebilir:** Kendi özel aşama mantıklarınızı kolayca ekleyin.

## 🚀 Kullanım

Kütüphaneyi kullanmak oldukça basittir. `QuestionFactory` ile bir soru oluşturup, `Add...` uzantı metotları ile aşamalar ekleyerek akışı tasarlayabilirsiniz.

### Örnek 1: Basit Doğru/Yanlış Cevap

Kullanıcının cevabına göre "Doğru" veya "Yanlış" yazdıran temel bir soru.

```csharp
using QA.Library.Factories;
using QA.Library.Extensions;

QuestionFactory
.Create("2+2=?")
.AddConditionalResponseStage<string>(
    conditionFunc: r => r == "4",
    then: "Doğru cevap!",
    otherwise: "Yanlış cevap!")
.Execute();
```

### Örnek 2: Girdiyi Doğrulama ve Aksiyon Alma

Bu örnekte kullanıcıdan bir sayı girmesi istenir. Girdi bir sayı değilse uyarı verilir. Geçerli bir sayı girilirse, bu sayı bir sonraki aşamaya aktarılır ve girilen sayı kadar ekrana "elma" yazdırılır.

```csharp
QuestionFactory.Create("Kaç adet 'elma' yazalım?")
.AddConditionalActionStage<string>(
    conditionFunc: r => int.TryParse(r, out var _),
    then: default,
    otherwise: (_) => Console.WriteLine("Bir sayı girmen bekleniyordu!"),
    thenReturn: (r) => int.Parse(r))
.AddConditionalActionStage<int>(
    conditionFunc: num => num > 0,
    then: (num) =>
    {
        for (int i = 0; i < num; i++)
            Console.WriteLine("elma");
    },
    otherwise: (_) => Console.WriteLine("Pozitif bir sayı girmen bekleniyordu!")
)
.Execute();
```

### Örnek 3: İç İçe Sorular

Birinci sorudan alınan cevap, ikinci sorunun içinde kullanılabilir. Bu örnek, iki sayıyı ayrı ayrı sorarak toplar ve sonucu ekrana yazar.

```csharp
QuestionFactory
.Create("İlk sayıyı girin")
.AddConditionStage<string>(
    conditionFunc: r => int.TryParse(r, out var _),
    thenReturn: r => int.Parse(r))
.AddStage<int>(num1 =>
{
    QuestionFactory.Create("İkinci sayıyı girin")
    .AddConditionStage<string>(
        conditionFunc: r => int.TryParse(r, out var _),
        thenReturn: r => int.Parse(r))
    .AddStage<int>(num2 =>
    {
        Console.WriteLine($"{num1} + {num2} = {num1 + {num2}}");
        return StageReturnFactory.CreateFinishExec();
    })
    .Execute();
    return StageReturnFactory.CreateFinishExec();
})
.Execute();
```

## 📜 Lisans

Bu proje MIT Lisansı altında lisanslanmıştır.
