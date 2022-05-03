using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferableObjects
{
    public class FileInformation
    {
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public FileInformation(string directory, string fileName, string extension)
        {
            Directory = directory;
            FileName = fileName;
            Extension = extension;
        }
        public FileInformation()
        {

        }
    }
}
