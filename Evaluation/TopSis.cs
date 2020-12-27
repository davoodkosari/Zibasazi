using Radyn.Evaluation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Radyn.Evaluation
{
    public class TopSis
    {

        public static List<TopSisReturnModel> CalcTopsis(List<TopSisModel> dictionary, Dictionary<string, double> formScores)
        {

            Matix matrixa = PrepareMatrix(dictionary);
            if (formScores.Count != matrixa.YCount)
            {
                throw new Exception("data is not valid");
            }

            Matix matrixb = NormalScores(formScores);
            Matix basematrix = MultiplicationMatrix(matrixa, matrixb);

            Matix aplus = GetBestValueColum(basematrix, true);
            Matix amin = GetBestValueColum(basematrix, false);


            Matix dplus = NormalBestValues(basematrix, aplus, true);
            Matix dmin = NormalBestValues(basematrix, amin, false);

            return FinalResult(dictionary, dplus, dmin);

        }

        private static List<TopSisReturnModel> FinalResult(List<TopSisModel> sisModels, Matix dplus, Matix dmin)
        {
            Matix matrixNodes = new Matix(dplus.Xcount, 1);
            foreach (MatrixNode matrixNode in matrixNodes)
            {
                double f = ((dplus[matrixNode.X, 1].Value ?? 0) + (dmin[matrixNode.X, 1].Value ?? 0));
                double value = (dmin[matrixNode.X, 1].Value ?? 0) / f;
                matrixNode.Value = (double)Math.Round(value, 3);
            }
            List<TopSisReturnModel> outdictionary = new List<TopSisReturnModel>();
            int grad = 1;
            foreach (MatrixNode matrixNode in matrixNodes.Where(x => x.Value > 0).OrderByDescending(x => x.Value).ToList())
            {
                outdictionary.Add(new TopSisReturnModel() { RefId = sisModels[matrixNode.X - 1].RefId, Rank = grad, Score = Math.Round(matrixNode.Value ?? 0, 3), });
                grad++;
            }
            foreach (MatrixNode matrixNode in matrixNodes.Where(x => x.Value == null || x.Value <= 0).ToList())
            {
                outdictionary.Add(new TopSisReturnModel() { RefId = sisModels[matrixNode.X - 1].RefId, Rank = outdictionary.Max(x => x.Rank) + 1, Score = matrixNode.Value, });

            }
            return outdictionary;
        }

        private static Matix NormalBestValues(Matix basematrix, Matix matix, bool plus)
        {
            Matix matix1 = new Matix(basematrix.Xcount, 1);
            foreach (MatrixNode matrixNode in matix1)
            {
                double sum = 0;
                List<MatrixNode> equalNodesXElement = basematrix.FindEqualNodesXElement(matrixNode.X);
                if (plus)
                {
                    foreach (MatrixNode node in equalNodesXElement)
                    {
                        double sumrow = ((node.Value ?? 0) - (matix[1, node.Y].Value ?? 0));
                        sum += Math.Pow(sumrow, 2);
                    }
                }
                else
                {
                    foreach (MatrixNode node in equalNodesXElement)
                    {
                        double sumrow = ((node.Value ?? 0) - (matix[1, node.Y].Value ?? 0));
                        sum += Math.Pow(sumrow, 2);
                    }
                }
                double val = Math.Sqrt(sum);
                matrixNode.Value = (double?)val;

            }
            return matix1;
        }

        private static Matix GetBestValueColum(Matix matix, bool plus)
        {
            Matix matix1 = new Matix(1, matix.YCount);
            foreach (MatrixNode matrixNode in matix1)
            {
                List<MatrixNode> equalNodesYElement = matix.FindEqualNodesYElement(matrixNode.Y);
                matrixNode.Value = plus ? equalNodesYElement.Max(x => x.Value) : equalNodesYElement.Min(x => x.Value);
            }
            return matix1;
        }

        private static Matix MultiplicationMatrix(Matix matixa, Matix matixb)
        {
            Matix matix1 = new Matix(matixa.Xcount, matixb.YCount);
            foreach (MatrixNode matrixNode in matix1)
            {
                List<MatrixNode> equalNodesXElement = matixa.FindEqualNodesXElement(matrixNode.X);
                List<MatrixNode> findEqualNodesYElement = matixb.FindEqualNodesYElement(matrixNode.Y);
                double val = 0;
                for (int i = 0; i < matixa.YCount; i++)
                {
                    double f = (equalNodesXElement[i].Value ?? 0) * (findEqualNodesYElement[i].Value ?? 0);
                    val += f;
                }
                matrixNode.Value = (double?)val;

            }
            return matix1;
        }
        private static Matix NormalScores(Dictionary<string, double> formScores)
        {
            List<KeyValuePair<string, double>> keyValuePairs = formScores.ToList();
            Matix matix1 = new Matix(formScores.Count, formScores.Count);
            for (int i = 1; i <= matix1.Xcount; i++)
            {
                for (int j = 1; j <= matix1.YCount; j++)
                {
                    matix1[i, j].Key = keyValuePairs[j - 1].Key;
                    matix1[i, j].Value = i == j ? keyValuePairs[j - 1].Value : 0;
                }
            }
            return matix1;
        }


        private static Matix PrepareMatrix(List<TopSisModel> dictionary)
        {

            int max = 1;
            for (int i = 1; i <= dictionary.Count; i++)
            {
                if (dictionary[i - 1].Scoreses.Count > max)
                {
                    max = dictionary[i - 1].Scoreses.Count;
                }
            }
            Matix matix = new Matix(dictionary.Count, max);
            for (int i = 1; i <= dictionary.Count; i++)
            {
                for (int j = 1; j <= max; j++)
                {
                    matix[i, j].Value = dictionary[i - 1].Scoreses.ToList()[j - 1].Value;
                    matix[i, j].Key = dictionary[i - 1].Scoreses.ToList()[j - 1].Key;
                }
            }
            MatrixNormal(matix);
            return matix;

        }
        private static void MatrixNormal(Matix matix)
        {
            Matix matix1 = new Matix(matix.Xcount, matix.YCount);
            foreach (MatrixNode matrixNode in matix)
            {
                List<MatrixNode> findEqualNodesYElement = matix.FindEqualNodesYElement(matrixNode.Y);
                double sum = 0;
                foreach (MatrixNode node in findEqualNodesYElement)
                {
                    sum += Math.Pow(node.Value ?? 0, 2);
                }

                double pow = Math.Sqrt(sum);
                double value = matrixNode.Value ?? 0;
                if (value > 0)
                {
                    double result = value / pow;
                    matix1[matrixNode.X, matrixNode.Y].Value = (double?)result;
                }
                else
                {
                    matix1[matrixNode.X, matrixNode.Y].Value = 0;
                }
            }
            foreach (MatrixNode matrixNode in matix1)
            {
                matix[matrixNode.X, matrixNode.Y].Value = matrixNode.Value;
            }
        }
    }
}
