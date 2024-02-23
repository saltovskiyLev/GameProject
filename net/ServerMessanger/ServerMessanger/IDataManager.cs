interface IDataManager
{
    bool CheckLoginAvailability(string login);

    //Int64 GetNewId();

    bool Auth(string login, string password);

    bool Register(string login, string password, string userName);

    User GetUserByLogIn(string logInId);

    string CreateInvite(int userId);

    List<string> GetInvites(int userId);

    int GetUserIdByInvite(string InviteCode);

    void AddContact(int FirstId, int SecondId);

    List<string> GetUsers(string sessionKey);
}
