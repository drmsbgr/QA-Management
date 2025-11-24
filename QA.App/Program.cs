using QA.Library.Factories;
using QA.Library.Extensions;

var response = QuestionFactory
.Create("2+2=?")
.AddConditionalResponseStage<string>(
    conditionFunc: r => r == "4",
    then: "Doğru cevap!",
    otherwise: "Yanlış cevap!")
.Execute<string>();

System.Console.WriteLine(response);

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
        Console.WriteLine($"{num1} + {num2} = {num1 + num2}");
        return StageReturnFactory.CreateFinishExec();
    })
    .Execute();
    return StageReturnFactory.CreateFinishExec();
})
.Execute();