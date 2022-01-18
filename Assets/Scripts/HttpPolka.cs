using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HttpPolka : MonoBehaviour{
    public Text Mint1Text;

    public string mintname;
    // Start is called before the first frame update
    void Start(){
        StartCoroutine(IMintEvent(mintname));
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
