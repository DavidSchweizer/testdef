using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter(Console.OpenStandardOutput(), System.Text.Encoding.UTF8);
            sw.AutoFlush = true;
            Console.SetOut(sw);
            //TekAreaDef T1 = new TekAreaDef(new Point(3, 4));
            //T1.Normalized().Dump(sw, "T1");
            //T1.FlipHorizontal().Dump(sw, "T1 flipped");
            //TekAreaDef T2 = new TekAreaDef(new Point(3, 4), new Point(4, 4), new Point(5, 4));
            //T2.Normalized().Dump(sw, "T2");
            //T2.Normalized().MatrixDump(sw);

            //T2.FlipHorizontal().Dump(sw, "T2 flipped");
            //TekAreaDef T3 = new TekAreaDef(new Point(3, 4), new Point(4, 4), new Point(5, 4),
            //    new Point(6, 4), new Point(6, 5)
            //    ).Normalized();
            //sw.WriteLine("T3:");
            //T3.DumpAsAsciiArt(sw);
            //sw.WriteLine("T3 h flipped:");
            //T3.FlipHorizontal().DumpAsAsciiArt(sw);
            //sw.WriteLine("T3 v flipped:");
            //T3.FlipVertical().DumpAsAsciiArt(sw);
            //////T3.MatrixDump(sw);

            ////T3.FlipHorizontal().Dump(sw, "T3 flipped");
            //TekAreaDef T4 = new TekAreaDef(new Point(3, 4), new Point(3, 5), new Point(3, 6),
            //    new Point(3, 7), new Point(2, 7)
            //    ).Normalized();

            //sw.WriteLine("\nT4:");
            //T4.DumpAsAsciiArt(sw);
            //sw.WriteLine("alternatives:");
            //List<TekAreaDef> T4L = T4.GetAlternatives();
            //foreach (TekAreaDef area in T4L)
            //{
            //    sw.WriteLine();
            //    area.DumpAsAsciiArt(sw);
            //}
            //sw.WriteLine("end alternatives");

            //T4.Normalized().MatrixDump(sw);

            //T4.FlipHorizontal().Dump(sw, "T4 flipped");
            //TekAreaDef T4c = T4b.Rotate90();
            //T4c.Dump(sw, "T4c");
            //T4c.MatrixDump(sw);
            //T4b.Normalized().Dump(sw, "T4b");
            //sw.WriteLine("T4 h flipped:");
            //T4.FlipHorizontal().DumpAsAsciiArt(sw);
            //sw.WriteLine("T4 v flipped:");
            //T4.FlipVertical().DumpAsAsciiArt(sw);

            ////Console.WriteLine();

            //TekAreaDef T5 = T3.Normalized().Rotate90();
            //TekAreaDef T6 = T5.Rotate90();
            //TekAreaDef T7 = T6.Rotate90();
            //TekAreaDef T8 = T7.Rotate90();
            //T3.Normalized().Dump(sw, "T3-normalized");
            //T5.Dump(sw, "T5");
            //T6.Dump(sw, "T6");
            //T7.Dump(sw, "T7");
            //T8.Dump(sw, "T8");
            //Console.WriteLine();
            //T3.Dump(sw, "T3");
            //if (T3.Equals(T8))
            //    sw.WriteLine("T3 equals T8");
            //else sw.WriteLine("T3 not equal T8");
            //if (T3.Equals(T7))
            //    sw.WriteLine("T3 equals T7");
            //else sw.WriteLine("T3 not equal T7");

            sw = new StreamWriter("areas.dmp");
            TekStandardAreas areas = new TekStandardAreas();
            for (int n = 1; n <= 5; n++)
            {
                sw.WriteLine(String.Format("nr points: {0}", n));
                for (int i = 0; i < areas.nCount(n); i++)
                {
                    areas.GetValue(i, n).DumpAsAsciiArt(sw);
                    sw.WriteLine();
                }
            }

            Console.ReadKey();
        }
    }
}
