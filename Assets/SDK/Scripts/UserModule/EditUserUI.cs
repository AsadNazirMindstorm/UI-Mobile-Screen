using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Nakama;

public class EditUserUI : MonoBehaviour
{

    public InputField Username; // Reference to Input Field
    public Image Avatar; //Reference to the Avatar
    public  MainMenuHandler Bobj; // Reference to the main menu button script
    public GameObject AvatarWindow; // Reference to the Avatars Scrollable Window in the Edit Screen

    public Image AvatarPrefab;
    IApiUsers userAccs;
    private int SelectedAvatar;

    // Use this for initialization
    async void Start()
    {
        try {

            User uObj = new();

            userAccs = await uObj.GetProfile(NakmaConnection.Instance.UserSession.UserId);

            //Load Avatars
            LoadAvatars();

            //attaching image to the 
            foreach (var userAcc in userAccs.Users)
            {
                Avatar.sprite = Bobj.Avatars[int.Parse(userAcc.AvatarUrl)];
                SelectedAvatar = int.Parse(userAcc.AvatarUrl);
            }

           
        }
        catch (Exception E)
        {
            Debug.Log("Exception in Start of Edit User Script :  " + E.Message);
        }


    }

    public async void SaveButton()
    {
        try {

            User uObj = new();

            //If user has entered an empty string
            if (Username.Equals("")) return;

            //Update the profile
            await uObj.UpdateProfile(NakmaConnection.Instance.UserSession.Username, Username.text, SelectedAvatar);
            Username.text = "";
        }
        catch(Exception E) {
            Debug.Log("Exception in save button "+ E.Message);
        }

    }


    //Load all the Avatar Images in the Grid
    public void LoadAvatars() {

        int index = 0;

        foreach (var avatar in Bobj.Avatars)
        {
            Image newAvatar = Instantiate(AvatarPrefab, AvatarWindow.transform);
            newAvatar.sprite = avatar;

            int currentIndex = index;
            // Optionally pass the avatar as a parameter if needed
            newAvatar.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => this.AvatarOnclick(currentIndex));
            index++;
        }

    }

    public void AvatarOnclick(int index)
    {
        try
        {
            Debug.Log("index in onclick is : " + index);
            SelectedAvatar = index;
            //change the avatars on selecting a new avatar
            Avatar.sprite = Bobj.Avatars[index];
        }
        catch(Exception E)
        {
            Debug.Log("Exception in Avatar On Click : " + E.Message);

        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
