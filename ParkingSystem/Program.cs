
// 1). Designing a Parking Lot System
/*
   Requirements
1: The parking lot should have multiple levels, each level with a certain number of parking spots.
2: The parking lot should support different types of vehicles, such as cars, motorcycles, and trucks.
3: Each parking spot should be able to accommodate a specific type of vehicle.
4: The system should assign a parking spot to a vehicle upon entry and release it when the vehicle exits.
5: The system should track the availability of parking spots and provide real-time information to customers.
*/

// 2. Library management system
/*
 1. The library should have multiple sections, each with a certain number of book slots.
2. The library should support different types of books: Fiction, Science, History.
3. Each section should only accommodate one type of book.
4. The system should allow a member to borrow a book by ISBN upon request.
5. The system should release the book slot when a member returns a book.
6. A member cannot borrow the same book twice simultaneously.
7. The system should track which member has which book.
8. The system should show real-time availability per book type.

VS 




 */

/*
3) Elevator Control System (Lift Dispatcher)

Scop: alocă cereri de lifturi în mod “optimal”.
Cerințe
Ai K lifturi, fiecare cu:
    etaj curent 
    direcție (Up/Down/Idle)
    coadă de opriri

Primești cereri de tip:
    HallCall(floor, direction)
    CabCall(elevatorId, floor)

Implementeaza un dispatcher care decide:
    ce lift preia un HallCall
    cum se inserează oprirea în ruta liftului

Constrângeri:
    minimizează timpul mediu de așteptare (heuristic OK)
    tratează corect stări concurente (simulare tick-based e OK)

Edge cases:
    două cereri simultane
    lift supraîncărcat (opțional)
    “starvation” pentru etaje rare

Ce testează: modelare stare, scheduling, politici.

*/


//using static OldParking;

using ParkingLiveCoding;

public class Program
{
    public static void Main()
    {
        //OldParkingLot.Start();
        //NewParkingLot.Start();
        //OldLibraryBookLending.Start();
        LibraryManagement.Start();
    }
}
