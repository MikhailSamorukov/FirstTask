using System;
using FileVisitor;
using FileVisitorRepository;
using FileVisitorRepository.Models;

namespace ConsoleApp3
{
    class Program
    {
        private static void Main()
        {
            var s = new FileSystemVisitor("D:\\",new FileSearcher(), i => i.Type.Equals(DirectoryTypes.Folder));
            s.Start += S_Start;
            s.Finish += S_Finish;
            s.FileFinded += SFileFinded;
            s.DirectoryFinded += SDirectoryFinded;
            s.FilteredFileFinded += SFilteredFileFinded;
            s.FilteredDirectoryFinded += SFilteredDirectoryFinded;

            var result = s.GetNonFilteredResult();
            Console.WriteLine();
            Console.WriteLine("Results: ");
            foreach (var item in result)
            {
                Console.WriteLine(item.ToString());
                Console.WriteLine(new string('-',50));
            }
            Console.ReadLine();
        }

        private static void SetToStateAgruments(DirectoryEventArgs e)
        {
            Console.WriteLine("Information: ");
            Console.WriteLine(e.ItemInformation);
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Do you want to continue?");
            Console.WriteLine("s - skip");
            Console.WriteLine("x - break");
            Console.WriteLine("other button - continue");
            var cki = Console.ReadKey();
            if (cki.KeyChar == 's')
                e.Skip = true;

            if (cki.KeyChar == 'x')
                e.Break = true;

            Console.WriteLine();
        }

        private static void S_Start(object sender, EventArgs e)
        {
            Console.WriteLine("Start");
            Console.WriteLine(new string('-', 50));
        }

        private static void S_Finish(object sender, EventArgs e)
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("End");
        }

        private static void SFileFinded(object sender, DirectoryEventArgs e)
        {
            Console.WriteLine("File found");
            SetToStateAgruments(e);
        }

        private static void SDirectoryFinded(object sender, DirectoryEventArgs e)
        {
            Console.WriteLine("Directory found");
            SetToStateAgruments(e);
        }

        private static void SFilteredFileFinded(object sender, DirectoryEventArgs e)
        {
            Console.WriteLine("Filtered file found");
            SetToStateAgruments(e);
        }

        private static void SFilteredDirectoryFinded(object sender, DirectoryEventArgs e)
        {
            Console.WriteLine("Filtered directory found");
            SetToStateAgruments(e);
        }
    }
}
