using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Svg;
using System.IO;

namespace Entities
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\test.svg");

            var sampleDoc = SvgDocument.Open(filePath );

            sampleDoc.Draw().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\test.png"));
        }
    }
}
