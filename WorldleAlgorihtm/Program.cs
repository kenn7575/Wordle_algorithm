using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

class Program
{

    static void Main()
    {
        var stopWatch = new System.Diagnostics.Stopwatch();

        stopWatch.Start();

        //import words from file
        List<string> alpha = readFile("alpha.txt");


        stopWatch.Stop();
        Console.WriteLine("Loading and filtering took {0} ms", stopWatch.Elapsed);



        //run algorithm
        stopWatch.Reset();
        stopWatch.Start();
        // List<List<string>> matches = Find5UniqueCombinations(alpha,1,1);

        try
        {

            DistributeTasks(2, alpha);
            stopWatch.Stop();
            Console.WriteLine("Done "+ stopWatch.Elapsed);
            Console.WriteLine("Done!" );
            Console.WriteLine("Milliseconds: " + stopWatch.Elapsed.Milliseconds);
            Console.WriteLine("Microseconds: " + stopWatch.Elapsed.Microseconds);
        } catch(Exception e)
        {
            Console.WriteLine("error: " +e);
        }






        Console.ReadKey();


        //print result

        // printRsult(matches, stopWatch.Elapsed);

    }
    static void printRsult(List<List<string>> matchesA, TimeSpan timeA)
    {
        Console.WriteLine("Finished in");
        Console.WriteLine("Minutes {0}", timeA.Minutes);
        Console.WriteLine("Seconds {0}", timeA.Seconds);
        Console.WriteLine("Milliseconds {0}", timeA.Milliseconds);
        Console.WriteLine("Nanoseconds {0}", timeA.Nanoseconds);
        Console.WriteLine();
        Console.WriteLine("Found " + matchesA.Count + " combinations in total.");

        Console.WriteLine();



        foreach (List<string> list in matchesA)
        {
            foreach (string word in list)
            {
                Console.Write(word + " " );
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
                    line = line.ToLower();
                    if (line.Length != 5) continue;
                    if (line.Distinct().Count() != 5) continue;
                    if (lines.Where(x => string.Concat(x, line).Distinct().Count() == 5).Count() > 0) continue;
                    lines.Add(line);
                    Console.Clear();
                    Console.WriteLine(+lines.Count());
                    
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("Error " + e.ToString());
        }
        return lines;
    }

    static void  DistributeTasks(int numOfThreads, List<string> words)
    {
        List<Task> threads = new();
        List<List<List<string>>> matches = new();

        
        Parallel.For(0, numOfThreads, x =>
        {
            matches.Add(Find5UniqueCombinations(words, numOfThreads, x+1)) ;
        });
        
        /*for (int i = 0; i < numOfThreads; i++)
        {
            threads.Add(i =>
            {
                Console.WriteLine("index : {0}", i);
                matches.Add(Find5UniqueCombinations(words, numOfThreads, i+1));
            }
            ));

            
        }
        foreach(Thread thr in threads)
        {
            thr.Start();
        }*/

        foreach(List<List<string>> lists in matches)
        {
            foreach (List<string> list in lists)
            {
                foreach (string word in list)
                {
                    Console.Write(word + " ");
                }
                Console.WriteLine();
            }
        }
    }

    static List<List<string>> Find5UniqueCombinations(List<string> words, int numberOfThreads, int threadNumber)
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

        int wordCountForFirstForLoop = wordCount / numberOfThreads;
        int startNumberForFirstLoop = (wordCountForFirstForLoop * threadNumber - wordCountForFirstForLoop)+1;
        int words2 = wordCount - wordCountForFirstForLoop * (numberOfThreads - threadNumber);
        Console.WriteLine("Started process on thread "+threadNumber+". Running words "+ startNumberForFirstLoop+" to "+(startNumberForFirstLoop + wordCountForFirstForLoop));

        int combinationBit1 = 0;
        int combinationBit2 = 0;
        int combinationBit3 = 0;
        int combinationBit4 = 0;


        for (int i = startNumberForFirstLoop; i < words2 - 4; i++)
        {
            combinationBit1 = bits[i];
            for (int j = 0; j < wordCount - 3; j++)
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

                            if ((combinationBit4 & bits[m]) == 0)
                            {
                                combinationBit4 |= bits[m];

                                List<string> combination = new List<string>
                           {
                               dirWords[bits[i]], dirWords[bits[j]], dirWords[bits[k]], dirWords[bits[l]], dirWords[bits[m]]
                           };
                                uniqueLetterCombinations.Add(combination);
                                foreach(string word in combination)
                                {
                                    Console.Write(word + " ");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }
        return uniqueLetterCombinations;
        
        static void FindMatches(ref int[] bits, int startIndex, int usedBits)
        {
            List<List<string>> combinations;

        }
    }
}