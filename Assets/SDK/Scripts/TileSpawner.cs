//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using UnityEngine.SceneManagement;

//public class TileSpawner : MonoBehaviour
//{
//    public GameObject tilePrefab;
//    public GameObject Content;

//    // Start is called before the first frame update
//    async void Start()
//    {
//        try
//        {
//            if (tilePrefab == null || Content == null)
//            {
//                Debug.LogError("tilePrefab or Content is not assigned.");
//                return;
//            }

//            LeaderBoard Lobj = new LeaderBoard();
//            RootResponseLeaderboard res = await Lobj.getAllRecords("Global");

//            // Check if res and res.data are valid
//            if (res == null || res.data == null || res.data.records == null)
//            {
//                Debug.LogError("Failed to retrieve leaderboard records.");
//                return;
//            }

//            // Loop through the leaderboard records
//            for (int i = 0; i < res.data.records.Count; i++)
//            {
//                //Debug.Log(res.data.records[i].username);
//                GameObject tile = GameObject.Instantiate(tilePrefab, Content.transform);
//                //Setting the username
//                tile.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = res.data.records[i].username;

//                //setting the score
//                tile.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = res.data.records[i].score.ToString();

//                //setting the rank
//                tile.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = res.data.records[i].rank.ToString();

//            }
//        }
//        catch(Exception E)
//        {
//            Debug.Log(E.Message);
//            SceneManager.LoadScene(ClientConstants.loadingScreenName);
//        }

//    }


//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
