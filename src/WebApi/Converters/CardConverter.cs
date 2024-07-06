using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace WebApi.Converters
{
    public class CardConverter : JsonConverter<Card>
    {


        public override void WriteJson(JsonWriter writer, Card value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("id");
            writer.WriteValue(value.CardId);
            //writer.WritePropertyName("name");
            //writer.WriteValue(value.Name);
            //writer.WritePropertyName("type");
            //writer.WriteValue(value.Type);

            if (value.Shortcuts != null)
            {
                writer.WritePropertyName("shortcuts");
                serializer.Serialize(writer, value.Shortcuts);
            }

            writer.WriteEndObject();
        }

        public override Card ReadJson(JsonReader reader, Type objectType, Card existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var stop = new Stopwatch();
            stop.Start();
            JObject jObject = JObject.Load(reader);
            
            var card = existingValue ?? jObject.ToObject<Card>(new JsonSerializer()) ?? new Card();

            var config = Singleton.Instance.GetValue(card.Type);
            card.Shortcuts = config.Shortcuts;
            card.Buttons = config.Buttons;
            Console.WriteLine($"tempo: {stop.ElapsedMilliseconds} ms");
            return card;
        }
        public bool CanConvert(Type objectType)
        {
            return objectType == typeof(Card);
        }
    }
}
