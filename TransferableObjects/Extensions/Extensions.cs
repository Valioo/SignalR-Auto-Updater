using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TransferableObjects.Extensions
{
    public class Extensions
    {
        public static string SortDictionary(Dictionary<int, byte[]> dictionary, UpdateInfo updateInfo)
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
            //Console.WriteLine(newByteArr.Length);
            newByteArr = Extensions.Decompress(newByteArr);
            //Console.WriteLine(newByteArr.Length);
            try
            {
                File.WriteAllBytes($@"{updateInfo.ClientDirectory}\{updateInfo.SoftInformation.Name}.{updateInfo.SoftInformation.FileType}", newByteArr);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Ok";
        }

        private static bool Concat<T>(ref T[] base_arr, T[] add_arr)
        {
            try
            {
                int base_size = base_arr.Length;
                int size_T = Marshal.SizeOf(base_arr[0]);
                Array.Resize(ref base_arr, base_size + add_arr.Length);
                Buffer.BlockCopy(add_arr, 0, base_arr, base_size * size_T, add_arr.Length * size_T);
            }
            catch (IndexOutOfRangeException ioor)
            {
                Console.WriteLine(ioor.Message);
                return false;
            }
            return true;
        }

        public static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }
}
