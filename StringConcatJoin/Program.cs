using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringConcatJoin
{
    /**
     * Experiment to find out which function is faster, string concat operation or String.Join
     * This operation is used to cut content for a short description/spoiler in search results or listing
     * 
     * string concat operation 08.901 s
     * string join operation 02.207 s
     * 
     * input string length is 44000
     **/
    class Program
    {
        static string data = @"Issit Blur like sotong Issit Don't fly my kite Buay Pai On lah! Blur like sotong Don't fly my aeroplane Catch No Ball Auntie. Ai See Buay See ORD loh Got problem ah? Botak Pariah Blur like sotong Obiang Don't fly my aeroplane Don't fly my kite. Chao Mugger Got problem ah? Kampung Sekali Encik. Don't fly my kite Issit He still small boy one Ar? Kena. He still small boy one Blur like sotong Buay Double Confirm Don't fly my kite. Makan Otah Buay Tahan Boleh Kiasi Agak-Agak Siao. My England not powderful!

Kaypoh Chop Chop Blur like sotong Don't fly my kite. Got problem ah? Ah Long Teh-O then you know! No horse run! Act Cute ACBC. Izzit? Gahmen Showflat Hokkien hae mee Don't fly my kite On lah! Yandao No horse run! Blur like sotong. Izzit? Char Bor Mug Kiasu Thiam Zhun Jibai Zai. no prawn fish oso can. Ai Tzai He still small boy one Blur like sotong Kopi-kosong No horse run! Don't fly my aeroplane Don't fly my aeroplane Bo Jio Heng.

Womit Kopi-pao Blur like sotong Encik He still small boy one Izzit? Chao Keng La Sai Don't fly my aeroplane. Double Confirm Gahmen On lah! On lah! He still small boy one. Jiak Kantang Si Mi Mampat Kar Chng Teh Issit. Your grandfather's place, ah?

Liddat oso can!? Sakar La Sai Izzit? No horse run! Teh-packet Blur like sotong Catch No Ball No horse run! Tio. Talk Cock Don't fly my kite Spoil On lah! Berak. Liddat oso can!?";

        static readonly int dataLengthFactor = 5;
        static readonly int repetition = 5000;
        static readonly int length = 5000;

        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder(data);
            for(int i=0; i<dataLengthFactor; ++i)
            {
                sb.Append(sb);
            }

            Console.WriteLine(String.Format("String size {0}", sb.Length));
            Console.WriteLine();

            string input = Regex.Replace(sb.ToString(), @"<(.|\n)*?>", " ");
            Stopwatch s = new Stopwatch();
            s.Start();
            for (int i = 0; i < repetition; ++i)
            {
                _CutConcat(input, length);
            }
            long m = s.ElapsedMilliseconds;
            TimeSpan t = TimeSpan.FromMilliseconds(m);
            Console.WriteLine(String.Format("Time taken for " + repetition + " _CutConcat(s) : {0:D2}:{1:D2}.{2:D2}", t.Minutes, t.Seconds, t.Milliseconds));
            Console.WriteLine();

            for (int i = 0; i < repetition; ++i)
            {
                _CutJoin(input, length);
            }
            s.Stop();
            m = s.ElapsedMilliseconds - m;
            t = TimeSpan.FromMilliseconds(m);

            Console.WriteLine(String.Format("Time taken for " + repetition + " _CutJoin(s) : {0:D2}:{1:D2}.{2:D2}", t.Minutes, t.Seconds, t.Milliseconds));
            Console.ReadLine();
        }

        static string _CutJoin(string input, int length)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            if (input.Length < length)
            {
                return input;
            }

            string[] stringArray = input.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var sum = 0;
            return string.Join(" ", stringArray.TakeWhile(s => { sum += s.Length; return sum < length; })) + "...";
        }

        static string _CutConcat(string txt, int maxLength)
        {
            
            if (!String.IsNullOrWhiteSpace(txt))
            {
                String[] words = txt.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (txt.Length <= maxLength)
                    return txt;

                String retVal = "";

                for (int i = 0; i < words.Length; i++)
                {
                    String word = words[i];

                    if (retVal.Length + word.Length > maxLength)
                    {
                        retVal += "...";
                        break;
                    }

                    if (!String.IsNullOrEmpty(retVal))
                    {
                        retVal += " ";
                    }

                    retVal = retVal + word;
                }

                return retVal.TrimEnd();
            }

            return txt;
        }
    }
}
