namespace GameFrame.UnityHelpers.Networking
{
    public enum NetworkEvent
    {
        //Events send by client
        CLIENT_TO_SERVER_HANDSHAKE,
        CLIENT_DISCONNECT,
        CLIENT_INSTANTIATE_REQUEST,
        //Events send by server
        SERVER_TO_CLIENT_HANDSHAKE,
        SERVER_DISCONNECT,
        SERVER_INSTANTIATE_RESPONSE
    }
}
