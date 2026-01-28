/*Designing a Parking Lot System

   Requirements
1: The parking lot should have multiple levels, each level with a certain number of parking spots.
2: The parking lot should support different types of vehicles, such as cars, motorcycles, and trucks.
3: Each parking spot should be able to accommodate a specific type of vehicle.
4: The system should assign a parking spot to a vehicle upon entry and release it when the vehicle exits.
5: The system should track the availability of parking spots and provide real-time information to customers.
*/

//using static OldParking;

public class Program
{
    private static readonly Random rnd = new Random(1234);
    private const int VEHICLES_COUNT = 10;

    public static void Main()
    {
        var level1 = new Level(1, new[] {
            new ParkingSpot(1,VehicleType.Car),
            new ParkingSpot(2,VehicleType.Car),
            new ParkingSpot(3,VehicleType.Car),
            new ParkingSpot(4,VehicleType.Car),
            new ParkingSpot(5,VehicleType.Car),
            new ParkingSpot(6,VehicleType.Truck),
            new ParkingSpot(7,VehicleType.Truck),
            new ParkingSpot(8,VehicleType.Motorcycle),
            new ParkingSpot(9,VehicleType.Motorcycle),
            new ParkingSpot(10,VehicleType.Motorcycle)
        });

        var level2 = new Level(2, new[] {
            new ParkingSpot(11,VehicleType.Truck),
            new ParkingSpot(12,VehicleType.Truck),
            new ParkingSpot(13,VehicleType.Motorcycle),
            new ParkingSpot(14,VehicleType.Motorcycle),
            new ParkingSpot(15,VehicleType.Motorcycle)
        });

        var lot = new ParkingLot(new[] { level1, level2 });

        var vehicles = new Vehicle[VEHICLES_COUNT];
        var parked = new ParkResult(false, null, null);
        //vs Count() ?
        for (int i = 0; i < vehicles.Length; i++)
        {
            var vehicleTypes = Enum.GetValues(typeof(VehicleType)).Length;
            var r = rnd.Next(vehicleTypes); //0,1,2

            switch (r)
            {
                case 0:
                    vehicles[i] = new Car(i.ToString());
                    break;
                case 1:
                    vehicles[i] = new Truck(i.ToString());
                    break;
                case 2:
                    vehicles[i] = new Motorcycle(i.ToString());
                    break;
            }
            parked = lot.TryPark(vehicles[i]);
            DisplayParkedMessage(vehicles[i], parked);
        }

        var duplicate = new Car("1");
        parked = lot.TryPark(duplicate);
        DisplayParkedMessage(duplicate, parked);

        DisplayAvailability(lot.GetStatus());

        DisplayLeaveMessage(lot.Leave("1"), "1");
        DisplayLeaveMessage(lot.Leave("2"), "2");
        DisplayLeaveMessage(lot.Leave("3"), "3");
        DisplayLeaveMessage(lot.Leave("4"), "4");
        DisplayLeaveMessage(lot.Leave("100"), "100");

        DisplayAvailability(lot.GetStatus());
    }

    private static void DisplayParkedMessage(Vehicle vehicle, ParkResult parked)
    {
        if (parked.Success)
        {
            Console.WriteLine($"{vehicle.Type.ToString()} with license {vehicle.LicensePlate} parked at Level {parked.LevelId}, Spot {parked.SpotId}");

        }
        else
        {
            string errorMessage = $"CANNOT PARK {vehicle.Type.ToString()} with license {vehicle.LicensePlate}";
            if (parked.DuplicateLicense)
            {
                errorMessage += " License plate already parked";
            }
            Console.WriteLine(errorMessage);
        }
    }

    public static void DisplayAvailability(ParkingAvailability s)
    {
        Console.WriteLine($"Free spots: {s.TotalFree} \nCars: {s.Cars}\nTrucks:{s.Trucks}\nMotorcycles:{s.Motorcycles}");
    }

    public static void DisplayLeaveMessage(bool success, string license)
    {
        if (success)
        {
            Console.WriteLine($"Vehicle with license {license} left");
        }
        else
        {
            Console.WriteLine($"Vehicle with license {license} NOT FOUND");
        }        
    }

    public enum VehicleType 
    {
        Car,
        Truck,
        Motorcycle
    }

    public abstract class Vehicle
    {
        /*
         * AUTO PROPERTIES,  no backing field - private readonly
         * get-only = immutable
         */
        public string LicensePlate { get; }
        public VehicleType Type { get; }

