using System;
using System.Text;

namespace Common
{
    public class Tools
    {
        private static readonly string EncodingName = "GB2312";

        public static string GenMessage(int len)
        {
            int area, code;

            StringBuilder builder = new(100);
            Random rand = new();

            for (int i = 0; i < len; i++)
            {
                area = rand.Next(16, 56);

                code = area == 55
                    ? rand.Next(1, 90)
                    : rand.Next(1, 95);

                builder.Append(Encoding.GetEncoding(EncodingName).GetString(new byte[] { Convert.ToByte(area + 160), Convert.ToByte(code + 160) }));
            }

            return builder.ToString();
        }
    }
}
