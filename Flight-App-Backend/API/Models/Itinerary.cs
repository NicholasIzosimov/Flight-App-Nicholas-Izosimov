using System.Text.Json.Serialization;

namespace API.Models;

public class Itinerary
{
    [JsonPropertyName("depatureAt")]
    public DateTime DepartureAt { get; set; }
    [JsonPropertyName("arriveAt")]
    public DateTime ArriveAt { get; set; }
    [JsonPropertyName("avaliableSeats")]
    public int AvailableSeats { get; set; }

    [JsonPropertyName("prices")]
    public List<Price> Prices { get; set; }

}