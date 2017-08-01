﻿using System;
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
        private const int FILE_FORMAT_LENGTH = 3;

        public abstract string[] SupportedFormats { get; }

        public bool IsFileFormatSupported(string path)
        {
            string fileFormat = path.Substring(path.Length - FILE_FORMAT_LENGTH);
            return SupportedFormats.Contains(fileFormat);
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
                throw new NotSupportedException();
            }

            return streamReader;
        }

        public virtual T ParseFile(string filePath)
        {
            T result = default(T);
            StreamReader fileStream = OpenFile(filePath);
            if (fileStream != null)
            {
                try
                {
                    result = ParseFile(fileStream);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {

            }
            return result;
        }

        public abstract T ParseFile(StreamReader fileStream);
    }
}