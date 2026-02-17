using System;
using System.Collections.Generic;
using System.Text;

/*Designing a Parking Lot System

   Requirements
1: The parking lot should have multiple levels, each level with a certain number of parking spots.
2: The parking lot should support different types of vehicles, such as cars, motorcycles, and trucks.
3: Each parking spot should be able to accommodate a specific type of vehicle.
4: The system should assign a parking spot to a vehicle upon entry and release it when the vehicle exits.
5: The system should track the availability of parking spots and provide real-time information to customers.
*/

namespace ParkingLiveCoding
{
    public class NewParkingLot
    {
        public static void Main()
        {
            var level1 = new Level(0, [
                new ParkingSpot(1,VehicleType.Car),
                new ParkingSpot(2,VehicleType.Car),
                new ParkingSpot(3,VehicleType.Car),
                new ParkingSpot(4,VehicleType.Car),
                new ParkingSpot(5,VehicleType.Car),
                new ParkingSpot(6,VehicleType.Car),
                new ParkingSpot(7,VehicleType.Car),
                new ParkingSpot(8,VehicleType.Car),
                new ParkingSpot(9,VehicleType.Truck),
                new ParkingSpot(10,VehicleType.Motorcycle)
            ]);

            var level2 = new Level(1, [
                new ParkingSpot(1,VehicleType.Truck),
                new ParkingSpot(2,VehicleType.Truck),
                new ParkingSpot(3,VehicleType.Truck),
                new ParkingSpot(4,VehicleType.Truck),
                new ParkingSpot(5,VehicleType.Truck),
                new ParkingSpot(6,VehicleType.Motorcycle),
                new ParkingSpot(7,VehicleType.Motorcycle),
                new ParkingSpot(8,VehicleType.Motorcycle),
                new ParkingSpot(9,VehicleType.Motorcycle),
                new ParkingSpot(10,VehicleType.Motorcycle)
            ]);

            var lot = new ParkingLot([level1, level2]);


            var vehicles = new List<Vehicle>
            {
                new Car("c1"),
                new Car("c2"),
                new Car("c3"),
                new Car("c4"),
                new Car("c5"),
                new Car("c6"),
                new Car("c7"),
                new Car("c8"),
                new Car("c9"),                
                new Truck("t1"),
                new MotorCycle("m1"),
                new MotorCycle("m2"),
                new MotorCycle("m3")
            };

            DisplayLotStatus(lot.GetStatus());

            foreach (var vehicle in vehicles)
            {
                var result = lot.TryPark(vehicle);
                DisplayParkedMessage(vehicle, result);
            }

            var vehicleThatDoesNotFit = new Car("c10");
            DisplayParkedMessage(vehicleThatDoesNotFit, lot.TryPark(vehicleThatDoesNotFit));

            var duplicateVehicle = new MotorCycle("m3");
            DisplayParkedMessage(duplicateVehicle, lot.TryPark(duplicateVehicle));


            DisplayLotStatus(lot.GetStatus());

            DisplayLeaveMessage(lot.Leave("c1"), "c1");
            DisplayLeaveMessage(lot.Leave("c1"), "c1");

            DisplayLotStatus(lot.GetStatus());
        }

        public class Result
        {
            public bool IsSuccess { get; }
            public string ErrorMessage { get; }

            protected Result(bool isSuccess, string errorMessage)
            {
                IsSuccess = isSuccess;
                ErrorMessage = errorMessage;
            }


            public static Result Success() => new Result(true, string.Empty);
            public static Result Failure(string error) => new Result(false, error);
        }

        public class Result<T> : Result
        {
            private readonly T? _value;

            public T Value
            {
                get
                {
                    if (!IsSuccess)
                        throw new InvalidOperationException("Cannot access Value of a failed result");
                    return _value!;
                }
            }

            protected Result(bool isSuccess, T? value, string error) 
                : base(isSuccess, error)
            {
                _value = value;
            }

            public static Result<T> Success(T value) => new (true, value, string.Empty);
            public static Result<T> Failure(string error) => new Result<T>(false, default, error);
        }

        public static void DisplayParkedMessage(Vehicle vehicle, Result<ParkLocation> res)
        {
            if (!res.IsSuccess)
            {                
                Console.WriteLine($"Cannot park {vehicle.Type.ToString()} with license {vehicle.LicensePlate}. {res.ErrorMessage}");
            }
            else
            {
                Console.WriteLine($"{vehicle.Type.ToString()} with license {vehicle.LicensePlate} parked on Level {res.Value.LevelId}  - Spot {res.Value.SpotId}");
            }
        }

