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
        public StreamWriter OpenFile(string path)
        {
            StreamWriter streamWriter = default(StreamWriter);
            try
            {
                FileStream fileStream = File.OpenWrite(path);
                streamWriter = new StreamWriter(fileStream);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return streamWriter;
        }

        public void Save(T content, string path)
        {
            using (StreamWriter fileStream = OpenFile(path))
            {
                if (fileStream != null)
                {
                    try
                    {
                        Save(content, fileStream);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        fileStream.Flush();
                    }
                }
                else
                {

                }

            }
        }

        public abstract void Save(T content, StreamWriter fileStream);

    }
}
