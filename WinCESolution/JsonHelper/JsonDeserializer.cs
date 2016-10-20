namespace JsonHelper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;    
    
    using System.Reflection;    
    using Helpers;

    public class JsonDeserializer
    {
        private static readonly Type _IListType = typeof(IList);
        private readonly JsonReader _reader;
        private readonly string _fieldPrefix;

        public JsonDeserializer(JsonReader reader) : this(reader, string.Empty){}
        public JsonDeserializer(JsonReader reader, string fieldPrefix)
        {
            _reader = reader;
            _fieldPrefix = fieldPrefix;
        }

        public static T Deserialize<T>(JsonReader reader)
        {
            return Deserialize<T>(reader, string.Empty);
        }
        public static T Deserialize<T>(JsonReader reader, string fieldPrefix)
        {
            return (T) new JsonDeserializer(reader, fieldPrefix).DeserializeValue(typeof(T));
        }

        

        private object DeserializeValue(Type type)
        {
            _reader.SkipWhiteSpaces();            
            if (type == typeof(int))
            {
                return _reader.ReadInt32();                
            }
            if (type == typeof(string))
            {
                return _reader.ReadString();                
            }
            if (type == typeof(double))
            {
                return _reader.ReadDouble();
            }
            if (type == typeof(DateTime))
            {
                return _reader.ReadDateTime();
            }
            if (_IListType.IsAssignableFrom(type))
            {
                return DeserializeList(type);
            }            
            if (type == typeof(char))
            {
                return _reader.ReadChar();
            }
            if (type.IsEnum)
            {
                return _reader.ReadEnum();
            }
            if (type == typeof(long))
            {
                return _reader.ReadInt64();
            }        
            if (type == typeof(float))
            {
                return _reader.ReadFloat();
            }
            if (type == typeof(short))
            {
                return _reader.ReadInt16();
            }
            return ParseObject(type);            
        }
        private object DeserializeList(Type listType)
        {
            _reader.SkipWhiteSpaces();
            _reader.AssertAndConsume(JsonTokens.StartArrayCharacter);            
            Type itemType = ListHelper.GetListItemType(listType);
            bool isReadonly;
            IList container = ListHelper.CreateContainer(listType, itemType, out isReadonly);
            while(true)
            {
                _reader.SkipWhiteSpaces();
                container.Add(DeserializeValue(itemType));
                _reader.SkipWhiteSpaces();                
                if (_reader.AssertNextIsDelimiterOrSeparator(JsonTokens.EndArrayCharacter))
                {
                    break;
                }                
            }
            if (listType.IsArray)
            {
                return ListHelper.ToArray((List<object>)container, itemType);
            }
            if (isReadonly)
            {
                return listType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[] { container.GetType() }, null).Invoke(new object[] { container });
            }
            return container;
        }
        private object ParseObject(Type type)
        {           
            _reader.AssertAndConsume(JsonTokens.StartObjectLiteralCharacter);
            ConstructorInfo constructor = ReflectionHelper.GetDefaultConstructor(type);
            object instance = constructor.Invoke(null);
            while (true)
            {
                _reader.SkipWhiteSpaces();
                string name = _reader.ReadString();
                if (!name.StartsWith(_fieldPrefix))
                {
                    name = _fieldPrefix + name;
                }
                FieldInfo field = ReflectionHelper.FindField(type, name);
                _reader.SkipWhiteSpaces();
                _reader.AssertAndConsume(JsonTokens.PairSeparator);                
                _reader.SkipWhiteSpaces();
                field.SetValue(instance, DeserializeValue(field.FieldType));
                _reader.SkipWhiteSpaces();
                if (_reader.AssertNextIsDelimiterOrSeparator(JsonTokens.EndObjectLiteralCharacter))
                {
                    break;
                } 
            }
            return instance;
        }
    }
}