using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HttpPolka : MonoBehaviour
{
    public static HttpPolka Instance;

    public Text count;

    public Text MintText;

    public List<string> accountKeys;


    public Text CurrPubKeyText;

    public Text SwallowerGene1;
    public string accountName;

    public Dropdown dropdown;

    public string mintname;

    public Button MintBtn;
    public InputField inputMintName;

    // public InputField chainMessageZone;
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (MintBtn != null) {
            MintBtn.onClick.AddListener(() => {
                StartMintEvent();
            });
        }

        if (accountKeys.Count > 0){
            accountName = accountKeys[0];
        } else {
            AddPubKeyToList("Bob");
            AddPubKeyToList("Alice");
            AddPubKeyToList("Charlie");
            AddPubKeyToList("Dave");
            AddPubKeyToList("Eve");
            AddPubKeyToList("Ferdie");
            // CurrPubKeyText.text = "请先生成一个账号";
            // CurrPubKey = "空";
        }
        accountName = accountKeys[0];
    }

    public void ChangeDropKey() {
        CurrPubKeyText.text = accountKeys[dropdown.value];
        accountName = CurrPubKeyText.text;
    }


    public void StartMintEvent(){
        mintname = inputMintName.text;
        StartCoroutine(IMintEvent(accountName,mintname));
    }

    //query user
    public void StartQueryMySwallowers(string accountName){
        StartCoroutine(UserSwallowers(accountName));
    }
    /// <summary>
    /// 添加生成出来的公钥
    /// </summary>
    /// <param name="key"></param>
    public void AddPubKeyToList(string key) {
        if (!accountKeys.Contains(key)) {
            accountKeys.Add(key);
            if (accountKeys.Count == 1) {
                CurrPubKeyText.text = accountKeys[0];
                accountName = CurrPubKeyText.text;
            }

            count.text = accountKeys.Count.ToString();

            List<Dropdown.OptionData> listOptions = new List<Dropdown.OptionData>();
            listOptions.Add(new Dropdown.OptionData(key));
            dropdown.AddOptions(listOptions);
            Debug.Log("目前有"+accountKeys.Count+"个公钥");
        }
    }

    IEnumerator IMintEvent(string accountName,string name) {
        string url = "http://localhost:5000/swallower/mintSwallower?account="+accountName+"&name="+name;
        WWW www = new WWW(url);
        yield return www;
        if (!www.isDone){
            Debug.Log("错误"+www.error);
            // chainMessageZone.text = "Mint失败--"+www.error;
            MintText.text = "Mint失败--" + www.error;
        } else {
            MintText.text = "Mint结果-"+www.text;
            // chainMessageZone.text = "Mint结果-" + www.text;
            StartQueryMySwallowers(this.accountName);
        }
    }

    // queryUsersSwallower
    IEnumerator UserSwallowers(string accountName) {
        string url = "http://localhost:5000/swallower/ownerSwallower?accountId="+accountName;
        WWW www = new WWW(url);
        yield return www;
        if (!www.isDone){
            Debug.Log("错误"+www.error);
            // chainMessageZone.text = "查询用户的吞噬者失败"+www.error;
            MintText.text = "Mint失败--" + www.error;
        } else {
            MintText.text = "查询用户吞噬者结果-"+www.text;
            
            MySwalloweres mySwallowers= JsonUtility.FromJson<MySwalloweres>(www.text);
            for(int i=0;i<mySwallowers.ownerSwallower.Count;i++){
                Debug.Log("用户的吞噬者:"+mySwallowers.ownerSwallower[i]);
                SwallowerGene1.text = mySwallowers.ownerSwallower[i];
            }
            Debug.Log("用户的吞噬者:"+mySwallowers.ownerSwallower[0]);
            SwallowerGene1.text = mySwallowers.ownerSwallower[0];
        }
    }

    [System.Serializable]
    public class MySwalloweres{
        public List<string> ownerSwallower;
    }
}
