﻿using ClosedXML.Excel;
using DecisionRulesTool.Model.IO;
using DecisionRulesTool.Model.Parsers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace DecisionRulesTool.Model.RuleTester.Result
{
    using DecisionRulesTool.Model.Model;

    public class TestRequestParser : BaseFileParser<TestObject>
    {
        public override string SupportedFormat => BaseFileFormat.FileExtensions.ExcelFile;

        public override TestObject ParseFile(StreamReader fileStream)
        {
            XLWorkbook workBook = new XLWorkbook(fileStream.BaseStream);
            return CreateTestRequestFromMetaData(workBook);
        }

        public string[] ReadDecisionAttributeValues(DataTable confusionMatrixTable)
        {
            List<string> availableValues = new List<string>();
            for (int i = 1; i < confusionMatrixTable.Columns.Count - 2; i++)
            {
                availableValues.Add(confusionMatrixTable.Columns[i].ColumnName);
            }

            return availableValues.ToArray();
        }

        public DataSet ReadDataSet(XLWorkbook workBook, string name, DataTable confusionMatrixTable, int decisionAttributeIndex)
        {
            IXLWorksheet labels = workBook.Worksheet("Labels");
            IXLWorksheet attributesMetaData = workBook.Worksheet("AttributesMetaData");

            DataTable labelsDataTable = labels.Table(0)?.AsNativeDataTable();
            DataTable attributesMetaDataTable = attributesMetaData.Table(0)?.AsNativeDataTable();

            DataSet dataSet = new DataSet(name, new List<Model.Attribute>(), new List<Model.Object>());

            int i = 0;
            foreach (DataRow attributeMetaData in attributesMetaDataTable.Rows)
            {
                string columnName = attributeMetaData[0].ToString();
                string type = attributeMetaData[1].ToString();
                string availableValuesString = attributeMetaData[2].ToString();

                string[] availableValues;
                if (i++ == decisionAttributeIndex)
                {
                    availableValues = ReadDecisionAttributeValues(confusionMatrixTable);
                }
                else
                {
                    if (String.IsNullOrEmpty(availableValuesString))
                    {
                        availableValues = new string[0];
                    }
                    else
                    {
                        availableValues = availableValuesString.Split(',');
                    }
                }


                dataSet.Attributes.Add(new Model.Attribute((AttributeType)Enum.Parse(typeof(AttributeType), type), columnName, availableValues));
            }

            foreach (DataRow row in labelsDataTable.Rows)
            {
                var values = row.ItemArray.Skip(3).ToArray();
                if (values.Length == dataSet.Attributes.Count)
                {
                    Model.Object dataObject = new Model.Object(dataSet, values);
                    dataSet.Objects.Add(dataObject);
                }
                else
                {
                    throw new FormatException("File has invalid structure");
                }
            }
            return dataSet;
        }

        public TestResult CreateTestResult(XLWorkbook workBook, ConfusionMatrix confusionMatrix)
        {
            IXLWorksheet labels = workBook.Worksheet("Labels");
            DataTable labelsDataTable = labels.Table(0)?.AsNativeDataTable();
            List<string> decisions = new List<string>();
            List<string> results = new List<string>();

            IXLWorksheet summaryWorksheet = workBook.Worksheet("Summary");
            DataTable summaryDataTable = summaryWorksheet.Table(0)?.AsNativeDataTable();
            decimal coverage = Convert.ToDecimal(summaryDataTable.Rows[0]["Coverage"]);
            decimal accuary = Convert.ToDecimal(summaryDataTable.Rows[0]["Accuracy"]);
            decimal totalAccuary = Convert.ToDecimal(summaryDataTable.Rows[0]["Total Accuracy"]);


            foreach (DataRow row in labelsDataTable.Rows)
            {
                decisions.Add(row[1].ToString());
                results.Add(row[2].ToString());
            }

            return new TestResult()
            {
                ConfusionMatrix = confusionMatrix,
                DecisionValues = decisions.ToArray(),
                ClassificationResults = results.ToArray(),
                Coverage = coverage,
                Accuracy = accuary,
                TotalAccuracy = totalAccuary
            };
        }

        public ConfusionMatrix ReadConfusionMatrix(DataTable confusionMatrixTable, Attribute decisionAttribute)
        {
            var confusionMatrix = new ConfusionMatrix(decisionAttribute);
            int tableLength = ClassificationResult.GetDecisionClasses(decisionAttribute).Length;

            int[,] values = new int[tableLength, tableLength];

            for (int i = 0; i < tableLength; i++)
            {
                DataRow row = confusionMatrixTable.Rows[i];
                for (int j = 1; j < tableLength + 1; j++)
                {
                    values[i, j - 1] = Convert.ToInt32(row[j]);
                }
            }

            confusionMatrix.Initialize(values);
            return confusionMatrix;
        }

        public TestObject CreateTestRequestFromMetaData(XLWorkbook workBook)
        {
            IXLWorksheet metaDataWorkbook = workBook.Worksheet("MetaData");
            IXLWorksheet confusionMatrixWorksheet = workBook.Worksheet("Confusion Matrix");
            DataTable confusionMatrixTable = confusionMatrixWorksheet.Table(0)?.AsNativeDataTable();
            DataTable metaDataTable = metaDataWorkbook.Table(0)?.AsNativeDataTable();
            DataRow metaDataRow = metaDataTable.Rows[0];


            string filters = metaDataRow["Filters"].ToString();
            string filtersShort = metaDataRow["Filters short"].ToString();
            string testSetName = metaDataRow["Test Set"].ToString();
            string ruleSetName = metaDataRow["Rule Set"].ToString();
            int decisionAttributeIndex = Convert.ToInt32(metaDataRow["Decision Attribute Index"].ToString());
            string conflictResolvingMethod = metaDataRow["Conflict Resolving Method"].ToString();

            DataSet testSet = ReadDataSet(workBook, testSetName, confusionMatrixTable, decisionAttributeIndex);
            Attribute decisionAttribute = testSet.Attributes.ElementAt(decisionAttributeIndex);
            RuleSet ruleSet = new RuleSetSubset(ruleSetName)
            {
                FiltersShortInfo = filtersShort,
                FiltersInfo = filters,
                DecisionAttribute = decisionAttribute
            };

            TestObject testRequest = new TestObject(ruleSet, testSet, (ConflictResolvingMethod)Enum.Parse(typeof(ConflictResolvingMethod), conflictResolvingMethod))
            {
                Progress = 100,
                IsReadOnly = true
            };

            ConfusionMatrix confusionMatrix = ReadConfusionMatrix(confusionMatrixTable, decisionAttribute);
            TestResult testResult = CreateTestResult(workBook, confusionMatrix);

            testRequest.TestResult = testResult;
            return testRequest;
        }
    }
}

