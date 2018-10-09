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
        Point TopLeft;
        List<Point> Deltas;
        int xSize, ySize;

        public TekAreaDef(Point topLeft, params Point[] deltas)
        {
            TopLeft = topLeft;
            Deltas = new List<Point>();
            foreach(Point value in deltas)
            {
                if (Deltas.Count == MAXTEK)
                    break;                    
                Deltas.Add(value);
            }
            ComputeSize();
        }

        private void ComputeSize()
        {
            if (Deltas.Count()==0)
            {
                xSize = 1;
                ySize = 1;
                return;
            }
            int xMin = MAXTEK, yMin = MAXTEK, xMax = 0, yMax = 0;
            foreach(Point value in Deltas)
            {
                if (value.X < xMin)
                    xMin = value.X;
                if (value.X > xMax)
                    xMax = value.X;
                if (value.Y < yMin)
                    yMin = value.Y;
                if (value.Y > yMax)
                    yMax = value.Y;
            }
            xSize = 1 + xMax - xMin;
            ySize = 1 + yMax - yMin;
        }

        public TekAreaDef Rotate90()
        {
            TekAreaDef result = new TekAreaDef(this.TopLeft);
            foreach(Point value in this.Deltas)
            {
                result.Deltas.Add(new Point(-value.Y, value.X));
            }
            result.ComputeSize();
            return result;
        }

        public void Dump(StreamWriter sw)
        {
            sw.Write(String.Format("Area: [{0},{1} (size:({2},{3})]", TopLeft.X, TopLeft.Y, xSize, ySize));
            foreach(Point value in Deltas)
            {
                sw.Write(" ({0},{1})", value.X, value.Y);
            }
            sw.WriteLine();
        }
    }

    public class SimpleMatrix
    {
        private double[,] values;
        public int xSize { get { return values.GetLength(0); } }
        public int ySize { get { return values.GetLength(1); } }

        public double GetValue(int i, int j)
        {
            return values[i, j];
        }
        public void SetValue(int i, int j, double value)
        {
            values[i, j] = value;
        }

        public SimpleMatrix(int xSize, int ySize)
        {
            values = new double[xSize, ySize];
        }

        public SimpleMatrix Multiply(SimpleMatrix matrix)
        {
            SimpleMatrix result = new SimpleMatrix(this.xSize, matrix.ySize);
            for (int i = 0; i < result.xSize; i++)
            {
                for (int j = 0; j < result.ySize; j++)
                {
                    result.values[i, j] = 0;
                    for (int k = 0; k < this.ySize; k++)
                        result.values[i, j] += this.values[i, k] * matrix.values[k, j];
                }
            }
            return result;
        }

        public SimpleMatrix Rotate90()
        {
            SimpleMatrix rotationMatrix = new SimpleMatrix(this.ySize, this.ySize);
            rotationMatrix.SetValue(0, 0, 0);
            rotationMatrix.SetValue(0, 1, -1);
            rotationMatrix.SetValue(1, 0, 1);
            rotationMatrix.SetValue(1, 1, 0);
            return Multiply(rotationMatrix);
        }

        public void Dump(StreamWriter sw)
        {
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                    sw.Write(String.Format("{0,2} ", values[i, j]));
                sw.WriteLine();
            }
        }
    }
}
