using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferableObjects.Enums;

namespace TransferableObjects.Server
{
    public class SoftInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public HashEncType SignatureType { get; set; }
        public DateTime LastUploadDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Version { get; set; }
        public FileType FileType { get; set; }
        public string DirectoryLocation { get; set; }
        public bool IsBetaVersion { get; set; }
    }
}
