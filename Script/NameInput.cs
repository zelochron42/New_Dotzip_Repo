using Photon.Pun;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;
    private const string PlayerPrefsNameKey = "PlayerName";
    private void Start()
    {
        SetUpInputField();
    }
    private void SetUpInputField(){
    if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)){return;}
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }
    public void SetPlayerName(string name){
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }
    public void SavePlayerName(){
        string PlayerName = nameInputField.text;

        PhotonNetwork.NickName = PlayerName;

        PlayerPrefs.SetString(PlayerPrefsNameKey, PlayerName);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
