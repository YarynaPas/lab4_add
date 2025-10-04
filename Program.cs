using System;

namespace NonlinearEquationsSolver
{
    class Program
    {
        public static double f(double x)
        {
            return x * x - 4; // x² - 4 = 0 → корені ±2
        }
        public static double fp(double x, double D)
        {
            return (f(x + D) - f(x)) / D;
        }

        public static double f2p(double x, double D)
        {
            return (f(x + D) + f(x - D) - 2 * f(x)) / (D * D);
        }

        public static (double root, int iterations) BisectionMethod(double a, double b, double epsilon)
        {
            int iterations = 0;

            if (f(a) * f(b) > 0)
                throw new ArgumentException("На інтервалі [a, b] немає кореня або їх парна кількість");

            if (Math.Abs(f(a)) < epsilon)
                return (a, iterations);

            if (Math.Abs(f(b)) < epsilon)
                return (b, iterations);

            double c;
            while (Math.Abs(b - a) > epsilon)
            {
                c = (a + b) / 2;
                iterations++;

                if (Math.Abs(f(c)) < epsilon)
                    return (c, iterations);

                if (f(a) * f(c) < 0)
                    b = c;
                else
                    a = c;
            }

            return ((a + b) / 2, iterations);
        }

        public static (double root, int iterations) NewtonMethod(double a, double b, double epsilon, int maxIterations)
        {
            double D = epsilon;
            double x = b;
            int iterations = 0;

            if (f(x) * f2p(x, D) < 0)
                x = a;
            else if (f(x) * f2p(x, D) == 0)
                throw new ArgumentException("Для заданого рівняння збіжність методу Ньютона не гарантується");

            for (int i = 1; i <= maxIterations; i++)
            {
                iterations = i;
                double fpx = fp(x, D);

                if (Math.Abs(fpx) < 1e-12)
                    throw new InvalidOperationException("Похідна близька до нуля - метод зупинено");

                double Dx = f(x) / fpx;
                x = x - Dx;

                if (Math.Abs(Dx) < epsilon)
                    return (x, iterations);
            }

            throw new InvalidOperationException($"За {maxIterations} ітерацій корінь з точністю {epsilon} не знайдено");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Розв'язування нелінійних рівнянь");
                    Console.WriteLine("Рівняння: f(x) = x² - 4 = 0");
                    Console.WriteLine("Оберіть метод:");
                    Console.WriteLine("1 - Метод бісекції");
                    Console.WriteLine("2 - Метод Ньютона");
                    Console.WriteLine("3 - Вихід");
                    Console.Write("Ваш вибір: ");

                    string choice = Console.ReadLine();

                    if (choice == "3")
                    {
                        Console.WriteLine("вихід");
                        break;
                    }
                    Console.WriteLine("\n--- Введення вхідних даних ---");

                    Console.Write("Введіть ліву межу інтервалу (a): ");
                    double a = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Введіть праву межу інтервалу (b): ");
                    double b = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Введіть точність (epsilon): ");
                    double epsilon = Convert.ToDouble(Console.ReadLine());

                    int maxIterations = 100;
                    if (choice == "2")
                    {
                        Console.Write("Введіть максимальну кількість ітерацій: ");
                        maxIterations = Convert.ToInt32(Console.ReadLine());
                    }

                    double root;
                    int iterations;

                    Console.WriteLine("\n--- Результати ---");

                    if (choice == "1")
                    {
                        Console.WriteLine("Метод: Бісекції");
                        (root, iterations) = BisectionMethod(a, b, epsilon);
                    }
                    else
                    {
                        Console.WriteLine("Метод: Ньютона");
                        (root, iterations) = NewtonMethod(a, b, epsilon, maxIterations);
                    }
                    Console.WriteLine("----------------------------");
                    Console.WriteLine($"Знайдений корінь: x = {root:F10}");
                    Console.WriteLine($"Значення функції: f(x) = {f(root):E2}");
                    Console.WriteLine($"Кількість ітерацій: {iterations}");
                    Console.WriteLine($"Точність: {epsilon}");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nПомилка: {ex.Message}");
                }

                Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }
    }
}