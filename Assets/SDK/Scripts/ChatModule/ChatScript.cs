
using UnityEngine;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System;
using Nakama;
using UnityEngine.UI;
using Nakama.TinyJson;

public class ChatScript : MonoBehaviour
{
    public InputField TextArea; // To get the text area from which we need to get the input
    public GameObject ChatWindow; // to populate the chat window
    public GameObject MessageSent; // Message Sent Prefab
    public GameObject MessageReceived; // Message received prefab

    public MainMenuScript buttonScriptObj; // Get the GameObject of the ButtonScript and use its public Avatar data members

    private Sprite[] Avatars;

    ConcurrentQueue<IApiChannelMessage> messageQueue;

    private Chat Cobj;

    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            messageQueue = new();
            this.Cobj = new(NakmaConnection.Instance);
            await Cobj.joinGlobalRoom(ClientConstants.globalRoomName, ChatHandler);

            IApiChannelMessageList messageList = await Cobj.GetAllMessages();

                foreach (var message in messageList.Messages)
                {
                    messageQueue.Enqueue(message);
                }
         
            //Assigning the same Avatars over here as button script
            Avatars = buttonScriptObj.Avatars;

        }
        catch (Exception E)
        {
            Debug.Log("Exception in ChatScript : " + E.Message);
        }

    }

    // Update is called once per frame
    async void Update()
    {
        while (messageQueue.Count > 0)
        {

            if (messageQueue.TryDequeue(out IApiChannelMessage msg))
                await PopulateChatMessaages(msg);

        }

    }


    //Used to Populate Messages
    async Task PopulateChatMessaages(IApiChannelMessage message)
    {
        try
        {

            //Get user accounts
            string[] ids = { message.SenderId };
            IApiUsers userAcc = await NakmaConnection.Instance.client.GetUsersAsync(NakmaConnection.Instance.UserSession, ids);

            string messageContent = JsonParser.FromJson<ChatPayload>(message.Content).message;


            GameObject ChildTobeSet;


            if (message.SenderId.Equals(NakmaConnection.Instance.UserSession.UserId))
            {
                ChildTobeSet = MessageSent;
            }
            else
            {
                ChildTobeSet = MessageReceived;
            }

            //Populating the messages
            GameObject messageBox = Instantiate(ChildTobeSet, ChatWindow.transform);
            messageBox.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = messageContent;
            messageBox.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = message.Username;

            ////Avatar Image Index
            foreach (var user in userAcc.Users)
            {
                int avatarImgIndex = user.AvatarUrl == null || user.AvatarUrl.Equals("") ? 0 : (int.Parse(user.AvatarUrl));
                //    ////Set the Avatar 
                messageBox.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Avatars[avatarImgIndex];
            }



        }
        catch (Exception E)
        {
            Debug.Log("Exception in Populate Chat Messages  : " + E.Message);
        }

    }


    //Chat Handler Callback function
    void ChatHandler(IApiChannelMessage message)
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
            await Cobj.SendMessage(Cobj.Channel, TextArea.text);
            TextArea.text = "";
        }
        catch (Exception E)
        {
            Debug.Log("Exception in Send Message button : " + E.Message);
        }



    }
    public void CloseButton()
    {
        
    }


}