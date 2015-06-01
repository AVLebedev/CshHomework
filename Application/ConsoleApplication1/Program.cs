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
    /// Интерфейс для реализации приёма входных данных от пользователя
    /// </summary>
    interface IFileName
    {
        string GetFileName(string message, string extention);
    }

    interface IInputFileName
    {
        string GetFileName(string message, string extention, int restriction);
    }

    /// <summary>
    /// Класс, принимающий от пользователя входные данные
    /// </summary>

    public class FileName : IFileName //IInputFileName
    {
        /// <summary>
        /// Ограничение, накладываемое на входной файл
        /// </summary>
        /// <param name="restriction"></param>
        ushort sizeRestriction = 0;
        uint strCountRestriction = 0;

        public FileName() { }

        public FileName(ushort sizeRestriction)
        {
            this.sizeRestriction = sizeRestriction;
        }

        /// <summary>
        /// strCountResrtiction - ограничение на число строк во входном файле
        /// </summary>
        /// <param name="strCountRestriction"></param>
        public FileName(uint strCountRestriction)
        {
            this.strCountRestriction = strCountRestriction;
        }
   
        /// <summary>
        /// Получение имени (каталога) выходного файла
        /// </summary>
        /// <param name="message"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public string GetFileName(string message, string extention)
        {
            string file = null;

            while (file == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                try
                {
                    file = Console.ReadLine(); //Имя файла
                    FileInfo validator = new FileInfo(file);
                    if (!(validator.Directory.Exists)) throw new System.IO.DirectoryNotFoundException("Директория не найдена!");
                    if (!(file.EndsWith(extention))) throw new System.Exception("Неверно выбрано раcширение файла!");                    

                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    file = null;
                    continue;
                }

                catch (System.Exception)
                {
                    file += extention;                  
                }
            }

            return file;
        }

        /// <summary>
        /// Получение имени (каталога) входного файла
        /// </summary>
        /// <param name="message"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public string GetInputFileName(string message, string extention)
        {
            string fileName = null;

            while (fileName == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                fileName = Console.ReadLine(); //Имя файла

                if (!(fileName.EndsWith(extention)) || !(File.Exists(fileName)))
                {
                    fileName = null;
                    Console.WriteLine("Ошибка: выбранный файл не существует или имеет неподходящее расширение!");
                }
                else
                {
                    FileInfo file = new FileInfo(fileName);
                    if (file.Length > sizeRestriction)
                    {
                        fileName = null;
                        Console.WriteLine("Ошибка: размер файла слишком велик!");
                    }
                }
            }
            return fileName;   
        }
    }

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
          FileName fileName = new FileName(); //Объект для получения входных данных от пользователя
 
           string iFullFileName = null;  //Имя входного файла с текстом
           string InputFile = GetFileName("Входной файл: ", iFullFileName, ".txt", size:2000000);                 

           string dFullFileName = null;  //Имя существующего файла-словаря
           string DictionaryFile = GetFileName("Файл словаря: ", dFullFileName, ".txt", strings_count: 100000);          

           //string oFullFileName = null;  //Имя выходного html-файла
           string OutputFile = fileName.GetFileName("Выходной файл: ", ".html");

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

      /*  public static string GetFileName(string message, string extention)
        {
            string file = null;
           
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                try
                {
                    file = Console.ReadLine(); //Имя файла                   
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
        } */

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
    

