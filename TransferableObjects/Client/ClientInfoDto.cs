using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferableObjects.Client
{
    public class ClientInfoDto
    {
        public string Hwid { get; set; }
        public ClientInfo ClientInformation { get; set; }
        public ClientInfoDto()
        {

        }
    }
}
