using System;
using System.Threading.Tasks;

namespace Volatile.Looper
{
    class Program
    {
        static bool terminate = false;
        public static void Main()
        {
            var t = Task.Run(() =>
            {
                Console.WriteLine( "Starting counter." );
                long counter = 0;
                while (!terminate)
                    counter++;
                Console.WriteLine( "Loop ended at: " + counter );
            });

            // Wait 2 seconds.
            Task.Delay(2000).Wait();

            Console.WriteLine("Signaling termination.");

            terminate = true;
            t.Wait();

            Console.WriteLine("Done, program ending.");
        }
    }
}
