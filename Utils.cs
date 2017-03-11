using System;
using System.Collections.Generic;


namespace GSCodegen
{
    class Utilities
    {
        /// <summary>
        /// Convertie la valeur en sa représentation hexadécimale
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string toHex(byte val)
        {
            string map = "0123456789ABCDEF";
            int low = val & 0x0F;
            int high = (val & 0xF0) >> 4;
            return String.Format("{0}{1}", map[high], map[low]);
        }

        /// <summary>
        /// Convertie la valeur en sa représentation hexadécimale
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string toHex(uint val)
        {
            string map = "0123456789ABCDEF";
            string buffer = "";

            for (int i = sizeof(uint) - 1; i >= 0; i--)
            {
                int low = (int)((val & (0x0F << i * 8)) >> i * 8);
                int high = (int)((val & (0xF0 << i * 8)) >> (4 + i * 8));
                buffer += String.Format("{0}{1}", map[high], map[low]);
            }
            return buffer;
        }

        /// <summary>
        /// Convertie la valeur en sa représentation hexadécimale
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string toHex(long val)
        {
            string map = "0123456789ABCDEF";
            string buffer = "";

            for (int i = sizeof(long) - 1; i >= 0; i--)
            {
                int low = (int)((val & (0x0FL << i * 8)) >> i * 8);
                int high = (int)((val & (0xF0L << i * 8)) >> (4 + i * 8));
                buffer += String.Format("{0}{1}", map[high], map[low]);
            }
            return buffer;
        }

        /// <summary>
        /// Convertie la valeur en sa représentation hexadécimale
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string toHex(UInt16 val)
        {
            string map = "0123456789ABCDEF";
            string buffer = "";

            for (int i = sizeof(UInt16) - 1; i >= 0; i--)
            {
                int low = (int)((val & (0x0F << i * 8)) >> i * 8);
                int high = (int)((val & (0xF0 << i * 8)) >> (4 + i * 8));
                buffer += String.Format("{0}{1}", map[high], map[low]);
            }
            return buffer;
        }

    }
}