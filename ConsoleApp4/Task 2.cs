using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    //  return new DirectoryInfo(path).GetFiles("*.*", SearchOption.AllDirectories).Sum(f => f.Length);
    public static class DirectoryExtension
    {

        public static long GetDirectoryTotalSize(string path)
        {
            long totalSize = 0;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                totalSize += fileInfo.Length;
            }
            return totalSize;
        }
    }
}
