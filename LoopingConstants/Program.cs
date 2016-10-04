using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoopingConstants
{
    /**
     * Experiment to find out whether compiler will handle optimization for a replace operation to a constants in lambda function
     * Answer is no
     * 
     * The query without replace is faster by +- 5 ms for 1 execution
     * For 20 times execution is faster by +- 34 ms
     * For 100 times execution is faster by +- 153 ms
     * 
     * Array size is 6144
     **/
    class Program
    {
        static readonly int dataLengthFactor = 9;
        static readonly int repetition = 100;
        static readonly string term = "yo dawg I AM NOT SLACKING, i am not slackingyo dawg I AM NOT SLACKING, ";
        static readonly string replaceInTerm = "yo dawg I AM NOT SLACKING, ";
        static string[] data = { 
                            "hr is slacking",
                            "boss is slacking",
                            "manager is slacking",
                            "backend is slacking",
                            "frontend is slacking",
                            "infra is slacking",
                            "ae is slacking",
                            "sa is slacking",
                            "everyone is slacking",
                            "but",
                            "i am not slacking",
                            "paid like shit"
                              };

        static void Main(string[] args)
        {
            //shitposting string to data array
            for(int i = 0; i < dataLengthFactor; ++i)
            {
                string[] temp = new string[data.Length*2];
                Array.Copy(data, temp, data.Length);
                Array.Copy(data, 0, temp, data.Length, data.Length);
                data = temp;
            }

            Console.WriteLine("Length of data: " + data.Length);
            Console.WriteLine();

            Expression<Func<string, bool>> queryExpressionWithReplace = x => x == term.Replace(replaceInTerm, "");

            string stupidlySimplerTerm = term.Replace(replaceInTerm, "");
            Expression<Func<string, bool>> queryExpression = x => x == stupidlySimplerTerm;

            Stopwatch s = new Stopwatch();
            s.Start();
            for (int i = 0; i < repetition; ++i)
            {
                data.AsQueryable().Where(queryExpressionWithReplace).ToList();
            }
            long m = s.ElapsedMilliseconds;
            TimeSpan t = TimeSpan.FromMilliseconds(m);
            Console.WriteLine(String.Format("Time taken for " + repetition + " search(s) with replace in condition : {0:D2}:{1:D2}.{2:D2}", t.Minutes, t.Seconds, t.Milliseconds));
            Console.WriteLine();

            for (int i = 0; i < repetition; ++i)
            {
                data.AsQueryable().Where(queryExpression).ToList();
            }
            s.Stop();
            m = s.ElapsedMilliseconds - m;
            t = TimeSpan.FromMilliseconds(m);

            Console.WriteLine(String.Format("Time taken for " + repetition + " search(s) without replace in condition : {0:D2}:{1:D2}.{2:D2}", t.Minutes, t.Seconds, t.Milliseconds));
            Console.ReadLine();
        }
    }
}
