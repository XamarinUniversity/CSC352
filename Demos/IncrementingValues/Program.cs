using System;
using System.Threading.Tasks;
using System.Linq;

namespace IncrementingValues
{
    public class Counter
    {
        int counter;
        public int Value => counter;

        public virtual void Increment() { 
            counter++; 
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Counter c = new Counter();

            // Increment counter 10k times with two threads.
            var t = Enumerable.Range(0,2)
                .Select(n => Task.Run(() => {
                    for (int i = 0; i < 5000; i++)
                        c.Increment();
                })).ToArray();

            // Wait for both to be finished.
            Task.WaitAll(t);

            // Output the result.
            Console.WriteLine($"Final value is {c.Value}.");
        }
    }
}
