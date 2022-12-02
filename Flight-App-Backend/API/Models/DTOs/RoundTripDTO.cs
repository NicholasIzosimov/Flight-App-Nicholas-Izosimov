using System.Text.Json.Serialization;

namespace API.Models.DTO;

public class RoundTripFlightDTO
{
    // //test properties
    // [JsonPropertyName("flights")]
    // public OneWayFlightDTO[] Flights { get; set; }
    [JsonPropertyName("flight_id1")]
    public string FlightId1 { get; set; }
    [JsonPropertyName("departureDestination1")]
    public string DepartureDestination1 { get; set; }

    [JsonPropertyName("arrivalDestination1")]
    public string ArrivalDestination1 { get; set; }

    [JsonPropertyName("itineraries1")]
    public List<Itinerary> Itineraries1 { get; set; }
    [JsonPropertyName("flight_id2")]
    public string FlightId2 { get; set; }
    [JsonPropertyName("departureDestination2")]
    public string DepartureDestination2 { get; set; }

    [JsonPropertyName("arrivalDestination2")]
    public string ArrivalDestination2 { get; set; }

    [JsonPropertyName("itineraries2")]
    public List<Itinerary> Itineraries2 { get; set; }

    // basic constructor
    [JsonConstructor]
    public RoundTripFlightDTO(
        string id1,
        string departureDestination1,
        string arrivalDestination1,
        List<Itinerary> itineraries1,
        string id2,
        string departureDestination2,
        string arrivalDestination2,
        List<Itinerary> itineraries2
        )
    {
        FlightId1 = id1;
        DepartureDestination1 = departureDestination1;
        ArrivalDestination1 = arrivalDestination1;
        Itineraries1 = itineraries1;
        FlightId2 = id2;
        DepartureDestination2 = departureDestination2;
        ArrivalDestination2 = arrivalDestination2;
        Itineraries2 = itineraries2;
    }


    // //test constructor
    // [JsonConstructor]
    // public RoundTripFlightDTO(
    //     OneWayFlightDTO flight1, OneWayFlightDTO flight2
    //     )
    // {
    //     Flights = new OneWayFlightDTO[] { flight1, flight2 };
    // }
}