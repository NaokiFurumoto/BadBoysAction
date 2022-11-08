using UnityEngine.SceneManagement;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Scene�J�ڎ��Ɉ��S�Ƀp�����[�^�[��n���N���X
/// ���V�[�����Ăяo��
/// var scene = await SceneLoader.Load<SceneB>("�V�[����");
/// ���C�Ӄ��\�b�h�̌Ăяo��(�^�C�~���O��sceneB��Awake�̌�AStart�̑O)
/// sceneB.SetArguments(123, new List<string> { "abc", "����������" });
/// ��
/// �擾�ł���R���|�[�l���g�̓��[�h��V�[���̃��[�g�K�w�ɔz�u����Ă���
/// GameObject�ɃA�^�b�`����Ă���R���|�[�l���g�Ɍ��肵�Ă��܂��B
/// �V�[�����[�h�͔񓯊������ɂȂ邽��async/await�Ń��[�h���������s���܂��B
/// </summary>
public static class LoadScene
{ 
    /// <summary>
    /// �񓯊����[�h
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="sceneName">�V�[����</param>
    /// <param name="">���[�h���[�h</param>
    /// <returns>���[�h��V�[���̃R���|�[�l���g</returns>
    public static UniTask<TComponent> Load<TComponent>(string sceneName,
        LoadSceneMode mode = LoadSceneMode.Single) where TComponent : Component
    {
        var tsk = new UniTaskCompletionSource<TComponent>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName, mode);
        return tsk.Task;

        void OnSceneLoaded(Scene scene, LoadSceneMode _mode)
        {
            //��x�Ă΂ꂽ��s�v�Ȃ̂ō폜
            SceneManager.sceneLoaded -= OnSceneLoaded;

            //���[�h�����V�[���̃��[�g�K�w��GameObject����w��R���|�[�l���g��1�擾����
            var target = GetFirstComponent<TComponent>(scene.GetRootGameObjects());

            tsk.TrySetResult(target);
        }
    }

    /// <summary>
    /// GameObject�z�񂩂�w��̃R���|�[�l���g����擾����
    /// </summary>
    /// <typeparam name="TComponent">�擾�ΏۃR���|�[�l���g</typeparam>
    /// <param name="gameObjects">GameObject�z��</param>
    /// <returns>�ΏۃR���|�[�l���g</returns>
    private static TComponent GetFirstComponent<TComponent>(GameObject[] gameObjects)
        where TComponent : Component
    {
        TComponent target = null;
        foreach (GameObject go in gameObjects)
        {
            target = go.GetComponent<TComponent>();
            if (target != null) break;
        }
        return target;
    }


    /// <summary>
    /// �ʏ탍�[�h
    /// </summary>
    /// <param name="sceneName">�V�[����</param>
    /// <param name="mode">�V�[�����[�h���[�h</param>
    public static void Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(sceneName, mode);
    }

}
