using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FileVisitor;
using FileVisitor.Abstract;
using FileVisitorRepository.Models;
using NUnit.Framework;

namespace FileVisitorTests
{
    [TestFixture]
    public class FileVisitorTest
    {
        const string SKIPPED_NON_FILTERED_FILE = "file10.txt";
        const string SKIPPED_NON_FILTERED_FOLDER = "folder2";
        const string SKIPPED_FILTERED_FILE = "file11.txt";
        const string SKIPPED_FILTERED_FOLDER = "folder4";

        private IFileSystemVisitor _fileSystemVisitor;
        [SetUp]
        public void Init()
        {
            _fileSystemVisitor = new FileSystemVisitor("D:\\",new FileSearcherMock());
            _fileSystemVisitor.FileFinded += _fileSystemVisitor_FileFinded;
            _fileSystemVisitor.DirectoryFinded += _fileSystemVisitor_DirectoryFinded;
            _fileSystemVisitor.FilteredFileFinded += _fileSystemVisitor_FilteredFileFinded;
            _fileSystemVisitor.FilteredDirectoryFinded += _fileSystemVisitor_FilteredDirectoryFinded;
        }

        private void _fileSystemVisitor_FilteredFileFinded(object sender, DirectoryEventArgs e)
        {
            if (e.ItemInformation.Contains(SKIPPED_FILTERED_FILE))
            {
                e.Skip = true;
            }
        }

        private void _fileSystemVisitor_FilteredDirectoryFinded(object sender, DirectoryEventArgs e)
        {
            if (e.ItemInformation.Contains(SKIPPED_FILTERED_FOLDER))
            {
                e.Skip = true;
            }
        }

        private void _fileSystemVisitor_DirectoryFinded(object sender, DirectoryEventArgs e)
        {
            if (e.ItemInformation.Contains(SKIPPED_NON_FILTERED_FOLDER))
            {
                e.Skip = true;
            }
        }

        private void _fileSystemVisitor_FileFinded(object sender, DirectoryEventArgs e)
        {
            if (e.ItemInformation.Contains(SKIPPED_NON_FILTERED_FILE))
            {
                e.Skip = true;
            }
        }
        private void _fileSystemVisitor_DirectoryFinded1(object sender, DirectoryEventArgs e)
        {
            if (e.ItemInformation.Contains(SKIPPED_NON_FILTERED_FOLDER))
            {
                e.Break = true;
            }
        }

        [Test]
        public void GetNonFilteredResult_Break()
        {
            //Arrange
            var mock = new FileSearcherMock();
            var fileSystemVisitorForBreak = new FileSystemVisitor("D:\\", mock);
            fileSystemVisitorForBreak.DirectoryFinded += _fileSystemVisitor_DirectoryFinded1;
            //Act
            var expectResult = mock.SearchFile("D:\\")
                                   .TakeWhile(i => String.Compare(SKIPPED_NON_FILTERED_FOLDER, i.Name, StringComparison.OrdinalIgnoreCase) != 0);
            var result = fileSystemVisitorForBreak.GetNonFilteredResult();
            //Assert

            Assert.True(expectResult.SequenceEqual(result));
        }

        [Test]
        public void GetFilteredResult()
        {
            //Arrange
            var condition = new Func<DirectoryItem, bool>(x=>x.Type == DirectoryTypes.Folder);
            var mock = new FileSearcherMock();
            var filteredSystemVisitor= new FileSystemVisitor("D:\\", mock, condition);
            //Act
            var expectResult = mock.SearchFile("D:\\").Where(condition);
            var result = filteredSystemVisitor.GetFilteredResult();
            //Assert
            Assert.True(expectResult.SequenceEqual(result));
        }

        [Test]
        public void GetNonFilteredResult_SkipedItems()
        {
            //Arrange
            var mock = new FileSearcherMock();
            //Act
            var directoryItems = mock.SearchFile("D:\\")
                                    .Where(i => !i.FullName.Contains(SKIPPED_NON_FILTERED_FOLDER))
                                    .Where(i=>!i.FullName.Contains(SKIPPED_NON_FILTERED_FILE));
            var result = _fileSystemVisitor.GetNonFilteredResult();
            //Assert
            Assert.True(directoryItems.SequenceEqual(result));
        }

        [Test]
        public void GetNonFilteredResult_SkipedFolders()
        {
            //Act
            var result = _fileSystemVisitor.GetNonFilteredResult();
            //Assert
            Assert.True(!result.Any(i=>i.FullName.Contains(SKIPPED_NON_FILTERED_FOLDER)));
        }

        [Test]
        public void GetNonFilteredResult_SkipedFiles()
        {
            //Act
            var result = _fileSystemVisitor.GetNonFilteredResult();
            //Assert
            Assert.True(!result.Any(i => i.FullName.Contains(SKIPPED_NON_FILTERED_FILE)));
        }
        [Test]
        public void GetFilteredResult_SkipedFolders()
        {
            //Act
            var result = _fileSystemVisitor.GetFilteredResult();
            //Assert
            Assert.True(!result.Any(i => i.FullName.Contains(SKIPPED_FILTERED_FOLDER)));
        }

        [Test]
        public void GetFilteredResult_SkipedFiles()
        {
            //Act
            var result = _fileSystemVisitor.GetFilteredResult();
            //Assert
            Assert.True(!result.Any(i => i.FullName.Contains(SKIPPED_FILTERED_FILE)));
        }
    }
}
