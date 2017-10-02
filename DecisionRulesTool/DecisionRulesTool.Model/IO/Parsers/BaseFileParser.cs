using DecisionRulesTool.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Parsers
{
    public abstract class BaseFileParser<T> : IFileParser<T>
    {
        public abstract string SupportedFormat { get; }

        public bool IsFileFormatSupported(string path)
        {
            string fileFormat = Path.GetExtension(path);
            return SupportedFormat.Equals(fileFormat);
        }

        protected string RemoveBrackets(string value)
        {
            return value.Substring(1, value.Length - 2);
        }

        public virtual StreamReader OpenFile(string path)
        {
            StreamReader streamReader = default(StreamReader);
            if (IsFileFormatSupported(path))
            {
                try
                {
                    streamReader = File.OpenText(path);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                throw new FileFormatNotSupportedException(path);
            }

            return streamReader;
        }

        public virtual T ParseFile(string filePath)
        {
            T result = default(T);
            StreamReader fileStream = OpenFile(filePath);
            try
            {
                result = ParseFile(fileStream);
            }
            catch (Exception ex)
            {
                throw new InvalidFileBodyException(ex.Message, filePath);
            }
            return result;
        }

        public abstract T ParseFile(StreamReader fileStream);
    }
}
