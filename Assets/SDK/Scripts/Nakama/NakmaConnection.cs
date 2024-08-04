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
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
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
            client = new Client(ClientConstants.type, ClientConstants.host, ClientConstants.port, ClientConstants.serverKey);
        }
        catch (Exception E)
        {
            Debug.Log("Exception Occurred in Making connection with Client : " + E.Message);
            throw new Exception("Connection could not be made to the server");
        }

    }

}