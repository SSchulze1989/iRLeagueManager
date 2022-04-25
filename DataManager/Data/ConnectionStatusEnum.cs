// MIT License

namespace iRLeagueManager.Data
{
    public enum ConnectionStatusEnum
    {
        Disconnected = 0,
        Connected = 1,
        Connecting = 2,
        DatabaseUnavailable = 3,
        NoConnection = 4,
        ConnectionError = 99
    }
}