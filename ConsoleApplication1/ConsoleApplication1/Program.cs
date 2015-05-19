using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace ConsoleApplication1
{
    class Program
    { const uint N=11;  //максимальное количество строк в выходном файле
      static char[] separator = {' ', ',', '.', ':', ';', '!', '?' }; //символы-разделители, которые необходимо учитывать при обработке текста
        static void Main(string[] args)                           
        {       
 
           string iFullFileName = null;  //Имя входного файла с текстом
           string InputFile = GetFileName("Входной файл: ", iFullFileName, ".txt", size:2000000);                 

           string dFullFileName = null;  //Имя существующего файла-словаря
           string DictionaryFile = GetFileName("Файл словаря: ", dFullFileName, ".txt", strings_count:100000);          

           string oFullFileName = null;  //Имя выходного html-файла
           string OutputFile = GetFileName("Выходной файл: ", oFullFileName, ".html");          
            
           FileReadingAndWriting(InputFile, OutputFile, DictionaryFile);

           Console.Write("Всё! Для продолжения нажмите <Enter>");
           Console.ReadLine();
        }

        //Получение от пользователя имени файла
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

        //Перегрузка метода GetFileName, содержит проверку существования и размера входного файла
        static string GetFileName(string message, string var, string extention, Int64 size)
        {
            
            while (var == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                var = Console.ReadLine(); //Имя файла

                if (!(var.EndsWith(extention)) || !(File.Exists(var)))
                {
                    var = null;
                    //Console.Clear();
                    Console.WriteLine("Ошибка: выбранный файл не существует или имеет неподходящее расширение!");
                    // throw new Exception("Не подходящее расширение файла!");

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

        //Перегрузка метода GetFileName, содержит проверку существования входного файла и числа строк в нём
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

       //Чтение входного файла и запись выходного
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
                    OutputFile.Write(output+"<br>");
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

        //Загрузка файла-словаря в память
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
        }

        //Нахождение в тексте входного файла слов из файла-словаря и их выделение
        static string Selection(string input, HashSet<string> Dictionary)
        {
            string[] inputArray = input.Split(' ');
            string output = null;

            foreach (string a in Dictionary)
            {
                for(int i=0; i<inputArray.Length; i++)
                {
                    if (inputArray[i].ToLower().TrimEnd(separator) == a.ToLower())  //сравнение слов с учётом регистра
                    {
                        inputArray[i] = "<b><em>" + inputArray[i] + "</em></b>"; //добавление html-тегов разметки к найденным словам                       
                    }
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
    

