using System;
public static class ClientConstants
{
    // Constants of Client side Nakama
    //public static string host = "localhost";
    public static string host = "13.60.75.218";
    public static string type = "http";
    public static int port = 7350;
    public static string serverKey = "defaultkey";


    //Scene index as constants
    public const int mainMenuSceneName = 1;
    public const int loadingScreenName = 0;


    //these go into the Configs into the server
    //Leaderboard
    public static string leaderBoard = "Global";

    //Global Room Name for Global Chat
    public static string globalRoomName = "GlobalRoom";
}
