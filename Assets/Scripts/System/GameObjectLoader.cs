using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���I�u�W�F�N�g�̏�Ԃ��Ǘ�����B
/// �S�ẴV�[���ɐݒu���āA�K�v��GO���������Ď����I�Ƀ��[�h����d�g��
/// </summary>
public class GameObjectLoader : MonoBehaviour
{
    //�O���p�����[�^�[
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

    //�����F���[�h��������
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

        //��ȏ㑶�݂��Ȃ��悤�ȑΉ�
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
        //�R���|�[�l���g�ő�p
        //DontDestroyOnLoad(this);

        foreach(GameObject go in loadgameObjectList)
        {
            if (go)
            {   //���݂��Ă����
                if (loadedGameObjectList.ContainsKey(go.name))
                {
                    //���[�h�ς�
                }
                else
                {
                    //���[�h����
                    GameObject Instance = Instantiate(go) as GameObject;
                    Instance.name = go.name;
                    Instance.transform.parent = this.gameObject.transform;
                    loadedGameObjectList.Add(go.name, Instance);
                }
            }
        }
    }
}
