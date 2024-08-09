using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoginButtonHandler : MonoBehaviour
{
    public Canvas LoadingCanvas;
    public Canvas LoginCanvas;
    public Canvas ErrorCanvas;

    NakmaConnection nakma;

    // Start is called before the first frame update
    void Start()
    {

        //Disabling all the canvases on intial load
        this.LoadingCanvas.enabled = false;
        this.ErrorCanvas.enabled = false;
        this.nakma = NakmaConnection.Instance;
    }


    public void Error()
    {
        //Toggling canvases on error screen
        this.ErrorCanvas.enabled = false;
        this.LoginCanvas.enabled = true;
        this.LoadingCanvas.enabled = false;
    }


    //Login Button Onclick Handler
    public async void Login()
    {
        try
        {

            //This causes an error when trying to Open Canvases Again

            //This works
            LoadingCanvas.enabled = true;
            LoginCanvas.enabled = false;
       
            //Creating an Auth Object for authentication
            Auth authObj = new(nakma);

            await authObj.AuthenticateClient();

        }
        catch(Exception E)
        {
            ErrorCanvas.enabled = true;
            LoadingCanvas.enabled = false;
            LoginCanvas.enabled = false;

            Debug.Log("Exception in Login Button " + E.Message);
            
        }


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
