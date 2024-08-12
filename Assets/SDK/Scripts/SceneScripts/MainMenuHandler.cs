using UnityEngine;
using Nakama;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


public class MainMenuHandler : MonoBehaviour
{
    //Canvases to toggle on Main Menu
    public Canvas mainMenuCanvas; // Reference to the main menu canvas
    public Canvas leaderboardCanvas; // Reference to the leaderboard canvas;
    public Canvas ScoreCanvas; // Score Input Canvas Reference
    public Canvas ChatCanvas; // Global Chat Canvas
    public Canvas EditCanvas; // Edit Profile Canvas
    public Canvas LoadingCanvas; // Loading Canvas reference

    //UI Handlers
    public LeaderBoardUI LeaderBoardUIHandler;
    public ScoreUI ScoreUIHandler;
    public ChatUI ChatUIHandler;

    //Loaded here once and then used everywhere else
    public Image AvatarImage; //Avatar Image
    public Sprite[] Avatars; // Array of Sprites of Avatar Images
    public Text DisplayName; //Display Name

    public IApiUsers UserAcc;

    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            //if the connection is lost or session was expired
            if (!NakmaConnection.Instance || NakmaConnection.Instance.UserSession.IsExpired) throw new Exception("Session Timed out");

            //Receiving Notification for online users
            NakmaConnection.Instance.Socket.ReceivedNotification += notification =>
            {
                Debug.Log("Notification content: "  + notification.Content);
            };


            //Toggling the Main Menu Canvas when the scene loads
            ToggleCanvas(mainMenuCanvas);

            //Get and Update the account
            await FetchAndUpdateUserAcc();

           
        }
        catch(Exception E)
        {
            //Throw User to login screen if the Nakma Instance is NULL or User Session is Expired
            Debug.Log("Exception in Start of ButtonScript " + E.Message);
            SceneManager.LoadScene(ClientConstants.loadingScreenName);
        }
    }

    private async Task FetchAndUpdateUserAcc()
    {
        User Uobj = new();

        //getting the accout
        this.UserAcc = await Uobj.GetProfile(NakmaConnection.Instance.UserSession.UserId);

        foreach (var userAcc in UserAcc.Users)
        {
            //Setting the display name and the sprite
            this.DisplayName.text = userAcc.DisplayName;

            //Setting the if it exists
            int avatarImgIndex = userAcc.AvatarUrl == null ? 0 : int.Parse(userAcc.AvatarUrl);

            //Debug.Log("The Avatar Index is : " + avatarImgIndex);

            //settign the avatar of the Logged in user
            this.AvatarImage.sprite = Avatars[avatarImgIndex];
        }
    }


    public async void Chat()
    {
        try
        {
            ToggleCanvas(this.LoadingCanvas);
            await ChatUIHandler.LoadChatCanvas(true,null);
            ToggleCanvas(this.ChatCanvas);
        }
        catch(Exception E)
        {
            Debug.Log("Chat Button Exception : " + E.Message);
        }

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


    //Toggle The Edit canvas from here
    public void EditButton()
    {
        ToggleCanvas(EditCanvas);
    }



    //enter score button handler
    public void Score()
    {

        ToggleCanvas(ScoreCanvas);
    }

    //sepaarate The leader Board
    //leader borad button handler
    public async void LeaderBoard()
    {
        try
        {
            await LeaderBoardUIHandler.PopulateLeaderBoard();
            ToggleCanvas(leaderboardCanvas);

        }
        catch (Exception E)
        {
            Debug.Log(E.Message);
        }
    }


    //Close Enter Score Handler
    public void CloseEnterScore()
    {
        ToggleCanvas(mainMenuCanvas);
    }


    //closing the leaderboard canvas
    public void CloseLeaderBoardButton()
    {
        ToggleCanvas(mainMenuCanvas);
    }

    //Closing the Edit Canvas
    public async void CloseEditButton()
    {
        await FetchAndUpdateUserAcc();
        ToggleCanvas(mainMenuCanvas);
    }

    

    public void ToggleCanvas(Canvas canvasToShow)
    {
        // Hide all canvases
        mainMenuCanvas.gameObject.SetActive(false);
        leaderboardCanvas.gameObject.SetActive(false);
        ScoreCanvas.gameObject.SetActive(false);
        ChatCanvas.gameObject.SetActive(false);
        EditCanvas.gameObject.SetActive(false);
        LoadingCanvas.gameObject.SetActive(false);

        // Show the selected canvas
        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
        }
    }

}
