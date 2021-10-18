using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<string> list1 = new List<string>() { "AA", "ББ", "AA", "CC", "AA", "CC", "FF", "AA" };
            FrequencyCounter<string>(list1);

            List<int> list2 = new List<int>() { 1, 3, 4, 2, 4, 2, 1, 1 };
            FrequencyCounter<int>(list2);

            ArrayList array1 = new ArrayList() { "AA", "ББ", "AA", 1, "AA", 1, "FF", "AA" };
            FrequencyCounter(array1);

            ArrayList array2 = new ArrayList() { 1, 3, 4, 2, 4, 2, 1, 1 };
            FrequencyCounter(array2);

            int [] array3 = new int[8] { 1, 3, 4, 2, 4, 2, 1, 1 };
            FrequencyCounter(new ArrayList(array3));

            //Console.WriteLine("--- LINQ ----");

            //Dictionary<int, int> dictionary = new Dictionary<int, int>();

            //from n
            //in array3
            //where dictionary.ContainsKey(n)
            //select n;



            Console.ReadLine();
        }

        /// <summary>
        /// Подсчет количества вхождений элементов для обобщенной коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        static void FrequencyCounter<T>(List<T> input)
        {
            Dictionary<T, int> dictionaryCounter = new Dictionary<T, int>();
            foreach (var e in input)
            {
                if (dictionaryCounter.ContainsKey(e)) dictionaryCounter[e]++;
                else dictionaryCounter[e] = 1;
            }
            ResultPrint<T>(dictionaryCounter);
        }

        /// <summary>
        /// Подсчет количества вхождений элементов для НЕобобщенной коллекции
        /// </summary>
        /// <param name="input"></param>
        static void FrequencyCounter(ArrayList input)
        {
            Dictionary<object, int> dictionaryCounter = new Dictionary<object, int>();
            foreach (var e in input)
            {
                if (dictionaryCounter.ContainsKey(e)) dictionaryCounter[e]++;
                else dictionaryCounter[e] = 1;               
            }
            ResultPrint<object>(dictionaryCounter);
        }
        
        static void ResultPrint<T>(Dictionary<T, int> print)
        {
            foreach (var e in print)
            {
                Console.WriteLine("{0} - {1}", e.Key, e.Value);
            }
            Console.WriteLine("-----------------------------------------------");
        }
    }
}
