using UnityEngine;
using Nakama;
using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;

public class Auth
{

    public Auth(NakmaConnection obj)
    {
    }


   public async Task AuthenticateClient()
    {

        try
        {
            //User Object Created
            User uObj = new();

            // Authentication logic
            NakmaConnection.Instance.UserSession = await NakmaConnection.Instance.client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            //Debug.Log("User Id is: " + NakmaConnection.Instance.UserSession.UserId);

            //await Socket creation
            await NakmaConnection.Instance.CreateSocket();

            //Get the user profile 
            var userAcc  = await uObj.GetProfile(NakmaConnection.Instance.UserSession.UserId);

            //check if he has the display name or not
            //if it does not set the defualt user id and avatar index
            foreach (var user in userAcc.Users)
                if (user.DisplayName == null || user.AvatarUrl == null || user.DisplayName.Equals("") || user.DisplayName.Equals(""))  
                    //Updating the user Profile to the default Values
                    await uObj.UpdateProfile(NakmaConnection.Instance.UserSession.Username, NakmaConnection.Instance.UserSession.Username, 5);


            //If Authenticated 
            SceneManager.LoadScene(ClientConstants.mainMenuSceneName);
        }
        catch(Exception E)
        {
            Debug.Log("There is a stupid exception in this." + E.Message);
            throw E;
        }
        
    }

    public bool isSessionExpired()
    {
        return NakmaConnection.Instance.UserSession.IsExpired;
    }
}
