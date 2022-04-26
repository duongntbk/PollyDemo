using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCustomPolicy.MyPolicies;
using DemoCustomPolicy.MyRandom;
using Polly;

namespace DemoCustomPolicy
{
    internal class Program
    {
        private static readonly IBitGetter bitGetter = new BitGetter();
        private static readonly INumberGenerator numberGenerator =
            new NumberGenerator(bitGetter);
        private static readonly INumberGenerator numberGeneratorRange =
            new NumberGeneratorInRange(numberGenerator, 91, 100);

        static async Task Main(string[] args)
        {
            WithoutPolicy();
            WithPolicy();
            await Timeout();
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }

        private static void WithoutPolicy()
        {
            Console.WriteLine("######################################");
            Console.WriteLine("Generating numbers without policy...");
            Console.WriteLine("######################################");
            Verify(() => numberGenerator.Next(91, 100), 1000);
        }

        private static void WithPolicy()
        {
            Console.WriteLine("######################################");
            Console.WriteLine("Generating numbers with policy...");
            Console.WriteLine("######################################");
            Verify(() => numberGeneratorRange.Next(91, 100), 1000);
        }

        private static void Verify(Func<int> generator, int tries)
        {
            var counter = new Dictionary<int, int>();
            for (var i = 0; i < tries; i++)
            {
                var num = generator();
                if (counter.ContainsKey(num))
                {
                    counter[num]++;
                }
                else
                {
                    counter[num] = 1;
                }
            }

            foreach (var key in counter.Keys.OrderBy(k => k))
            {
                Console.WriteLine($"{key}: {(double)counter[key] / tries * 100}%");
            }
        }

        private static async Task Timeout()
        {
            Console.WriteLine("######################################");
            Console.WriteLine("Starting long running task...");
            Console.WriteLine("######################################");
            var pollyContext = new Context(nameof(Timeout));
            var policy = DuongTimeoutPolicy.Create(500);
            try
            {
                await policy.ExecuteAsync(async ctx =>
                {
                    await Task.Delay(1000);
                }, pollyContext);
                Console.WriteLine("Task was not cancelled.");
            }
            catch (DuongTimeoutException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
