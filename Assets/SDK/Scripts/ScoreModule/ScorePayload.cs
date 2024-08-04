using System;


// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ScoreData
{
    public int maxNumScore ;
    public int updateTime ;
    public int expiryTime ;
    public string ownerId ;
    public string username ;
    public int score ;
    public MyMetadata metadata ;
    public int createTime ;
    public int rank ;
    public string leaderboardId ;
    public int subscore ;
    public int numScore ;
}

public class MyMetadata
{
}

public class RootScoreResponse
{
    public string status ;
    public string message ;
    public Data data ;
}

