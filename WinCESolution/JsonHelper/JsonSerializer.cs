namespace JsonHelper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Helpers;

    public class JsonSerializer
    {
        //private readonly StringBuilder _builder;
        private readonly JsonWriter _writer;
        private ArrayList _currentGraph;
        private string _fieldPrefix;

        public JsonSerializer(JsonWriter writer) : this(writer, string.Empty)
        {
        }
        public JsonSerializer(JsonWriter writer, string fieldPrefix)
        {
            _writer = writer;
            _currentGraph = new ArrayList(0);
            _fieldPrefix = fieldPrefix;   
        }
        
        public static void Serialize(JsonWriter writer, object instance)
        {
            Serialize(writer, instance, string.Empty);
        }
        public static void Serialize(JsonWriter writer, object instance, string fieldPrefix)
        {
            new JsonSerializer(writer, fieldPrefix).SerializeValue(instance);
        }
        
        private void SerializeValue(object value)
        {
            if (value == null)
            {
                _writer.WriteNull();
                return;
            }
            if (value is string)
            {
                _writer.WriteString((string) value);
                return;
            }
            if (value is int || value is long || value is short || value is float || value is byte || value is sbyte || value is uint || value is ulong || value is ushort || value is double)
            {
                _writer.WriteRaw(value.ToString());               
                return;
            }
            if (value is char)
            {
                _writer.WriteChar((char) value);                
                return;
            }
            if (value is bool)
            {
                _writer.WriteBool((bool)value);                
                return;
            }
            if (value is DateTime)
            {
                _writer.WriteDate((DateTime) value);                
                return;
            }
            if (value is IDictionary)
            {
                SerializeDictionary((IDictionary) value);
                return;
            }
            if (value is IEnumerable)
            {
                SerializeEnumerable((IEnumerable) value);
                return;
            }
            SerializeObject(value);
        }

        private void SerializeObject(object @object)
        {
            if (@object == null)
            {
                return;
            }
            List<FieldInfo> fields = ReflectionHelper.GetSerializableFields(@object.GetType());
            if (fields.Count == 0)
            {
                return;
            }
            if (_currentGraph.Contains(@object))
            {
                throw new JsonException("Recursive reference found. Serialization cannot complete. Consider marking the offending field with the NonSerializedAttribute");
            }

            ArrayList oldGraph = _currentGraph;
            ArrayList currentGraph = new ArrayList(_currentGraph);
            _currentGraph = currentGraph;
            _currentGraph.Add(@object);

            _writer.BeginObject();
            SerializeKeyValue(GetKeyName(fields[0]), ReflectionHelper.GetValue(fields[0], @object), true);
            for (int i = 1; i < fields.Count; ++i)
            {
                SerializeKeyValue(GetKeyName(fields[i]), ReflectionHelper.GetValue(fields[i], @object), false);
            }
            _writer.EndObject();
            _currentGraph = oldGraph;
        }

        private string GetKeyName(MemberInfo field)
        {
            string name = field.Name;
            return name.StartsWith(_fieldPrefix) ? name.Substring(_fieldPrefix.Length) : name;
        }

        private void SerializeEnumerable(IEnumerable value)
        {
            IEnumerator e = value.GetEnumerator();
            _writer.BeginArray();            
            if (e.MoveNext())
            {
                SerializeValue(e.Current);
            }
            while (e.MoveNext())
            {
                _writer.SeparateElements();
                SerializeValue(e.Current);
            }            
            _writer.EndArray();
        }

        private void SerializeDictionary(IDictionary value)
        {
            IDictionaryEnumerator e = value.GetEnumerator();
            _writer.BeginObject();            
            if (e.MoveNext())
            {
                SerializeKeyValue(e.Key.ToString(), e.Value, true);
            }
            while (e.MoveNext())
            {
                SerializeKeyValue(e.Key.ToString(), e.Value, false);
            }            
            _writer.EndObject();
        }

        private void SerializeKeyValue(string key, object value, bool isFirst)
        {
            if (!isFirst)
            {
                _writer.SeparateElements();
                _writer.NewLine();
            }
            _writer.WriteKey(key);
            SerializeValue(value);
        }
    }
}