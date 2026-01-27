//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ParkingLiveCoding
//{
//    public class LibraryBookLending
//    {
//        /*
//        Simplified book lending system:
//        - allows book register (title, author, category)
//        - allows member register
//        - allows borrowing and returning of books
//        -       - doesn't allow borrowing of an borrowed book
//        - displays all books: available VS borrowed
//         */

//        public class Member
//        {
//            public int Id { get; }
//            public string Name { get; }

//            //when creating, should provide next Id from Library.Members.Count()+1
//            public Member(string name, int id)
//            {
//                Name = name;
//                Id = id;
//            }
//        }

//        public class Book
//        {
//            public int Id { get; }
//            public string Title { get; }
//            public string Author { get; }
//            public string Category { get; }
//            public Member? LentToMember { get; private set; }
//            public bool Available => LentToMember == null;

//            public Book(string title, string author, string category, int id)
//            {

//                Title = title;
//                Author = author;
//                Category = category;
//                Id = id;
//            }

//            public bool Borrow(Member to)
//            {
//                if (!Available)
//                {
//                    return false;
//                }

//                LentToMember = to;
//                return true;
//            }

//            public bool Return()
//            {
//                if (Available)
//                {
//                    return false;
//                }

//                LentToMember = null;
//                return true;
//            }
//        }

//        public class Library
//        {
//            public List<Book> Books { get; private set; } = [];
//            public List<Member> Members { get; private set; } = [];


//            public Library()
//            {
//            }

//            public void RegisterBooks(IEnumerable<Book> books)
//            {
//                Books.AddRange(books.Where(b => !Books.Contains(b)));
//            }

//            public void DisplayBooks()
//            {
//                var books = Books.OrderBy(b => b.Title);

//                Console.WriteLine($"Books:");
//                foreach (var b in books)
//                {
//                    Console.WriteLine($"Title: {b.Title}, Author: {b.Author}, Available: {b.Available} ");
//                }
//            }

//            public bool RegisterMember(Member member)
//            {
//                if (Members.Any(m => m.Id == member.Id))
//                {
//                    return false;
//                }

//                Members.Add(member);
//                return true;
//            }

//            public bool Borrow(int bookId, int memberId)
//            {
//                var book = Books.FirstOrDefault(b => b.Id == bookId);
//                var member = Members.FirstOrDefault(m => m.Id == memberId);
//                if (book == null || member == null)
//                {
//                    return false;
//                }

//                if (!book.Available)
//                {
//                    Console.WriteLine("Cannot borrow already lent book!");
//                    return false;
//                }

//                return book.Borrow(member);
//            }
//    }

//        public static void Main()
//        {
//            var books = new[]
//            {
//                new Book("Book1", "Author1", "CAT1",1),
//                new Book("Book2", "Author1", "CAT2",2),
//                new Book("Book3", "Author2", "CAT3",3),
//                new Book("Book4", "Author2", "CAT1",4),
//                new Book("Book5", "Author3", "CAT2",5),
//                new Book("Book6", "Author3", "CAT3",6),
//                new Book("Book7", "Author3", "CAT4",7)
//            };

//            var library = new Library();
//            library.RegisterBooks(books);

//            library.DisplayBooks();

//            library.RegisterMember(new Member("John", 1));

//            //borrow book
//            library.Borrow(1, 1);
//            library.Borrow(2, 1);
//            library.Borrow(3, 1);
//            //try borrow same book twice
//            library.Borrow(1, 1);

//            //display books
//            library.DisplayBooks();
//        }
//    }
//}
