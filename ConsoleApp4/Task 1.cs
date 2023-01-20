using System;
using System.IO;
using System.Reflection.PortableExecutable;
using Task2;

namespace DirectoryManager
{
    public class CleaningResult
    {
        public int DeletedFoldersCount { get; set; }
        public int DeletedFilesCount { get; set; }
        public long FreeStorageInBytes { get; set; }
    }

    class Program
    {
        static void Main3(string[] args)
        {

            // read all files and folder in the root path
            // check last acceess date and delete them if it is greated than 30 minute

            string rootPath = @"C:\Users\Ольга\Desktop\Новая папка";

            //1. Показать, сколько весит папка до очистки.Использовать метод из задания 2.
            BeforeCleaning(rootPath);

            //2.Выполнить очистку.
            var cleaningResult = ScanAndRemoveOldItemsInDirectory(rootPath);

            //3.Показать сколько файлов удалено и сколько места освобождено.
            Console.WriteLine($"Deleted items {cleaningResult.DeletedFilesCount} Free storage space in bytes:{cleaningResult.FreeStorageInBytes}");


            //4.Показать, сколько папка весит после очистки.
            AfterCleaning(rootPath);


            // ProcessDirectory(rootPath);
        }

        private static void BeforeCleaning(string rootPath)
        {
            Console.WriteLine($"Before cleaning folder total size in bytes:{DirectoryExtension.GetDirectoryTotalSize(rootPath)}");
        }

        private static void AfterCleaning(string rootPath)
        {
            Console.WriteLine($"After cleaning folder total size in bytes:{DirectoryExtension.GetDirectoryTotalSize(rootPath)}");
        }

        private static CleaningResult ScanAndRemoveOldItemsInDirectory(string rootPath)
        {
            CleaningResult cleaningResult = new CleaningResult();
            TimeSpan timeSpan = TimeSpan.FromMinutes(30);
            foreach (var item in GetDirectoryItems(rootPath))
            {
                //Console.WriteLine($"Processing item:{item}");
                if (LastAccessIsGreaterThan(item, timeSpan))
                {
                    if (File.GetAttributes(item).HasFlag(FileAttributes.Directory))
                    {
                        cleaningResult.DeletedFoldersCount++;   
                    }
                    else
                    {
                        cleaningResult.DeletedFilesCount++;
                    }
                    // Console.WriteLine($"last access is greater then 30 minutes:{item}");
                    cleaningResult.FreeStorageInBytes += DeleteDirectoryItem(item);
                }
                else
                {
                    // Console.WriteLine($"Skip deleting item:{item}");
                }
            }
            return cleaningResult;
        }

        static long DeleteDirectoryItem(string path)
        {
            long size = 0;
            //Console.WriteLine($"Delete item:{path}");
            if (File.Exists(path))
            {
                File.Delete(path);
                size = new FileInfo(path).Length;
            }
            else
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            return size;
        }

        static bool LastAccessIsGreaterThan(string path, TimeSpan timeSpan)
        {
            // check it is directory or not
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                return DateTime.Now - directoryInfo.LastAccessTime > timeSpan;
            }

            FileInfo fileInfo = new FileInfo(path);
            return DateTime.Now - fileInfo.LastAccessTime > timeSpan;
        }

        static IEnumerable<string> GetDirectoryItems(string path)
        {
            return Directory.EnumerateFileSystemEntries(path);
        }

    }
}