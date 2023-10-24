using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

class Program
{
    public static List<List<int>> matches = new();
    static void Main()
    {
        try
        {
            //import words from file
            List<string> words = readFile("alpha.txt");
            Console.WriteLine("Loading and filtering complete.");

            //convert to binaary
            Dictionary<int, string> dirWords = new Dictionary<int, string>();
            int wordCount = words.Count();
            int[] bits = new int[wordCount];
            for (int i = 0; i < wordCount; i++)
            {
                var bit = 0;
                foreach (var ch in words[i])
                {
                    bit |= 1 << (ch - 'a');
                }
                dirWords.TryAdd(bit, words[i]);
                bits[i] = bit;
            }

            //run algorithm


            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            DistributeTasks(1, bits);
            stopWatch.Stop();

            //print results
            //printRsult();
        } catch(Exception e)
        {
            Console.WriteLine("error: " +e);
        }
        Console.ReadKey();
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
    static void  DistributeTasks(int numOfThreads, int[] bits)
    {
        List<Task> threads = new();
        List<List<string>> matches = new();

        
        Parallel.For(0, numOfThreads, x =>
        {
            Find5UniqueCombinations(bits, numOfThreads, x + 1) ;
        });
    }
    

    static void Find5UniqueCombinations(int[] bits, int numOfThreads, int threadIndex)
    {
       
        int wordCount = bits.Length;

        

        void RecursiveCombination(int index, int combinationBit, List<int> currentCombination)
        {
            if (currentCombination.Count == 5)
            {
                matches.Add(new List<int>(currentCombination));
                try
                {
                    Console.Clear();   
                    Console.WriteLine(matches.Count());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString()); 
                }
                return;
            }

            for (int i = index; i < wordCount; i++)
            {
                if ((combinationBit & bits[i]) == 0)
                {
                    currentCombination.Add(bits[i]);
                    combinationBit |= bits[i];
                    RecursiveCombination(i + 1, combinationBit, currentCombination);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                    combinationBit ^= bits[i];
                }
            }
        }

        RecursiveCombination(0, 0, new List<int>());

        
    }
    /*static List<List<string>> Find5UniqueCombinations(List<string> words, int numberOfThreads, int threadNumber)
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
                                    Console.Clear();
                                    Console.WriteLine("Core: " + threadNumber);
                                    Console.WriteLine("Index: " + uniqueLetterCombinations.Count()); ;

                                }
                                
                            }
                        }
                    }
                }
            }
        }
        return uniqueLetterCombinations;
    }*/
}
