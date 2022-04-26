using System;
using System.Runtime.InteropServices;

namespace SignalRClient.Services
{
    internal static class Extensions
    {
        public static bool Concat<T>(ref T[] base_arr,T[] add_arr)
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
    }
}
