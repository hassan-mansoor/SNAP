using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Tree
{
    public class CompositeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Composite));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Composite composite = (Composite)value;

            // Need to use reflection here because Elements is private
            PropertyInfo prop = typeof(Composite).GetProperty("Elements", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Element> children = (List<Element>)prop.GetValue(composite);
            
            JArray array = new JArray();

            foreach (Element e in children)
            {
               
                array.Add(JToken.FromObject(e, serializer));
            }
            JObject obj = new JObject();
            obj.Add(composite.Value, array);
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
