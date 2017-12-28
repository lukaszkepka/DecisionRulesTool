using DecisionRulesTool.Model.FileSavers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleTester
{
    public class BackupManager
    {
        private readonly string backupDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Backup");
        private readonly IFileSaver<TestObject> resultSaver;

        private string backupLocation;

        public BackupManager(IFileSaver<TestObject> resultSaver)
        {
            this.resultSaver = resultSaver;
        }

        public void Initialize(bool dumpResults)
        {
            if (dumpResults)
            {
                backupLocation = Path.Combine(backupDirectory, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                if (!Directory.Exists(backupLocation))
                {
                    Directory.CreateDirectory(backupLocation);
                }
            }
        }

        public void Backup(TestObject testRequest)
        {
            try
            {
                string filePath = Path.Combine(backupLocation, testRequest.GetFileName());
                resultSaver.Save(testRequest, filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fatal error during backuping result : {ex.Message}");
            }
        }
    }
}
