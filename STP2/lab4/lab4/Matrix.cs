using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace lab4
{
    public class MyException : Exception
    {
        public MyException(string s) : base(s) { }
    }

    public class Matrix
    {
        private readonly int[,] _matrix;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public Matrix(int i, int j)
        {
            if (i <= 0)
            {
                throw new MyException($"недопустимое значение строки = {i}");
            }
            if (j <= 0)
            {
                throw new MyException($"недопустимое значение столбца = {j}");
            }
            Rows = i;
            Cols = j;
            _matrix = new int[i, j];
        }

        public Matrix(int[,] matrix, int i, int j)
        {
            if (i <= 0)
            {
                throw new MyException($"недопустимое значение строки = {i}");
            }
            if (j <= 0)
            {
                throw new MyException($"недопустимое значение столбца = {j}");
            }
            Rows = i;
            Cols = j;
            _matrix = new int[i,j];
            Array.Copy(matrix, _matrix, matrix.Length);
        }

        public int this[int i, int j]
        {
            get
            {
                if (i < 0 | i > Rows - 1)
                {
                    throw new MyException($"неверное значение i = {i}");
                }
                if (j < 0 | j > Cols - 1)
                {
                    throw new MyException($"неверное значение j = {j}");
                }
                return _matrix[i, j];
            }
            set
            {
                if (i < 0 | i > Rows - 1)
                {
                    throw new MyException($"неверное значение i = {i}");
                }
                if (j < 0 | j > Cols - 1)
                {
                    throw new MyException($"неверное значение j = {0}");
                }
                _matrix[i, j] = value;
            }
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
            {
                throw new MyException($"количество строк и столбцов в матрицах не одинаково");
            }

            Matrix c = new(a.Rows, a.Cols);
            for (int i = 0;
                i < a.Rows;
                i++)
            {
                for (int j = 0;
                    j < a.Cols;
                    j++)
                {
                    c[i, j] = a._matrix[i, j] + b._matrix[i, j];
                }
            }
            return c;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
            {
                throw new MyException($"количество строк и столбцов в матрицах не одинаково");
            }

            Matrix c = new(a.Rows, a.Cols);
            for (int i = 0;
                i < a.Rows;
                i++)
            {
                for (int j = 0;
                    j < a.Cols;
                    j++)
                {
                    c[i, j] = a._matrix[i, j] - b._matrix[i, j];
                }
            }
            return c;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Rows != b.Cols)
            {
                throw new MyException($"матрицы не согласованы");
            }

            Matrix c = new(a.Rows, a.Cols);
            for (int i = 0;
                i < a.Rows;
                i++)
            {
                for (int j = 0;
                    j < a.Cols;
                    j++)
                {
                    c[i, j] = 0;
                    for (int k = 0;
                        k < b.Cols;
                        ++k)
                    {
                        c[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return c;
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
            {
                throw new MyException($"количество строк и столбцов в матрицах не одинаково");
            }

            bool q = true;
            for (int i = 0;
                i < a.Rows;
                i++)
                for (int j = 0;
                    j < a.Cols;
                    j++)
                {
                    if (a[i, j] != b[i, j])
                    {
                        q = false;
                        break;
                    }
                }
            return q;
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }

        public Matrix Tranpose()
        {
            if (Rows != Cols)
            {
                throw new MyException($"матрица не квадратная");
            }

            Matrix c = new(Rows, Cols);
            for (int i = 0;
                i < Rows;
                i++)
            {
                for (int j = 0;
                    j < Cols;
                    j++)
                {
                    c[i, j] = _matrix[j, i];
                }
            }
            return c;
        }

        public int Min()
        {
            int minElement = int.MaxValue;

            for (int i = 0;
                i < Rows;
                i++)
            {
                for (int j = 0;
                    j < Cols;
                    j++)
                {
                    if (minElement > _matrix[i, j])
                    {
                        minElement = _matrix[i, j];
                    }
                }
            }
            return minElement;
        }

        public override string ToString()
        {
            return $"{{{{{JsonConvert.SerializeObject(_matrix).Trim('[', ']').Replace("[", "{").Replace("]", "}")}}}}}";
        }

        public void Show()
        {
            for (int i = 0;
                i < Rows;
                i++)
            {
                for (int j = 0;
                    j < Cols;
                    j++)
                {
                    Console.Write("\t" + this[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public override bool Equals(object? obj)
        {
            return this == (obj as Matrix);
        }

        public override int GetHashCode()
        {
            return _matrix.GetHashCode();
        }
    }
}
