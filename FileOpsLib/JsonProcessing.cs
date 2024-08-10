using System.Text;
using System.Text.Json;
using ChargesLibrary;

namespace FileOpsLibrary;


/// <summary>
/// Класс для обработки JSON данных.
/// </summary>
public class JsonProcessing : IProcessing
{

    /// <summary>
    /// Чтение данных из JSON.
    /// </summary>
    /// <param name="stream">Поток, содержащий JSON данные.</param>
    /// <returns>Массив объектов ElectricCharger.</returns>
    public ElectricCharger[] Read(Stream stream)
    {
        if (stream is { Length: > 0 })
        {
            return JsonSerializer.Deserialize<ElectricCharger[]>(stream);
        }

        throw new ArgumentException("Пустой файл.");
    }

    /// <summary>
    /// Запись данных в формате JSON в поток.
    /// </summary>
    /// <param name="chargers">Массив объектов ElectricCharger.</param>
    /// <returns>Поток с JSON данными.</returns>
    public Stream Write(ElectricCharger[] chargers)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[\n");
        foreach (ElectricCharger c in chargers)
        {
            sb.Append(c.ConvertToJson());
            sb.Append(",\n");
        }

        sb.Remove(sb.Length - 2, 1);
        sb.Append(']');
        MemoryStream memoryStream = new MemoryStream();
        using (StreamWriter writer = new StreamWriter(memoryStream, leaveOpen: true))
        {
            writer.Write(sb);
        }
        memoryStream.Position = 0;

        return memoryStream;
    }
}