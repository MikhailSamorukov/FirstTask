using System;

namespace FileVisitor
{
    public class DirectoryEventArgs : EventArgs
    {
        public bool Skip { get; set; }
        public bool Break { get; set; }
        public string ItemInformation { get; set; }

        public DirectoryEventArgs(bool skip, bool @break, string itemInformation)
        {
            Skip = skip;
            Break = @break;
            ItemInformation = itemInformation;
        }
    }
}
