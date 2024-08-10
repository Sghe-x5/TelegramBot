using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace ChargesLibrary;


/// <summary>
/// Представляет информацию о зарядной станции для электромобилей.
/// </summary>
public class ElectricCharger
{
    [JsonPropertyName("id")]
    [Name("id")]
    public string Id { get; init; }

    [JsonPropertyName("name")]
    [Name("name")]
    public string Name { get; init; }

    [JsonPropertyName("admarea")]
    [Name("admarea")]
    public string AdmArea { get; init; }

    [JsonPropertyName("district")]
    [Name("district")]
    public string District { get; init; }

    [JsonPropertyName("address")]
    [Name("address")]
    public string Address { get; init; }

    [JsonPropertyName("longitude_wgs84")]
    [Name("longitude_wgs84")]
    public string Longitude { get; init; }

    [JsonPropertyName("latitude_wgs84")]
    [Name("latitude_wgs84")]
    public string Latitude { get; init; }

    [JsonPropertyName("global_id")]
    [Name("global_id")]
    public string GlobalId { get; init; }

    [JsonPropertyName("geodata_center")]
    [Name("geodata_center")]
    public string GeoDataCenter { get; init; }

    [JsonPropertyName("geoarea")]
    [Name("geoarea")]
    public string GeoArea { get; init; }

    public ElectricCharger() { }

    /// <summary>
    /// Преобразует информацию о зарядной станции в формат CSV.
    /// </summary>
    /// <returns>Строка в формате CSV.</returns>
    public string ConvertToCsv()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(
            $"\"\";\"{Id}\";\"{Name}\";\"{AdmArea}\";\"{District}\";\"{Address}\";\"{Longitude}\";\"{Latitude}\";\"{GlobalId}\";\"{GeoDataCenter}\";\"{GeoArea}\";\"\"");
        return sb.ToString();
    }

    /// <summary>
    /// Преобразует информацию о зарядной станции в формат JSON.
    /// </summary>
    /// <returns>Строка в формате JSON.</returns>
    public string ConvertToJson()
    {
        StringBuilder json = new StringBuilder();
        json.Append("  {\n");
        json.Append("    \"object_category_id\": \"\",\n");
        json.Append($"    \"id\": \"{Id}\",\n");
        json.Append($"    \"name\": \"{Name}\",\n");
        json.Append($"    \"admarea\": \"{AdmArea}\",\n");
        json.Append($"    \"district\": \"{District}\",\n");
        json.Append($"    \"longitude_wgs84\": \"{Longitude}\",\n");
        json.Append($"    \"latitude_wgs84\": \"{Latitude}\",\n");
        json.Append($"    \"global_id\": \"{GlobalId}\",\n");
        json.Append($"    \"geodata_center\": \"{GeoDataCenter}\",\n");
        json.Append($"    \"geoarea\": \"{GeoArea}\"\n");
        json.Append("  }");

        return json.ToString();
    }
}