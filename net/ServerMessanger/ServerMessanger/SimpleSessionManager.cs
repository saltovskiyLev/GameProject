class SimpleSessionManager : ISessionManager
{
    Random r = new Random();
    Dictionary<string, string> Sessions = new Dictionary<string, string>();
    public string CreateSession(string login)
    {
        string key = "";


        for(int i = 0; i < 10; i++)
        {
            int random = r.Next();
            key += random.ToString();
        }


        Sessions.Add(key, login);
        return key;
    }

    public string GetLogin(string key)
    {
        string login = "";
        if(Sessions.ContainsKey(key))
        {
            login = Sessions[key];
        }

        return login;
    }
}