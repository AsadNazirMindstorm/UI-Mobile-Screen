using System;
using Nakama;
using System.Threading.Tasks;
using Nakama.TinyJson;
using System.Collections.Generic;

public class Chat
{
    NakmaConnection instance;
    public IChannel Channel;


    public Chat(NakmaConnection instance)
    {
        this.instance = instance;
    }

    //joiniing a global room
    public async Task joinGlobalRoom(string roomName, Action<IApiChannelMessage> MessageCallBack)
    {
        try
        {
            if (NakmaConnection.Instance.Socket.IsConnected == false)
                await NakmaConnection.Instance.CreateSocket();

            var persistence = true;
            var hidden = false;
            Channel = await NakmaConnection.Instance.Socket.JoinChatAsync(ClientConstants.globalRoomName, ChannelType.Room, persistence, hidden);

            instance.Socket.ReceivedChannelMessage += message =>
            { 
                //Invoking the call back function
                MessageCallBack?.Invoke(message);
            };

        }
        catch (Exception E) {
            throw E;
        }

    }
    public async Task SendMessage(IChannel Channel, String text)
    {
        try
        {

            //Getting the user text
            string UserText = text;
     
            //Converting the message payload
            var data = new Dictionary<string, string>
            {{"message",UserText } };

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
