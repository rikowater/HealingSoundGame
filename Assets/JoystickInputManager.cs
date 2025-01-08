using UnityEngine;

public class JoystickInputManager : MonoBehaviour
{
    public static JoystickInputManager Instance;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public FixedJoystick inputMove;

    private void Awake()
    {
        // シングルトンパターンを実装
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);  // シーン変更後もオブジェクトを破棄しない
        }
        else
        {
            Destroy(gameObject); // 他のインスタンスがあれば破棄
        }
    }

    void Update()
    {
        // ジョイスティックの入力を取得
        Horizontal = inputMove.Horizontal;
        Vertical = inputMove.Vertical;
    }

}
