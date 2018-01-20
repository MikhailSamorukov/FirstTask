
using System.Collections.Generic;
using FileVisitorRepository.Abstract;
using FileVisitorRepository.Models;

namespace FileVisitorTests
{
    public class FileSearcherMock : IFileSearcher
    {
        public IEnumerable<DirectoryItem> SearchFile(string startPoint)
        {
            return new List<DirectoryItem>()
            {
                new DirectoryItem("D:\\test\\", "test", DirectoryTypes.Folder),
                new DirectoryItem("D:\\test\\folder1\\", "folder1", DirectoryTypes.Folder),
                new DirectoryItem("D:\\test\\folder2\\", "folder2", DirectoryTypes.Folder),
                new DirectoryItem("D:\\test\\folder1\\file1.txt", "file1", DirectoryTypes.File),
                new DirectoryItem("D:\\test\\folder3\\file1.txt", "file1", DirectoryTypes.File),
                new DirectoryItem("D:\\test\\file1.txt", "file1", DirectoryTypes.File),
                new DirectoryItem("D:\\test\\folder2\\file10.txt", "file10", DirectoryTypes.File),
                new DirectoryItem("D:\\test\\folder4\\", "folder4", DirectoryTypes.Folder),
                new DirectoryItem("D:\\test\\folder4\\file11.txt", "file11", DirectoryTypes.File),
                new DirectoryItem("D:\\test\\folder4\\file5.txt", "file5", DirectoryTypes.File),
                new DirectoryItem("D:\\test\\folder4\\folder5\\", "folder5", DirectoryTypes.Folder)
            };
        }
    }
}