using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nakama.TinyJson;

public class LeaderBoard
{
    NakmaConnection nakma;
    public LeaderBoard()
    {
        this.nakma = NakmaConnection.Instance;
    }


    // Function to get all leaderboard records
    public async Task<RootResponseLeaderboard> getAllRecords(string table)
    {
        //call custom RPC here and the payload is { leaderBoard: table }
        //Debug.Log() the records by toString()

        try
        {
            // Create the payload object
            var data = new Dictionary<string, string>
            {
                { "leaderBoardId", table }
            };


            //connerting data to Json
            var jsonData = data.ToJson();

            // Call the custom RPC
            var response = await nakma.client.RpcAsync(nakma.UserSession, "getleaderboardrpc", jsonData);
            RootResponseLeaderboard RObj= JsonParser.FromJson<RootResponseLeaderboard>(response.Payload);

            
            return RObj;

        }
        catch (Exception e)
        {
            //System.Console.WriteLine("Error calling RPC: " + e.Message);
            throw e;
        }
    }


}
