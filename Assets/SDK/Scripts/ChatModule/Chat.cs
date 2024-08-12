using System;
using Nakama;
using System.Threading.Tasks;
using Nakama.TinyJson;
using System.Collections.Generic;
using UnityEngine;

public class Chat
{
    NakmaConnection instance;
    public IChannel Channel;

    // Declare a field for the event handler
    private static Action<IApiChannelMessage> _messageHandler;

    public Chat(NakmaConnection instance)
    {
        this.instance = instance;
    }

    //Attach Listner

    //joiniing a global room
    public async Task JoinGlobalRoom(string roomName, Action<IApiChannelMessage> MessageCallBack)
    {
        try
        {
            // Ensure socket is connected
            if (NakmaConnection.Instance.Socket.IsConnected == false)
            {
                await NakmaConnection.Instance.CreateSocket();
            }

            var persistence = true;
            var hidden = false;

            AttachMessageReceiveListner(MessageCallBack);

            // Join the chat room
            Channel = await NakmaConnection.Instance.Socket.JoinChatAsync(roomName, ChannelType.Room, persistence, hidden);

            
        }
        catch (Exception ex)
        {
            // Handle the exception (e.g., logging)
            // It’s generally a good idea to log exceptions or handle them in a way that's appropriate for your app
            throw;  // Re-throws the original exception without losing stack trace
        }
    }

    public void AttachMessageReceiveListner(Action<IApiChannelMessage> MessageCallBack)
    {
        try {
            // Remove the old listener first
            if (_messageHandler != null)
            {
                NakmaConnection.Instance.Socket.ReceivedChannelMessage -= _messageHandler;
            }

            // Store the new callback handler
            _messageHandler = message =>
            {
                MessageCallBack?.Invoke(message);
            };

            // Add the new listener
            NakmaConnection.Instance.Socket.ReceivedChannelMessage += _messageHandler;
        }
        catch(Exception E)
        {
            throw E;
        }

    }

    public async Task LeaveChat()
    {
        try {

        }
        catch(Exception E)
        {
            throw E;
        }

    }


    public async Task JoinOneVOneChat(string userId, Action<IApiChannelMessage> MessageCallBack)
    {
        try
        {
            AttachMessageReceiveListner(MessageCallBack);
            this.Channel = await NakmaConnection.Instance.Socket.JoinChatAsync(userId, ChannelType.DirectMessage, true, false);
            Debug.Log(Channel);
        }
        catch(Exception E)
        {
            Debug.Log("Excetpion in 1 Join 1 v1 Chat fucntion : " + E.Message);
            throw E;
        }

    }

    public async Task SendMessage(IChannel Channel, String text, int avatar, string displayName)
    {
        try
        {

            //Getting the user text
            string UserText = text;
     
            //Converting the message payload
            var data = new Dictionary<string, string>
            {
                {"message",UserText },
                {"avatar",avatar.ToString() },
                {"displayName", displayName }
            };

            string JsonText = data.ToJson();

            // 0 length
            if (UserText.Length == 0) return;

            //Response from the server
            IChannelMessageAck response = await NakmaConnection.Instance.Socket.WriteChatMessageAsync(Channel, JsonText);
         

            //if the response is Ok or Not
            if (response.Code != 0) throw new Exception("Error Message Not Sent ! . Error Code : " + response.Code);

        }
        catch (Exception E)
        {
            throw E;

        }
    }



    public ChatPayload getChatMessage(IApiChannelMessage message)
    {

        try {
            ChatPayload chatPayloadObj = JsonParser.FromJson<ChatPayload>(message.Content);
            return chatPayloadObj;
        }
        catch(Exception E)
        {
            throw E;
        }

    }

    //Function to fectch all the messages from the channel
    public async Task<IApiChannelMessageList> GetAllMessages(IChannel channel=null)
    {
        try
        {
            IApiChannelMessageList messageList;

            IChannel channelToFetchFrom = channel == null ? this.Channel : channel;
            messageList = await NakmaConnection.Instance.client.ListChannelMessagesAsync(NakmaConnection.Instance.UserSession,channelToFetchFrom, 100);

            return messageList;
            
        }
        catch(Exception E)
        {
            throw E;
        }

    }


}
