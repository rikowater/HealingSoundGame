using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class JoystickManager : MonoBehaviour
{
    [System.Serializable]
    private class JoystickData
    {
        [SerializeField] private string targetSceneName;  // ジョイスティックが存在するシーン名
        [SerializeField] private string joystickObjectName; // ジョイスティックオブジェクトの名前

        public string TargetSceneName => targetSceneName; // 読み取り専用プロパティ
        public string JoystickObjectName => joystickObjectName;
    }

    [SerializeField] private List<JoystickData> joysticks = new List<JoystickData>(); // ジョイスティックデータのリスト
    private Dictionary<JoystickData, GameObject> joystickObjects = new Dictionary<JoystickData, GameObject>();

    public FixedJoystick ActiveJoystick { get; private set; } // 現在使用中のジョイスティック

    void Start()
    {
        foreach (var joystick in joysticks)
        {
            // シーンがすでに読み込まれている場合、ジョイスティックオブジェクトを探す
            if (SceneManager.GetSceneByName(joystick.TargetSceneName).isLoaded)
            {
                FindJoystickObject(joystick);
            }
            else
            {
                // シーンを読み込み
                SceneManager.LoadSceneAsync(joystick.TargetSceneName, LoadSceneMode.Additive).completed += (operation) =>
                {
                    FindJoystickObject(joystick);
                };
            }
        }
    }

    private void FindJoystickObject(JoystickData joystick)
{
    Scene targetScene = SceneManager.GetSceneByName(joystick.TargetSceneName);

    if (targetScene.IsValid())
    {
        foreach (GameObject rootObject in targetScene.GetRootGameObjects())
        {
            if (rootObject.name == joystick.JoystickObjectName)
            {
                joystickObjects[joystick] = rootObject;
                ActiveJoystick = rootObject.GetComponent<FixedJoystick>();

                if (ActiveJoystick != null)
                {
                    Debug.Log("ジョイスティックを設定しました: " + rootObject.name);
                }
                else
                {
                    Debug.LogError("ジョイスティックのコンポーネントが見つかりません: " + rootObject.name);
                }

                return;
            }
        }

        Debug.LogWarning("指定されたジョイスティックが見つかりません: " + joystick.JoystickObjectName);
    }
    else
    {
        Debug.LogError("指定されたシーンが無効です: " + joystick.TargetSceneName);
    }
}

}
