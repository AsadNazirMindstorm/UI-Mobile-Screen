using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


public class ButtonScript : MonoBehaviour
{
    public Canvas mainMenuCanvas; // Reference to the main menu canvas
    public Canvas leaderboardCanvas; // Reference to the leaderboard canvas
    public Canvas ScoreCanvas; // Score Input Canvas Reference
    public InputField ScoreInputField; // Score Input Field Reference
    public Text ScoreErrorText; // Score Status Text Reference

    public GameObject tilePrefab; // Tile Prefab
    public GameObject Content; // Scrolable View Content

    private int userScore=0; // intial value is taken 0

    // Start is called before the first frame update
    void Start()
    {
        //Score Error Text will be Empty
        ScoreErrorText.text = "";

        //Toggling the Main Menu Canvas when the scene loads
        ToggleCanvas(mainMenuCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        //if the user session expires than bring back the login screen
        try
        {
            if (NakmaConnection.Instance.UserSession.IsExpired == true)
                SceneManager.LoadScene(ClientConstants.loadingScreenName);
        }
        catch(Exception E)
        {
            //or if the connection is lost during the game it will throw an exception
            // again bring back the user to the login screen
            SceneManager.LoadScene(ClientConstants.loadingScreenName);
        }

    }

    //Empty Play Button Handler
    public void Play()
    {
        try
        {
          //Empty it does nothing
        }
        catch(Exception E){
            Debug.Log(E.Message);
        }

    }

    //Enter Score Button Handler
    public async void EnterScore()
    {
        try
        {
            //Empty Input Handling over here
            if (ScoreInputField.text.Equals("")) return;

            //setting the user score
            this.userScore = int.Parse(ScoreInputField.text);

            //Debugger 
            Debug.Log("This is the user score : " + this.userScore);

            //setting the score on leaderboard
            Score SObj = new();

            //if it is less than 0 throw an exception it should not set it
            if (this.userScore < 0) throw new Exception("Score cannot be less than zero");

            //else set the score
            await SObj.AddScore(this.userScore);

            ScoreErrorText.text = "Entered Successfully";
        }
        catch(Exception E)
        {
            ScoreErrorText.text = E.Message;
            Debug.Log("Exception in EnterScore Button" + E.Message);

        }

    }


    //enter score button handler
    public void Score()
    {

        ToggleCanvas(ScoreCanvas);
        //Debug.Log("Options has been pressed\n");
    }

    //leader borad button handler
    public async void LeaderBoard()
    {
        try
        {
            await this.PopulateLeaderBoard();
            ToggleCanvas(leaderboardCanvas);

        }
        catch (Exception E)
        {
            Debug.Log(E.Message);
        }
    }


    //Close Enter Score Handler
    public void closeEnterScore()
    {
        ToggleCanvas(mainMenuCanvas);
    }



    //closing the leaderboard canvas
    public void Close()
    {
        ToggleCanvas(mainMenuCanvas);
    }

    //helper function for toggling the canvases
    private void ToggleCanvas(Canvas canvasToShow)
    {
        // Hide all canvases
        mainMenuCanvas.enabled =false;
        leaderboardCanvas.enabled= false;
        ScoreCanvas.enabled = false;


        // Show the selected canvas
        if (canvasToShow != null)
        {
            canvasToShow.enabled= true;
        }
    }

    private async Task PopulateLeaderBoard()
    {
        try
        {
            //Clear Old Tiles first and then populate New ones
           ClearOldTiles();

            if (tilePrefab == null || Content == null)
            {
                Debug.LogError("tilePrefab or Content is not assigned.");
                return;
            }

            LeaderBoard Lobj = new LeaderBoard();
            RootResponseLeaderboard res = await Lobj.getAllRecords("Global");

            // Check if res and res.data are valid
            if (res == null || res.data == null || res.data.records == null)
            {
                Debug.LogError("Failed to retrieve leaderboard records.");
                return;
            }

            // Loop through the leaderboard records
            for (int i = 0; i < res.data.records.Count; i++)
            {
                //Debug.Log(res.data.records[i].username);
                GameObject tile = GameObject.Instantiate(tilePrefab, Content.transform);
                //Setting the username
                tile.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = res.data.records[i].username;

                //setting the score
                tile.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = res.data.records[i].score.ToString();

                //setting the rank
                tile.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = res.data.records[i].rank.ToString();

            }
        }
        catch (Exception E)
        {
            Debug.Log(E.Message);
            SceneManager.LoadScene(ClientConstants.loadingScreenName);
        }
    }

    // Method to clear old tiles
    private void ClearOldTiles()
    {
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }

    }
}
