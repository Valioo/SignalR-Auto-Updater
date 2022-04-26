using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SignalRClient.Context;
using SignalRClient.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TransferableObjects;

namespace SignalRClient
{
    internal class Program
    {
        static readonly ClientInfoService clientService = new ClientInfoService(new ClientInfoContext());

        static void Main(string[] args)
        {
            var conn = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatHub")
                .Build();
            conn.StartAsync().Wait();
            string clientInfoJson = clientService.GetJsonClientInfo();
            UpdateInfo updateInfo;

            conn.InvokeAsync("SendMessage", clientInfoJson);   
            conn.On("ReceiveMessage", (string updateInfoS) =>
            {
                if (updateInfoS == "")
                {
                    Console.WriteLine("Up-to-date");
                }
                else
                {
                    updateInfo = JsonConvert.DeserializeObject<UpdateInfo>(updateInfoS);
                    Console.WriteLine(updateInfoS);
                }
            });

            Dictionary<int, byte[]> dictionary = new Dictionary<int, byte[]>();

            conn.On("ReceiveByte", (byte[] byteChunkArr, int num, bool ready) =>
            {
                dictionary.Add(num, byteChunkArr);
                if (ready)
                {
                    string returnStatement = SortDictionary(dictionary);
                    conn.InvokeAsync("ReturnStatement", returnStatement, clientInfoJson);
                }
            });

            conn.On("UpdateDatabaseClient", (string clientDirectory, string version) =>
            {
                clientService.AddClientInfoAsync(clientDirectory, version);
            });

            Console.ReadLine();
        }

        private static string SortDictionary(Dictionary<int,byte[]> dictionary)
        {
            byte[] newByteArr = dictionary.GetValueOrDefault(0);
            int i = 0;
            foreach (var item in dictionary.OrderBy(a => a.Key))
            {
                if (i == 0)
                {
                    i++;
                    continue;
                }
                Extensions.Concat(ref newByteArr, item.Value);
            }

            try
            {
                File.WriteAllBytes(@"C:\TestPr\Client\Windows11DragAndDropToTaskbarFix.exe", newByteArr);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Ok";
        }
    }
}
