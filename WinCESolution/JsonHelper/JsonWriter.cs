namespace JsonHelper
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    
    public class JsonWriter : IDisposable 
    {
        private bool _disposed;
        private const int _indentationSpaces = 2;
        private int _currentIndentation;
        private readonly TextWriter _writer;

        public JsonWriter(TextWriter output)
        {
            _writer = output;
        }
        public JsonWriter(Stream output) : this(new StreamWriter(output, Encoding.UTF8)){}
        public JsonWriter(string file) : this(new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read)){}
        public JsonWriter(StringBuilder output) : this(new StringWriter(output, CultureInfo.InvariantCulture)){}

        public virtual void WriteNull()
        {
            WriteRaw("null");            
        }
        public virtual void WriteString(string value)
        {
            _writer.Write(string.Concat(JsonTokens.StringDelimiter, value.Replace("\"", "\\\""), JsonTokens.StringDelimiter));
        }
        public virtual void WriteRaw(string value)
        {
            _writer.Write(value);
        }
        public virtual void WriteChar(char value)
        {
            WriteString(value.ToString());
        }
        public virtual void WriteBool(bool value)
        {
            WriteRaw(value ? "true" : "false");
        }
        public virtual void WriteDate(DateTime date)
        {
            WriteString(date.ToString("G", CultureInfo.InvariantCulture));
        }
        public virtual void WriteKey(string key)
        {
            Indent();
            WriteString(key);
            _writer.Write(JsonTokens.PairSeparator);
        }

        public virtual void BeginObject()
        {
            NewLineAndIndent();
            _writer.Write(JsonTokens.StartObjectLiteralCharacter);
            NewLine();
            _currentIndentation += 1;
        }
        public virtual void EndObject()
        {
            NewLine();
            _currentIndentation -= 1;
            Indent();
            _writer.Write('}');
        }
        public virtual void EndArray()
        {
            _writer.Write(JsonTokens.EndArrayCharacter);
        }
        public virtual void BeginArray()
        {
            _writer.Write(JsonTokens.StartArrayCharacter);
        }
        public virtual void SeparateElements()
        {
            _writer.Write(JsonTokens.ElementSeparator);
        }

        [Conditional("DEBUG")]
        public virtual void NewLineAndIndent()
        {
            NewLine();
            Indent();
        }
        [Conditional("DEBUG")]
        public virtual void Indent()
        {
            _writer.Write(new string(' ', _indentationSpaces * _currentIndentation));
        }
        [Conditional("DEBUG")]
        public virtual void NewLine()
        {
            _writer.Write('\n');
        }

        public void Flush()
        {
            _writer.Flush();
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
                    Flush();                    
                    _writer.Close();
                }                
                _disposed = true;
            }
        }
    }
}
