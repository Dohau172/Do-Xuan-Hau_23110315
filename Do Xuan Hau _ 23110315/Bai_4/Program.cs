using System;
using System.Text.RegularExpressions;

namespace BaseConverterApp
{
    enum NumeralBase
    {
        Binary = 2,
        Decimal = 10,
        Hexadecimal = 16
    }

    class BaseConverter
    {
        public bool IsValid(string input, NumeralBase b)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string pattern = b switch
            {
                NumeralBase.Binary => @"^-?[01]+$",
                NumeralBase.Decimal => @"^-?[0-9]+$",
                NumeralBase.Hexadecimal => @"^-?[0-9a-fA-F]+$",
                _ => ""
            };
            return Regex.IsMatch(input.Trim(), pattern);
        }

        public string Convert(string input, NumeralBase fromBase, NumeralBase toBase)
        {
            int value = System.Convert.ToInt32(input, (int)fromBase);
            string output = System.Convert.ToString(value, (int)toBase);
            if (toBase == NumeralBase.Hexadecimal) output = output.ToUpperInvariant();
            return output;
        }
    }

    class ConsoleApp
    {
        private readonly BaseConverter _converter = new BaseConverter();

        public void Run()
        {
            while (true)
            {
                ShowMenu();
                NumeralBase fromBase = ReadBase("Chon he co so dau vao (1-3): ");
                NumeralBase toBase = ReadBase("Chon he co so dau ra (1-3): ");

                Console.Write("Nhap gia tri can doi: ");
                string input = (Console.ReadLine() ?? "").Trim();

                if (_converter.IsValid(input, fromBase))
                {
                    try
                    {
                        string result = _converter.Convert(input, fromBase, toBase);
                        Console.WriteLine($"=> Gia tri doi duoc: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Loi: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Du lieu khong hop le!");
                }

                Console.Write("\nTiep tuc? (y/n): ");
                if ((Console.ReadLine() ?? "").Trim().ToLower() != "y") break;
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\n--- CHUONG TRINH DOI CO SO (2, 10, 16) ---");
            Console.WriteLine("1. Binary (BIN)");
            Console.WriteLine("2. Decimal (DEC)");
            Console.WriteLine("3. Hexadecimal (HEX)");
        }

        private static NumeralBase ReadBase(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
                {
                    return choice switch
                    {
                        1 => NumeralBase.Binary,
                        2 => NumeralBase.Decimal,
                        3 => NumeralBase.Hexadecimal,
                        _ => NumeralBase.Decimal
                    };
                }
                Console.WriteLine("Lua chon khong hop le!");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new ConsoleApp().Run();
        }
    }
}
