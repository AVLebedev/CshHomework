using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    { const uint N=100;  ///максимальное количество строк в выходном файле
      static char[] separator = {' ', ',', '.', ':', ';', '!', '?' }; //символы-разделители, которые необходимо учитывать при обработке текста
        static void Main(string[] args)
      {
          Stopwatch time = new Stopwatch();   //Измеряет время обработки
 
           string iFullFileName = null;  //Имя входного файла с текстом
           string InputFile = GetFileName("Входной файл: ", iFullFileName, ".txt", size:2000000);                 

           string dFullFileName = null;  //Имя существующего файла-словаря
           string DictionaryFile = GetFileName("Файл словаря: ", dFullFileName, ".txt", strings_count: 100000);          

           string oFullFileName = null;  //Имя выходного html-файла
           string OutputFile = GetFileName("Выходной файл: ", oFullFileName, ".html");

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
        /// Получение от пользователя имени файла
        /// </summary>
        /// <param name="message"></param>
        /// <param name="var"></param>
        /// <param name="extention"></param>
        /// <returns></returns>

        static string GetFileName(string message, string var, string extention)
        {
            while (var == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                var = Console.ReadLine(); //Имя файла
                
                if (!(var.EndsWith(extention)))
                {
                    var = null;                    
                    Console.WriteLine("Ошибка: выбранный файл не существует или имеет неподходящее расширение!");                   
                }
            }
            return var;
        }

        /// <summary>
        /// Перегрузка метода GetFileName, содержит проверку существования и размера входного файла
        /// </summary>
        /// <param name="message"></param>
        /// <param name="var"></param>
        /// <param name="extention"></param>
        /// <param name="size"></param>
        /// <returns></returns>

        static string GetFileName(string message, string var, string extention, Int64 size)
        {
            
            while (var == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                var = Console.ReadLine(); //Имя файла

                if (!(var.EndsWith(extention)) || !(File.Exists(var)))
                {
                    var = null;                    
                    Console.WriteLine("Ошибка: выбранный файл не существует или имеет неподходящее расширение!");
                }
                else
                {
                    FileInfo file = new FileInfo(var);
                    if (file.Length > size)
                    {
                        var = null;
                        Console.WriteLine("Ошибка: размер файла слишком велик!");
                    }
                }
            }
            return var;   
        }

        /// <summary>
        /// Перегрузка метода GetFileName, содержит проверку существования и числа строк в нём
        /// </summary>
        /// <param name="message"></param>
        /// <param name="var"></param>
        /// <param name="extention"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        static string GetFileName(string message, string var, string extention, int strings_count)
        {
            while (var == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                var = Console.ReadLine(); //Имя файла

                if (!(var.EndsWith(extention)) || !(File.Exists(var)))
                {
                    var = null;
                    Console.WriteLine("Ошибка: выбранный файл не существует или имеет неподходящее расширение!");
                }
                else if (File.ReadAllLines(var).Length > strings_count)
                {
                    var = null;
                    Console.WriteLine("Ошибка: количество строк в файле превышает максимальное ({0})!", strings_count);
                }
            }
            return var;
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

        /*
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
                    DicSet.Add(input.Trim(' '));
                }
            }
                return DicSet;            
        } */

        /// <summary>
        /// Загрузка файла-словаря в память с использованием класса Hashtable
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>

        static Hashtable DictionaryFileLoading(string FileName)
        {  
            Hashtable DicSet = new Hashtable(); //используется хэш-таблица (класс Hashtable)
            
            using (StreamReader dicFile = new StreamReader(FileName, Encoding.GetEncoding(1251)))
            {                
                string input = null;
                int i = 0;
                while ((input = dicFile.ReadLine()) != null)
                {
                    if (!(DicSet.ContainsValue(input)))
                    {
                        DicSet.Add(i, input.Trim(' ').ToLower());
                        i++;
                    }    
                    
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

        //static string Selection(string input, HashSet<string> Dictionary)
        static string Selection(string input, Hashtable Dictionary)
        {
            string[] inputArray = input.Split(' ');
            string output = null;

             for(int i=0; i<inputArray.Length; i++)
                {                
                    if (Dictionary.ContainsValue(inputArray[i].ToLower().TrimEnd(separator)))  //поиск в хэш-таблице по значению
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
    

