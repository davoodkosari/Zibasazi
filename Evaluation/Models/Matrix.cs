using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.Evaluation.Models
{
    public class MatrixNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double? Value { get; set; }

        public string Key { get; set; }
    }

    public class Matix : List<MatrixNode>
    {
        public int Xcount { get; set; }
        public int YCount { get; set; }
        public Matix()
        {

        }
        public Matix(int x, int y)
        {
            Xcount = x;
            YCount = y;
            this.Make(x, y);
        }
        public void Make(int x, int y)
        {
            Xcount = x;
            YCount = y;
            for (int i = 1; i <= x; i++)
            {
                for (int j = 1; j <= y; j++)
                {
                    this.Add(new MatrixNode() { X = i, Y = j });
                }
            }
        }

        public MatrixNode this[int x, int y]
        {
            get { return this.FindNode(x, y); }
            set
            {
                var node = this.FindNode(x, y);
                node.Value = value.Value;
                node.Key = value.Key;
            }
        }
        public MatrixNode FindEqualNodeXElement(int x)
        {
            var nod = this.FirstOrDefault(node => node.X == x && node.Y == node.X);
            if (nod == null)
                throw new Exception(string.Format("There is no equal node in  {0} position", x));
            return nod;
        }
        public MatrixNode FindEqualNodeYElement(int y)
        {
            var nod = this.FirstOrDefault(node => node.Y == y && node.Y == node.X);
            if (nod == null)
                throw new Exception(string.Format("There is no equal node in {0} position", y));
            return nod;
        }
        public List<MatrixNode> FindEqualNodesYElement(int y)
        {
            return this.Where(node => node.Y == y).ToList();

        }
        public List<MatrixNode> FindEqualNodesXElement(int x)
        {
            return this.Where(node => node.X == x).ToList();

        }
        private MatrixNode FindNode(int x, int y)
        {
            var nod = this.FirstOrDefault(node => node.X == x && node.Y == y);
            if (nod == null)
                throw new Exception(string.Format("There is no node in {0},{1} position", x, y));
            return nod;
        }
    }
}
