using System.Collections.Generic;
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
    using System.Globalization;

    public abstract class RsesFileParser<T> : BaseFileParser<T>
    {
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

        protected object GetAttributeValue(Attribute attribute, string value)
        {
            object attributeValue = value;
            if (RsesFileFormat.MissingValueChars.Contains(attributeValue))
            {
                attributeValue = Attribute.MissingValue;
            }
            else
            {
                switch (attribute.Type)
                {
                    case AttributeType.Numeric:
                        attributeValue = Convert.ToDouble(attributeValue, CultureInfo.InvariantCulture);
                        break;
                    case AttributeType.Integer:
                        attributeValue = Convert.ToInt32(attributeValue, CultureInfo.InvariantCulture);
                        break;
                }
            }
            return attributeValue;
        }

        protected virtual IEnumerable<Attribute> ParseAttributes(StreamReader fileStream)
        {
            int attributesCount = Convert.ToInt32(GetSectionValue(fileStream, RsesFileFormat.AttributesSectionHeader));
            Attribute[] attributes = new Attribute[attributesCount];
            int attributeIndex = 0;
            while (attributeIndex < attributesCount)
            {
                string fileLine = fileStream.ReadLine();
                if (!string.IsNullOrEmpty(fileLine))
                {
                    string[] lineWords = fileLine.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    AttributeType type = (AttributeType)Enum.Parse(typeof(AttributeType), lineWords[1], true);
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