        public static void DisplayLotStatus(ParkingLotAvailability availability)
        {
            Console.WriteLine($"Free spots total: {availability.Car + availability.Truck + availability.Motorcycle}. Cars:{availability.Car}, Trucks: {availability.Truck}, Motorcycles: {availability.Motorcycle}");
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

        public record ParkLocation(int LevelId, int SpotId);

        public record ParkingLotAvailability(int Car, int Truck, int Motorcycle);

        public abstract class Vehicle
        {
            //auto properties, get-only, immutable
            public string LicensePlate { get; }
            public VehicleType Type { get; }

            protected Vehicle(string licensePlate, VehicleType type)
            {
                //input normalization
                LicensePlate = (licensePlate ?? "").Trim();

                //validation
                if (string.IsNullOrWhiteSpace(licensePlate))
                    throw new ArgumentException("License plate must be provided", nameof(licensePlate));

                Type = type;
            }
        }

        public class Car(string license) : Vehicle(license, VehicleType.Car) { }
        public class Truck(string license) : Vehicle(license, VehicleType.Truck) { }
        public class MotorCycle(string license) : Vehicle(license, VehicleType.Motorcycle) { }

        public class ParkingSpot
        {
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
                return IsAvailable && vehicle.Type == Type;
            }

            public Result TryPark(Vehicle vehicle)
            {
                if (!CanFit(vehicle))
                    return Result.Failure("Vehicle does not fit into requested parking spot");

                Vehicle = vehicle;
                return Result.Success();
            }

            public bool Leave()
            {
                if (IsAvailable)
                    return false;

                Vehicle = null;
                return true;
            }
        }

        // Id, List<ParkingSpots, TryPark(), Leave(), GetAvailability()
        public class Level
        {
            public int Id { get; }

            private readonly List<ParkingSpot> _spots;

            public Level(int id, IEnumerable<ParkingSpot> spots)
            {
                if (id < 0)
                    throw new ArgumentOutOfRangeException(nameof(id), "Id must be >=0");

                ArgumentNullException.ThrowIfNull(spots);

                _spots = [.. spots];

                if (_spots.Count == 0)
                    throw new ArgumentException("Level must have at least one spot", nameof(spots));

                Id = id;
            }

            public Result<ParkLocation> TryPark(Vehicle vehicle)
            {
                var spot = _spots.FirstOrDefault(s => s.CanFit(vehicle));

                if (spot == null)
                    return Result<ParkLocation>.Failure("No parking spot available for this vehicle");

                return spot.TryPark(vehicle).IsSuccess
                    ? Result<ParkLocation>.Success(new ParkLocation(Id, spot.Id))
                    : Result<ParkLocation>.Failure("No parking spot available for this vehicle");
            }

            public bool Leave(int spotId)
            {
                var spot = _spots.FirstOrDefault(s => s.Id == spotId);

                return spot?.Leave() ?? false;
            }

            public int GetLevelAvailability(VehicleType type)
            {
                return _spots.Count(s => s.Type == type && s.IsAvailable);
            }
        }

        public class ParkingLot
        {
            private readonly List<Level> _levels;
            private readonly Dictionary<string, ParkLocation> _occupancy = new();

            public ParkingLot(IEnumerable<Level> levels)
            {
                ArgumentNullException.ThrowIfNull(levels);

                if (!levels.Any())
                {
                    throw new ArgumentException("Parking lot levels cannot be empty", nameof(levels));
                }
                _levels = [.. levels];
            }

            public Result<ParkLocation> TryPark(Vehicle vehicle)
            {
                if (_occupancy.ContainsKey(vehicle.LicensePlate))
                {
                    return Result<ParkLocation>.Failure("License plate already parked");
                }

                foreach (var level in _levels)
                {
                    var result = level.TryPark(vehicle);
                    if (result.IsSuccess)
                    {
                        _occupancy.Add(vehicle.LicensePlate, result.Value);

                        return result;
                    }
                }

                return Result<ParkLocation>.Failure("No available spot found");
            }

            public bool Leave(string license)
            {
                var licenseIsParked = _occupancy.TryGetValue(license, out var parkedLocation);

                if (!licenseIsParked)
                    return false;

                (int levelId, int spotId) = parkedLocation;

                var level = _levels.FirstOrDefault(l => l.Id == levelId);

                if (level != null && level.Leave(spotId))
                {
                    _occupancy.Remove(license);
                    return true;
                }
                return false;
            }

            public ParkingLotAvailability GetStatus()
            {
                var cars = _levels.Sum(t => t.GetLevelAvailability(VehicleType.Car));
                var trucks = _levels.Sum(t => t.GetLevelAvailability(VehicleType.Truck));
                var motorcycles = _levels.Sum(t => t.GetLevelAvailability(VehicleType.Motorcycle));

                return new ParkingLotAvailability(cars, trucks, motorcycles);
            }
        }
    }
}
