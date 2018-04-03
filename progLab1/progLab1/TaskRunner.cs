using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace progLab1
{
    public class TaskRunner
    {
        public Menu MainMenu;

        private const string dateFormat = "d-m-yyyy";
        private Regex isbnRegex = new Regex(@"(?=[-0-9 ]{13}$)(?:[0-9]+[- ]){3}[0-9]*[0-9]$");
        private List<Book> books = new List<Book>();
        private ITaskIO taskIO;

        public TaskRunner(ITaskIO taskIO)
        {
            this.taskIO = taskIO;
            MainMenu = new Menu("Book catalog", taskIO);

            MainMenu.Items.AddRange(new List<MenuItem>
            {
                new MenuItem()
                {
                    Label = "Add book",
                    Key = '1',
                    Function = AddBook
                },
                new MenuItem()
                {
                    Label = "Find a book by ISBN",
                    Key = '2',
                    Function = GetBookByISBN
                },
                new MenuItem()
                {
                    Label = "Find a book by keywords",
                    Key = '3',
                    Function = GetBooksByKeywords
                },
                new MenuItem()
                {
                    Label = "Quit",
                    Key = '0',
                    Function = () => false
                },
                new MenuItem()
                {
                    Label = "Library",
                    Key = '8',
                    Function = ListBooks
                },
            });
        }
        
        public void Run()
        {
            MainMenu.Display();
        }
        
        private bool GetBooksByKeywords()
        {
            string input;

            do
            {
                taskIO.Write("Enter keywords separated by space: ");
                input = taskIO.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));

            var keywords = input.Split(' ');
            var search = new List<(Book book, int amountOfFoundWords)>();
            var annotationSearch = new List<(Book book, IEnumerable<string> wordsFoundInAnnotation)>();

            foreach (var book in books)
            {
                var words = new List<string>((book.Title + ' ' + book.Author + ' ' + book.PublicationDate + ' ' + book.ISBN).Split(' '));
                var annotationWords = new List<string>(book.Annotation.Split(' '));

                var found = words.FindAll(s => keywords.Contains(s));
                var foundInAnnotation = annotationWords.FindAll(s => keywords.Contains(s));

                search.Add((book, found.Count + foundInAnnotation.Count));
                annotationSearch.Add((book, foundInAnnotation.Distinct()));
            }

            foreach (var t in search.OrderByDescending(x => x.amountOfFoundWords))
            {
                (var book, var amountOfFoundWords) = t;
                if (amountOfFoundWords > 0)
                {
                    taskIO.WriteLine(book.bookInfo(true));
                    taskIO.WriteLine($"Words amount: {amountOfFoundWords}");
                    taskIO.WriteLine("Words in annotation: ");

                    var words = annotationSearch.First(x => x.book == book).wordsFoundInAnnotation;

                    foreach (var word in words)
                    {
                        taskIO.WriteLine($"\t+ {word}");
                    }
                }
            }

            return true;
        }

        private bool GetBookByISBN()
        {
            if (books.Count == 0)
            {
                taskIO.WriteLine("There's no books in the library.");
                return true;
            }

            string input;

            do
            {
                taskIO.Write("Enter book ISBN (1-333-55555-1): ");
                input = taskIO.ReadLine().Trim();
            } while (!isbnRegex.IsMatch(input));

            var book = books.Find(b => string.Compare(b.ISBN, input, true) == 0);

            if (book is null)
            {
                taskIO.WriteLine("No books were found.");
            }
            else
            {
                taskIO.WriteLine($"Book info:\n{book}");
            }

            return true;
        }

        private bool ListBooks()
        {
            foreach (var book in books)
                taskIO.WriteLine($"{book}");

            return true;
        }
        
        private bool AddBook()
        {
            var book = new Book();
            string input;

            do
            {
                taskIO.Write("Input title: ");
                input = taskIO.ReadLine().Trim();
            } while (string.IsNullOrWhiteSpace(input));

            book.Title = input;

            do
            {
                taskIO.Write("Input author: ");
                input = taskIO.ReadLine().Trim();
            } while (string.IsNullOrWhiteSpace(input));

            book.Author = input;

            do
            {
                taskIO.Write("Input annotation: ");
                input = taskIO.ReadLine().Trim();
            } while (string.IsNullOrWhiteSpace(input));

            book.Annotation = input;

            do
            {
                taskIO.Write("Input ISBN (1-333-55555-1): ");
                input = taskIO.ReadLine().Trim();
            } while (!isbnRegex.IsMatch(input));

            book.ISBN = input;

            if (books.Find(b => string.Compare(b.ISBN, book.ISBN, true) == 0) != null)
            {
                taskIO.WriteLine("There's a book with the same ISBN in library.");
                return true;
            }

            DateTime date;
            do
            {
                taskIO.Write("Input publication date (d-m-yyyy): ");
                input = taskIO.ReadLine().Trim();
            } while (!DateTime.TryParseExact(input, dateFormat, new CultureInfo("ru-RU"), DateTimeStyles.AdjustToUniversal, out date));

            book.PublicationDate = date;
            books.Add(book);

            return true;
        }
    }
}
