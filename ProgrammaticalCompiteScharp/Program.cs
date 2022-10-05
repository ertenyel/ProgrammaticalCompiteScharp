using Combinatorics.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace ProgrammaticalCompiteScharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(">> Считаю комбинации и коэффициенты...\n");

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

            Console.WriteLine("\n>> Решаю системы уравнений...\n");

            List<Vector<double>> SolutionCompute = new List<Vector<double>>();
            for (int i = 0; i < coefficients.Count; i++)
            {
                double[,] coefficientsArray = new double[10, 10];
                double[] resultFunctionArray = new double[10];
                for (int j = 0; j < coefficients[i].Count; j++)
                {
                    for (int ii = 0; ii < coefficients[i][j].Count; ii++)
                    {
                        coefficientsArray[j, ii] = coefficients[i][j][ii];
                    }
                    resultFunctionArray[j] = ResultFunction[i][j];
                }
                if (i == 0)
                {
                    Console.WriteLine("\n>> Один из массивов коэффициентов:\n");
                    for (int j = 0; j < coefficientsArray.GetLength(0); j++)
                    {
                        for (int ii = 0; ii < coefficientsArray.GetLength(1); ii++)
                        {
                            Console.Write("{0, 15}", coefficientsArray[j, ii]);
                        }
                        Console.WriteLine();
                    }                    
                    Console.WriteLine("\n>> Один из массивов f(x):\n");
                    for (int j = 0; j < resultFunctionArray.Length; j++)
                    {
                        Console.Write(resultFunctionArray[j] + "\t");
                    }
                }                
                Matrix<double> left = Matrix<double>.Build.DenseOfArray(coefficientsArray);
                Vector<double> right = Vector<double>.Build.Dense(resultFunctionArray);
                SolutionCompute.Add(left.Solve(right));
            }

            Console.WriteLine("\n>> Нахожу значения для новых функций...\n");            
            List<List<double>> ResultNewFunction = new List<List<double>>();
            for (int i = 0; i < coefficients.Count; i++)
            {
                List <double> TempResultNewFunction = new List<double>();
                for (int j = 0; j < coefficients[i].Count; j++)
                {
                    double value = 0;
                    for (int ii = 0; ii < coefficients[i][j].Count; ii++)
                    {
                        value += SolutionCompute[j][ii] * coefficients[i][j][ii];
                    }
                    TempResultNewFunction.Add(value);
                }
                ResultNewFunction.Add(TempResultNewFunction);
            }

            Console.WriteLine("\n>> Нахожу средние значения ошибок аппроксимаций...\n");
            List<double> AvgErrorApproximately = new List<double>();
            for (int i = 0; i < ResultNewFunction.Count; i++)
            {
                double value = 0;
                for (int j = 0; j < ResultNewFunction[i].Count; j++)
                {
                    value += Math.Abs((ResultFunction[i][j] - ResultNewFunction[i][j]) / ResultFunction[i][j]);
                }
                AvgErrorApproximately.Add((value/10)*100);
            }

            Console.WriteLine("\n>> Ищу наиболее точное приближение...\n");
            double minValue = 0;
            int minValueIndex = 0;

            for (int i = 0; i < AvgErrorApproximately.Count; i++)
            {
                if (i == 0)
                {
                    minValue = AvgErrorApproximately[i];
                    minValueIndex = i;
                }
                else if (minValue > AvgErrorApproximately[i])
                {
                    minValue = AvgErrorApproximately[i];
                    minValueIndex = i;
                }
            }


            /*****Перепроверка******/
            Console.WriteLine($"\n>> Перепроверяем: \n");

            Vector<double> CheckSolutionCompute;
            double[,] coefficientsCheckArray = new double[10, 10];
            double[] resultFunctionCheckArray = new double[10];
            for (int i = 0; i < coefficients[minValueIndex].Count; i++)
            {
                for (int j = 0; j < coefficients[minValueIndex][i].Count; j++)
                {
                    coefficientsCheckArray[i, j] = coefficients[minValueIndex][i][j];
                }
                resultFunctionCheckArray[i] = ResultFunction[minValueIndex][i];
            }
            Matrix<double> leftCheck = Matrix<double>.Build.DenseOfArray(coefficientsCheckArray);
            Vector<double> rightCheck = Vector<double>.Build.Dense(resultFunctionCheckArray);
            CheckSolutionCompute = leftCheck.Solve(rightCheck);
            Console.WriteLine(CheckSolutionCompute);
            /*****Перепроверка******/




            Console.WriteLine($"\n>> Минимальное среднее значение аппроксимации: {minValue}\n");
            Console.WriteLine($"\n>> Результат решения системы уравнений:\n");
            Console.WriteLine(SolutionCompute[minValueIndex]);

            Console.WriteLine($"\n>> При каких коэффициентах достигнуто данное значение: \n");

            foreach (var item in coefficients[minValueIndex])
            {
                for (int i = 0; i < item.Count; i++)
                    Console.Write("{0,15}", item[i]);
                Console.WriteLine();
            }

            Console.WriteLine($"\n>> При каких значениях f(x) достигнуто данное значение: \n");

            foreach (var item in ResultFunction[minValueIndex])
                Console.Write(item + "\t");


            Console.WriteLine($"\n>> Решение функции от данных точек: \n");

            foreach (var item in ResultNewFunction[minValueIndex])
                Console.Write(item + "\t");
            Console.ReadKey();
        }
    }
}
