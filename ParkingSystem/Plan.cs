//using System;
//using System.Collections.Generic;
//using System.Linq;
///*Designing a Parking Lot System

//   Requirements
//1: The parking lot should have multiple levels, each level with a certain number of parking spots.
//2: The parking lot should support different types of vehicles, such as cars, motorcycles, and trucks.
//3: Each parking spot should be able to accommodate a specific type of vehicle.
//4: The system should assign a parking spot to a vehicle upon entry and release it when the vehicle exits.
//5: The system should track the availability of parking spots and provide real-time information to customers.
//*/

//public enum VehicleType
//{
//    Car,
//    Truck,
//    Motorcycle
//}


//public abstract class Vehicle
//{
//    public VehicleType Type { get; }
//    public string LicensePlate { get; }

//    //protected, only initialized on creation! ( new Car/Truck,..)
//    protected Vehicle(string plate, VehicleType type)
//    {
//        LicensePlate = plate;
//        Type = type;
//    }
//}

//public class Car : Vehicle
//{
//    public Car(string plate) : base(plate, VehicleType.Car) { }
//}

//public class Truck : Vehicle
//{
//    public Truck(string plate) : base(plate, VehicleType.Truck) { }
//}

//public class Motorcycle : Vehicle
//{
//    public Motorcycle(string plate) : base(plate, VehicleType.Motorcycle) { }
//}

////ParkingSpot
//// IsOccupied, VehicleType
//// Park(), Leave()
//public class ParkingSpot
//{
//    public int Id { get; }
//    public Vehicle? ParkedVehicle { get; private set; }
//    public VehicleType SpotType { get; }
//    public bool IsOccupied => ParkedVehicle != null;

//    public ParkingSpot(int id, VehicleType vehicleType)
//    {
//        Id = id;
//        SpotType = vehicleType;
//    }

//    public bool CanFit(VehicleType type)
//    {
//        return ParkedVehicle == null && type == SpotType;
//    }

//    public bool Park(Vehicle vehicle)
//    {
//        if (!CanFit(vehicle.Type))
//        {
//            return false;
//        }

//        ParkedVehicle = vehicle;
//        return true;
//    }

//    public void Leave() => ParkedVehicle = null;

//}



////ParkingLevel
//// List<ParkingSpot>
//// Park(), Leave(), Status()
//public class Level
//{
//    public int Id { get; }

//    /*
//	backing field: 
//	- nu se initializeaza aici, doar se declara
//	- nu poate si accesat din exterior, doar in constructor se initializeaza
//	- 
//	*/
//    private readonly List<ParkingSpot> _spots;

//    public Level(int id, IEnumerable<ParkingSpot> spots)
//    {
//        Id = id;
//        _spots = spots.ToList();
//    }

//    public ParkingSpot? FindFreeSpot(Vehicle vehicle)
//    {
//        return _spots.FirstOrDefault(s => s.SpotType == vehicle.Type && !s.IsOccupied);
//    }

//    public bool Park(Vehicle vehicle)
//    {
//        var spot = FindFreeSpot(vehicle);
//        if (spot != null)
//        {
//            return spot.Park(vehicle);
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public int GetAvailableSpots(VehicleType type)
//    {
//        return _spots.Count(s => s.SpotType == type && !s.IsOccupied);
//    }

//    //public void Leave() {}
//}

////ParkingLot
//// List<Level>
//// Park(), Leave(), Status
//public class ParkingLot
//{
//    public List<Level> _levels { get; }

//    public ParkingLot(IEnumerable<Level> levels)
//    {
//        _levels = levels.ToList();
//    }

//    public (ParkingSpot?, Level?) Park(Vehicle vehicle)
//    {
//        foreach (var level in _levels)
//        {
//            var spot = level.FindFreeSpot(vehicle);
//            if (spot != null)
//            {
//                level.Park((vehicle));
//                return (spot, level);
//            }
//        }
//        return (null, null);
//    }

//    public void DisplayAvailability()
//    {
//        var carSpots = 0;
//        var truckSpots = 0;
//        var motorSpots = 0;

//        foreach (var level in _levels)
//        {
//            carSpots += level.GetAvailableSpots(VehicleType.Car);
//            truckSpots += level.GetAvailableSpots(VehicleType.Truck);
//            motorSpots += level.GetAvailableSpots(VehicleType.Motorcycle);
//        }
//        Console.WriteLine("Free spots: Car:{0}, Truck:{1}, Motorcycle:{2}", carSpots, truckSpots, motorSpots);
//    }
//}

//public class Program
//{
//    //public static void Main()
//    //{
//    //    var level0 = new Level(0, new[]
//    //                       {
//    //                           new ParkingSpot(1,VehicleType.Car),
//    //                           new ParkingSpot(2,VehicleType.Car),
//    //                           new ParkingSpot(3,VehicleType.Car),
//    //                           new ParkingSpot(4,VehicleType.Truck),
//    //                           new ParkingSpot(5,VehicleType.Truck),
//    //                           new ParkingSpot(6,VehicleType.Motorcycle)
//    //                       }
//    //                      );
//    //    var level1 = new Level(1, new[]
//    //                       {
//    //                           new ParkingSpot(1,VehicleType.Car),
//    //                           new ParkingSpot(2,VehicleType.Car),
//    //                           new ParkingSpot(3,VehicleType.Car),
//    //                           new ParkingSpot(4,VehicleType.Truck),
//    //                           new ParkingSpot(5,VehicleType.Truck),
//    //                           new ParkingSpot(6,VehicleType.Motorcycle)
//    //                       }
//    //                      );
//    //    var level2 = new Level(2, new[]
//    //                       {
//    //                           new ParkingSpot(1,VehicleType.Car),
//    //                           new ParkingSpot(2,VehicleType.Car),
//    //                           new ParkingSpot(3,VehicleType.Car),
//    //                           new ParkingSpot(4,VehicleType.Truck),
//    //                           new ParkingSpot(5,VehicleType.Truck),
//    //                           new ParkingSpot(6,VehicleType.Motorcycle)
//    //                       }
//    //                      );
//    //    var lot = new ParkingLot(new[] { level0, level1, level2 });

//    //    lot.DisplayAvailability();

//    //    //park some vehicles
//    //    for (int i = 0; i < 10; i++)
//    //    {
//    //        var car = new Car(i.ToString());
//    //        var (spot, level) = lot.Park(car);
//    //        if (spot != null)
//    //        {
//    //            Console.WriteLine("Car with license {0} parked at Level {1}, spot {2}", car.LicensePlate, level.Id, spot.Id);
//    //        }
            
//    //    }
//    //    lot.DisplayAvailability();
//    //}
//}