using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalR.Server.Models;
using SignalR.Server.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TransferableObjects.Client;

namespace SignalR.Server
{
    public class ChatHub : Hub
    {
        SoftInformationService _softInfoService = new SoftInformationService(new ServerDbContext());
        public async Task SendMessage(string clientInfo)
        {
            var clientInfoDto = JsonConvert.DeserializeObject<ClientInfoDto>(clientInfo);
            var updateInfo = _softInfoService.GetUpdateInfoAsync(clientInfoDto).Result;
            var updateInfoString = JsonConvert.SerializeObject(updateInfo);

            if (updateInfo == null)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "");
            }
            else
            {
                byte[] byteArray = File.ReadAllBytesAsync($@"{updateInfo.SoftInformation.DirectoryLocation}\{updateInfo.SoftInformation.Name}.{updateInfo.SoftInformation.FileType}").Result;

                await Clients.Caller.SendAsync("ReceiveMessage", updateInfoString);

                var byteChunk = GetBytesIntoChunks(byteArray);
                bool isReady = false;

                for (int i = 0; i < byteChunk.Count; i++)
                {
                    if (i == byteChunk.Count - 1)
                    {
                        isReady = true;
                    }
                    var element = byteChunk.ElementAt(i);
                    await Clients.Caller.SendAsync("ReceiveByte", element.Value, element.Key, isReady);
                }
            }
        }
        public async Task ReturnStatement(string returnStatement, string clientInfoJson)
        {
            if (returnStatement == "Ok")
            {
                Console.WriteLine("Working perfectly");
                var clientInfoDto = JsonConvert.DeserializeObject<ClientInfoDto>(clientInfoJson);
                var updateInfo = _softInfoService.GetUpdateInfoAsync(clientInfoDto).Result;
                await Clients.Caller.SendAsync("UpdateDatabaseClient", clientInfoDto.ClientInformation.Directory, updateInfo.SoftInformation.Version);
            }
            else
                Console.WriteLine(returnStatement);

        }

        private Dictionary<int,byte[]> GetBytesIntoChunks(byte[] byteArrayInitial)
        {
            var dictionary = new Dictionary<int, byte[]>();
            int splitSize = new Random().Next(byteArrayInitial.Length/25, byteArrayInitial.Length/10);
            var arrays = byteArrayInitial.Split(splitSize);
            int i = 0;
            foreach (var arr in arrays)
            {
                dictionary.Add(i, arr.ToArray());
                i++;
            }

            dictionary = dictionary.Shuffle();
            return dictionary;
        }
    }
}