using Microsoft.EntityFrameworkCore;
using System;

namespace TransferableObjects.Client
{
    //[Keyless]
    public class ClientInfo
    {
        public int Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Directory { get; set; }
        public string CurrentVersion { get; set; }
    }
}
