using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace ConsoleApplication1
{
   
    /// <summary>
    /// Класс-заглушка, используется для тестов
    /// </summary>
    public class MockFileName : IFileName
    {
        static string file;
        public MockFileName(string TestFileName) {
            file = TestFileName;   
        }
        public string GetFileName(string message, string extention)
        {
            Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
            try
            {                
                if (!(file.EndsWith(extention))) throw new System.Exception("Неверно выбрано раcширение файла!");
            }
            // catch (System.IO.DirectoryNotFoundException)
            //{

           // }

            catch (System.Exception)
            {
                file += extention;
                //Console.WriteLine(e.Message);                    
            }

            return file;
        }
    } 

   public class Program
    { const uint N=100;  ///максимальное количество строк в выходном файле
      static char[] separator = {' ', ',', '.', ':', ';', '!', '?' }; //символы-разделители, которые необходимо учитывать при обработке текста
        static void Main(string[] args)
      {
          Stopwatch time = new Stopwatch();   //Измеряет время обработки         

           InputFileName inFile = new InputFileName(20000);
           string InputFile = inFile.GetFileName("Входной файл: ", ".txt");

           DictionaryFileName dicFile = new DictionaryFileName(100000);
           string DictionaryFile = dicFile.GetFileName("Файл словаря: ", ".txt");

           OutputFileName outFile = new OutputFileName();
           string OutputFile = outFile.GetFileName("Выходной файл: ", ".html");

           Console.WriteLine();

           time.Start();
           FileReadingAndWriting(InputFile, OutputFile, DictionaryFile);
           time.Stop();
         
           Console.WriteLine("Обработано за {0} миллисекунд", time.ElapsedMilliseconds.ToString());

           Console.WriteLine();
           Console.Write("Всё! Для продолжения нажмите <Enter>");
           Console.ReadLine();

        }       

       /// <summary>
       /// Чтение входного файла и запись выходного
       /// </summary>
       /// <param name="InputFileName"></param>
       /// <param name="OutputFileName"></param>
       /// <param name="DictionaryFileName"></param>

        static void FileReadingAndWriting(string InputFileName, string OutputFileName, string DictionaryFileName)
        { 
            //Входной поток, определяемый именем и кодировкой входного файла 
            using (StreamReader sr = new StreamReader(InputFileName, Encoding.GetEncoding(1251))) 
            {                   
                string input = null;  //Переменная для временного хранения считываемой строки
                DictionaryFileLoading(DictionaryFileName);

                StreamWriter OutputFile = new StreamWriter(OutputFileName);  //Поток вывода               

                string output = null;  //Переменная для временного хранения обработанной строки
                int n = 0;        //счётчик количества строк в полученном файле
                int i = 0;        //счётчик создающихся выходных файлов

              while ((input = sr.ReadLine()) != null)
                {
                    output = Selection(input, DictionaryFileLoading(DictionaryFileName));
                    OutputFile.Write(output + "<br>"); 
                    n++;
                    if (n == N)   //при достижении в выходном выйле максимального числа строк
                    {
                        OutputFile.Close();  //закрыть поток
                        i++;                        
                        //Создать новый файл и продолжить работу с ним
                        OutputFile = new StreamWriter(OutputFileName.Replace(".html", "_" + i.ToString() + ".html"));
                        n = 0;
                    }
                }                 
                OutputFile.Close();
             }
        }

        
        /// <summary>
        /// Загрузка файла-словаря в память с использованием HashSet
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>

        static HashSet<string> DictionaryFileLoading(string FileName)
        {
            HashSet<string> DicSet = new HashSet<string>(); //используется множество строк (неупорядоченное)
            using (StreamReader dicFile = new StreamReader(FileName, Encoding.GetEncoding(1251)))
            {
                string input = null;
                while ((input = dicFile.ReadLine()) != null)
                {                   
                    DicSet.Add(input.Trim(' ').ToLower());
                }
            }
                return DicSet;            
        } 

        /// <summary>
        /// Нахождение в тексте входного файла слов из файла-словаря и их выделение
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Dictionary"></param>
        /// <returns></returns>

        static string Selection(string input, HashSet<string> Dictionary)
       
        {
            string[] inputArray = input.Split(' ');
            string output = null;

             for(int i=0; i<inputArray.Length; i++)
                {                
                    if (Dictionary.Contains(inputArray[i].ToLower().TrimEnd(separator)))  //поиск слов в контейнере
                    {
                        inputArray[i] = "<b><em>" + inputArray[i] + "</em></b>"; //добавление html-тегов разметки к найденным словам                       
                    }
                 }
                            
            foreach (string b in inputArray)
            {
                output += b + ' ';
            }
            return output;
        }            

    }
}
    

