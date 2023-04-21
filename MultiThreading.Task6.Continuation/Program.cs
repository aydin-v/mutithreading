/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var source = new CancellationTokenSource();

            //Task a
            Task.Factory.StartNew(() =>
            {
                source.Cancel();
                source.Token.ThrowIfCancellationRequested();
            }, source.Token).ContinueWith((cancelledTask) =>
            {
                Console.WriteLine(cancelledTask.Status);
                throw new Exception();
            }).ContinueWith((failedTask) =>
            {
                Console.WriteLine(failedTask.Status);
            }).ContinueWith((successfulTask) =>
            {
                Console.WriteLine(successfulTask.Status);
            });

            Console.ReadKey();
            Console.WriteLine();

            //Task b
            Task.Factory.StartNew(() =>
            {
                source.Cancel();
                source.Token.ThrowIfCancellationRequested();
            }, source.Token).ContinueWith((cancelledTask) =>
            {
                Console.WriteLine(cancelledTask.Status);
                throw new Exception();
            }, TaskContinuationOptions.NotOnRanToCompletion).ContinueWith((failedTask) =>
            {
                Console.WriteLine(failedTask.Status);
            }, TaskContinuationOptions.NotOnRanToCompletion).ContinueWith((successfulTask) =>
            {
                Console.WriteLine(successfulTask.Status);
            }, TaskContinuationOptions.NotOnRanToCompletion);


            Console.ReadKey();
            Console.WriteLine();

            //Task c
            Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "TestThread";
                Console.WriteLine($"Thread name is {Thread.CurrentThread.Name}");
                throw new Exception();

            }).ContinueWith((failedTask) =>
            {
                Console.WriteLine(failedTask.Status);
                Console.WriteLine($"Thread name is {Thread.CurrentThread.Name}");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            Console.ReadKey();
            Console.WriteLine();

            //Task d
            Task.Factory.StartNew(() =>
            {
                source.Cancel();
                source.Token.ThrowIfCancellationRequested();
            }, source.Token).ContinueWith((cancelledTask) =>
            {
                Console.WriteLine(cancelledTask.Status);
                Console.WriteLine($"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            Console.ReadLine();
        }


    }
}
