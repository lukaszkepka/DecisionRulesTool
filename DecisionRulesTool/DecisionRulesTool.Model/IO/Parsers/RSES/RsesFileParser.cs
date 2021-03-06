﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace DecisionRulesTool.Model.Parsers
{
    using IO;
    using Model;

    public abstract class RsesFileParser<T> : BaseFileParser<T>
    {
        protected RsesFileFormat fileFormat;
        public RsesFileParser()
        {
            fileFormat = new RsesFileFormat();
        }

        protected string GetSectionValue(StreamReader fileStream, string sectionName)
        {
            string sectionValue = null;
            bool sectionFound = false;
            do
            {
                string fileLine = fileStream.ReadLine();
                string[] lineWords = fileLine.Split(' ');
                if (lineWords[0].Equals(sectionName))
                {
                    sectionFound = true;
                    sectionValue = lineWords[1];
                }
            } while (!sectionFound && !fileStream.EndOfStream);

            if (fileStream.EndOfStream && !sectionFound)
            {
                throw new InvalidDataException($"Section: \"{sectionName}\" not found!");
            }
            return sectionValue;
        }

        protected virtual IEnumerable<Attribute> ParseAttributes(StreamReader fileStream)
        {
            int attributesCount = Convert.ToInt32(GetSectionValue(fileStream, fileFormat.AttributesSectionHeader));
            Attribute[] attributes = new Attribute[attributesCount];
            int attributeIndex = 0;
            while (attributeIndex < attributesCount)
            {
                string fileLine = fileStream.ReadLine();
                if (!string.IsNullOrEmpty(fileLine))
                {
                    string[] lineWords = fileLine.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    AttributeType type = fileFormat.GetAttributeType(lineWords[1]);
                    string name = lineWords[0];
                    //attribute.Accuary = attribute.Type == Attribute.Category.NUMERIC ? Convert.ToInt32(lineWords[2]) : default(int?);
                    Attribute attribute = new Attribute(type, name);
                    attributes[attributeIndex++] = attribute;
                }
            }
            return attributes;
        }
    }
}
