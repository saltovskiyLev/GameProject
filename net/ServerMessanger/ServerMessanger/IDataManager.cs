interface IDataManager
{
    bool CheckLoginAvailability(string login);

    Int64 GetNewId();

    bool Auth(string login, string password);

    bool Register(string login, string password);
}
