using System.Collections.Generic;

namespace progLab1
{
    public class Menu
    {
        public List<MenuItem> Items;
        private string title;
        private ITaskIO taskIO;

        public Menu(string title, ITaskIO taskIO)
        {
            Items = new List<MenuItem>();
            this.title = title;
            this.taskIO = taskIO;
        }   

        public void Display()
        {
            MenuItem result;

            do
            {
                foreach (MenuItem item in Items)
                {
                    taskIO.WriteLine($"{item.Key}. {item.Label}");
                }

                string key;

                do
                {
                    taskIO.Write("> ");
                    key = taskIO.ReadLine().Trim().ToLower();
                } while (key.Length != 1 || Items.Find(search => search.Key == key[0]) == null);

                taskIO.WriteLine("");

                result = Items.Find(search => search.Key == key[0]); 
            } while (result.Function());
        }
    }
}
