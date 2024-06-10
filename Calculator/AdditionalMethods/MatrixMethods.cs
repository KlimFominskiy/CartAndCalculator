using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Methods;

/// <summary>
/// Методы поиска чисел в матрице.
/// </summary>
public class MatrixMethods
{
    /// <summary>
    /// Метод ищет наименьшее положительного число в матрице.
    /// </summary>
    /// <param name="matrix">Матрица, в которой осуществляется поиск.</param>
    /// <returns>Наименьшее положительного число в матрице.</returns>
    public double? FindTheSmallestPositiveNumberInMatrix(double[,] matrix)
    {
        double? theSmallestPositiveNumber = null;
        foreach (double matrixElement in matrix)
        {
            if (matrixElement > 0)
            {
                if (theSmallestPositiveNumber is null)
                {
                    theSmallestPositiveNumber = matrixElement;
                }
                else if (matrixElement < theSmallestPositiveNumber)
                {
                    theSmallestPositiveNumber = matrixElement;
                }
            }
        }

        return theSmallestPositiveNumber;
    }

    /// <summary>
    /// Метод ищет наибольшее отрицателное число в матрице.
    /// </summary>
    /// <param name="matrix">Матрица, в которой осуществляется поиск.</param>
    /// <returns>Наибольшее отрицательное число в матрице.</returns>
    public double? FindTheLargestNegativeNumberInMatrix(double[,] matrix)
    {
        double? theLargestNegativeNumber = null;
        foreach (double matrixElement in matrix)
        {
            if (matrixElement < 0)
            {
                if (theLargestNegativeNumber is null)
                {
                    theLargestNegativeNumber = matrixElement;
                }
                else if (matrixElement > theLargestNegativeNumber)
                {
                    theLargestNegativeNumber = matrixElement;
                }
            }
        }

        return theLargestNegativeNumber;
    }
}
