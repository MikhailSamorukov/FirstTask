using System;
using System.Collections.Generic;
using FileVisitorRepository.Models;

namespace FileVisitor.Abstract
{
    public interface IFileSystemVisitor
    {
        event EventHandler Start;
        event EventHandler Finish;
        event EventHandler<DirectoryEventArgs> FileFinded;
        event EventHandler<DirectoryEventArgs> DirectoryFinded;
        event EventHandler<DirectoryEventArgs> FilteredFileFinded;
        event EventHandler<DirectoryEventArgs> FilteredDirectoryFinded;
        IEnumerable<DirectoryItem> GetNonFilteredResult();
        IEnumerable<DirectoryItem> GetFilteredResult();
    }
}