using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accounts : MonoBehaviour{
    public Text Mint1Text;

    public string accountName;
    // Start is called before the first frame update
    void Start(){
        // StartCoroutine(IMintEvent(accountName));
    }

    public void setAccountName(string accountName){
        this.accountName = accountName;
    }

    IEnumerator IMintEvent(string name) {
        string url = "http://localhost:5000/api/metaData";
        WWW www = new WWW(url);
        yield return www;
        if (!www.isDone){
            Debug.Log("wait for response!");
        } else {
            Mint1Text.text = www.text;
            Debug.Log(www.text);
        }
    }
}
