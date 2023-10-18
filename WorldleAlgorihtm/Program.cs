using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, "words.txt");

        List<string> words = readFile("beta.txt");

        List<string> fiveLetterWords = find5LetterWords(words);
        List<string> nonDublicateLetterWords = removeDublicates(fiveLetterWords);
        

        List<List<string>> matches = find5UniqueCombinations(nonDublicateLetterWords);
        Console.WriteLine("100% done!");
        Console.WriteLine("Found "+matches.Count+ " combinations in total.");
        Console.WriteLine("Press any key to render combinations.");
        Console.ReadKey();
        foreach(List<string> list in matches)
        {
            foreach(string word in list)
            {
                Console.Write(word + " ");
            }
            Console.WriteLine();

        }





        Console.ReadKey();
    }
    static List<string> find5LetterWords(List<string> lines)
    {
        List<string> fiveLetterWords = new ();
        foreach (string line in lines)
        {
            if (line.Length == 5)
            {

                fiveLetterWords.Add(line);
            }
        }
        return fiveLetterWords;
    }
    static List<string> removeDublicates(List<string> words)
    {
        List<string> nonDublicateLetterWords = new List<string>();

        foreach (string word in words)
        {
            List<char> lettersInWord = new List<char>();
            int index = 0;

            foreach (char letter in word)
            {
                index++;
                if (lettersInWord.Contains(letter))
                {
                    break;
                }
                else
                {
                    lettersInWord.Add(letter);
                }
                if (index == 5)
                {
                    nonDublicateLetterWords.Add(word);
                    Console.Write(word + " ");
                }
            }
        }
        Console.WriteLine(nonDublicateLetterWords.Count);
        return nonDublicateLetterWords;
    }
    static int countLetterJ(List<string> words)
    {
        int count = 0;
        foreach(string word in words)
        {
            foreach(char letter in word)
            {
                if (letter == 'j')
                {
                    count++;
                }
            }
        }
        return count;
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
    static void calculateProgress(long index)
    {

        double totalCombinationCount = 77;


        double percent =(index / totalCombinationCount) * 100;

        Console.Clear();
        Console.WriteLine( percent = Math.Truncate(percent * 1000) / 1000);

        for (int i = (int)Math.Floor(percent); i >= 0; i--)
        {
            Console.Write("#");
        }
        Console.WriteLine();
        for (int i = 100; i >= 0; i--)
        {
            Console.Write("_");
        }
    }
    static List<List<string>> find5UniqueCombinations(List<string> words)
    {
        // Read the list of 5-letter words from a file or another source


        // Create a list to store combinations of words with unique letters
        List<List<string>> uniqueLetterCombinations = new List<List<string>>();

        // Generate all combinations of 5 words
        
        for (int i = words.Count; i > 0; i--)
        {  
            for (int j = i + 1; j > 0; j--)
            {
                for (int k = j + 1; k > 0; k--)
                {
                    for (int l = k + 1; l > 0; l--)
                    {  
                        for (int m = l + 1; m > 0; m--)
                        {
                            List<string> combination = new List<string>
                            {
                                words[i], words[j], words[k], words[l], words[m]
                            };
                            // Check if the combination has no repeated letters
                            if (HasUniqueLetters(combination))
                            {
                                //index++;
                                foreach(string word in combination)
                                {
                                    Console.Write(word + " ");
                                }
                                Console.WriteLine();
                                
                                //Console.Clear();
                                //calculateProgress(index);
                                uniqueLetterCombinations.Add(combination);
                            }
                        }
                    }
                }
            }
        }
        return uniqueLetterCombinations;
    }
    static bool HasUniqueLetters(List<string> words)
    {
        var letters = new HashSet<char>();

        foreach (var word in words)
        {
            foreach (char letter in word)
            {
                if (letters.Contains(letter))
                {
                    return false;
                }
                letters.Add(letter);
            }
        }

        return true;
    }
    /*static List<List<byte>> compress(List<string> words)
    {
        List<List<byte>> listOfBytes = new();
        foreach(string word in words)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(new char[] { 'a' });
            byte byteValue = byteArray[0];

            foreach (char character in word)
            { 
                char byte_ = character;
            }
        }
    }*/
}
