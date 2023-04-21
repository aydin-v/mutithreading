/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> numbers = new List<int>();
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private static Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();


            Thread threadPrinter = new Thread(new ThreadStart(PrintItem));
            threadPrinter.Start();

            Thread.Sleep(100);

            Thread threadAdder = new Thread(new ThreadStart(AddItem));
            threadAdder.Start();

            threadPrinter.Join();

            Console.ReadLine();
        }

        static void AddItem()
        {
            for (int i = 0; i < 10; i++)
            {
                numbers.Add(random.Next(100));
                manualResetEvent.Set();
                manualResetEvent.Reset();
                manualResetEvent.WaitOne();
            }
        }

        static void PrintItem()
        {
            for (int i = 0; i < 10; i++)
            {
                manualResetEvent.WaitOne();
                Console.WriteLine(string.Join(", ", numbers.Select(x => x.ToString())));
                manualResetEvent.Set();
                manualResetEvent.Reset();
            }
        }
    }
}
