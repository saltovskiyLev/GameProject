using System;
using System.Collections.Generic;

namespace homevork3
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateLibrary_WithErrors();

            Console.ReadKey();
        }

        class Book
        {
            public string Author;
            public string Title;
            public int Pages;
            public int Edition;
        }

        static void CreateLibrary_WithErrors()
        {
            // это список книг в домашней библиотеке
            List<Book> books = new List<Book>();

            // первая книга
            Book b = new Book();
            b.Author = "J.R.R.Tolkien";
            b.Title = "The Lord of the Rings";
            b.Pages = 930;
            b.Edition = 7;
            books.Add(b);

            // вторая книга
            b = new Book();
            b.Author = "Steve McConnell";
            b.Title = "Code Complete";
            b.Pages = 1120;
            b.Edition = 4;
            books.Add(b);

            // третья книга
            b = new Book();
            b.Author = "Martin Fowler";
            b.Title = "Refactoring. Improving the Design of Existing Code.";
            b.Pages = 860;
            b.Edition = 2;
            books.Add(b);

            // с помощью отдельной функции выводим информацию обо всех книгах на экран
            ShowLibrary_WithErrors(books);

        }

        static void ShowLibrary_WithErrors(List<Book> books)
        {
            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine("Author: " + books[i].Author);
                Console.WriteLine("Title: " + books[i].Title);
                Console.WriteLine("Pages: " + books[i].Pages);
                Console.WriteLine("Edition: " + books[i].Edition);
                Console.WriteLine();
            }

        }
    }
}
