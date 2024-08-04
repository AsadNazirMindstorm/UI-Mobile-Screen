using System.Collections;
using System.Collections.Generic;
using System;


//Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

[Serializable]
public class Data
{
    public List<Record> records;
    public List<object> ownerRecords ;
    public object nextCursor ;
    public object prevCursor ;
    public int rankCount ;
}

[Serializable]
public class Metadata
{
}

[Serializable]
public class Record
{
    public int subscore ;
    public int maxNumScore ;
    public int createTime ;
    public int rank ;
    public string username ;
    public int score ;
    public int numScore ;
    public Metadata metadata ;
    public int updateTime ;
    public int expiryTime ;
    public string leaderboardId ;
    public string ownerId ;
}

[Serializable]
public class RootResponseLeaderboard
{
    public string status ;
    public string message ;
    public Data data ;
}

