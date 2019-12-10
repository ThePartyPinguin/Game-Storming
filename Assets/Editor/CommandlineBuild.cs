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
            BuildServerNormal();
        }
        else
        {
            ServerBuilder.StartLatestBuild();
        }
    }

    [MenuItem("Build/Standalone server, normal", false, 9)]
    public static void BuildServerNormal()
    {
        bool buildSucceeded = ServerBuilder.Build(BuildOptions.Development);

        if (buildSucceeded)
        {
            ServerBuilder.StartLatestBuild();
        }
        else
        {
            Debug.LogError("Something went wrong building the server");
        }
    }

    [MenuItem("Build/Standalone server, headless", false, 10)]
    public static void BuildServerheadless()
    {
        bool buildSucceeded = ServerBuilder.Build(BuildOptions.EnableHeadlessMode);

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
