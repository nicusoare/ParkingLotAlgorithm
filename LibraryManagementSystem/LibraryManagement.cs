using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

/*  Library Management System Requirements
Context: Build a simple library system for a small community library with ~100 books.

Core Features:
1. The system should maintain a catalog of books with title, author, ISBN, and genre.
2. Members can search for books by title, author, or ISBN.
3. Members can borrow available books (maximum 5 books per member).
4. Each loan has a 14-day return period tracked by the system.
5. Members can return books, making them available for others.
6. The system should prevent borrowing the same book twice by the same member (same ISBN) simultaneously.
7. The system should show real-time availability of books in the catalog.
8. Support multiple copies of the same book (same ISBN, different copy IDs).

Technical Constraints:
- In-memory storage (no database)
- Focus on clean code, SOLID principles, and testability
- Proper error handling for invalid operations

Evaluation Criteria:
- Class design and responsibility distribution
- Edge case handling
- Code clarity and naming
- Time management (aim for working solution in 45 min, tests in remaining 15 min)
*/

namespace LibraryManagementSystem
{
    public class LibraryManagement
    {
        public static void Start()
        {
            //1. Create Books Catalogue - Library
            var library = new Library();



            //2. Create Members

            //3. Search for Books

            //4. Borrow books (valid + edge cases: more than 5 per member, same isbn twice, expiration exceeded, no copy left)

            //5. Return Books (valid + invalid: book not borrowed)

            //6. Display availability snapshot

        }
    }

    //orchestrator
    public sealed class Library
    {
        private readonly Dictionary<string, Book> _booksByIsbn = new(); //target-typed new() VS collection expression []

        public Library()
        {

        }

        public void AddBook(Book b, int copies)
        {
            //validation



            //add copies
            b.AddCopies(copies);
        }

        //BorrowBook

        //ReturnBook

        //SearchBooks

        //GetAvailability

    }

    public enum Genre
    {
        History,
        Literature,
        Children,
        Music,
        Family
    }

    public sealed class Book
    {
        private const int DEFAULT_NUMBER_OF_COPIES = 5;
        //title, author, ISBN, and genre
        public string Title { get; }
        public string Author { get; }
        public string ISBN { get; }
        public Genre Genre { get; }

        private readonly List<BookCopy> _copies = [];
        public IReadOnlyList<BookCopy> Copies => _copies.AsReadOnly();

        public int TotalCopies => _copies.Count;
        public int AvailableCopies => _copies.Count(c => c.IsAvailable);

        public Book(string title, string author, string isbn, Genre genre)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Genre = genre;
        }

        public List<BookCopy> AddCopies(int numberOfCopies = DEFAULT_NUMBER_OF_COPIES)
        {
            var copies = new List<BookCopy>();

            for (int i = 0; i < numberOfCopies; i++)
            {
                var item = new BookCopy(this, _copies.Count + 1);
                copies.Add(item);
            }

            _copies.AddRange(copies);

            return copies;
        }
    }

    public sealed class BookCopy
    {
        public string ExternalId => $"{Book.ISBN}-{CopyNumber}:D3";
        public int CopyNumber { get; } // Id
        public Book Book { get; }

        public int? BorrowedByMemberId { get; private set; }
        public bool IsAvailable => BorrowedByMemberId == null;

        public BookCopy(Book book, int copyNumber)
        {
            Book = book;
            CopyNumber = copyNumber;
        }

        //Borrow

        //MakeAvailable
    }

    public sealed class Member
    {
        public int Id { get; }

        public string Name { get; }

        public List<BookCopy> BorrowedBookCopies = [];

        public Member(int id, string name)
        {

        }
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        protected Result(bool isSuccess, string errorMessage)
        {
            if (!isSuccess && string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage), "Failure must provide an error message");

            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new (true, string.Empty);
        public static Result Failure(string error) => new (false, error);
    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Cannot get value of a failed result");
                return _value!;
            }
        }

        protected Result(bool isSuccess, T? value, string error) : base(isSuccess, error)
        {
            if (isSuccess && value == null)
                throw new ArgumentNullException(nameof(value), "Success must provide a value");

            _value = value;
        }

        public static Result<T> Success(T value) => new (true, value, string.Empty);
        public static new Result<T> Failure(string error) => new (false, default, error);
    }
}
