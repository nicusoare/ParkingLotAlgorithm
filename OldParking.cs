///*Designing a Parking Lot System

//   Requirements
//1: The parking lot should have multiple levels, each level with a certain number of parking spots.
//2: The parking lot should support different types of vehicles, such as cars, motorcycles, and trucks.
//3: Each parking spot should be able to accommodate a specific type of vehicle.
//4: The system should assign a parking spot to a vehicle upon entry and release it when the vehicle exits.
//5: The system should track the availability of parking spots and provide real-time information to customers.
//*/

//public class OldParking
//{
//    /*
//     ParkingLot
//    has multiple Level
//    supports different VehicleType : Car, Motorcycle, Truck

//    Park(VehicleType) - assigns a ParkingSpot on entry
//    Leave() - release ParkingSpot
//    DisplayAvailability() - display free ParkingSpots per VehicleType

//     */
//    public enum VehicleType
//    {
//        Car,
//        Motorcycle,
//        Truck
//    }

//    //abstract base class defining properties common to all vehicles
//    public abstract class Vehicle
//    {
//        //auto-properties (no private readonly backing field)
//        public string LicensePlate { get; } // get only makes these properties IMMUTABLE
//        public VehicleType Type { get; }

//        //protected constructor = these properties are exclusively initialized on creation 
//        protected Vehicle(string licensePlate, VehicleType vehicleType)
//        {
//            if (string.IsNullOrWhiteSpace(licensePlate))
//                throw new ArgumentException("License plate cannot be empty", nameof(licensePlate));

//            LicensePlate = licensePlate;
//            Type = vehicleType;
//        }
//    }

//    public class Car : Vehicle
//    {
//        public Car(string plate) : base(plate, VehicleType.Car) { }
//    }

//    public class Motorcycle : Vehicle
//    {
//        public Motorcycle(string plate) : base(plate, VehicleType.Motorcycle) { }
//    }

//    public class Truck : Vehicle
//    {
//        public Truck(string plate) : base(plate, VehicleType.Truck) { }
//    }

//    /*
//    suporta un tip de vehicule
//    este liber / ocupat
//    park()
//    leave()
//     */
//    public class ParkingSpot
//    {
//        public int Id { get; }
//        public VehicleType SpotType { get; }
//        public Vehicle? ParkedVehicle { get; private set; }
//        public bool IsOccupied => ParkedVehicle != null;

//        public ParkingSpot(int id, VehicleType spotType)
//        {
//            Id = id;
//            SpotType = spotType;
//        }

//        public bool CanFit(Vehicle vehicle) => !IsOccupied && vehicle.Type == SpotType;

//        public bool Park(Vehicle vehicle)
//        {
//            if (!CanFit(vehicle))
//            {
//                return false;
//            }
//            ParkedVehicle = vehicle;
//            return true;
//        }

//        public void Leave() => ParkedVehicle = null;
//    }

//    /*
//    * LevelNumber (nr etajului)
//    Spots
//    * SearchFreeSpot()
//    Park()
//    Leave()
//    DisplayStatus()

//     */
//    //Level
//    public class Level
//    {
//        //se asigneaza in constructor, fara set
//        public int LevelNumber { get; }

//        /*se creaza doar odata, in constructor
//        ! backing field - deoarece 
//        public List<...> {get;} in exterior e mutabila, poti Add() / Remove(),  modifica elemente
//        */
//        private readonly List<ParkingSpot> _spots;
//        //public IReadOnlyList<ParkingSpot> Spots => _spots.AsReadOnly();

//        public Level(int levelNumber, IEnumerable<ParkingSpot> spots)
//        {
//            if (spots == null)
//                throw new ArgumentNullException(nameof(spots));

//            LevelNumber = levelNumber;

//            //  ToList() ~= new List<ParkingSpot>(spots) - copie defensiva, fara referinta externa
//            //  shallow copy, raman referinte la toate ParkingSpot, nu la spots
//            _spots = spots.ToList();
//        }

//        //ParkVehicle
//        public bool ParkVehicle(Vehicle vehicle, out ParkingSpot? spot)
//        {
//            spot = _spots.FirstOrDefault(s => s.CanFit(vehicle));
//            return spot != null && spot.Park(vehicle);
//        }

//        //Leave
//        public void Leave(int spotId)
//        {
//            var spot = _spots.FirstOrDefault(s => s.Id == spotId);
//            spot?.Leave();
//        }

//        //GetAvailableSpots

//        public int GetAvailableSpots(VehicleType type) => _spots.Count(s => !s.IsOccupied && s.SpotType == type);
//    }

//    // ParkingLot
//    public class ParkingLot
//    {
//        //definit o singura data
//        private readonly List<Level> _levels;

//        public ParkingLot(IEnumerable<Level> levels)
//        {
//            _levels = levels.ToList();
//        }

