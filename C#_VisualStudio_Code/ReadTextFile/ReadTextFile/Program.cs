using System;
using System.IO;

namespace ReadTextFile
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                String line;
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(@"WriteSimple/testread.txt");

                    //Read the first line of text
                    line = sr.ReadLine();

                    //Continue to read until you reach end of file
                    //while (line != null)
                    //{
                    //    //write the lie to console window
                    //    Console.WriteLine(line);
                    //    //Read the next line
                    //    line = sr.ReadLine();
                    //}
                    Console.WriteLine(line);
                    //close the file
                    sr.Close();
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
            }
        }
    }
}
