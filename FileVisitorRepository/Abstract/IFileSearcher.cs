using System.Collections.Generic;
using FileVisitorRepository.Models;

namespace FileVisitorRepository.Abstract
{
    public interface IFileSearcher
    {
        IEnumerable<DirectoryItem> SearchFile(string startPoint);
    }
}