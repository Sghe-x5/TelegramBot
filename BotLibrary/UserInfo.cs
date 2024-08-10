namespace BotLibrary;

/// <summary>
/// Представляет информацию о пользователе бота.
/// </summary>
public class UserInfo
{
    private int _typeOfAction;

    /// <summary>
    /// Состояния пользователя.
    /// </summary>
    public enum UserStates
    {
        None,
        WaitingForCallback,
        UploadingFile,
        ChoosingAttributeForSorting,
        ChoosingAttributeForSelection,
        EnteringValueForSelection
    }

    /// <summary>
    /// Тип выполняемого действия.
    /// </summary>
    public int TypeOfAction
    {
        get => _typeOfAction;
        set
        {
            if (value is <= 5 and >= 0)
            {
                _typeOfAction = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"{_typeOfAction} ", "Значение действия лежит в диапазоне [0; 5].");
            }
        }
    }

    public string? LastQueryId { get; set; }

    public UserStates State { get; set; }
    public string? File { get; set; }
    public bool? IsCsv { get; set; }

    public string? ValueForSelection { get; set; }

    public UserInfo()
    {
        State = UserStates.None;
    }
}