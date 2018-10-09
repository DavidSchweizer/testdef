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
            TekAreaDef T1 = new TekAreaDef(new Point(3, 4));
            T1.Dump(sw);
            TekAreaDef T2 = new TekAreaDef(new Point(3, 4), new Point(1, 0), new Point(2, 0));
            T2.Dump(sw);
            TekAreaDef T3 = new TekAreaDef(new Point(3, 4), new Point(1, 0), new Point(2, 0),
                new Point(3, 0), new Point(3, 1), new Point(4, 0)
                );
            T3.Dump(sw);
            TekAreaDef T4 = new TekAreaDef(new Point(3, 4), new Point(0, 1), new Point(0, 2),
                new Point(0, 3), new Point(-1, 3), new Point(0, 4)
                );
            T4.Dump(sw);

            Console.WriteLine();
            Console.WriteLine();
            TekAreaDef T5 = T3.Rotate90();
            TekAreaDef T6 = T5.Rotate90();
            TekAreaDef T7 = T6.Rotate90();
            TekAreaDef T8 = T7.Rotate90();
            T3.Dump(sw);
            T5.Dump(sw);
            T6.Dump(sw);
            T7.Dump(sw);
            T8.Dump(sw);



            Console.ReadKey();
        }
    }
}
