using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using NCMB;
using NCMB.Extensions;
using TMPro;
using System;
using naichilab;

public class RankingScene : MonoBehaviour
{
    private const string OBJECT_ID = "objectId";
    private const string COLUMN_SCORE = "score";
    private const string COLUMN_NAME = "name";

    [SerializeField] TextMeshProUGUI txt_UserName;
    [SerializeField] TextMeshProUGUI txt_HiScore;
    [SerializeField] TextMeshProUGUI txt_Score;
    [SerializeField] Button closeButton;
    [SerializeField] RectTransform scrollViewContent;
    [SerializeField] RectTransform scrollViewNotContent;
    [SerializeField] GameObject rankingNodePrefab;
    [SerializeField] GameObject readingNodePrefab;
    [SerializeField] GameObject notFoundNodePrefab;
    [SerializeField] GameObject unavailableNodePrefab;

    private string objectid = null;
    private RankingInfo board;
    private IScore lastScore;
    private NCMBObject ncmbRecord;

    //他のシーンから代入される値：Score以外
    public string UserName;
    public int Score;
    public int Hiscore;
    public bool IsGameOver = false;

    private string ObjectID
    {
        get { return objectid ?? (objectid = PlayerPrefs.GetString(BoardIdPlayerPrefsKey, null)); }
        set
        {
            if (objectid == value)
                return;
            PlayerPrefs.SetString(BoardIdPlayerPrefsKey, objectid = value);
        }
    }

    private string BoardIdPlayerPrefsKey
    {
        get { return string.Format("{0}_{1}_{2}", "board", board.ClassName, OBJECT_ID); }
    }

    private void Start()
    {
        board = RankingLoader.Instance.CurrentRanking;
        lastScore = RankingLoader.Instance.LastScore;

        StartCoroutine(GetHighScoreAndRankingBoard());
    }

    IEnumerator GetHighScoreAndRankingBoard()
    {
        txt_Score.text = lastScore.TextForDisplay;
        //txt_Score.text = score.ToString();
        txt_UserName.text = UserName;

        //ハイスコア取得
        {
            txt_HiScore.text = "取得中...";

            var hiScoreCheck = new YieldableNcmbQuery<NCMBObject>(board.ClassName);
            hiScoreCheck.WhereEqualTo(OBJECT_ID, ObjectID);
            yield return hiScoreCheck.FindAsync();

            if (hiScoreCheck.Count > 0)
            {
                //既にハイスコアは登録されている
                ncmbRecord = hiScoreCheck.Result.First();

                var s = board.BuildScore(ncmbRecord[COLUMN_SCORE].ToString());
                txt_HiScore.text = s != null ? s.TextForDisplay : "エラー";

            }
            else
            {
                //登録されていない
                txt_HiScore.text = "-----";
            }
        }

        //ランキング取得
        yield return StartCoroutine(LoadRankingBoard());

        //var highScore = board.BuildScore(ncmbRecord[COLUMN_SCORE].ToString());
        //Debug.Log(string.Format("登録済みスコア:{0} 今回スコア:{1} ハイスコア更新:{2}", highScore.Value, lastScore.Value));
    }
    public void SendScores()
    {
        //yield return new WaitForSeconds(5.0f);
        StartCoroutine(SendScoreEnumerator());
    }

    public void SendScore()
    {
        StartCoroutine(SendScoreEnumerator());
    }

    private IEnumerator SendScoreEnumerator()
    {
        //sendScoreButton.interactable = false;
        txt_HiScore.text = "送信中...";

        //ハイスコア送信
        if (ncmbRecord == null)
        {
            ncmbRecord = new NCMBObject(board.ClassName);
            ncmbRecord.ObjectId = ObjectID;
        }

        //後で変更
        ncmbRecord[COLUMN_NAME] = UserName;
        ncmbRecord[COLUMN_SCORE] = lastScore.Value;
        NCMBException errorResult = null;

        yield return ncmbRecord.YieldableSaveAsync(error => errorResult = error);

        if (errorResult != null)
        {
            //NCMBのコンソールから直接削除した場合に、該当のobjectIdが無いので発生する（らしい）
            ncmbRecord.ObjectId = null;
            yield return ncmbRecord.YieldableSaveAsync(error => errorResult = error); //新規として送信
        }

        //ObjectIDを保存して次に備える
        ObjectID = ncmbRecord.ObjectId;

        txt_HiScore.text = lastScore.TextForDisplay;

        yield return StartCoroutine(LoadRankingBoard());

    }


    /// <summary>
    /// ランキング取得＆表示
    /// </summary>
    /// <returns>The ranking board.</returns>
    private IEnumerator LoadRankingBoard()
    {
        int nodeCount = scrollViewContent.childCount;
        for (int i = nodeCount - 1; i >= 0; i--)
        {
            Destroy(scrollViewContent.GetChild(i).gameObject);
        }

        var msg = Instantiate(readingNodePrefab, scrollViewContent);

        //2017.2.0b3の描画されないバグ暫定対応
        MaskOffOn();

        var so = new YieldableNcmbQuery<NCMBObject>(board.ClassName);
        so.Limit = 30;
        if (board.Order == ScoreOrder.OrderByAscending)
        {
            so.OrderByAscending(COLUMN_SCORE);
        }
        else
        {
            so.OrderByDescending(COLUMN_SCORE);
        }

        yield return so.FindAsync();

        Debug.Log("データ取得 : " + so.Count.ToString() + "件");
        Destroy(msg);

        if (so.Error != null)
        {
            Instantiate(unavailableNodePrefab, scrollViewContent);
        }
        else if (so.Count > 0)
        {
            int rank = 0;
            foreach (var r in so.Result)
            {
                var n = Instantiate(rankingNodePrefab, scrollViewContent);
                var rankNode = n.GetComponent<RankingNode>();
                rankNode.NoText.text = (++rank).ToString();
                rankNode.NameText.text = r[COLUMN_NAME].ToString();

                var s = board.BuildScore(r[COLUMN_SCORE].ToString());
                rankNode.ScoreText.text = s != null ? s.TextForDisplay : "エラー";
            }
        }
        else
        {
            Instantiate(notFoundNodePrefab, scrollViewNotContent);
        }

        //ここで
        if (IsGameOver)
        {
           //yield return new WaitForSeconds(0.5f);
           SendScores();
           IsGameOver = false;
        }
    }

    public void OnCloseButtonClick()
    {
        SoundManager.Instance.PlayOneShot(AppSound.Instance.SE_MENU_CANCEL);
        closeButton.interactable = false;
        IsGameOver = false;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Ranking");//アンロード
    }

    private void MaskOffOn()
    {
        //2017.2.0b3でなぜかScrollViewContentを追加しても描画されない場合がある。
        //親maskをOFF/ONすると直るので無理やり・・・
        var m = scrollViewContent.parent.GetComponent<Mask>();
        m.enabled = false;
        m.enabled = true;
    }
}
