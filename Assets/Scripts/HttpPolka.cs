using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HttpPolka : MonoBehaviour
{
    public static HttpPolka Instance;

    public Text count;

    public Text MintText;

    public List<string> MineKeys;


    public Text CurrPubKeyText;
    public string CurrPubKey;

    public Dropdown dropdown;

    public string mintname;

    public Button MintBtn;
    public InputField inputMintName;

    public InputField mintMesInput;
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

        if (MineKeys.Count > 0)
        {
            CurrPubKey = MineKeys[0];
        }
        else {
            CurrPubKeyText.text = "请先生成一个账号";
            CurrPubKey = "空";
        }
    }

    public void ChangeDropKey() {
        CurrPubKeyText.text = MineKeys[dropdown.value];
        CurrPubKey = CurrPubKeyText.text;
    }


    public void StartMintEvent()
    {
        mintname = inputMintName.text;
        StartCoroutine(IMintEvent(mintname));
    }
    /// <summary>
    /// 添加生成出来的公钥
    /// </summary>
    /// <param name="key"></param>
    public void AddPubKeyToList(string key) {
        if (!MineKeys.Contains(key)) {
            MineKeys.Add(key);
            if (MineKeys.Count == 1) {
                CurrPubKeyText.text = MineKeys[0];
                CurrPubKey = CurrPubKeyText.text;
            }

            count.text = MineKeys.Count.ToString();

            List<Dropdown.OptionData> listOptions = new List<Dropdown.OptionData>();
            listOptions.Add(new Dropdown.OptionData(key));
            dropdown.AddOptions(listOptions);
            Debug.Log("目前有"+MineKeys.Count+"个公钥");
        }
    }

    IEnumerator IMintEvent(string name) {
        string url = "http://localhost:5000/swallower/mintSwallower?name="+name;
        WWW www = new WWW(url);
        yield return www;
        if (!www.isDone)
        {
            Debug.Log("错误");
            mintMesInput.text = "Mint失败--"+www.error;
            MintText.text = "Mint失败--" + www.error;
        }
        else {
            MintText.text = "Mint结果-"+www.text;
            mintMesInput.text = "Mint结果-" + www.text;
        }
    }
}
