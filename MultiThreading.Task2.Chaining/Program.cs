/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Random random = new Random();
            Task.Factory.StartNew(() =>
            {
                var intArray = new int[10];

                for (int i = 0; i < intArray.Length; i++)
                {
                    intArray[i] = random.Next(-500, 500);
                }
                PrintValues(intArray, 1);
                return intArray;
            }).ContinueWith((previousTask) =>
            {
                var intArray = previousTask.Result;
                for (int i = 0; i < intArray.Length; i++)
                {
                    intArray[i] = random.Next(-500, 500) * intArray[i];
                }
                PrintValues(intArray, 2);
                return intArray;
            }).ContinueWith((previousTask) =>
            {
                var intArray = previousTask.Result;
                Array.Sort(intArray);
                PrintValues(intArray, 3);
                return intArray;
            }).ContinueWith((previousTask) =>
            {
                var average = previousTask.Result.Average();
                Console.WriteLine($"Average is {average}");
            });

            Console.ReadLine();
        }

        private static void PrintValues(int[] array, int taskNumber)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine($"Task number: {taskNumber} - Value{i}: {array[i]}");
            }
        }
    }
}
