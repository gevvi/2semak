using System;
using System.Collections.Generic;

namespace Automobile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Automobile:");

            // Инициализация калькулятора расхода топлива
            IFuelConsumptionCalculator fuelConsumptionCalculator = new StandardFuelConsumptionCalculator();

            // Инициализация менеджера пассажиров
            IPassengerManager passengerManager = new PassengerManager();

            // Инициализация класса Automobile
            Automobile automobile = new Automobile(fuelConsumptionCalculator, passengerManager);

            Console.WriteLine($"Maker: {automobile.Maker}, Model: {automobile.Model}, Year: {automobile.Year}");

            // Тестирование расчета расстояния до опустошения топливного бака
            automobile.FuelVolume = 50;
            automobile.EnginePower = 100;
            double distance = automobile.GetDistanceToEmpty();
            Console.WriteLine($"Distance to empty: {distance}");

            // Тестирование добавления и удаления пассажиров
            automobile.AddPassenger(new Passenger("John", 70));
            automobile.AddPassenger(new Passenger("Mary", 60));
            Console.WriteLine("Passengers:");
            automobile.DisplayPassengers();
            automobile.RemovePassenger("John");
            Console.WriteLine("Passengers after removing John:");
            automobile.DisplayPassengers();
        }
    }

    public interface IFuelConsumptionCalculator
    {
        double CalculateDistanceToEmpty(double fuelVolume, double enginePower);
    }

    public class StandardFuelConsumptionCalculator : IFuelConsumptionCalculator
    {
        public double CalculateDistanceToEmpty(double fuelVolume, double enginePower)
        {
            return fuelVolume * 100 / enginePower;
        }
    }

    public interface IPassengerManager
    {
        void AddPassenger(Passenger passenger);
        void RemovePassenger(string name);
        void DisplayPassengers();
    }

    public class PassengerManager : IPassengerManager
    {
        private List<Passenger> passengers = new List<Passenger>();

        public void AddPassenger(Passenger passenger)
        {
            if (passengers.Count >= 50)
                throw new InvalidOperationException("Cannot add more passengers");
            passengers.Add(passenger);
        }

        public void RemovePassenger(string name)
        {
            passengers.RemoveAll(p => p.Name == name);
        }

        public void DisplayPassengers()
        {
            foreach (var passenger in passengers)
            {
                Console.WriteLine($"Name: {passenger.Name}, Mass: {passenger.Mass}");
            }
        }
    }

    public class Automobile
    {
        // Оставляем только свойства, связанные с данными автомобиля
        public string Maker { get; set; } = "Toyota";
        public string Model { get; set; } = "Camry";
        public ulong Year { get; set; } = 2011;
        public double EnginePower { get; set; } = 167;
        public double FuelVolume { get; set; } = 70;

        private IFuelConsumptionCalculator fuelConsumptionCalculator;
        private IPassengerManager passengerManager;

        public Automobile(IFuelConsumptionCalculator calculator, IPassengerManager manager)
        {
            fuelConsumptionCalculator = calculator;
            passengerManager = manager;
        }

        public double GetDistanceToEmpty()
        {
            return fuelConsumptionCalculator.CalculateDistanceToEmpty(FuelVolume, EnginePower);
        }

        public void AddPassenger(Passenger passenger)
        {
            passengerManager.AddPassenger(passenger);
        }

        public void RemovePassenger(string name)
        {
            passengerManager.RemovePassenger(name);
        }

        public void DisplayPassengers()
        {
            passengerManager.DisplayPassengers();
        }
    }

    public class Passenger
    {
        public string Name { get; set; }
        public double Mass { get; set; }

        public Passenger(string name, double mass)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Passenger name cannot be empty or null");
            if (mass <= 0)
                throw new ArgumentException("Passenger mass must be greater than zero");
            Name = name;
            Mass = mass;
        }
    }
}
