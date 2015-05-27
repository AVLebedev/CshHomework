using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ProgramTests
{
    public class Class1
    {
        [Test]
        public void Test1()
        {
            Assert.That(ConsoleApplication1.Program.GetFileName("Some message", "another file.txt", ".html"), Is.EqualTo("another file.txt"));
        }
    }
}
