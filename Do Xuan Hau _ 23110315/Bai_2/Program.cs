using System;
using System.Numerics;

class ArrayProcessor
{
    private int[] arr = Array.Empty<int>();
    public void Input()
    {
        // SỬA: IMinMaxValue -> IMinValue
        int n = ReadInt("Nhap so thu tu n: ", IMinValue: 0);
        arr = new int[n];
        for (int i = 0; i < n; i++)
        {
            arr[i] = ReadInt($"Nhap phan tu thu {i}: ");
        }
    }
    public void Display()
    {
        Console.WriteLine("Cac phan tu trong mang la: ");
        foreach (var item in arr)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
    public void BubbleSort()
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    swapped = true;
                }
            }
            if (!swapped) break;
        }
    }
    public void QuickSort()
    {
        if (arr.Length > 1)
            QuickSort(0, arr.Length - 1);

    }
    public void QuickSort(int left, int right)
    {
        if (left >= right) return;
        int pivot = arr[(left + right) / 2];
        int i = left, j = right;
        while (i <= j)
        {
            while (arr[i] < pivot) i++;
            while (arr[j] > pivot) j--;
            if (i <= j)
            {
                (arr[i], arr[j]) = (arr[j], arr[i]);
                i++;
                j--;
            }
        }
        if (left < j) QuickSort(left, j);
        if (i < right) QuickSort(i, right);
    }
    public int LinearSearch(int x)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == x) return i;
        }
        return -1;
    }
    public int BinarySearch(int x)
    {
        int left = 0, right = arr.Length - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid] == x) return mid;
            if (arr[mid] < x) left = mid + 1;
            else right = mid - 1;
        }
        return -1;
    }
    public int[] CloneArray() => (int[])arr.Clone();
    public void SetArray(int[] newArr) => arr = (int[])newArr.Clone();

    private int ReadInt(string prompt, int IMinValue = int.MinValue, int IMaxValue = int.MaxValue)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value) && value >= IMinValue && value <= IMaxValue)
                return value;
            Console.WriteLine($"Vui long nhap so nguyen trong khoang [{IMinValue}, {IMaxValue}]");
        }
    }
}

class Program
{
    static void Main()
    {
        ArrayProcessor arrayProcessor = new ArrayProcessor();
        arrayProcessor.Input();
        arrayProcessor.Display();
        Console.WriteLine("Chon phuong phap sap xep: 1. Bubble Sort  2. Quick Sort");
        int choice = ReadInt("Nhap lua chon cua ban (1 hoac 2): ", 1, 2);
        if (choice == 1)
        {
            arrayProcessor.BubbleSort();
            Console.WriteLine("Mang sau khi sap xep bang Bubble Sort:");
        }
        else
        {
            arrayProcessor.QuickSort();
            Console.WriteLine("Mang sau khi sap xep bang Quick Sort:");
        }
        arrayProcessor.Display();
        int x = ReadInt("Nhap gia tri can tim kiem: ");
        int linearResult = arrayProcessor.LinearSearch(x);
        Console.WriteLine(linearResult != -1 ? $"Tim thay {x} tai vi tri {linearResult} bang Linear Search." : $"{x} khong ton tai trong mang (Linear Search).");
        int binaryResult = arrayProcessor.BinarySearch(x);
        Console.WriteLine(binaryResult != -1 ? $"Tim thay {x} tai vi tri {binaryResult} bang Binary Search." : $"{x} khong ton tai trong mang (Binary Search).");
    }
    private static int ReadInt(string prompt, int IMinValue = int.MinValue, int IMaxValue = int.MaxValue)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value) && value >= IMinValue && value <= IMaxValue)
                return value;
            Console.WriteLine($"Vui long nhap so nguyen trong khoang [{IMinValue}, {IMaxValue}]");
        }
    }
}
