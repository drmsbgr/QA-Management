using QA.Library.Factories;
using QA.Library.Extensions;

QuestionFactory
.Create("Sayı 1:")
.AddConditionStage<string>(
    conditionFunc: r => int.TryParse(r, out var _),
    thenReturn: r => int.Parse(r),
    otherwise: (_) => Console.WriteLine("Sayı girmeniz bekleniyordu!"))
.AddStage<int>(
    num1 =>
    {
        QuestionFactory
        .Create("Sayı 2:")
        .AddConditionStage<string>(
            conditionFunc: r => int.TryParse(r, out var _),
            thenReturn: r => int.Parse(r),
            otherwise: (_) => Console.WriteLine("Sayı girmeniz bekleniyordu!")
        )
        .AddStage<int>(
            num2 =>
            {
                QuestionFactory
                .Create("İşlem seçin (+,-,*,/)")
                .AddConditionStage<string>(
                    conditionFunc: r => r == "+" || r == "-" || r == "*" || r == "/",
                    thenReturn: r => r,
                    otherwise: (_) => Console.WriteLine("Geçersiz işlem!"))
                .AddExecAndFinishStage<string>(
                    op => Console.WriteLine($"{num1} {op} {num2} = {op switch { "+" => num1 + num2, "-" => num1 - num2, "*" => num1 * num2, "/" => num1 / num2, _ => 0 }}")
                )
                .Execute();

                return StageReturnFactory.CreateFinishExec();
            }
        )
        .Execute();

        return StageReturnFactory.CreateFinishExec();
    }
)
.Execute();

var response = QuestionFactory
.Create("2+2=?")
.AddConditionalResponseStage<string>(
    conditionFunc: r => r == "4",
    then: "Doğru cevap!",
    otherwise: "Yanlış cevap!")
.Execute<string>();

Console.WriteLine(response);

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

QuestionFactory
.Create("ekmek mi aldın yoksa süt mü aldın?")
.AddConditionalActionStage<string>(
    conditionFunc: r => r == "ekmek" || r == "süt",
    then: default,
    otherwise: (_) => Console.WriteLine("başka bir şey almadın bence :)"),
    thenReturn: (r) => r
)
.AddConditionalReturnStage<string>(
    conditionFunc: r => r == "ekmek",
    thenReturn: r => "{data:ekmek}",
    otherwiseReturn: r => "{data:süt}"
)
.AddStage<string>(
    r =>
    {
        Console.WriteLine($"{r}");
        return StageReturnFactory.CreateFinishExec();
    }
)
.Execute();