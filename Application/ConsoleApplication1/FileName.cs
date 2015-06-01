using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication1
{
      /// <summary>
    /// Интерфейс для реализации приёма входных данных от пользователя
    /// </summary>
    interface IFileName
    {
        string GetFileName(string message, string extention);
    }

    /// <summary>
    /// Класс, принимающий от пользователя входные данные (имя файла)
    /// </summary>
    public class FileName
    {
        /// <summary>
        /// Строка, хранящая полное или частичное имя файла
        /// </summary>
       protected string fileName = null;
        /// <summary>
        /// Ограничение, накладываемое на входной файл
        /// </summary>
       protected uint restriction;
    }

    /// <summary>
    /// Входной файл с текстом
    /// </summary>
    public class InputFileName : FileName, IFileName //IInputFileName
    {
        
        public InputFileName() { }

        public InputFileName(ushort sizeRestriction)
        {
            this.restriction = sizeRestriction;
        }       

        /// <summary>
        /// Получение имени (каталога) входного файла
        /// </summary>
        /// <param name="message"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public string GetFileName(string message, string extention)
        {

            while (fileName == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                try
                {
                    fileName = Console.ReadLine();
                    FileInfo validator = new FileInfo(fileName);
                    if (!(fileName.EndsWith(extention))) throw new System.ArgumentException("Неверно выбрано раcширение файла!");
                    if (!(File.Exists(fileName))) throw new System.IO.FileNotFoundException("Файл не существует!");                    
                    if (validator.Length > restriction) throw new System.Exception("Размер файла слишком велик!");
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    fileName = null;
                    continue;
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    fileName = null;
                    continue;
                }               
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    fileName = null;
                    continue;
                }
            }

            return fileName;
        }
    }

    /// <summary>
    /// Получение имени (каталога) файла-словаря
    /// </summary>
    public class DictionaryFileName : FileName, IFileName
    {
        public DictionaryFileName() { }        

         /// <summary>
        /// strCountResrtiction - ограничение на число строк во входном файле
        /// </summary>
        /// <param name="strCountRestriction"></param>
        public DictionaryFileName(uint strCountRestriction)
        {
            this.restriction = strCountRestriction;
        }

        /// <summary>
        /// Получение имени (каталога) файла-словаря
        /// </summary>
        /// <param name="message"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public string GetFileName(string message, string extention)
        {

            while (fileName == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                try
                {
                    fileName = Console.ReadLine(); 
                    if (!(fileName.EndsWith(extention))) throw new System.ArgumentException("Неверно выбрано раcширение файла!");                  
                    if (!(File.Exists(fileName))) throw new System.IO.FileNotFoundException("Файл не существует!");                   
                    if (File.ReadAllLines(fileName).Length > restriction) throw new System.Exception("Число строк в файле слишком велико!");
                }
                catch (System.ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    fileName = null;
                    continue;
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    fileName = null;
                    continue;
                }                
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    fileName = null;
                    continue;
                }
            }
            return fileName;
        }

        
    }

     /// <summary>
    /// Получение имени (каталога) файла-словаря
    /// </summary>
    public class OutputFileName : FileName, IFileName
    {
        /// <summary>
        /// Получение имени (каталога) выходного файла
        /// </summary>
        /// <param name="message"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public string GetFileName(string message, string extention)
        {
            while (fileName == null)
            {
                Console.Write(message);   //Сообщение, которое выводится при запросе имени файла
                try
                {
                    fileName = Console.ReadLine();
                    FileInfo validator = new FileInfo(fileName);
                    if (!(validator.Directory.Exists)) throw new System.IO.FileNotFoundException("Выбранный путь не существует!");
                    if (!(fileName.EndsWith(extention))) throw new System.ArgumentException("Неверно выбрано раcширение файла!");                   
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    fileName = null;
                    continue;
                }
                catch (System.ArgumentException)
                {
                    fileName += extention;
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    fileName = null;
                    continue;
                }
            }
            return fileName;
        }
    }
}
