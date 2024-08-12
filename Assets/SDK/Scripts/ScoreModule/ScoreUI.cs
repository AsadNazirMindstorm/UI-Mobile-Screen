using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    public InputField ScoreInputField; // Score Input Field Reference
    public Text ScoreErrorText; // Score Status Text Reference
    private int UserScore;

    public MainMenuHandler MainMenuHandlerUI;

    // Use this for initialization
    void Start()
    {
        this.UserScore = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public async void EnterScoreButton()
    {
        try
        {
            //Empty Input Handling over here
            if (ScoreInputField.text.Equals("") || ScoreInputField.text.Trim().Equals("") || ScoreInputField.text.Length==0 ) return;

            //setting the user score
            this.UserScore = int.Parse(ScoreInputField.text);

            //Score Module Service Module
            Score SObj = new();

            //if it is less than 0 throw an exception it should not set it
            if (this.UserScore < 0) throw new Exception("Score cannot be less than zero");

            //else set the score throw Service class Object of Score Module
            await SObj.AddScore(this.UserScore);

            ScoreErrorText.text = "Entered Successfully";

            ScoreInputField.text = "";
        }
        catch (Exception E)
        {
            ScoreErrorText.text = E.Message;
            Debug.Log("Exception in EnterScore Button" + E.Message);

        }
    }

    public void CloseScoreButton()
    {
        MainMenuHandlerUI.ToggleCanvas(MainMenuHandlerUI.mainMenuCanvas);
    }

}
