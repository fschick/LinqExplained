﻿using System;
using System.Collections.Generic;

namespace FS.LinqExplained
{
    public static class DumpExtensions
    {
        public static void Dump(this IEnumerable<string> content, string caption = null)
        {
            caption ??= content.GetType().Name;
            Console.WriteLine(caption);
            foreach (var line in content)
                Console.WriteLine($"\t{line}");
            Console.WriteLine();
        }
    }
}
