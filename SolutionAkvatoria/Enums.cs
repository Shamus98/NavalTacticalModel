namespace NavalTacticalModel
{
    // Типы двигателей БЭК
    public enum EngineType
    {
        Stopped,
        Electric,
        Gasoline
    }

    // Типы событий модели
    public enum ModelEventType
    {
        Start,
        Finish,
        StartDetect,
        EndDetect,
        ChangeEngine,
        Attack
    }

    // Стороны конфликта
    public enum ConflictSide
    {
        Blue,
        Green
    }
}