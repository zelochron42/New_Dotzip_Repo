using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class EndgameUI : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] string menuScene;
    RoundScorer scoreObject;
    TextMeshProUGUI textObject;
    string displayText;
    bool routineStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = displayText;

        AdminPings admin = FindObjectOfType<AdminPings>();
        if (admin)
            Destroy(admin.transform.parent.parent.gameObject);

        if (!routineStarted && PhotonNetwork.IsConnectedAndReady && PhotonNetwork.IsMasterClient) {
            routineStarted = true;
            StartCoroutine(PostgameRoutine());
        }
    }

    IEnumerator PostgameRoutine() {
        while (!PhotonNetwork.IsConnectedAndReady) {
            yield return new WaitForSeconds(0.1f);
            displayText = "Connecting...";
        }
        yield return new WaitForSeconds(0.5f);
        displayText = "Getting round data...";
        yield return new WaitForSeconds(2f);
        scoreObject = FindObjectOfType<RoundScorer>();
        if (!scoreObject) {
            displayText = "Can't find score object! Terminating...";
            yield return new WaitForSeconds(2f);
            PhotonNetwork.LoadLevel(menuScene);
            yield break;
        }
        int[] roundData = scoreObject.GetGameData();
        int spawned = roundData[0];
        int deaths = roundData[1];
        int disconnects = roundData[2];
        int victories = roundData[3];
        int playerCount = spawned - disconnects;
        float survivalRate = 0f;
        if (spawned > 0) {
            survivalRate = (float)victories / playerCount;
        }

        if (spawned == 0)
            displayText = "No players detected.";
        else if (survivalRate == 0f)
            displayText = "No survivors!\nEveryone loses!";
        else if (survivalRate <= 0.5f)
            displayText = "Only a few survived.\nBe more careful!";
        else if (survivalRate >= 1f)
            displayText = "Everyone made it.\nCongratulations!";
        else
            displayText = "Most of you made it out!\nGreat work!";
        yield return new WaitForSeconds(2f);
        displayText = "Survivors: " + victories + " out of " + playerCount +
            "\nTotal deaths: " + deaths +
            "\nDisconnects: " + disconnects +
            "\nSurvival rate: " + (survivalRate * 100f).ToString("0.0") + "%";
        yield return new WaitForSeconds(6f);
        PhotonNetwork.Destroy(scoreObject.gameObject);
        displayText = "Returning to main menu...";
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel(menuScene);
        yield break;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(displayText);
        }
        else if (stream.IsReading) {
            displayText = (string)stream.ReceiveNext();
        }
    }
}
