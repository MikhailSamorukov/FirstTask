using FileVisitorRepository.Models;
using System;
using System.Collections.Generic;

namespace FileVisitor
{
    static class Extentions
    {
        public static IEnumerable<DirectoryItem> Filter(this IEnumerable<DirectoryItem> collection, Func<DirectoryItem, bool> filterCondition)
        {
            foreach (var item in collection)
            {
                if (filterCondition(item))
                    yield return item;
            }
        }
    }
}
