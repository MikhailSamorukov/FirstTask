using System;
using System.Collections.Generic;
using System.Linq;
using FileVisitor.Abstract;
using FileVisitorRepository.Models;
using FileVisitorRepository.Abstract;

namespace FileVisitor
{
    public class FileSystemVisitor: IFileSystemVisitor
    {
        private delegate void EventType(string foundFile);
        private bool _skip;
        private bool _break;
        private readonly Func<DirectoryItem, bool> _filterCondition;
        private readonly string _directoryStartPoint;
        private readonly IFileSearcher _fileSearcher;

        public event EventHandler Start;
        public event EventHandler Finish;
        public event EventHandler<DirectoryEventArgs> FileFinded;
        public event EventHandler<DirectoryEventArgs> DirectoryFinded;
        public event EventHandler<DirectoryEventArgs> FilteredFileFinded;
        public event EventHandler<DirectoryEventArgs> FilteredDirectoryFinded;

        public FileSystemVisitor(string directoryStartPoint, IFileSearcher fileSearcher)
        {
            _fileSearcher = fileSearcher;
            _directoryStartPoint = directoryStartPoint;
            _filterCondition = null;
        }

        public FileSystemVisitor(string directoryStartPoint, IFileSearcher fileSearcher, Func<DirectoryItem, bool> filterCondition)
            : this(directoryStartPoint, fileSearcher)
        {
            _filterCondition = filterCondition;
        }

        public IEnumerable<DirectoryItem> GetNonFilteredResult()
        {
            return GetSearchResult(false);
        }

        public IEnumerable<DirectoryItem> GetFilteredResult()
        {
            return GetSearchResult(true);
        }

        private void OnStart()
        {
            Start?.Invoke(this, null);
        }

        private void OnFinish()
        {
            Finish?.Invoke(this, null);
            _skip = false;
            _break = false;
        }

        private void FindInvoke(EventHandler<DirectoryEventArgs> @event, string foundFile)
        {
            var args = new DirectoryEventArgs(false, false, foundFile);
            @event?.Invoke(this, args);
            if (args.Skip)
                _skip = true;
            if (args.Break)
                _break = true;
        }

        private void OnFileFinded(string foundFile)
        {
            FindInvoke(FileFinded, foundFile);
        }

        private void OnDirectoryFinded(string foundFile)
        {
            FindInvoke(DirectoryFinded, foundFile);
        }

        private void OnFilteredFileFinded(string foundFile)
        {
            FindInvoke(FilteredFileFinded, foundFile);
        }

        private void OnFilteredDirectoryFinded(string foundFile)
        {
            FindInvoke(FilteredDirectoryFinded, foundFile);
        }

        private IEnumerable<DirectoryItem> GetSearchResult(bool isDataFiltered)
        {
            var result = new List<DirectoryItem>();
            var skipedDirectoryItem = new List<string>();
            var fileFinded = isDataFiltered ? new EventType(OnFilteredFileFinded) : OnFileFinded;
            var folderFinded = isDataFiltered ? new EventType(OnFilteredDirectoryFinded) : OnDirectoryFinded;
            var filterCondition = !isDataFiltered || _filterCondition == null ? x => true : _filterCondition;

            OnStart();
            foreach (var item in _fileSearcher.SearchFile(_directoryStartPoint).Filter(filterCondition))
            {
                if (skipedDirectoryItem.Any(x => item.FullName.Contains(x)))
                    continue;

                if (item.Type == DirectoryTypes.Folder)
                {
                    folderFinded(item.FullName);
                   
                }
                else
                {
                    fileFinded(item.FullName);
                }

                if (_break)
                {
                    OnFinish();
                    return result;
                }

                if (_skip)
                    skipedDirectoryItem.Add(item.FullName);

                if (!skipedDirectoryItem.Any(x => item.FullName.Contains(x)))
                    result.Add(item);

                _skip = false;
            }
            OnFinish();

            return result;
        }
    }
}
