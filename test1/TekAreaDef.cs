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
            Update();
        }

        public int PointCount { get { return Points.Count;  } }

        public TekAreaDef(params Point[] values)
        {
            initLists();
            foreach (Point value in values)
            {
                if (Points.Count == MAXTEK)
                    throw new Exception(String.Format("Too many values for area: already {0} fields present", Points.Count));
                Points.Add(value);
            }
            Update();
        }

        private void ComputeDeltas()
        {
            Deltas.Clear();
            if (Points.Count == 0)
                return;
            Point P = Points[0];
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

        private void Update()
        {
            ComputeSize();
            ComputeDeltas();
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

        private void AddPoint(Point P)
        {
            Points.Add(P);
            Update();
        }

        public TekAreaDef FlipVertical()
        {
            TekAreaDef result = new TekAreaDef();

            int yMax = yMaximum();
            for (int i = 0; i < Points.Count; i++)
            {
                Point P = Points[i];
                result.AddPoint(new Point(P.X, yMax - P.Y));
            }
            return result;
        }

        public TekAreaDef FlipHorizontal()
        {
            TekAreaDef result = new TekAreaDef();
            int xMax = xMaximum();
            for (int i = 0; i < Points.Count; i++)
            {
                Point P = Points[i];
                result.AddPoint(new Point(xMax - P.X, P.Y));
            }
            return result;
        }

        public TekAreaDef Rotate90()
        {
            TekAreaDef result = new TekAreaDef(this.Points[0]);
            foreach (Point value in this.Deltas)
            {
                result.Deltas.Add(new Point(-value.Y, value.X));
                Point p = result.Deltas.ElementAt(result.Deltas.Count - 1);
                result.AddPoint(new Point(this.Points[0].X + p.X, this.Points[0].Y + p.Y));
            }
            return result;
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

        public bool IsInList(List<TekAreaDef> list)
        {
            foreach (TekAreaDef value in list)
                if (this.Equals(value))
                    return true;
            return false;
        }

        private void AddAlternative(TekAreaDef area, List<TekAreaDef> list)
        {
            if (!area.IsInList(list))
                list.Add(area);
            TekAreaDef area2 = area.FlipHorizontal();
            if (!area2.IsInList(list))
                list.Add(area2);
            area2 = area.FlipVertical();
            if (!area2.IsInList(list))
                list.Add(area2);
        }

        public List<TekAreaDef> GetAlternatives()
        {
            List<TekAreaDef> result = new List<TekAreaDef>();
            AddAlternative(this.Normalized(), result);
            // rotations
            AddAlternative(this.Rotate90(), result);
            AddAlternative(this.Rotate180(), result);
            AddAlternative(this.Rotate180().Rotate90(), result);
            return result;
        }

        public string[] AsAsciiArt(char NoChar = '.', char ShowChar = 'X')
        {
            string[] result = new string[ySize];
            for (int i = 0; i < ySize; i++)
                result[i] = new String(NoChar, xSize);
            for (int i = 0; i < Points.Count; i++)
            {
                int xPos = Points[i].X - xMinimum();
                int yPos = Points[i].Y - yMinimum();
                result[yPos] = result[yPos].Substring(0, xPos) + ShowChar + 
                    result[yPos].Substring(xPos + 1, xSize - xPos-1);
            }
            return result;
        }

        public void DumpAsAsciiArt(StreamWriter sw, char NoChar = '.', char ShowChar = 'X')
        {
            string[] art = AsAsciiArt(NoChar, ShowChar);
            foreach (string s in art)
                sw.WriteLine(s);
            sw.Flush();
        }

    }

    class TekStandardAreas
    {
        private List<TekAreaDef> values;

        public TekAreaDef GetValue(int i) 
        {
            return values[i];
        }
        public int Count { get { return values.Count; } }
        public int nCount(int nFields)
        {
            int result = 0;
            foreach (TekAreaDef value in values)
                if (value.PointCount == nFields)
                    result++;
            return result;
        }

        public TekAreaDef GetValue(int i, int nFields)
        {
            int j = i;
            TekAreaDef result = null;
            foreach(TekAreaDef value in values)
            {
                if (value.PointCount != nFields)
                    continue;
                if (j == 0)
                {
                    result = value;
                    break;
                }
                j--;
            }
            return result;
        }

        private void AddAlternatives(TekAreaDef value)
        {
            foreach (TekAreaDef val in value.GetAlternatives())
            if (!val.IsInList(values))
                values.Add(val);
        }
        private void Add1FieldAreas()
        {
            AddAlternatives(new TekAreaDef(new Point(0, 0)));           
        }
        private void Add2FieldAreas()
        {
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1)));
        }
        private void Add3FieldAreas()
        {
            // straight
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2)));
            // cornered
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(1, 0)));
        }
        private void Add4FieldAreas()
        {
            // straight
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3)));
            // cornered 1
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 2)));
            // cornered 2
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 1)));
            // big square
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(1, 0), new Point(1, 1)));
        }
        private void Add5FieldAreas()
        {
            // straight
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0,4)));
            // cornered 1
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(1, 3)));
            // cornered 2
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 1), new Point(1, 2)));
            // cornered 3
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 2), new Point(2, 2)));
            // cornered 4
            AddAlternatives(new TekAreaDef(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 1), new Point(2, 1)));
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
