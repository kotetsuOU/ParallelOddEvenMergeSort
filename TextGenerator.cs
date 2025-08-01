using System;
using System.IO;
using System.Threading.Tasks;

namespace MySortLib
{
    public class TextGenerator
    {
        int fileCount;
        int n;
        int processors;
        string type;

        public TextGenerator(int n = 8, int processors = 1, int fileCount = 1, string type = "int")
        {
            this.fileCount = fileCount;
            this.n = n;
            this.processors = processors;
            this.type = type;
            Console.WriteLine($"Generating {fileCount} text files with {n} pairs of numbers each, using {processors} processors.");

            // Validate n to be a power of 2
            if (Math.Log(n, 2) % 1 != 0)
            {
                Console.WriteLine("n must be a power of 2.");
                this.n = (int)Math.Pow(2, Math.Ceiling(Math.Log(n, 2)));
                Console.WriteLine($"n adjusted to {this.n} to be a power of 2.");
            }
        }

        public void GenerateTextFile()
        {
            Random random = new Random();
            for (int i = 0; i < fileCount; i++)
            {
                string fileName = $"input{i + 1}.txt";
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine($"{n} {processors}");
                    for (int j = 0; j < n; j++)
                    {
                        if (type == "int")
                        {
                            int a = random.Next(-50, 51);
                            int b = random.Next(-50, 51);
                            writer.WriteLine($"{a} {b}");
                        }
                        else if (type == "float")
                        {
                            float a = (float)(random.NextDouble() * 100 - 50);
                            float b = (float)(random.NextDouble() * 100 - 50);
                            writer.WriteLine($"{a} {b}");
                        }
                    }
                }
                //Console.WriteLine($"Generated file: {fileName}");
            }
        }
    }
}
