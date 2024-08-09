using System;
using System.Threading.Tasks;
using Nakama;

public class User
{
    public User()
    {
        
    }

    public async Task UpdateProfile(string userName, string displayName, int avatar = 1)
    {
        try
        {
            await NakmaConnection.Instance.client.UpdateAccountAsync(NakmaConnection.Instance.UserSession, userName, displayName, avatar.ToString());
        }
        catch(Exception E) {
            throw E;
        }


    }

    public async Task<IApiUsers> GetProfile(string userId)
    {
        try
        {
            string[] ids = { userId };

            //awaiting
            IApiUsers userAcc = await NakmaConnection.Instance.client.GetUsersAsync(NakmaConnection.Instance.UserSession, ids);

            return userAcc;
        }
        catch (Exception E)
        {
            throw E;
        }

    }


}
