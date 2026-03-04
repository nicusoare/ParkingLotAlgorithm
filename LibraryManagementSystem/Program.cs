/*
Cerinte functionale + non-functionale pentru un Library Management System 
(pe care sa il discuti / implementezi partial la interviu mid-senior C#):

1. Entitati principale: Book, Author, Member, Loan, Reservation, LibraryBranch
2. Operatii CRUD complete pe cărți + căutare avansată (titlu, autor, ISBN, categorie, disponibilitate)
3. Împrumut & returnare: reguli fine (max 3 cărți/persoană, termen 14 zile, penalizări automate)
4. Istoric împrumuturi + stare actuală a fiecărei cărți (Available / OnLoan / Reserved / Damaged)
5. Autentificare & autorizare: roluri (Member, Librarian, Admin) – minim JWT sau Identity
6. Validări business: nu poți împrumuta carte deja împrumutată, notificări la expirare
7. Arhitectură curată: DDD-lite / Clean Architecture / Vertical Slice + CQRS opțional
8. Testare: minim 70% coverage unit + integration tests (xUnit + Moq + Testcontainers)
9. Performanță & scalabilitate: paging, caching (IMemoryCache / Redis), indexare DB
10. Logging, exception handling global, ProblemDetails, HealthChecks + OpenTelemetry traces

Bonus mid-senior: Result pattern / ErrorOr, FluentValidation, Mapster/AutoMapper, EF Core owned types, eventual consistency.
*/
namespace LibraryManagementSystem
{
    public class Program
    {
        public static void Main()
        {
            LibraryManagement.Start();
        }

    }
}