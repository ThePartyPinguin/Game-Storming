public class Singleton<T> where T : new()
{
    private static T _instance;

    private static readonly object padLock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                    return _instance;
                }
            }

            return _instance;
        }
    }
}
