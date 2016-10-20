namespace JsonHelper
{
    using System.Globalization;
    using System.IO;

    public class Converter
    {
        public static void Serialize(Stream output, object instance)
        {
            Serialize(output, instance, string.Empty);
        }
        public static void Serialize(Stream output, object instance, string fieldPrefix)
        {
            using (JsonWriter writer = new JsonWriter(output))
            {
                JsonSerializer.Serialize(writer, instance, fieldPrefix);
            }   
        }
        public static void Serialize(string file, object instance)
        {
            Serialize(file, instance, string.Empty);
        }
        public static void Serialize(string file, object instance, string fieldPrefix)
        {
            using (JsonWriter writer = new JsonWriter(file))
            {
                JsonSerializer.Serialize(writer, instance, fieldPrefix);                
            }              
        }
        public static string Serialize(object instance)
        {
            return Serialize(instance, string.Empty);
        }
        public static string Serialize(object instance, string fieldPrefix)
        {
            using (StringWriter sw = new StringWriter(CultureInfo.InvariantCulture))
            using (JsonWriter writer = new JsonWriter(sw))
            {
                JsonSerializer.Serialize(writer, instance, fieldPrefix);
                return sw.ToString();
            }               
        }

        public static T Deserialize<T>(Stream input)
        {
            return Deserialize<T>(input, string.Empty);
        }
        public static T Deserialize<T>(Stream input, string fieldPrefix)
        {
            using (JsonReader reader = new JsonReader(input))
            {
                return JsonDeserializer.Deserialize<T>(reader);
            }
        }
        public static T DeserializeFromFile<T>(string file)
        {
            return DeserializeFromFile<T>(file, string.Empty);
        }
        public static T DeserializeFromFile<T>(string file, string fieldPrefix)
        {
            using (JsonReader reader = new JsonReader(new FileStream(file, FileMode.Open, FileAccess.Read)))
            {
                return JsonDeserializer.Deserialize<T>(reader);
            }  
        }
        public static T Deserialize<T>(string json)
        {
            return Deserialize<T>(json, string.Empty);
        }
        public static T Deserialize<T>(string json, string fieldPrefix)
        {
            using (StringReader sr = new StringReader(json))
            using (JsonReader reader = new JsonReader(sr))
            {
                return JsonDeserializer.Deserialize<T>(reader, fieldPrefix);                
            }
        }
    }
}
