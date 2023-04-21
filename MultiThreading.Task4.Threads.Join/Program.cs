/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static readonly Semaphore _semaphore = new Semaphore(0, 9);
        private const int maxThreadCount = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("Task a");
            CreateThread(maxThreadCount);

            Console.WriteLine();

            Console.WriteLine("Task a");
            CreateThreadByThreadPool(maxThreadCount);

            Console.ReadLine();
        }

        static void CreateThread(object remainingThreadCount)
        {
            var count = (int)remainingThreadCount;
            Console.WriteLine(count);

            count--;
            if (count == 0)
                return;

            Thread thread = new Thread(new ParameterizedThreadStart(CreateThread));
            thread.Start(count);
            thread.Join();
        }

        private static void CreateThreadByThreadPool(int count)
        {
            if (count == 0)
                return;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.WriteLine(count--);

                CreateThreadByThreadPool(count);

                _semaphore.Release();
            });

            _semaphore.WaitOne();
        }
    }
}
