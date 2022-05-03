using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SignalRClient.Context;
using SignalRClient.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using TransferableObjects;
using TransferableObjects.Extensions;

namespace SignalRClient
{
    internal class Program
    {
        static readonly ClientInfoService clientService = new ClientInfoService(new ClientInfoContext());
        static UpdateInfo updateInfo;

        static void Main(string[] args)
        {
            var conn = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chatHub", (opts) =>
                {
                    opts.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            // always verify the SSL certificate
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => { return true; };
                        return message;
                    };
                })
                .Build();
            try
            {
                conn.StartAsync().Wait();   

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            string clientInfoJson = clientService.GetJsonClientInfo();


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
                    string returnStatement = Extensions.SortDictionary(dictionary, updateInfo);
                    conn.InvokeAsync("ReturnStatement", returnStatement, clientInfoJson);
                }
            });

            conn.On("UpdateDatabaseClient", async (string clientDirectory, string version) =>
            {
                await clientService.AddClientInfoAsync(clientDirectory, version);
            });


            Console.ReadLine();
        }
    }
}
