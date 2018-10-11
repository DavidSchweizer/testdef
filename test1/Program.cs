using System;
using System.Drawing;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
            sw.AutoFlush = true;
            Console.SetOut(sw);
            //TekAreaDef T1 = new TekAreaDef(new Point(3, 4));
            //T1.Normalized().Dump(sw, "T1");
            //T1.FlipHorizontal().Dump(sw, "T1 flipped");
            //TekAreaDef T2 = new TekAreaDef(new Point(3, 4), new Point(4, 4), new Point(5, 4));
            //T2.Normalized().Dump(sw, "T2");
            //T2.FlipHorizontal().Dump(sw, "T2 flipped");
            TekAreaDef T3 = new TekAreaDef(new Point(3, 4), new Point(4, 4), new Point(5, 4),
                new Point(6, 4), new Point(6, 5)
                );
            T3.Normalized().Dump(sw, "T3");
            T3.FlipHorizontal().Dump(sw, "T3 flipped");
            //TekAreaDef T4 = new TekAreaDef(new Point(3, 4), new Point(3, 5), new Point(3, 6),
            //    new Point(3, 7), new Point(2, 7)
            //    );
            //T4.Normalized().Dump(sw, "T4");
            //T4.FlipHorizontal().Dump(sw, "T4 flipped");
            //TekAreaDef T4b = T4.Normalized();
            //T4b.Normalized().Dump(sw, "T4b");
            //T4b.FlipHorizontal().Dump(sw, "T4b flipped");

            //Console.WriteLine();
            
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



            Console.ReadKey();
        }
    }
}
