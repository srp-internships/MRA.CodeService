namespace Shared.Test
{
    public class DotNetCodeTestBase
    {
        protected string GetInValidCode()
        {
            var code = @"
                    using Systemasd;
                    static class Program
                    {
                        static void Main()
                        {asd
                            Console.WriteLine(""Hello World"");
                        }
                    }
                    ";
            return code;
        }

        protected string GetValidCode()
        {
            var code = @"
                    using System;
                    static class Program
                    {
                        static void Main()
                        {
                            Console.WriteLine(""Hello World"");
                        }
                    }
                    ";
            return code;
        }

        protected string GetSumOfResolvedFunctionTaskCode()
        {
            var code = @"
                        static class Function
                        {
                            public static int Sum(int a, int b)
                            {
                                return a + b;
                            }
                        }
                    ";
            return code;
        }

        protected string GetStackoverflowCode()
        {
            var code = @"
                        static class Function
                        {
                            public static int Sum(int a, int b)
                            {
                               return Function.Sum(a, b);
                            }
                        }
                    ";
            return code;
        }

        protected string GetTimeoutCode()
        {
            var code = @"
                        static class Function
                        {
                            public static int Sum(int a, int b)
                            {
                               System.Threading.Tasks.Task.Delay(5010).Wait();
                               return a + b;
                            }
                        }
                    ";
            return code;
        }

        protected string GetSumUnResolvedFunctionTaskCode()
        {
            var code = @"
                        static class Function
                        {
                            public static int Sum(int a, int b)
                            {
                                return a * b;
                            }
                        }
                    ";
            return code;
        }

        protected string GetSumFunctionTaskTestCode()
        {
            var code = @"
                        using NUnit.Framework;
                        class FunctionTests
                        {
                            [Test]
                            public void FunctionTest()
                            {
                                int a = 100;
                                int b = 100;
                                int summ = Function.Sum(a, b);
                                Assert.That(summ, Is.EqualTo(200), ""Sum function is not returned number as expected"");
                            }
                        }
                    ";
            return code;
        }

        protected string GetSumFunctionTaskTestCaseCode()
        {
            var code = @"
                        using NUnit.Framework;
                        class FunctionTests
                        {
                            [TestCase(100,100,200)]
                            [TestCase(200,100,300)]
                            [TestCase(100,50,150)]
                            public void FunctionTest(int a, int b, int expected)
                            {
                                int summ = Function.Sum(a, b);
                                Assert.That(summ, Is.EqualTo(expected), ""Sum function is not returning the expected number"");
                            }
                        }
                    ";
            return code;
        }

        protected string GetSumThreeNumbersOfResolvedFunctionTaskCode()
        {
            var code = @"
                        public class Function
                        {
                            public int Sum(int a, int b, int c)
                            {
                                return a + b + c;
                            }
                        }
                    ";
            return code;
        }
        protected string GetSumThreeNumbersOfUnResolvedFunctionTaskCode()
        {
            var code = @"
                        public class Function
                        {
                            public int Sum(int a, int b, int c)
                            {
                                return a + b - c;
                            }
                        }
                    ";
            return code;
        }

        protected string GetSumThreeNumbersFunctionTaskTestCaseCode()
        {
            var code = @"
                        using NUnit.Framework;
                        class FunctionTests
                        {
                            [TestCase(100,100,200,400)]
                            [TestCase(200,100,300,600)]
                            [TestCase(100,50,150,300)]
                            public void FunctionTest(int a, int b, int c, int expected)
                            {
                                Function f = new Function();
                                int summ = f.Sum(a, b, c);
                                Assert.That(summ, Is.EqualTo(expected), ""Sum function is not returning the expected number"");
                            }
                        }
                    ";
            return code;
        }

        protected string ArrayInversionFunctionCode()
        {
            var code = @"
                        public static class Exercise
                        {
                            public static int[] ArrayInversion(int[] numbers)
                            {
                                int n = numbers.Length;
                                for (int i = 0; i < n / 2; i++)
                                {
                                    int temp = numbers[i];
                                    numbers[i] = numbers[n - i - 1];
                                    numbers[n - i - 1] = temp;
                                }
                                return numbers;
                            }
                        }
                    ";
            return code;
        }

        protected string ArrayInversionFunctionTestCode()
        {
            var code = @"
                        using NUnit.Framework;
                        class FunctionTests
                        {
                            [Test]
                            public void Exercise_Test()
                            {
                                var result = Exercise.ArrayInversion(new int[] { 8, 3, 0, 2 });
                                Assert.That(result, Is.EqualTo(new int[] { 2, 0, 3, 8 }));
                                result = Exercise.ArrayInversion(new int[] { 2, 4 });
                                Assert.That(result, Is.EqualTo(new int[] { 4, 2 }));
                            }
                        }
                    ";
            return code;
        }

    }
}
