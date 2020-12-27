using Radyn.Evaluation.Models;
using Radyn.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radyn.Evaluation
{
    public class AHP
    {

        private static Matix PrepareMatrix(Dictionary<string, double> dictionary)
        {
            dictionary = dictionary.OrderByDescending(x => x.Value).ToDictionary(f => f.Key, f => f.Value);
            var mastermatix = new Matix(1, dictionary.Count);
            var matix = new Matix(dictionary.Count, dictionary.Count);
            var yIndex = 1;
            foreach (var keyValuePair in dictionary)
            {
                mastermatix[1, yIndex].Value = keyValuePair.Value;
                mastermatix[1, yIndex].Key = keyValuePair.Key;
                yIndex++;
            }
            foreach (var cell in matix)
            {
                double? value = null;
                if (cell.X < cell.Y)
                {
                    var matrixNode = mastermatix[1, cell.X];
                    if (matrixNode != null)
                    {
                        var node = mastermatix[1, cell.Y];
                        if (node != null)
                            value = (matrixNode.Value - node.Value) + 1;
                    }
                }
                matix[cell.X, cell.Y].Value = value;
                matix[cell.X, cell.Y].Key = mastermatix[1, cell.Y].Key;
            }

            return InvertMatrix(matix);
        }
        private static Matix InvertMatrix(Matix matix)
        {
            foreach (var matrixNode in matix)
            {
                var x = matrixNode.X;
                matrixNode.X = matrixNode.Y;
                matrixNode.Y = x;
                if (matrixNode.X == matrixNode.Y)
                    matrixNode.Value = 1;
            }
            foreach (var matrixNode in matix)
            {
                if (matrixNode.Value.HasValue) continue;
                var oneNodeValue = matix.FindEqualNodeXElement(matrixNode.Y);
                var firstOrDefault = matix[matrixNode.Y, matrixNode.X];
                if (oneNodeValue == null || firstOrDefault == null) continue;
                var value = (oneNodeValue.Value / firstOrDefault.Value);
                matrixNode.Value = (double?)Math.Round((value ?? 0), 3);
            }

            return matix;

        }
        public static Dictionary<string, double> Calculation(Dictionary<string, double> dictionary)
        {
            try
            {

                var calculation = new Dictionary<string, double>();
                if (!dictionary.Any()) return calculation;
                calculation = PreCalculation(dictionary);
                var sum = calculation.Sum(x => x.Value);
                foreach (var f in calculation.Keys.ToList())
                {
                    calculation[f] = calculation[f] / sum;
                }
                return calculation;
            }
            catch (KnownException ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw;

            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex.InnerException);
            }

        }

        public static Dictionary<string, double> PreCalculation(Dictionary<string, double> dictionary)
        {
            try
            {

                var calculation = new Dictionary<string, double>();
                if (!dictionary.Any()) return calculation;
                var matrix = PrepareMatrix(dictionary);
                for (int i = 1; i <= matrix.Max(x => x.X); i++)
                {
                    double val = 1;
                    for (int j = 1; j <= matrix.Max(x => x.Y); j++)
                    {
                        val = (double)(val * (matrix[j, i].Value ?? 0));
                    }
                    double max = matrix.Max(x => x.X);
                    var valpow = 1 / max;
                    var ebbvalue = (double)Math.Pow(val, valpow);
                    calculation.Add(matrix[i, 1].Key, (double)Math.Round(ebbvalue, 6));

                }
                return calculation;
            }
            catch (KnownException ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw;

            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex.InnerException);
            }

        }
        public static double GetWeight(Dictionary<string, double> dictionary)
        {
            try
            {
                var val = dictionary.Aggregate<KeyValuePair<string, double>, double>(1, (current, f) => current * f.Value);
                var valpow = 1 / (double)dictionary.Count;
                var ebbvalue = (double)Math.Pow(val, valpow);
                return (double)Math.Round(ebbvalue, 6);
            }
            catch (KnownException ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw;

            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex.InnerException);
            }

        }
    }
}
