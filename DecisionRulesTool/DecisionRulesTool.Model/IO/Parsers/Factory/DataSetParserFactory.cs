﻿using DecisionRulesTool.Model.Exceptions;
using DecisionRulesTool.Model.IO.Parsers._4eMka;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model.Parsers.RSES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO.Parsers.Factory
{
    public class DataSetParserFactory : IFileParserFactory<DataSet>
    {
        public IFileParser<DataSet> Create(string fileExtension)
        {
            IFileParser<DataSet> dataSetParser = default(IFileParser<DataSet>);
            switch (fileExtension)
            {
                case BaseFileFormat.FileExtensions.RSESDataset:
                    dataSetParser = new RsesDataSetParser();
                    break;
                case BaseFileFormat.FileExtensions._4emkaDataset:
                    dataSetParser = new _4eMkaDataSetParser();
                    break;
                default:
                    throw new FileFormatNotSupportedException(fileExtension);
            }
            return dataSetParser;
        }
    }
}
