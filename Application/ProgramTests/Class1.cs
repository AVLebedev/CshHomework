using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ConsoleApplication1;

namespace ProgramTests
{
    public class Class1
    {        
       
        [Test]
        public void GetFileName_ShortName_OutputFormat()
        {
            //Тест проверяет корректность обработки данных (имени файла без указания каталога,
                                                              //полученного от пользователя) в методе GetFileName
                       
            //Получено только имя файла, без расширения и полного пути
            MockFileName TestFileName1 = new MockFileName("somefile");
            //Получено имя файла и его расширение
            MockFileName TestFileName2 = new MockFileName("somefile.html");                    

           
            //Проверить, что в если в имени файла отсутствует расширение, то оно будет автоматически добавлено
            Assert.That(TestFileName1.GetFileName("Some message", ".html"), Is.EqualTo("somefile.html"));
            //Проверить, что если в имени файла расширение, соответствующее аргументу метода, то оно будет передано без изменений
            Assert.That(TestFileName2.GetFileName("Some message", ".html"), Is.EqualTo("somefile.html"));
            //Проверяется добавление нужного расширения к имени файла, если имя содержит другое расширение
            Assert.That(TestFileName2.GetFileName("Test №3", ".txt"), Is.EqualTo("somefile.html.txt"));
            
        }

        [Test]
        public void GetFileName_FullName_Output()
        {
            //Тест проверяет корректную обработку данных в случае, когда пользователь задаёт полное имя файла (включая каталог размещения)
            //Используется паттерн ААА

            //Arrange: получено имя файла, его расширение и каталог размещения
            string outputFileName = @"D:\somefile.html";
            string extention = ".html";
            string message = "Test 4";
            MockFileName TestFullFileName1 = new MockFileName(outputFileName);
            
            //Act
            var result = TestFullFileName1.GetFileName(message, extention);

            //Assert
            Assert.That(result, Is.EqualTo(outputFileName));           

        }

        [Test]
        public void GetFileName_DirectoryNotFound()
        {
            //Тест проверяет ситуацию, когда на вход подаётся несуществующая директория с файлом
            string outputFileName = @"M:\file.html";
            string extention = ".html";
            string message = "Test 5";
            MockFileName TestFileName = new MockFileName(outputFileName);

            //Act
            var result = TestFileName.GetFileName(message, extention);

            //Assert
            

            
        }
        
    }
}
