using System.Text;
using ChargesLibrary;
using CsvHelper;
using CsvHelper.Configuration;
using static System.Globalization.CultureInfo;
using System.Formats.Asn1;

namespace FileOpsLibrary;

/// <summary>
/// Класс для обработки CSV файлов.
/// </summary>
public class CsvProcessing : IProcessing
{
    /// <summary>
    /// Чтение данных из CSV файла.
    /// </summary>
    /// <param name="stream">Поток, содержащий данные CSV файла.</param>
    /// <returns>Массив объектов ElectricCharger.</returns>
    public ElectricCharger[] Read(Stream stream)
    {
        CsvConfiguration config = new CsvConfiguration(InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
            DetectDelimiter = true
        };

        using StreamReader streamReader = new StreamReader(stream);
        ElectricCharger[] chargers;
        using (CsvReader reader = new CsvReader(streamReader, config))
        {
            chargers = reader.GetRecords<ElectricCharger>().ToArray();
        }

        if (chargers.Length > 1)
        {
            if (string.Equals(chargers[0].Name, "Наименование"))
            {
                return chargers[1..];
            }
            else
            {
                return chargers;
            }
        }
        else if (chargers.Length == 1 && chargers[0].Name != "Наименование")
        {
            return chargers;
        }

        throw new ArgumentException("Файл пуст.");
    }

    /// <summary>
    /// Запись данных в CSV файл.
    /// </summary>
    /// <param name="chargers">Массив объектов ElectricCharger.</param>
    /// <returns>Поток с данными в формате CSV.</returns>
    public Stream Write(ElectricCharger[] chargers)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(
            "\"object_category_Id\";\"ID\";\"Name\";\"AdmArea\";\"District\";\"Address\";\"Longitude_WGS84\";\"Latitude_WGS84\";\"global_id\";\"geodata_center\";\"geoarea\";\n");
        sb.Append(
            "\"object_category_Id\";\"Код\";\"Наименование\";\"Административный округ\";\"Район\";\"Адрес\";\"Долгота в WGS-84\";\"Широта в WGS-84\";\"global_id\";\"geodata_center\";\"geoarea\";\n");


        foreach (ElectricCharger c in chargers)
        {
            sb.Append(c.ConvertToCsv());
            sb.Append('\n');
        }
        sb.Remove(sb.Length - 1, 1);
        MemoryStream memoryStream = new MemoryStream();
        using (StreamWriter writer = new StreamWriter(memoryStream, leaveOpen: true))
        {
            writer.Write(sb.ToString());
        }
        memoryStream.Position = 0;
        return memoryStream;
    }
}