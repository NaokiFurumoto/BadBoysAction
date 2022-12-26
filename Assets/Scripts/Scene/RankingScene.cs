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

    //���̃V�[�������������l�FScore�ȊO
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

        //�n�C�X�R�A�擾
        {
            txt_HiScore.text = "�擾��...";

            var hiScoreCheck = new YieldableNcmbQuery<NCMBObject>(board.ClassName);
            hiScoreCheck.WhereEqualTo(OBJECT_ID, ObjectID);
            yield return hiScoreCheck.FindAsync();

            if (hiScoreCheck.Count > 0)
            {
                //���Ƀn�C�X�R�A�͓o�^����Ă���
                ncmbRecord = hiScoreCheck.Result.First();

                var s = board.BuildScore(ncmbRecord[COLUMN_SCORE].ToString());
                txt_HiScore.text = s != null ? s.TextForDisplay : "�G���[";

            }
            else
            {
                //�o�^����Ă��Ȃ�
                txt_HiScore.text = "-----";
            }
        }

        //�����L���O�擾
        yield return StartCoroutine(LoadRankingBoard());

        //var highScore = board.BuildScore(ncmbRecord[COLUMN_SCORE].ToString());
        //Debug.Log(string.Format("�o�^�ς݃X�R�A:{0} ����X�R�A:{1} �n�C�X�R�A�X�V:{2}", highScore.Value, lastScore.Value));
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
        txt_HiScore.text = "���M��...";

        //�n�C�X�R�A���M
        if (ncmbRecord == null)
        {
            ncmbRecord = new NCMBObject(board.ClassName);
            ncmbRecord.ObjectId = ObjectID;
        }

        //��ŕύX
        ncmbRecord[COLUMN_NAME] = UserName;
        ncmbRecord[COLUMN_SCORE] = lastScore.Value;
        NCMBException errorResult = null;

        yield return ncmbRecord.YieldableSaveAsync(error => errorResult = error);

        if (errorResult != null)
        {
            //NCMB�̃R���\�[�����璼�ڍ폜�����ꍇ�ɁA�Y����objectId�������̂Ŕ�������i�炵���j
            ncmbRecord.ObjectId = null;
            yield return ncmbRecord.YieldableSaveAsync(error => errorResult = error); //�V�K�Ƃ��đ��M
        }

        //ObjectID��ۑ����Ď��ɔ�����
        ObjectID = ncmbRecord.ObjectId;

        txt_HiScore.text = lastScore.TextForDisplay;

        yield return StartCoroutine(LoadRankingBoard());

    }


    /// <summary>
    /// �����L���O�擾���\��
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

        //2017.2.0b3�̕`�悳��Ȃ��o�O�b��Ή�
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

        Debug.Log("�f�[�^�擾 : " + so.Count.ToString() + "��");
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
                rankNode.ScoreText.text = s != null ? s.TextForDisplay : "�G���[";
            }
        }
        else
        {
            Instantiate(notFoundNodePrefab, scrollViewNotContent);
        }

        //������
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
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Ranking");//�A�����[�h
    }

    private void MaskOffOn()
    {
        //2017.2.0b3�łȂ���ScrollViewContent��ǉ����Ă��`�悳��Ȃ��ꍇ������B
        //�emask��OFF/ON����ƒ���̂Ŗ������E�E�E
        var m = scrollViewContent.parent.GetComponent<Mask>();
        m.enabled = false;
        m.enabled = true;
    }
}
