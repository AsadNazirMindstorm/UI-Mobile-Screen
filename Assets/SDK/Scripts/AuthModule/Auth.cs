using UnityEngine;
using Nakama;
using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;

public class Auth
{
    public NakmaConnection instance;

    public Auth(NakmaConnection obj)
    {
        this.instance = obj;
    }


   public async Task AuthenticateClient()
    {

        try
        {

            // Authentication logic
            instance.UserSession = await instance.client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            Debug.Log("User Id is: " + instance.UserSession.UserId);

            //If Authenticated 
            SceneManager.LoadScene(ClientConstants.mainMenuSceneName);
        }
        catch(Exception E)
        {
            throw E;
        }
        
    }

    public bool isSessionExpired()
    {
        return instance.UserSession.IsExpired;
    }
}
