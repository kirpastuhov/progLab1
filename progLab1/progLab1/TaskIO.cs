using System;

namespace progLab1
{
    public class TaskIO : ITaskIO
    {
        public void Write(string text) => Console.Write(text);
        public void WriteLine(string text) => Write(text + '\n');
        public string ReadLine() => Console.ReadLine();
    }
}
