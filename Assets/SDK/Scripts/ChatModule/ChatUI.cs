using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System;
using Nakama;
using UnityEngine.UI;
using Nakama.TinyJson;

public class ChatUI : MonoBehaviour
{
    public InputField TextArea; // To get the text area from which we need to get the input
    public GameObject ChatWindow; // to populate the chat window
    public GameObject MessageSentPrefab; // Message Sent Prefab
    public GameObject MessageReceivedPrefab; // Message received prefab

    public MainMenuHandler MainMenuHandlerUI; // Get the GameObject of the ButtonScript and use its public Avatar data members
    private Dictionary<string, IApiUsers> UserProfileCache;
  
    ConcurrentQueue<IApiChannelMessage> messageQueue;


    
    private Chat Cobj;

    // Start is called before the first frame update
    void Start()
    {

        UserProfileCache = new();
        //Initilizing message Queu
        messageQueue = new();
    }

    public async Task LoadChatCanvas(bool isGlobalChat, string userId)
    {
        try
        {
            UserProfileCache = new();
            this.Cobj = new(NakmaConnection.Instance);
            if (isGlobalChat)
                await Cobj.JoinGlobalRoom(ClientConstants.globalRoomName, ChatHandler);
            else
                await Cobj.JoinOneVOneChat(userId,ChatHandler);

            await this.LoadOldMessages();

        }
        catch (Exception E)
        {
            Debug.Log("Exception in ChatScript : " + E.Message);
        }
    }

   

    public async Task LoadOldMessages()
    {
        DestroyOldMessagePrefabs();

        IApiChannelMessageList messageList = await Cobj.GetAllMessages();

        foreach (var message in messageList.Messages)
        {
            //Debug.Log(message.Content.FromJson<ChatPayload>().message);
            //messageQueue.Enqueue(message);
            await PopulateChatMessaages(message);
        }

    }

    public void DestroyOldMessagePrefabs()
    {
        foreach (Transform child in ChatWindow.transform)
        {
            Destroy(child.gameObject);
        }
    }



    // Update is called once per frame
   async void Update()
    {
        //reacreate the coonnection if its lost
        if (NakmaConnection.Instance.Socket == null || NakmaConnection.Instance.Socket.IsConnected==false)
            await NakmaConnection.Instance.CreateSocket();

        while (messageQueue.Count > 0)
        {

            if (messageQueue.TryDequeue(out IApiChannelMessage msg))
                await PopulateChatMessaages(msg);

        }

    }


    public async Task PopulateChatMessaages(IApiChannelMessage message)
    {
        try
        {
            // Service class of user Object
            User UObj = new();
            IApiUsers userAcc;

            // Check if user profile is already in cache
            if (!UserProfileCache.TryGetValue(message.SenderId, out userAcc))
            {
                // Fetch the user profile if not in cache
                userAcc = await UObj.GetProfile(message.SenderId);

                // Cache the user profile
                UserProfileCache[message.SenderId] = userAcc;
            }

            // Parsing the data
            string messageContent = JsonParser.FromJson<ChatPayload>(message.Content).message;

            GameObject ChildTobeSet;

            if (message.SenderId.Equals(NakmaConnection.Instance.UserSession.UserId))
            {
                ChildTobeSet = MessageSentPrefab;
            }
            else
            {
                ChildTobeSet = MessageReceivedPrefab;
            }

            // Populating the messages
            GameObject messageBox = Instantiate(ChildTobeSet, ChatWindow.transform);
            messageBox.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = messageContent;

            // Avatar Image Index
            foreach (var user in userAcc.Users)
            {
                int avatarImgIndex = user.AvatarUrl == null || user.AvatarUrl.Equals("") ? 0 : (int.Parse(user.AvatarUrl));
                messageBox.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = user.DisplayName;

                // Set the Avatar
                Debug.Log("Avatar Image Index is in pop : " + avatarImgIndex);
                messageBox.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = MainMenuHandlerUI.Avatars[avatarImgIndex];
            }
        }
        catch (Exception E)
        {
            Debug.Log("Exception in Populate Chat Messages  : " + E.Message);
        }
    }



    //Chat Handler Callback function
    public void ChatHandler(IApiChannelMessage message)
    {
        try
        {
            messageQueue.Enqueue(message);
        }
        catch (Exception E)
        {
            Debug.Log("Exception in Chat Handler Call Back function : " + E.Message);
        }

    }


    //Chat Window Send Button
    public async void SendButton()
    {
        try
        {
            foreach (var userAcc in MainMenuHandlerUI.UserAcc.Users)
            {
                //Sending the message data with avatar and display name to reduce database hits
                await Cobj.SendMessage(Cobj.Channel, TextArea.text,int.Parse(userAcc.AvatarUrl), userAcc.DisplayName);
                TextArea.text = "";
            }
        }
        catch (Exception E)
        {
            Debug.Log("Exception in Send Message button : " + E.Message);
        }



    }

    public async void OneOnoneChatButton(string userId)
    {
        try
        {
            this.Cobj = new(NakmaConnection.Instance);
            await Cobj.JoinOneVOneChat(userId, ChatHandler);
            Debug.Log("User id is : " + userId);
        }
        catch(Exception E)
        {
            Debug.Log("Exception in 1 v 1 chat button  : " + E.Message);
        }

    }


    //Close Button for Chat Canvas
    public void CloseButton()
    {
        Cobj.LeaveChat();
        UserProfileCache.Clear();
        MainMenuHandlerUI.ToggleCanvas(MainMenuHandlerUI.mainMenuCanvas);
    }


}