        //protected - these props are exclusively initialized on creation
        protected Vehicle(string licensePlate, VehicleType type)
        {
            LicensePlate = (licensePlate ?? "").Trim();

            if (string.IsNullOrWhiteSpace(LicensePlate))
                throw new ArgumentException("License plate cannot be empty", nameof(licensePlate));
                        
            Type = type;
        }
    }

    public record ParkingAvailability(
        int TotalFree,
        int Cars,
        int Trucks,
        int Motorcycles
        );

    public record ParkResult(bool Success, int? LevelId, int? SpotId, bool DuplicateLicense=false);

    public class Car(string licensePlate) : Vehicle(licensePlate, VehicleType.Car) { }
    public class Truck(string licensePlate): Vehicle(licensePlate, VehicleType.Truck) { }
    public class Motorcycle(string licensePlate): Vehicle(licensePlate, VehicleType.Motorcycle) { }

    public class ParkingSpot
    {
        //auto props, get only, immutable
        public int Id { get; }
        public VehicleType Type { get; }
                
        public Vehicle? Vehicle { get; private set; }
        public bool IsAvailable => Vehicle == null;


        public ParkingSpot(int id, VehicleType type)
        {
            Id = id;
            Type = type;
        }

        public bool CanFit(Vehicle vehicle)
        {
            return (Vehicle == null && Type == vehicle.Type);
        }

        public bool Park(Vehicle vehicle)
        {
            if (!CanFit(vehicle))
                return false;

            Vehicle = vehicle;
            return true;
        }

        public bool Leave()
        {
            if (Vehicle == null)
                return false;

            Vehicle = null;
            return true;
        }
    }

    public class Level
    {
        public int Id { get; }

        //backing field
        private readonly List<ParkingSpot> _spots;

        public Level(int id, IEnumerable<ParkingSpot> spots)
        {
            Id = id;

            // shallow copy 
            _spots = [.. spots];
        }
                
        public bool TryPark(Vehicle vehicle, out ParkingSpot? spot)
        {
            spot = _spots.FirstOrDefault(t => t.CanFit(vehicle));
                        
            return (spot != null && spot.Park(vehicle));            
        }

        public bool Leave(int spotId)
        {
            var spot = _spots.FirstOrDefault(t => t.Id == spotId);

            return spot?.Leave() ?? false;
        }

        public int GetLevelStatus(VehicleType type)
        {
            return _spots.Count(t => t.IsAvailable && t.Type == type);
        }
    }

    public class ParkingLot
    {
        private readonly List<Level> _levels;

        record ParkingLocation(int LevelId, int SpotId);

        private readonly Dictionary<string, ParkingLocation> _occupancy = new ();

        public ParkingLot(IEnumerable<Level> levels)
        {
            _levels = [.. levels];
        }

        public ParkResult TryPark(Vehicle vehicle)
        {
            int? levelParked = null;
            int? spotId = null;

            if (_occupancy.ContainsKey(vehicle.LicensePlate))
            {
                return new ParkResult(false, null, null, true);
            }

                foreach (var level in _levels)
            {
                if (level.TryPark(vehicle, out var spot))
                {
                    levelParked = level.Id;
                    spotId = spot?.Id;

                    _occupancy.Add(vehicle.LicensePlate, new ParkingLocation(level.Id, spot.Id));
                    return new ParkResult(true, levelParked, spotId);
                }
            }            
            return new ParkResult(false, null, null);
        }

        public ParkingAvailability GetStatus()
        {
            var cars = _levels.Sum(level => level.GetLevelStatus(VehicleType.Car));
            var trucks = _levels.Sum(level => level.GetLevelStatus(VehicleType.Truck));
            var motorcycles = _levels.Sum(level => level.GetLevelStatus(VehicleType.Motorcycle));

            return new ParkingAvailability(cars + trucks + motorcycles, cars, trucks, motorcycles);
        }

        public bool Leave(string license)
        {
            var licenseIsParked = _occupancy.TryGetValue(license, out var parkedLocation);
            if (!licenseIsParked)
                return false;

            (int levelId, int spotId) = parkedLocation;
            var level = _levels.FirstOrDefault(t=>t.Id==levelId);

            if (level != null && level.Leave(spotId))
                {
                    _occupancy.Remove(license);
                    return true;
                }

            return false;
        }
    }
}