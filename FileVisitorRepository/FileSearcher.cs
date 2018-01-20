using System;
using System.Collections.Generic;
using System.IO;
using FileVisitorRepository.Abstract;
using FileVisitorRepository.Models;

namespace FileVisitorRepository
{
    public class FileSearcher : IFileSearcher
    {
        public IEnumerable<DirectoryItem> SearchFile(string startPoint)
        {
            DirectoryInfo di;
            try
            {
                 di = new DirectoryInfo(startPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                yield break;
            }
            DirectoryInfo[] subDir;
            if (di.Parent != null)
                yield return new DirectoryItem(di.FullName, di.Name, DirectoryTypes.Folder);
            try
            {
                subDir = di.GetDirectories();
            }
            catch
            {
                yield break;
            }
            foreach (var t in subDir)
                foreach (var item in SearchFile(t.FullName))
                    yield return item;
            var fi = di.GetFiles();
            foreach (var t in fi)
                yield return new DirectoryItem(t.FullName, t.Name, DirectoryTypes.File);
        }
    }
}
