using System;
class Program
{
    static void Main()
    {
        // FIX: Đổi nhãn cho đúng ý nghĩa biến
        Console.Write("Nhap so hang: ");
        int rows = int.Parse(Console.ReadLine());
        Console.Write("Nhap so cot: ");
        int cols = int.Parse(Console.ReadLine());

        int[,] matrix1 = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"Nhap phan tu [{i}, {j}]: ");
                matrix1[i, j] = int.Parse(Console.ReadLine());
            }
        }

        Console.WriteLine("Ma tran vua nhap la: ");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix1[i, j] + "\t");
            }
            Console.WriteLine();
        }

        // --- Ma tran thu 2 ---
        Console.WriteLine("Nhap so hang ma tran thu 2: ");
        int rows2 = int.Parse(Console.ReadLine());
        Console.WriteLine("Nhap so cot ma tran thu 2: ");
        int cols2 = int.Parse(Console.ReadLine());

        // FIX QUAN TRỌNG: Luon tao va nhap matrix2 de dung cho ca cong va nhan
        int[,] matrix2 = new int[rows2, cols2];
        for (int i = 0; i < rows2; i++)
        {
            for (int j = 0; j < cols2; j++)
            {
                Console.Write($"Nhap phan tu [{i}, {j}] cua ma tran thu 2: ");
                matrix2[i, j] = int.Parse(Console.ReadLine());
            }
        }

        // --- Cong 2 ma tran ---
        if (rows != rows2 || cols != cols2)
        {
            Console.WriteLine("Khong the cong 2 ma tran vi khong cung kich thuoc");
        }
        else
        {
            int[,] sumMatrix = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sumMatrix[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            Console.WriteLine("Ma tran tong la: ");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(sumMatrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        // --- Nhan 2 ma tran ---
        if (cols != rows2)
        {
            Console.WriteLine("Khong the nhan 2 ma tran vi so cot cua ma tran thu 1 khong bang so hang cua ma tran thu 2");
        }
        else
        {
            int[,] productMatrix = new int[rows, cols2];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    productMatrix[i, j] = 0;
                    for (int k = 0; k < cols; k++)
                    {
                        productMatrix[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }
            Console.WriteLine("Ma tran tich la: ");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    Console.Write(productMatrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        // --- Chuyen vi ma tran 1 ---
        int[,] transposeMatrix = new int[cols, rows];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                transposeMatrix[j, i] = matrix1[i, j];
            }
        }
        Console.WriteLine("Ma tran chuyen vi la: ");
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Console.Write(transposeMatrix[i, j] + "\t");
            }
            Console.WriteLine();
        }

        // --- Tim max/min tren ma tran 1 ---
        int max = matrix1[0, 0];
        int min = matrix1[0, 0];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix1[i, j] > max) max = matrix1[i, j];
                if (matrix1[i, j] < min) min = matrix1[i, j];
            }
        }
        Console.WriteLine($"Gia tri lon nhat trong ma tran la: {max}");
        Console.WriteLine($"Gia tri nho nhat trong ma tran la: {min}");
    }
}
