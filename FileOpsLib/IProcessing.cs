using ChargesLibrary;

namespace FileOpsLibrary;

/// <summary>
/// Интерфейс для обработки данных.
/// </summary>
public interface IProcessing
{
    public ElectricCharger[] Read(Stream stream);
    public Stream Write(ElectricCharger[] chargers);
}