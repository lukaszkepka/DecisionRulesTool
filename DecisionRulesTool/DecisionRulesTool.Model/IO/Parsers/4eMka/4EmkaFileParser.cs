using DecisionRulesTool.Model.IO;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Parsers._4eMka
{
    using Model;

    public abstract class _4eMkaFileParser<T> : BaseFileParser<T>
    {
        protected _4eMkaFileFormat fileFormat;

        public _4eMkaFileParser()
        {
            fileFormat = new _4eMkaFileFormat();
        }

        protected void MoveStreamToSection(StreamReader streamReader, string sectionName)
        {
            string fileLine = streamReader.ReadLine();
            while (!streamReader.EndOfStream && !fileLine.Equals(sectionName))
            {
                fileLine = streamReader.ReadLine();
            }

            if (streamReader.EndOfStream)
            {
                throw new InvalidDataException($"Section: \"{sectionName}\" not found!");
            }
        }

        protected virtual IEnumerable<Attribute> ParseAttributes(StreamReader fileStream)
        {
            IList<Attribute> attributes = new List<Attribute>();
            string fileLine, typeString;
            string[] attributeNameParts, fileLineParts;

            MoveStreamToSection(fileStream, fileFormat.AttributesSectionHeader);
            do
            {
                fileLine = fileStream.ReadLine();
                fileLineParts = fileLine.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                attributeNameParts = fileLineParts[0].Split(' ');
                typeString = fileLineParts[1].Trim(new[] { ' ', '\t' });
                if (attributeNameParts[0].Equals("+"))
                {
                    string name = attributeNameParts[1];
                    AttributeType type = fileFormat.GetAttributeType(typeString);
                    string[] availableValues = new string[0];
                    if (type == AttributeType.Symbolic)
                    {
                        //Get available values for symbolic type
                        availableValues = RemoveBrackets(typeString).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        availableValues = availableValues.Select(x => x.Trim(' ', '[', ']')).ToArray();
                    }
                    Attribute attribute = new Attribute(type, name, availableValues);
                    attributes.Add(attribute);
                }
            } while (!attributeNameParts[0].Equals("decision"));

            //Set decision attribute at the end of list
            Attribute decisionAttribute = attributes.Where(x => x.Name.Equals(typeString)).FirstOrDefault();
            attributes.Remove(decisionAttribute);
            attributes.Add(decisionAttribute);
            return attributes;
        }


    }
}
