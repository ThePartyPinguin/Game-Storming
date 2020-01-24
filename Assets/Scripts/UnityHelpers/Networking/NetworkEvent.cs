namespace GameFrame.UnityHelpers.Networking
{
    public enum NetworkEvent
    {
        //Events send by client
        CLIENT_TO_SERVER_HANDSHAKE,
        CLIENT_DISCONNECT,
        CLIENT_SEND_NAME,
        CLIENT_SEND_IDEA,
        CLIENT_SEND_TOPIC,


        //Events send by server
        SERVER_TO_CLIENT_HANDSHAKE,
        SERVER_DISCONNECT,
        SERVER_REQUEST_TOPIC,
        SERVER_REGISTERED_CLIENT,
        SERVER_WAITING_ON_TOPIC,
        SERVER_REQUEST_NAME,
        SERVER_START_GAME,
        SERVER_STOP_BUILDER,
        SERVER_ASSIGN_NEW_BUILDER,
        SERVER_END_GAME
    }
}
