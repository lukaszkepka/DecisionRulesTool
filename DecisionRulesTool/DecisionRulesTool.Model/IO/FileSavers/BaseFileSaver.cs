using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.FileSavers
{
    public abstract class BaseFileSaver<T> : IFileSaver<T>
    {
        public FileStream OpenFile(string path)
        {
            FileStream fileStream = default(FileStream);
            try
            {
                fileStream = File.OpenWrite(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return fileStream;
        }

        public void Save(T content, string path)
        {
            FileStream fileStream = OpenFile(path);
            if (fileStream != null)
            {
                try
                {
                    Save(content, fileStream);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {

            }
        }

        public abstract void Save(T content, FileStream fileStream);

    }
}
