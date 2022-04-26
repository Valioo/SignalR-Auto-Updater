using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferableObjects.Server;

namespace TransferableObjects
{
    public class UpdateInfo
    {
        public SoftInformation SoftInformation { get; set; }
        public string ClientDirectory { get; set; }
        public UpdateInfo(SoftInformation updateInformation, string clientDirectory)
        {
            SoftInformation = updateInformation;
            ClientDirectory = clientDirectory;
        }
        public UpdateInfo()
        {

        }
    }
}
