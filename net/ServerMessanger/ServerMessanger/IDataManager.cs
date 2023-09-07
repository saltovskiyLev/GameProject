interface IDataManager
{
    bool CheckLoginAvailability(string login);

    Int64 GetNewId();

    bool Register(string login, string password);
}