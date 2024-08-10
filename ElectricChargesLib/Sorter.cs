namespace ChargesLibrary;

/// <summary>
/// Сортирует массив зарядных станций.
/// </summary>
/// <param name="chargers">Массив зарядных станций.</param>
/// <param name="order">Порядок сортировки (true - по возрастанию, false - по убыванию).</param>
/// <returns>Отсортированный массив зарядных станций.</returns>
public class Sorter
{
    public ElectricCharger[] Sort(ElectricCharger[] chargers, bool order)
    {
        ElectricCharger[] sorted = chargers.OrderBy(charger => charger.AdmArea).ToArray();
        if (!order)
        {
            Array.Reverse(sorted);
            return sorted;
        }
        else
        {
            return sorted;
        }
    }
}