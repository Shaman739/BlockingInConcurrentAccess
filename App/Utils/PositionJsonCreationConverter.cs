using BlockingResourcesInConcurrentAccess.Core.Contract;
using BlockingResourcesInConcurrentAccess.Core.Contract.Figures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace BlockingResourcesInConcurrentAccess.Utils
{
    public class PositionJsonCreationConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        protected Position Create(Type objectType, JObject jObject)
        {
            var position = new Position();
            JObject type = jObject.Value<JObject>("figure");
            var typeFigure = type.Value<string>("type");
            position.Figure = typeFigure switch
            {
                "Circle" => new Circle(),
                "Triangle" => new Triangle(),
                "Square" => new Square(),
                _ => throw new ArgumentException($"Unknown type of figure '{typeFigure}'")
            };

            return position;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Position).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType,
          object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jObject = JObject.Load(reader);

            Position target = Create(objectType, jObject);

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value,
          JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

