using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SignalRClient.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransferableObjects;
using TransferableObjects.Client;

namespace SignalRClient.Services
{
    internal class ClientInfoService
    {
        private readonly ClientInfoContext _context;
        public ClientInfoService(ClientInfoContext context)
        {
            _context = context;
        }
        public async Task<ClientInfo> GetClientInfoLatestAsync()
        {
            return await _context.ClientInfos
                .OrderByDescending(a => a.CurrentVersion)
                .ThenByDescending(a => a.LastUpdate)
                .FirstAsync();
        }

        public void AddClientInfoAsync(string directory, string version)
        {
            ClientInfo clInfo = new ClientInfo();
            clInfo.LastUpdate = DateTime.Now;
            clInfo.CurrentVersion = version;
            clInfo.Directory = directory;
            _context.ClientInfos.Add(clInfo);
            _context.SaveChanges();
        }

        internal string GetJsonClientInfo()
        {
            ClientInfoDto clientInfo = new ClientInfoDto();
            clientInfo.ClientInformation =  GetClientInfoLatestAsync().Result;
            clientInfo.Hwid = GetUserId();
            return JsonConvert.SerializeObject(clientInfo);
        }
        private static string GetUserId()
        {
            return libc.hwid.HwId.Generate();
        }
    }
}
