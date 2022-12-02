using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Models.DTO;
using System.Text.Json;
using System.Net;
using System.IO;

namespace API.Controllers;

[ApiController]
public class FlightsController : ControllerBase
{
    [HttpPost("flights/book")]
    public async Task<IActionResult> BookFlight(
        string? flightId1,
        DateTime? departureDate1,
        string? flightId2,
        DateTime? departureDate2,
        int amountOfPassengers = 1
    )
    {
        Console.WriteLine(flightId1);
        Console.WriteLine(flightId2);
        List<Flight> updatedFlights = new();
        string jsonToWrite = "";

        using (
            StreamReader r = new StreamReader(
                "../data/Flights.json"
            )
        )
        {
            Console.WriteLine("id1: " + flightId1);
            Console.WriteLine("depdate1: " + departureDate1);
            Console.WriteLine("id2: " + flightId2);
            Console.WriteLine("depdate2: " + departureDate2);
            Console.WriteLine("#pass: " + amountOfPassengers);
            var json = await r.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500); // internal server error
            }
            var deserJson = JsonSerializer.Deserialize<List<Flight>>(json);

            // if (deserJson.Count == 0) return StatusCode(500); // internal server error
            // for oneway
            if (!string.IsNullOrWhiteSpace(flightId1) && string.IsNullOrWhiteSpace(flightId2))
            {
                Console.WriteLine("One way!");
                if (deserJson != null)
                {
                    var flightsToUpdate1 = deserJson.Where(x => x.FlightId == flightId1).ToList();
                    var flightsToNotUpdate1 = deserJson
                        .Where(x => x.FlightId != flightId1)
                        .ToList();
                    var itinerariesToUpdate1 = flightsToUpdate1
                        .SelectMany(f => f.Itineraries)
                        .Where(i => i.DepartureAt.ToString() == departureDate1.ToString())
                        .ToList();
                    var itinerariesToNotUpdate1 = flightsToUpdate1
                        .SelectMany(f => f.Itineraries)
                        .Where(i => i.DepartureAt.ToString() != departureDate1.ToString())
                        .ToList();
                    Console.WriteLine(itinerariesToUpdate1[0].AvailableSeats);
                    itinerariesToUpdate1[0].AvailableSeats -= amountOfPassengers;

                    var updatedItinerary1 = itinerariesToUpdate1[0];

                    List<Itinerary> togetherAgain1 = itinerariesToNotUpdate1
                        .Concat(itinerariesToUpdate1)
                        .ToList();
                    flightsToUpdate1[0].Itineraries = togetherAgain1;
                    var flightsTogetherAgain1 = flightsToNotUpdate1
                        .Concat(flightsToUpdate1)
                        .ToList();
                    jsonToWrite = JsonSerializer.Serialize(flightsTogetherAgain1);
                }
            }
            // for round trip
            if (!string.IsNullOrWhiteSpace(flightId1) && !string.IsNullOrWhiteSpace(flightId2))
            {
                Console.WriteLine("Round trip!");
                if (deserJson != null)
                {
                    var flightsToUpdate1 = deserJson.Where(x => x.FlightId == flightId1).ToList();
                    var flightsToNotUpdate1 = deserJson
                        .Where(x => x.FlightId != flightId1 && x.FlightId != flightId2)
                        .ToList();

                    var flightsToUpdate2 = deserJson.Where(x => x.FlightId == flightId2).ToList();
                    // var flightsToNotUpdate2 = deserJson.Where(x => x.FlightId != flightId2).ToList(); // // previous

                    var itinerariesToUpdate1 = flightsToUpdate1
                        .SelectMany(f => f.Itineraries)
                        .Where(i => i.DepartureAt.ToString() == departureDate1.ToString())
                        .ToList();
                    var itinerariesToNotUpdate1 = flightsToUpdate1
                        .SelectMany(f => f.Itineraries)
                        .Where(i => i.DepartureAt.ToString() != departureDate1.ToString())
                        .ToList();

                    var itinerariesToUpdate2 = flightsToUpdate2
                        .SelectMany(f => f.Itineraries)
                        .Where(i => i.DepartureAt.ToString() == departureDate2.ToString())
                        .ToList();
                    var itinerariesToNotUpdate2 = flightsToUpdate2
                        .SelectMany(f => f.Itineraries)
                        .Where(i => i.DepartureAt.ToString() != departureDate2.ToString())
                        .ToList();

                    var listOfItinerariesListToUpdate = flightsToUpdate1
                        .Select(f => f.Itineraries)
                        .ToList();
                    Console.WriteLine(
                        "total amount of itineraries1: "
                            + (itinerariesToNotUpdate1.Count + itinerariesToUpdate1.Count)
                    );
                    Console.WriteLine(
                        "total amount of itineraries1 to not update: "
                            + (itinerariesToNotUpdate1.Count)
                    );
                    Console.WriteLine(
                        "total amount of itineraries1 to update: " + (itinerariesToUpdate1.Count)
                    );
                    Console.WriteLine("itineraries1: " + (itinerariesToUpdate1[0].DepartureAt));
                    Console.WriteLine("---------------------------------------------------------");
                    Console.WriteLine(
                        "total amount of itineraries2: "
                            + (itinerariesToNotUpdate2.Count + itinerariesToUpdate2.Count)
                    );
                    Console.WriteLine(
                        "total amount of itineraries2 to not update: "
                            + (itinerariesToNotUpdate2.Count)
                    );
                    Console.WriteLine(
                        "total amount of itineraries2 to update: " + (itinerariesToUpdate2.Count)
                    );
                    Console.WriteLine("itineraries2: " + (itinerariesToUpdate2[0].DepartureAt));
                    Console.WriteLine("---------------------------------------------------------");

                    // THIS WORKS FINE
                    itinerariesToUpdate1.ForEach(
                        i => Console.WriteLine("base: " + i.AvailableSeats)
                    );
                    itinerariesToUpdate2.ForEach(
                        i => Console.WriteLine("base: " + i.AvailableSeats)
                    );

                    // update availableSeats in itineraries
                    itinerariesToUpdate1[0].AvailableSeats -= amountOfPassengers;
                    itinerariesToUpdate2[0].AvailableSeats -= amountOfPassengers;

                    var updatedItinerary1 = itinerariesToUpdate1[0];
                    var updatedItinerary2 = itinerariesToUpdate2[0];

                    Console.WriteLine("updated: " + itinerariesToUpdate1[0].AvailableSeats);
                    Console.WriteLine("updated: " + itinerariesToUpdate2[0].AvailableSeats);
                    Console.WriteLine("---------------------------------------------------------");

                    List<Itinerary> togetherAgain1 = itinerariesToNotUpdate1
                        .Concat(itinerariesToUpdate1)
                        .ToList();
                    List<Itinerary> togetherAgain2 = itinerariesToNotUpdate2
                        .Concat(itinerariesToUpdate2)
                        .ToList();

                    Console.WriteLine("total togetheragain1 after join: " + togetherAgain1.Count);
                    Console.WriteLine("total togetheragain2 after join: " + togetherAgain2.Count);
                    Console.WriteLine("---------------------------------------------------------");

                    flightsToUpdate1[0].Itineraries = togetherAgain1;
                    flightsToUpdate2[0].Itineraries = togetherAgain2;

                    var flightsTogetherAgain1 = flightsToNotUpdate1
                        .Concat(flightsToUpdate1)
                        .ToList();
                    // var flightsTogetherAgain2 = flightsToNotUpdate2.Concat(flightsToUpdate2).ToList(); // //previous
                    var flightsTogetherAgain2 = flightsTogetherAgain1
                        .Concat(flightsToUpdate2)
                        .ToList();

                    var allFlightsTogetherAtLast = flightsTogetherAgain1.Concat(
                        flightsTogetherAgain2
                    );
                    // jsonToWrite = JsonSerializer.Serialize(allFlightsTogetherAtLast);   // //previous
                    jsonToWrite = JsonSerializer.Serialize(flightsTogetherAgain2);

                    Console.WriteLine("VERDICT: IM GOD");
                }
            }
        }

        return Ok("posted");
    }

    [HttpGet("/allflights")]
    public async Task<IActionResult> GetAllFlights()
    {
        string path = Directory.GetCurrentDirectory();
        List<Flight> allFlights = new();
        using (StreamReader r = new StreamReader("../data/Flights.json"))
        {
            if (!string.IsNullOrEmpty(r.ToString()))
            {
                var json = await r.ReadToEndAsync();
                Console.WriteLine(json);
                allFlights = JsonSerializer.Deserialize<List<Flight>>(json);
            }
            else
            {
                return StatusCode(500);
            }
        }
        return Ok(allFlights);
    }

    [HttpGet("/flights/search")]
    public async Task<IActionResult> GetAllFlightsBySearchCriteria(
        string? departureLocation = null,
        string? departureDate = null,
        string? arrivalDate = null,
        string? returnDate = null,
        string? arrivalDestination = null,
        bool roundTrip = true,
        int adults = 1,
        int children = 0,
        bool direct = false
    )
    {
        // enforce invariants
        if (adults <= 0 || children < 0)
            return BadRequest("Amount of adults must be 1+ and children can't be negative");

        List<Flight> allFlights = new();

        using (StreamReader r = new StreamReader("../data/Flights.json"))
        {
            var json = await r.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500); // internal server error
            }
            allFlights = JsonSerializer.Deserialize<List<Flight>>(json);
        }

        if (allFlights == null)
            return StatusCode(500);

        // if !roundTrip
        if (
            !roundTrip
            && !string.IsNullOrWhiteSpace(departureLocation)
            && !string.IsNullOrWhiteSpace(arrivalDestination)
        )
        {
            var locationFiltered = allFlights
                .Where(
                    x =>
                        x.DepartureDestination.ToLower() == departureLocation.ToLower()
                        && x.ArrivalDestination.ToLower() == arrivalDestination.ToLower()
                )
                .ToList();

            List<Itinerary> itineraries = new();
            var flightId = "";
            var departureDest = "";
            var arrivalDest = "";
            foreach (var route in locationFiltered)
            {
                flightId = route.FlightId;
                departureDest = route.DepartureDestination;
                arrivalDest = route.ArrivalDestination;
                for (var i = 0; i < route.Itineraries.Count; i++)
                {
                    if (
                        route.Itineraries[i].DepartureAt.ToString().Split(" ")[0] == departureDate
                        && route.Itineraries[i].AvailableSeats >= (adults + children)
                    )
                    {
                        itineraries.Add(route.Itineraries[i]);
                    }
                }
            }
            OneWayDTO completedFiltering = new(flightId, departureDest, arrivalDest, itineraries);
            List<OneWayDTO> list = new() { completedFiltering };
            return Ok(list);
        }

        // for roundtrip
        if (
            roundTrip
            && !string.IsNullOrWhiteSpace(departureLocation)
            && !string.IsNullOrWhiteSpace(arrivalDestination)
        )
        {
            // for outbound flight
            var locationFiltered1 = allFlights
                .Where(
                    x =>
                        x.DepartureDestination.ToLower() == departureLocation.ToLower()
                        && x.ArrivalDestination.ToLower() == arrivalDestination.ToLower()
                )
                .ToList();

            List<Itinerary> itineraries1 = new();

            var flightId1 = "";
            var departureDest1 = "";
            var arrivalDest1 = "";
            foreach (var route in locationFiltered1)
            {
                flightId1 = route.FlightId;
                departureDest1 = route.DepartureDestination;
                arrivalDest1 = route.ArrivalDestination;
                for (var i = 0; i < route.Itineraries.Count; i++)
                {
                    if (
                        route.Itineraries[i].DepartureAt.ToString().Split(" ")[0] == departureDate
                        && route.Itineraries[i].AvailableSeats >= (adults + children)
                    )
                    {
                        itineraries1.Add(route.Itineraries[i]);
                    }
                }
            }
            OneWayDTO outbound = new(flightId1, departureDest1, arrivalDest1, itineraries1);

            // for return flight
            var locationFiltered2 = allFlights
                .Where(
                    x =>
                        x.DepartureDestination.ToLower() == arrivalDestination.ToLower()
                        && x.ArrivalDestination.ToLower() == departureLocation.ToLower()
                )
                .ToList();
            List<Itinerary> itineraries2 = new();

            var flightId2 = "";
            var departureDest2 = "";
            var arrivalDest2 = "";
            foreach (var route in locationFiltered2)
            {
                flightId2 = route.FlightId;
                departureDest2 = route.DepartureDestination;
                arrivalDest2 = route.ArrivalDestination;
                for (var i = 0; i < route.Itineraries.Count; i++)
                {
                    if (
                        route.Itineraries[i].DepartureAt.ToString().Split(" ")[0] == returnDate
                        && route.Itineraries[i].AvailableSeats >= (adults + children)
                    )
                    {
                        itineraries2.Add(route.Itineraries[i]);
                    }
                }
            }
            OneWayDTO returnFlight = new(flightId2, departureDest2, arrivalDest2, itineraries2);
            List<OneWayDTO> flightList = new() { outbound, returnFlight };
            Helpers.Sleep(3000);
            return Ok(flightList);
        }

        // return internal server error
        return StatusCode(500);
    }

    public static class Helpers
    {
        public static void Sleep(int milliseconds)
        {
            System.Threading.Thread.Sleep(3000);
        }
    }
    // [HttpPost("/flights/book")]
}
