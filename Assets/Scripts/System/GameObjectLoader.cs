using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームオブジェクトの状態を管理する。
/// 全てのシーンに設置して、必要なGOを検索して自動的にロードする仕組み
/// </summary>
public class GameObjectLoader : MonoBehaviour
{
    //外部パラメーター
    public GameObject[] LoadGameObjectList_Awake;
    public GameObject[] LoadGameObjectList_Start;
    public GameObject[] LoadGameObjectList_Update;
    public GameObject[] LoadGameObjectList_FixedUpdate;


    [System.NonSerialized]
    public Dictionary<string, GameObject>
        loadedGameObjectList_Awake = new Dictionary<string, GameObject>();
    [System.NonSerialized]
    public bool loaded_Awake = false;

    [System.NonSerialized]
    public Dictionary<string, GameObject>
        loadedGameObjectList_Start = new Dictionary<string, GameObject>();
    [System.NonSerialized]
    public bool loaded_Start = false;

    [System.NonSerialized]
    public Dictionary<string, GameObject>
        loadedGameObjectList_Update = new Dictionary<string, GameObject>();
    [System.NonSerialized]
    public bool loaded_Update = false;

    [System.NonSerialized]
    public Dictionary<string, GameObject>
        loadedGameObjectList_FixedUpdate = new Dictionary<string, GameObject>();
    [System.NonSerialized]
    public bool loaded_FixedUpdate = false;

    //内部：ロード完了判定
    private bool loaded = false;

    private void Awake()
    {
        bool loadedAll = false;
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach(GameObject go in gos)
        {
            GameObjectLoader goloader = go.GetComponent<GameObjectLoader>();
            if (goloader)
            {
                if (goloader.loaded)
                {
                    loadedAll = true;
                    break;
                }
            }
        }

        //一つ以上存在しないような対応
        if (loadedAll)
        {
            Destroy(gameObject);
            return;
        }

        loaded = true;

        if (!loaded_Awake)
        {
            loaded_Awake = true;
            LoadGameObject(LoadGameObjectList_Awake, loadedGameObjectList_Awake);
        }
    }

    private void Start()
    {
        if (!loaded_Start)
        {
            loaded_Start = true;
            LoadGameObject(LoadGameObjectList_Start, loadedGameObjectList_Start);
        }
    }

    private void Update()
    {
        if (!loaded_Update)
        {
            loaded_Update = true;
            LoadGameObject(LoadGameObjectList_Update, loadedGameObjectList_Update);
        }
    }

    private void FixedUpdate()
    {
        if (!loaded_FixedUpdate)
        {
            loaded_Update = true;
            LoadGameObject(LoadGameObjectList_FixedUpdate, loadedGameObjectList_FixedUpdate);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loadgameObjectList"></param>
    /// <param name="loadedGameObjectList"></param>
    private void LoadGameObject(GameObject[] loadgameObjectList, 
                                Dictionary<string,GameObject> loadedGameObjectList)
    {
        //コンポーネントで代用
        //DontDestroyOnLoad(this);

        foreach(GameObject go in loadgameObjectList)
        {
            if (go)
            {   //存在していれば
                if (loadedGameObjectList.ContainsKey(go.name))
                {
                    //ロード済み
                }
                else
                {
                    //ロードする
                    GameObject Instance = Instantiate(go) as GameObject;
                    Instance.name = go.name;
                    Instance.transform.parent = this.gameObject.transform;
                    loadedGameObjectList.Add(go.name, Instance);
                }
            }
        }
    }
}
