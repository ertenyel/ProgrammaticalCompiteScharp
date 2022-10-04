﻿using Combinatorics.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Solvers;
using System;

namespace ProgrammaticalCompiteScharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Считаю комбинации и коэффициенты...");

            int[] period = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            Combinations<int> combinationsList = new Combinations<int>(period, 10);            
            List<List<double>> ResultFunction = new List<List<double>>();
            List<List<List<long>>> coefficients = new List<List<List<long>>>();
            int k = 0;
            foreach (var item in combinationsList)
            {
                List<List<long>> groupCoefficinets = new List<List<long>>();
                List<double> TempResultFunction = new List<double>();
                for (int i = 0; i < item.Count; i++)
                {
                    List<long> tempCoefficients = new List<long>();
                    for (int j = 0; j < 10; j++)
                        tempCoefficients.Add((long)Math.Pow(item[i], j));

                    TempResultFunction.Add(Math.Sin(item[i]) * Math.Exp(item[i] / 2) + Math.Exp(-item[i] / 2));
                    groupCoefficinets.Add(tempCoefficients);
                }
                ResultFunction.Add(TempResultFunction);
                coefficients.Add(groupCoefficinets);
                k++;
            }

            Console.WriteLine("Решаю системы уравнений...");

            List<Vector<double>> SolutionCompute = new List<Vector<double>>();
            for (int i = 0; i < coefficients.Count; i++)
            {
                double[,] coefficientsArray = new double[10, 10];
                double[] resultFunctionArray = new double[10];
                for (int j = 0; j < coefficients[j].Count; j++)
                {
                    for (int ii = 0; ii < coefficients[i][j].Count; ii++)
                    {
                        coefficientsArray[j, ii] = coefficients[i][j][ii];
                        resultFunctionArray[ii] = ResultFunction[j][ii];
                    }
                }
                if (i == 0)
                {
                    Console.WriteLine("Один из массивов коэффициентов:");
                    for (int j = 0; j < coefficientsArray.GetLength(0); j++)
                    {
                        for (int ii = 0; ii < coefficientsArray.GetLength(1); ii++)
                        {
                            Console.Write("{0, 15}", coefficientsArray[j, ii]);
                        }
                        Console.WriteLine();
                    }                    
                    Console.WriteLine("\nОдин из массивов f(x):");
                    for (int j = 0; j < resultFunctionArray.Length; j++)
                    {
                        Console.Write(resultFunctionArray[j] + "\t");
                    }
                }
                Matrix<double> left = Matrix<double>.Build.DenseOfArray(coefficientsArray);
                Vector<double> right = Vector<double>.Build.Dense(resultFunctionArray);
                SolutionCompute.Add(left.Solve(right));
            }

            Console.WriteLine("Нахожу средние значения аппроксимации...");            
            List<List<double>> ResultNewFunction = new List<List<double>>();
            for (int i = 0; i < coefficients.Count; i++)
            {
                List<double> TempResultNewFunction = new List<double>();
                for (int j = 0; j < coefficients[i].Count; j++)
                {
                    double value = 0;
                    for (int ii = 0; ii < coefficients[i][j].Count; ii++)
                    {
                        value += SolutionCompute[j][ii] * Math.Pow(coefficients[i][j][ii], ii);
                    }
                    TempResultNewFunction.Add(value);
                }
                ResultNewFunction.Add(TempResultNewFunction);
            }
            return;
        }
    }
}
