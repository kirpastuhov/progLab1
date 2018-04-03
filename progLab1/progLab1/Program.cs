namespace progLab1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var taskIO = new TaskIO();
            var runProgram = new TaskRunner(taskIO);

            runProgram.Run();
        }
    }
}
