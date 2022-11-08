using UnityEngine.SceneManagement;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Scene遷移時に安全にパラメーターを渡すクラス
/// ■シーンを呼び出す
/// var scene = await SceneLoader.Load<SceneB>("シーン名");
/// ■任意メソッドの呼び出し(タイミングはsceneBのAwakeの後、Startの前)
/// sceneB.SetArguments(123, new List<string> { "abc", "あいうえお" });
/// ※
/// 取得できるコンポーネントはロード先シーンのルート階層に配置されている
/// GameObjectにアタッチされているコンポーネントに限定しています。
/// シーンロードは非同期処理になるためasync/awaitでロード処理を実行します。
/// </summary>
public static class LoadScene
{ 
    /// <summary>
    /// 非同期ロード
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="sceneName">シーン名</param>
    /// <param name="">ロードモード</param>
    /// <returns>ロード先シーンのコンポーネント</returns>
    public static UniTask<TComponent> Load<TComponent>(string sceneName,
        LoadSceneMode mode = LoadSceneMode.Single) where TComponent : Component
    {
        var tsk = new UniTaskCompletionSource<TComponent>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName, mode);
        return tsk.Task;

        void OnSceneLoaded(Scene scene, LoadSceneMode _mode)
        {
            //一度呼ばれたら不要なので削除
            SceneManager.sceneLoaded -= OnSceneLoaded;

            //ロードしたシーンのルート階層のGameObjectから指定コンポーネントを1つ取得する
            var target = GetFirstComponent<TComponent>(scene.GetRootGameObjects());

            tsk.TrySetResult(target);
        }
    }

    /// <summary>
    /// GameObject配列から指定のコンポーネントを一つ取得する
    /// </summary>
    /// <typeparam name="TComponent">取得対象コンポーネント</typeparam>
    /// <param name="gameObjects">GameObject配列</param>
    /// <returns>対象コンポーネント</returns>
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
    /// 通常ロード
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="mode">シーンロードモード</param>
    public static void Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(sceneName, mode);
    }

}
