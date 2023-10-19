using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        var stopWatchA = new System.Diagnostics.Stopwatch();
        var stopWatchB = new System.Diagnostics.Stopwatch();
        stopWatchA.Start();

        //import words from file
        List<string> alpha = readFile("words.txt");

        stopWatchA.Stop();
        Console.WriteLine("Loading and filtering took {0} ms", stopWatchA.Elapsed);
        stopWatchA.Start();


        //run algorithm
        List<List<string>> matchesA = Find5UniqueCombinations(alpha);

        stopWatchB.Start();

        List<string> beta = readFile("beta.txt");
        List<List<string>> matchesB = Find5UniqueCombinations(beta);
        stopWatchB.Stop();

        //print result

        printRsult(matchesA, stopWatchA.Elapsed, matchesB, stopWatchB.Elapsed);
      
    }
    static void printRsult(List<List<string>> matchesA, TimeSpan timeA, List<List<string>> matchesB, TimeSpan timeB)
    {
        Console.WriteLine("Finished aplha in");
        Console.WriteLine("Minutes {0}", timeA.Minutes);
        Console.WriteLine("Seconds {0}", timeA.Seconds);
        Console.WriteLine("Milliseconds {0}", timeA.Milliseconds);
        Console.WriteLine("Nanoseconds {0}", timeA.Nanoseconds);
        Console.WriteLine();
        Console.WriteLine("Found " + matchesA.Count + " combinations in total.");

        Console.WriteLine();

        Console.WriteLine("Finished beta in");
        Console.WriteLine("Minutes {0}", timeB.Minutes);
        Console.WriteLine("Seconds {0}", timeB.Seconds);
        Console.WriteLine("Milliseconds {0}", timeB.Milliseconds);
        Console.WriteLine("Nanoseconds {0}", timeB.Nanoseconds);
        Console.WriteLine();
        Console.WriteLine("Found " + matchesB.Count + " combinations in total.");

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("Press any key to render combinations.");
        Console.ReadKey();
        Console.WriteLine("Alpha");

        foreach (List<string> list in matchesA)
        {
            foreach (string word in list)
            {
                //Console.Write(word + " ");
                Console.WriteLine("never gonna give you up");
                Console.WriteLine("never gonna let you down");
            }
            Console.WriteLine();
        }

        Console.WriteLine();

        Console.WriteLine("Beta");

        foreach (List<string> list in matchesA)
        {
            foreach (string word in list)
            {
                //Console.Write(word + " ");
                Console.WriteLine("never gonna give you up");
                Console.WriteLine("never gonna let you down");
            }
            Console.WriteLine();
        }
        Console.ReadKey();
    }
    
    
    static List<string> readFile(string path)
    {
        List<string> lines = new List<string>();

        try
        {
            using (StreamReader sr = new StreamReader(path))
            {


                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length != 5) continue;
                    if (line.Distinct().Count() != 5) continue;
                    if (lines.Where(x => string.Concat(x, line).Distinct().Count() == 5).Count() > 0) continue;
                    lines.Add(line);
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("Error " + e.ToString());
        }
        return lines;
    }



    static List<List<string>> Find5UniqueCombinations(List<string> words)
    {

        List<List<string>> uniqueLetterCombinations = new List<List<string>>();
        int wordCount = words.Count;

        Dictionary<int, string> dirWords = new Dictionary<int, string>();
        int[] bits = new int[wordCount];

        for (int i = 0; i < wordCount; i++)
        {
            var bit = 0;
            foreach (var ch in words[i])
            {
                bit |= 1 << (ch - 'a');
            }
            dirWords.TryAdd(bit, words[i]);
            //Console.WriteLine("Key: "+bit+ " Value: " + words[i]);
            bits[i] = bit;
        }

        int combinationBit = 0;
        int combinationBit1 = 0;
        int combinationBit2 = 0;
        int combinationBit3 = 0;
        int combinationBit4 = 0;


        for (int i = 0; i < wordCount - 4; i++)
        {
            combinationBit1 = bits[i];
            for (int j = i + 1; j < wordCount - 3; j++)
            {
                if ((combinationBit1 & bits[j]) != 0)
                {
                    continue;
                }
                combinationBit2 = combinationBit1;
                combinationBit2 |= bits[j];
                for (int k = j + 1; k < wordCount - 2; k++)
                {
                    if ((combinationBit2 & bits[k]) != 0)
                    {
                        continue;
                    }
                    combinationBit3 = combinationBit2;
                    combinationBit3 |= bits[k];
                    for (int l = k + 1; l < wordCount - 1; l++)
                    {
                        if ((combinationBit3 & bits[l]) != 0)
                        {
                            continue;
                        }
                        combinationBit4 = combinationBit3;
                        combinationBit4 |= bits[l];
                        for (int m = l + 1; m < wordCount; m++)
                        {
                            //int combinationBit = bits[i] & bits[j] & bits[k] & bits[l] & bits[m];
                            if ((combinationBit4 & bits[m]) == 0)
                            {
                                combinationBit4 |= bits[m];

                                List<string> combination = new List<string>
                            {
                                dirWords[bits[i]], dirWords[bits[j]], dirWords[bits[k]], dirWords[bits[l]], dirWords[bits[m]]
                            };
                                uniqueLetterCombinations.Add(combination);
                                Console.Clear();
                                Console.WriteLine(uniqueLetterCombinations.Count());
                                //foreach(string word in combination)
                                //{
                                //    Console.Write(word+ " ");
                                //}
                                //Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }
        return uniqueLetterCombinations;
    }
}
