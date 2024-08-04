using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama.TinyJson;

public class Score
{

    // Function to Add score into the leader board for logged in User (current user)
    // It calls matchendrpc custom rpc to enter the data into the leaderboard
    public async Task<RootScoreResponse> AddScore(int score)
    {
        try
        {
            //User State
            var userState = new Dictionary<string, int>
            { {"xp" ,score },
                { "level", 1},
                {"coins", 1 }
            };
            var data = new Dictionary<string, Dictionary<string, int>>
            {
                { "userState", userState}
            };


            //Json Payload
            string jsonPayload = data.ToJson();

            //Getting Response from the server
            var res  = await NakmaConnection.Instance.client.RpcAsync(NakmaConnection.Instance.UserSession, "matchendrpc", jsonPayload);

            //Converting the response from string to Json
            RootScoreResponse RObj = JsonParser.FromJson<RootScoreResponse>(res.Payload);

            return RObj;

        }
        catch (Exception E)
        {
            throw E;
        }

    }


}
