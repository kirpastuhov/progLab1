using System;
using System.Text;

namespace progLab1
{
    public class Book
    {
        public string Title;
        public string Author;
        public string Annotation;
        public string ISBN;
        public DateTime PublicationDate;

        public string  bookInfo(bool annotation = true)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Name: ").Append(Title).Append('\n');
            stringBuilder.Append("Author: ").Append(Author).Append('\n');
            stringBuilder.Append("ISBN: ").Append(ISBN).Append('\n');
            stringBuilder.Append("PublicationDate: ").Append(PublicationDate.ToString("d-m-yyyy")).Append('\n');

            if (annotation)
            {
                stringBuilder.Append("Annotation: ").Append(Annotation).Append('\n');
            }

            return stringBuilder.ToString();
        }
        
        public override string ToString() => bookInfo(annotation: true);
    }
}
