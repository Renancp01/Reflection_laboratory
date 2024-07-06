using Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ProcessCardAttribute : Attribute
{
    // Pode adicionar propriedades ao atributo, se necessário
}

public class Config
{
    public string Key { get; set; }
    public string Value { get; set; }
}

//public class Configs
//{
//    public string Type { get; set; }
//    public Config Config { get; set; }
//}

[ProcessCard]
public class Card
{
    public string Type { get; set; }
    public Guid CardId { get; set; }
    public string Number { get; set; }

    public List<Shortcut> Shortcuts { get; set; }

    public List<Button> Buttons { get; set; }

}

//public class CardResponse
//{
//    public IEnumerable<Card> CardHolderCards { get; set; }
//    public IEnumerable<Card> AdditionalCards { get; set; }
//    public IEnumerable<Card> TemporaryCards { get; set; }
//}

public static class ObjectProcessor
{
    //private static readonly List<Configs> ConfigsList = new List<Configs>
    //{
    //    new Configs { Type = "Holder", Config = new Config { Key = "1234", Value = "Holder Config Value" } },
    //    new Configs { Type = "Additional", Config = new Config { Key = "9012", Value = "Additional Config Value" } },
    //    new Configs { Type = "Temporary", Config = new Config { Key = "3456", Value = "Temporary Config Value" } }
    //};

    public static void Process(object obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var type = obj.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (!typeof(IEnumerable).IsAssignableFrom(property.PropertyType) ||
                !property.PropertyType.IsGenericType) continue;
            var itemType = property.PropertyType.GetGenericArguments()[0];
            
            if (itemType.GetCustomAttribute<ProcessCardAttribute>() == null)
                continue;
            
            var collection = (IEnumerable)property.GetValue(obj);
            if (collection == null) 
                continue;
            
            var processedCollection = Activator.CreateInstance(
                typeof(List<>).MakeGenericType(itemType)) as IList;

            foreach (var item in collection)
            {
                processedCollection?.Add(ProcessItem(item));
            }

            property.SetValue(obj, processedCollection);
        }
    }

    private static object ProcessItem(object item)
    {
        var type = item.GetType();
        var newItem = Activator.CreateInstance(type);

        foreach (var property in type.GetProperties())
        {
            var value = property.GetValue(item);
            property.SetValue(newItem, value);
        }

        // Busca o valor da configuração com base no tipo e número do cartão
        var typeProperty = type.GetProperty("Type")?.GetValue(item)?.ToString();
        var numberProperty = type.GetProperty("Number")?.GetValue(item)?.ToString();

        //var configValue = ConfigsList
        //    .Where(c => c.Type == typeProperty)
        //    .Select(c => c.Config)
        //    .FirstOrDefault(config => config.Key == numberProperty)?.Value;

        // Define o valor da configuração no novo objeto
        //type.GetProperty("ConfigValue")?.SetValue(newItem, configValue);

        return newItem;
    }
}

//// Exemplo de uso
//public static void Main()
//{
//    var cardResponse = new CardResponse
//    {
//        CardHolderCards = new List<Card>
//        {
//            new Card { CardId = Guid.NewGuid(), Type = "Holder", Number = "1234" },
//            new Card { CardId = Guid.NewGuid(), Type = "Holder", Number = "5678" }
//        },
//        AdditionalCards = new List<Card>
//        {
//            new Card { CardId = Guid.NewGuid(), Type = "Additional", Number = "9012" }
//        },
//        TemporaryCards = new List<Card>
//        {
//            new Card { CardId = Guid.NewGuid(), Type = "Temporary", Number = "3456" }
//        }
//    };

//    ObjectProcessor.Process(cardResponse);

//    // Exibir os novos objetos
//    foreach (var card in cardResponse.CardHolderCards)
//    {
//        Console.WriteLine($"Type: {card.Type}, CardId: {card.CardId}, Number: {card.Number}, ConfigValue: {card.ConfigValue}");
//    }

//    foreach (var card in cardResponse.AdditionalCards)
//    {
//        Console.WriteLine($"Type: {card.Type}, CardId: {card.CardId}, Number: {card.Number}, ConfigValue: {card.ConfigValue}");
//    }

//    foreach (var card in cardResponse.TemporaryCards)
//    {
//        Console.WriteLine($"Type: {card.Type}, CardId: {card.CardId}, Number: {card.Number}, ConfigValue: {card.ConfigValue}");
//    }
//}
