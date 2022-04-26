using Microsoft.EntityFrameworkCore;
using SignalR.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferableObjects.Client;
using TransferableObjects;
using TransferableObjects.Server;

namespace SignalR.Server.Services
{
    public class SoftInformationService
    {
        private readonly ServerDbContext _context;
        public SoftInformationService(ServerDbContext context)
        {
            _context = context;
        }
        public async Task<UpdateInfo> GetUpdateInfoAsync(ClientInfoDto clientInfo)
        {
            Client client = await _context.Clients
                .Where(a => a.ClientHwid == clientInfo.Hwid)
                .FirstAsync();
            if (client == null)
            {
                return null;
            }
            bool isBeta = client.IsBetaTester;
            string hwid = client.ClientHwid;

            SoftInformation softInfo = await _context.SoftInfos
                .Where(a => a.IsBetaVersion == isBeta)
                .OrderByDescending(a => a.Version)
                .FirstAsync();

            if (clientInfo.ClientInformation.CurrentVersion == softInfo.Version)
            {
                return null;
            }

            UpdateInfo updateInfo = new UpdateInfo();
            updateInfo.SoftInformation = softInfo;
            updateInfo.ClientDirectory = clientInfo.ClientInformation.Directory;
            return updateInfo;
        }
    }
}