//        public (Level?, ParkingSpot?) Park(Vehicle vehicle)
//        {
//            foreach (var level in _levels)
//            {
//                if (level.ParkVehicle(vehicle, out var spot))
//                {
//                    return (level, spot);
//                }
//            }

//            return (null, null);
//        }

//        public void Leave(int levelNr, int spotId)
//        {
//            var level = _levels.FirstOrDefault(l => l.LevelNumber == levelNr);

//            level?.Leave(spotId);
//        }

//        public void PrintAvailability()
//        {
//            Console.WriteLine("Availability:");

//            //add each level's free spots into global totals
//            var totals = new Dictionary<VehicleType, int>
//            {
//                { VehicleType.Car, 0 },
//                { VehicleType.Truck, 0 },
//                { VehicleType.Motorcycle, 0 }
//            };

//            foreach (var level in _levels)
//            {
//                var cars = level.GetAvailableSpots(VehicleType.Car);
//                var trucks = level.GetAvailableSpots(VehicleType.Truck);
//                var motorcycles = level.GetAvailableSpots(VehicleType.Motorcycle);

//                Console.WriteLine(
//                    $"Level {level.LevelNumber}: " +
//                    $"Cars: {cars}, " +
//                    $"Trucks: {trucks}, " +
//                    $"Motorcycle: {motorcycles}"
//                    );

//                totals[VehicleType.Car] += cars;
//                totals[VehicleType.Motorcycle] += motorcycles;
//                totals[VehicleType.Truck] += trucks;
//            }

//            Console.WriteLine("Totals : " +
//                              $"Cars: {totals[VehicleType.Car]}, " +
//                              $"Trucks: {totals[VehicleType.Truck]}, " +
//                              $"Motorcycle: {totals[VehicleType.Motorcycle]}"
//            );
//        }
//    }

//    public static void Main()    
//    {
//        //create levels
//        var level0 = new Level(0, [
//            new ParkingSpot(1, VehicleType.Car),
//            new ParkingSpot(2, VehicleType.Car),
//            new ParkingSpot(3, VehicleType.Car),
//            new ParkingSpot(4, VehicleType.Car),
//            new ParkingSpot(5, VehicleType.Motorcycle),
//            new ParkingSpot(6, VehicleType.Motorcycle)
//        ]);

//        var level1 = new Level(1, new[]
//        {
//            new ParkingSpot(1, VehicleType.Car),
//            new ParkingSpot(2, VehicleType.Car),
//            new ParkingSpot(3, VehicleType.Truck),
//            new ParkingSpot(4, VehicleType.Truck),
//            new ParkingSpot(5, VehicleType.Truck),
//            new ParkingSpot(6, VehicleType.Truck)
//        });

//        var level2 = new Level(2, new[]
//        {
//            new ParkingSpot(1, VehicleType.Car),
//            new ParkingSpot(2, VehicleType.Car),
//            new ParkingSpot(3, VehicleType.Car),
//            new ParkingSpot(4, VehicleType.Car),
//            new ParkingSpot(5, VehicleType.Motorcycle),
//            new ParkingSpot(6, VehicleType.Motorcycle)
//        });

//        //create parking lot with 3 levels
//        var lot = new ParkingLot(new[] { level0, level1, level2 });
//        //display free spots (per type)
//        lot.PrintAvailability();

//        var car = new Car("");
//        //park some cars
//        for (int i = 0; i < 5; i++)
//        {
//            car = new Car($"NR-{i}");
//            var (level, spot) = lot.Park(car);
//            if (spot != null)
//            {
//                Console.WriteLine($"Car  {car.LicensePlate} parked at L {level.LevelNumber} and S {spot.Id}");
//            }
//            else
//            {
//                Console.WriteLine($"Cannot Park Car {car.LicensePlate}");
//            }
//        }

//        for (int i = 10; i < 15; i++)
//        {
//            var truck = new Truck($"NR-{i}");
//            var (level, spot) = lot.Park(truck);
//            if (spot != null)
//            {
//                Console.WriteLine($"Truck  {truck.LicensePlate} parked at L {level.LevelNumber} and S {spot.Id}");
//            }
//            else
//            {
//                Console.WriteLine($"Cannot Park Truck  {truck.LicensePlate}");
//            }
//        }

//        for (int i = 20; i < 25; i++)
//        {
//            var motorcycle = new Motorcycle($"NR-{i}");
//            var (level, spot) = lot.Park(motorcycle);
//            if (spot != null)
//            {
//                Console.WriteLine($"Motorcycle  {motorcycle.LicensePlate} parked at L {level.LevelNumber} and S {spot.Id}");
//            }
//            else
//            {
//                Console.WriteLine($"Cannot Park Motorcycle  {motorcycle.LicensePlate}");
//            }
//        }
//        //display free spots (per type)
//        lot.PrintAvailability();

//        //some cars leave
//        lot.Leave(0, 1);
//        lot.Leave(1, 3);
//        lot.Leave(2, 5);

//        //display free spots (per type)
//        lot.PrintAvailability();

//    }
//}