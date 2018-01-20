using System;

namespace FileVisitorRepository.Models
{
    public enum DirectoryTypes
    {
        Folder,
        File
    }

    public class DirectoryItem
    {
        public string FullName { get; }
        public string Name { get; }
        public DirectoryTypes Type { get; }

        public DirectoryItem(string fullName, string name, DirectoryTypes type)
        {
            FullName = fullName;
            Name = name;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DirectoryItem object2))
                return false;

            return object2.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public override string ToString()
        {
            return ($"Full name: {FullName}" +
                    $"{ Environment.NewLine}" +
                    $"Name: {Name}" +
                    $"{ Environment.NewLine}" +
                    $"Type: {Type}" +
                    $"{ Environment.NewLine}");
        }
    }
}
