namespace ChargesLibrary;

public class Selector
{
    /// <summary>
    /// Выбирает зарядные станции по заданным параметрам.
    /// </summary>
    /// <param name="chargers">Массив зарядных станций.</param>
    /// <param name="type">Тип параметра для выборки (1 - Административный округ, 2 - Район).</param>
    /// <param name="valueToSelect">Значение(я) параметра для выборки.</param>
    /// <returns>Массив выбранных зарядных станций.</returns>
    public ElectricCharger[] Select(ElectricCharger[] chargers, int type, string[] valueToSelect)
    {
        List<ElectricCharger> selected = new List<ElectricCharger>();
        if (valueToSelect.Length == 1)
        {
            switch (type)
            {
                case 1:
                    selected = chargers.Where(charger => charger.AdmArea == valueToSelect[0]).ToList();
                    break;
                case 2:
                    selected = chargers.Where(charger => charger.District == valueToSelect[0]).ToList();
                    break;
            }
        }
        else if (valueToSelect.Length == 3)
        {
            selected = chargers.Where(charger => charger.AdmArea == valueToSelect[0] &&
                                                  charger.Longitude == valueToSelect[1] &&
                                                  charger.Latitude == valueToSelect[2]).ToList();
        }
        else
        {
            throw new ArgumentException("Некорректная выборка");
        }

        if (selected.Count == 0)
        {
            throw new ArgumentException("Ничего не найдено, начните сначала");
        }

        return selected.ToArray();
    }
}