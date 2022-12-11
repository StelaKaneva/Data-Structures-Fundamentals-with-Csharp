using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.DeliveriesManager
{
    public class AirlinesManager : IAirlinesManager
    {
        private Dictionary<string, Flight> flightsById = new Dictionary<string, Flight>();
        private Dictionary<string, Airline> airlinesById = new Dictionary<string, Airline>();

        public void AddAirline(Airline airline)
        {
            if (this.airlinesById.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }

            this.airlinesById.Add(airline.Id, airline);
        }

        public void AddFlight(Airline airline, Flight flight)
        {
            if (!this.airlinesById.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }

            this.flightsById.Add(flight.Id, flight);

            this.airlinesById[airline.Id].Flights.Add(flight);

            flight.Airline = airline;
        }

        public bool Contains(Airline airline)
        {
            return this.airlinesById.ContainsKey(airline.Id);
        }

        public bool Contains(Flight flight)
        {
            return this.flightsById.ContainsKey(flight.Id);
        }

        public void DeleteAirline(Airline airline)
        {
            if (!this.airlinesById.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }

            this.airlinesById.Remove(airline.Id);

            var removed = this.flightsById.Values.Where(f => f.Airline.Id == airline.Id).ToArray();

            foreach (var flight in removed)
            {
                this.flightsById.Remove(flight.Id);
            }
        }

        public IEnumerable<Airline> GetAirlinesOrderedByRatingThenByCountOfFlightsThenByName()
        {
            return this.airlinesById.Values
                        .OrderByDescending(a => a.Rating)
                        .ThenByDescending(a => a.Flights.Count)
                        .ThenBy(a => a.Name);
        }

        public IEnumerable<Airline> GetAirlinesWithFlightsFromOriginToDestination(string origin, string destination)
        {
            return this.
                    airlinesById.Values
                    .Where(a => a.Flights
                        .Any(f => f.IsCompleted == false
                        && f.Origin == origin && f.Destination == destination));
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            return this.flightsById.Values;
        }

        public IEnumerable<Flight> GetCompletedFlights()
        {
            return this.flightsById.Values.Where(f => f.IsCompleted == true);
        }

        public IEnumerable<Flight> GetFlightsOrderedByCompletionThenByNumber()
        {
            return this.flightsById.Values.OrderBy(f => f.IsCompleted ? 1 : 0).ThenBy(f => f.Number);
        }

        public Flight PerformFlight(Airline airline, Flight flight)
        {
            if (!this.airlinesById.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }

            if (!this.flightsById.ContainsKey(flight.Id))
            {
                throw new ArgumentException();
            }

            flight.IsCompleted = true;

            return flight;
        }
    }
}
