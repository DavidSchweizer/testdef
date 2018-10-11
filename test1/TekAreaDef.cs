using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace test1
{
    public class TekAreaDef
    {
        const int MAXTEK = 5;
        List<Point> Points;
        List<Point> Deltas;
        int xSize, ySize;

        private void initLists()
        {
            Deltas = new List<Point>();
            Points = new List<Point>();
        }

        public TekAreaDef(TekAreaDef value)
        {
            initLists();
            foreach (Point point in value.Points)
                Points.Add(new Point(point.X, point.Y));
            foreach (Point point in value.Deltas)
                Deltas.Add(new Point(point.X, point.Y));
            ComputeSize();
        }

        public TekAreaDef(params Point[] values)
        {
            initLists();
            foreach (Point value in values)
            {
                if (Points.Count == MAXTEK)
                    throw new Exception(String.Format("Too many values for area: already {0} fields present", Points.Count));
                Points.Add(value);
            }
            ComputeDeltas();
            ComputeSize();
        }

        private void ComputeDeltas()
        {
            Point P = Points[0];
            Deltas.Clear();
            for (int i = 1; i < Points.Count; i++)
                Deltas.Add(new Point(Points[i].X - P.X, Points[i].Y - P.Y));
        }

        private void ComputeSize()
        {
            if (Points.Count() == 1)
            {
                xSize = 1;
                ySize = 1;
                return;
            }
            xSize = 1 + xMaximum() - xMinimum();
            ySize = 1 + yMaximum() - yMinimum();
        }

        private int xMinimum()
        {
            int result = Int32.MaxValue;
            foreach (Point value in Points)
            {
                if (value.X < result)
                    result = value.X;
            }
            return result;
        }

        private int yMinimum()
        {
            int result = Int32.MaxValue;
            foreach (Point value in Points)
            {
                if (value.Y < result)
                    result = value.Y;
            }
            return result;
        }

        private int xMaximum()
        {
            int result = -1;
            foreach (Point value in Points)
            {
                if (value.X > result)
                    result = value.X;
            }
            return result;
        }

        private int yMaximum()
        {
            int result = -1;
            foreach (Point value in Points)
            {
                if (value.Y > result)
                    result = value.Y;
            }
            return result;
        }

        private int Compare(Point p1, Point p2)
        {
            if (p1.X < p2.X)
                return -1;
            else if (p1.X > p2.X)
                return 1;
            else if (p1.Y < p2.Y)
                return -1;
            else if (p1.Y > p2.Y)
                return 1;
            else return 0;
        }

        private void Swap(List<Point> list, int index1, int index2)
        {
            Point tem = new Point(list[index1].X, list[index1].Y);
            list[index1] = list[index2];
            list[index2] = tem;
        }

        public void Shift(int DeltaX, int DeltaY)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = new Point(Points[i].X + DeltaX, Points[i].Y + DeltaY);
            }
        }

        public TekAreaDef Normalized()
        {
            TekAreaDef result = new TekAreaDef(this);
            for (int i = 0; i < Points.Count; i++) // simple bubblesort
                for (int j = i + 1; j < Points.Count; j++)
                {
                    switch (Compare(result.Points[i], result.Points[j]))
                    {
                        case 1:
                            Swap(result.Points, i, j);
                            break;
                        case 0:
                        case -1: break;
                    }
                }
            result.ComputeDeltas();
            result.Shift(-result.xMinimum(), -result.yMinimum());
            return result;
        }

        public bool Equals(TekAreaDef other)
        {
            TekAreaDef N1 = this.Normalized();
            TekAreaDef N2 = other.Normalized();
            if (N1.Points.Count != N2.Points.Count)
                return false;
            for (int i = 0; i < N1.Points.Count; i++)
                if (N1.Points[i].X != N2.Points[i].X || N1.Points[i].Y != N2.Points[i].Y)
                    return false;
            return true;
        }

        static public StreamWriter DebugLog = new StreamWriter("c:/temp/debug.log");
        public void Debug(string format, params object[] items)
        {
            DebugLog.WriteLine(format, items);
            DebugLog.Flush();
        }

        public TekAreaDef FlipHorizontal()
        {
            TekAreaDef result = this.Normalized();
            result.Dump(DebugLog, "before flip");
            for (int i = 0; i < Points.Count / 2; i ++)
            {                
                int i2 = Points.Count - 1 - i;
                Swap(result.Points, i, i2);
                result.Dump(DebugLog, String.Format("after i = {0},  i2 = {1}", i, i2));
            }
            result.ComputeDeltas();
            result.ComputeSize();
            Debug("flipped");
            return result;
        }

        public TekAreaDef Rotate90()
        {
            TekAreaDef result = new TekAreaDef(this.Points[0]);
            foreach (Point value in this.Deltas)
            {
                result.Deltas.Add(new Point(-value.Y, value.X));
                Point p = result.Deltas.ElementAt(result.Deltas.Count - 1);
                result.Points.Add(new Point(this.Points[0].X + p.X, this.Points[0].Y + p.Y));
            }
            result.ComputeSize();
            return result.Normalized();
        }

        public TekAreaDef Rotate180()
        {
            return Rotate90().Rotate90();
        }

        public void Dump(StreamWriter sw, string msg = "")
        {
            sw.Write(String.Format("{0}  [(size:({1},{2})]\n\tpoints: ", msg, xSize, ySize));
            foreach (Point value in Points)
            {
                sw.Write(" ({0},{1})", value.X, value.Y);
            }
            sw.Write("\n\tdeltas: ");
            foreach (Point value in Deltas)
            {
                sw.Write(" ({0},{1})", value.X, value.Y);
            }
            sw.WriteLine();
        }
    }

    class TekStandardAreas
    {
        List<TekAreaDef> values;

        private void Add1FieldAreas()
        {
            values.Add(new TekAreaDef(new Point(0,0)));
        }
        private void Add2FieldAreas()
        {
            TekAreaDef newvalue = new TekAreaDef(new Point(0, 0), new Point(0, 1));
            values.Add(newvalue);
            values.Add(newvalue.Rotate180());
        }
        private void Add3FieldAreas()
        {
            TekAreaDef newvalue = new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2));
            values.Add(newvalue);
            values.Add(newvalue.Rotate180());
            // cornered
            newvalue = new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(1, 0));
            values.Add(newvalue);
            newvalue = newvalue.Rotate90();
            values.Add(newvalue);
            newvalue = newvalue.Rotate90();
            values.Add(newvalue);
            newvalue = newvalue.Rotate90();
            values.Add(newvalue);
        }

        private void Add4FieldAreas()
        {
            // straight
            TekAreaDef newvalue = new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3));
            values.Add(newvalue);
            values.Add(newvalue.Rotate180());
        }
        private void Add5FieldAreas()
        {
            // straight
            TekAreaDef newvalue = new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0,4));
            values.Add(newvalue);
            values.Add(newvalue.Rotate180());
        }

        public TekStandardAreas()
        {
            values = new List<TekAreaDef>();
            Add1FieldAreas();
            Add2FieldAreas();
            Add3FieldAreas();
            Add4FieldAreas();
            Add5FieldAreas();
        }
    }
}
