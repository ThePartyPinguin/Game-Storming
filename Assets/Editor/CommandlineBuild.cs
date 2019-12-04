using UnityEditor;
using UnityEngine;

public class CommandlineBuild : MonoBehaviour
{
    public static bool ServerBuildExists()
    {
        return ServerBuilder.BuildExists();
    }

    [MenuItem("Build/Start standalone server", false, 0)]
    public static void StartServer()
    {
        if (!ServerBuildExists())
        {
            Debug.Log("No server build exists, building now");
            BuildServer();
        }
        else
        {
            ServerBuilder.StartLatestBuild();
        }
    }

    [MenuItem("Build/Standalone server", false, 10)]
    public static void BuildServer()
    {
        bool buildSucceeded = ServerBuilder.Build();

        if (buildSucceeded)
        {
            ServerBuilder.StartLatestBuild();
        }
        else
        {
            Debug.LogError("Something went wrong building the server");
        }
    }
}
