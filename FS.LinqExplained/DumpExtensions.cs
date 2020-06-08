using System;
using System.Collections.Generic;

namespace FS.LinqExplained
{
    public static class DumpExtensions
    {
        public static void Dump(this IEnumerable<string> content)
        {
            Console.WriteLine(content.GetType().Name);
            foreach (var line in content)
                Console.WriteLine($"\t{line}");
            Console.WriteLine();
        }
    }
}
