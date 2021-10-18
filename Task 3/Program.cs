using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>()
            {
                {"four",4 },
                {"two",2 },
                { "one",1 },
                {"three",3 },
            };
            //var d = dict.OrderBy(delegate (KeyValuePair<string, int> pair) { return pair.Value; });

            IOrderedEnumerable<KeyValuePair<string, int>> dictionaryResult = dict.OrderBy(pair => pair.Value);

            foreach (var pair in dictionaryResult)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
            Console.ReadKey();
        }
    }
}
