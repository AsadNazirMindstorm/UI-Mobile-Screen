using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Nakama;


public class NakmaConnection : MonoBehaviour
{
    public IClient client;
    public ISession UserSession;
    public ISocket Socket;

    private static NakmaConnection instance;

    public static NakmaConnection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NakmaConnection>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject("NakmaConnection");
                    instance = singleton.AddComponent<NakmaConnection>();
                    instance.client = new Client(ClientConstants.type, ClientConstants.host, ClientConstants.port, ClientConstants.serverKey);
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }


    public async Task CreateSocket()
    {
        try
        {
            Socket = client.NewSocket();

            //Socket Creation
            const bool appearOnline = true;
            const int connectionTimeout = 30;
            await Socket.ConnectAsync(UserSession, appearOnline, connectionTimeout);
        }
        catch(Exception E)
        {
            Debug.Log("Error in establishing Scoket Connection " + E.Message);
        }

    }


    // Awake ????
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        try
        {
            instance.client = new Client(ClientConstants.type, ClientConstants.host, ClientConstants.port, ClientConstants.serverKey);

        }
        catch (Exception E)
        {
            Debug.Log("Exception Occurred in Making connection with Client : " + E.Message);
            throw new Exception("Connection could not be made to the server");
        }

    }

}