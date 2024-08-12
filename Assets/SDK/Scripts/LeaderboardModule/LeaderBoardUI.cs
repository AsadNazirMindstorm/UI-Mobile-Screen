using UnityEngine;
using Nakama;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LeaderBoardUI : MonoBehaviour
{
   
    // LeaderBoard Tile Prefab
    public GameObject LeaderBoardTilePrefab;
    public GameObject Content;


    //MainMenuScript Handler
    public MainMenuHandler MainMenuHandlerUI;

    //ChatUI handler
    public ChatUI ChatUIHandler;

    // Use this for initialization
    void Start()
    {

    }

    // Method to clear old tiles
    private void ClearOldTiles()
    {
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }

    }

    public async Task PopulateLeaderBoard()
    {
        try
        {
            //Clear Old Tiles first and then populate New ones
            ClearOldTiles();

            if (LeaderBoardTilePrefab == null || Content == null)
            {
                Debug.LogError("tilePrefab or Content is not assigned.");
                return;
            }

            //LeaderBoard Service class 
            LeaderBoard Lobj = new LeaderBoard();

            //Service Class fetching the records from the database
            RootResponseLeaderboard res = await Lobj.getAllRecords(ClientConstants.leaderBoard);

            // Check if res and res.data are valid
            if (res == null || res.data == null || res.data.records == null)
            {
                Debug.LogError("Failed to retrieve leaderboard records.");
                return;
            }

            // Loop through the leaderboard records
            for (int i = 0; i < res.data.records.Count; i++)
            {
                //Using Get Child is necessary over here as we are creating the tiles on runtime
                //And The LeaderBoard Tile Prefab has a specific Desgin set to it
              
                GameObject tile = GameObject.Instantiate(LeaderBoardTilePrefab, Content.transform);
                //Setting the username
                tile.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = res.data.records[i].username;

                //setting the score
                tile.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = res.data.records[i].score.ToString();

                //setting the rank
                tile.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = res.data.records[i].rank.ToString();

                //For closure
                int index = i;
                //setting the 1 v 1 chat button
                tile.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(
                    async () => {
                        await ChatUIHandler.LoadChatCanvas(false, res.data.records[index].ownerId);
                        MainMenuHandlerUI.ToggleCanvas(MainMenuHandlerUI.ChatCanvas);
                    }
                    );
            }
        }
        catch (Exception E)
        {
            Debug.Log("Error in Populating LeaderBoard :" +E.Message);
        }
    }

    public void CloseLeaderBoardButton()
    {
        MainMenuHandlerUI.ToggleCanvas(MainMenuHandlerUI.mainMenuCanvas);   
    }


    // Update is called once per frame
    void Update()
    {

    }
}
