using System;
using System.IO;
using System.Web.Hosting;

namespace jeanie.Lib
{
    public static class FileHelpers
    {
        public static string Base64Encode(string filepath)
        {
            using (var file = File.OpenRead(HostingEnvironment.MapPath(filepath)))
            {
                var buffer = new byte[file.Length];
                if (file.Read(buffer, 0, (int) file.Length) == file.Length)
                {
                    return Convert.ToBase64String(buffer);
                }
            }

            return null;
        }
    }
}
