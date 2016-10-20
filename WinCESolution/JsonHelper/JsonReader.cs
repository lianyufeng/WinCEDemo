namespace JsonHelper
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class JsonReader : IDisposable
    {
        private readonly TextReader _reader;
        private bool _disposed;
        public JsonReader(TextReader input)
        {
            _reader = input;
        }
        public JsonReader(Stream input) : this(new StreamReader(input, Encoding.UTF8)) { }        
        public JsonReader(string input) : this(new StringReader(input)) { }

        public virtual void SkipWhiteSpaces()
        {            
            while(true)
            {
                char c = Peek();
                if (!char.IsWhiteSpace(c))
                {
                    break;
                }
                _reader.Read();
            }
        }
        public virtual int ReadInt32()
        {
            string value = ReadNumericValue();
            return value == null ? 0 : Convert.ToInt32(value);
        }
        public virtual string ReadString()
        {
            AssertAndConsume(JsonTokens.StringDelimiter);            
            StringBuilder sb = new StringBuilder(25);
            bool isEscaped = false;

            while(true)
            {
                char c = Read();
                if (c == '\\' && !isEscaped)
                {
                    isEscaped = true;
                    continue;
                }
                if (isEscaped)
                {
                    sb.Append(FromEscaped(c));
                    isEscaped = false;
                    continue;
                }
                if (c == '"')
                {                 
                    break;
                }
                sb.Append(c);
            }            
            string str = sb.ToString();
            return str == "null" ? null : str;
        }
        public virtual double ReadDouble()
        {
            string value = ReadNumericValue();            
            return value == null ? 0 : Convert.ToDouble(value);
        }
        public virtual DateTime ReadDateTime()
        {
            string str = ReadString();
            return str == null ? DateTime.MinValue : DateTime.ParseExact(str, "G", CultureInfo.InvariantCulture);
        }
        public virtual char ReadChar()
        {
            string str = ReadString();            
            if (str == null)
            {
                return (char) 0;
            }
            if (str.Length > 1)
            {
                throw new JsonException("Expecting a character, but got a string");
            }                        
            return str[0];
        }
        public virtual int ReadEnum()
        {
            return ReadInt32();            
        }
        public virtual long ReadInt64()
        {
            string value = ReadNumericValue();
            return value == null ? 0 : Convert.ToInt64(value);
        }
        public virtual float ReadFloat()
        {
            string value = ReadNumericValue();
            return value == null ? 0 : Convert.ToSingle(value);
        }
        public virtual short ReadInt16()
        {
            string value = ReadNumericValue();
            return value == null ? (short)0 : Convert.ToInt16(value);
        }
        public virtual string ReadNumericValue()
        {
            return ReadNonStringValue('0');
        }
        public virtual string ReadNonStringValue(char offset)
        {
            StringBuilder sb = new StringBuilder(10);
            while (true)
            {
                char c = Peek();
                if (IsDelimiter(c))
                {
                    break;
                }
                int read = _reader.Read();
                if (read >= '0' && read <= '9')
                {
                    sb.Append(read - offset);
                }
                else
                {
                    sb.Append((char) read);
                }                
            }
            string str = sb.ToString();
            return str == "null" ? null : str;
        }
        public virtual bool IsDelimiter(char c)
        {
            return (c == JsonTokens.EndObjectLiteralCharacter || c == JsonTokens.EndArrayCharacter || c == JsonTokens.ElementSeparator || IsWhiteSpace(c));
        }
        public virtual bool IsWhiteSpace(char c)
        {
            return char.IsWhiteSpace(c);
        }

        public virtual char Peek()
        {
            int c = _reader.Peek();
            return ValidateChar(c);
        }
        public virtual char Read()
        {
            int c = _reader.Read();
            return ValidateChar(c);
        }
        private char ValidateChar(int c)
        {
            if (c == -1)
            {
                throw new JsonException("End of data");
            }
            return (char)c;
        }
        
        public virtual string FromEscaped(char c)
        {
            switch (c)
            {
                case '"':
                    return "\"";
                case '\\':
                    return "\\";
                case 'b':
                    return "\b";
                case 'f':
                    return "\f";
                case 'r':
                    return "\r";
                case 'n':
                    return "\n";
                case 't':
                    return "\t";
                default:
                    throw new ArgumentException("Unrecognized escape character: " + c);
            }
        }

        protected internal virtual void AssertAndConsume(char character)
        {
            char c = Read();
            if (c != character)
            {
                throw new JsonException(string.Format("Expected character '{0}', but got: '{1}'", character, c));
            }
        }
        protected internal bool AssertNextIsDelimiterOrSeparator(char endDelimiter)
        {
            char delimiter = Read();
            if (delimiter == endDelimiter)
            {
                return true;
            }
            if (delimiter == ',')
            {
                return false;                
            }
            throw new JsonException("Expected array separator or end of array, got: " + delimiter);            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _reader.Close();
                }
                _disposed = true;
            }
        }
    }
}
