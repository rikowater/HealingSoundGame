using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject canvas; // キャンバス全体のGameObject
    public GameObject[] panels; // 4つのパネルを配列で管理
    private int currentPanelIndex = 0; // 現在表示中のパネルのインデックス

    private void Start()
    {
        // キャンバスの表示制御
        int showCanvas = PlayerPrefs.GetInt("ShowCanvas", 0);
        if (showCanvas == 1)
        {
            // キャンバスを表示し、最初のパネルを初期化
            canvas.SetActive(true);
            PlayerPrefs.SetInt("ShowCanvas", 0); // フラグをリセット
            PlayerPrefs.Save();

            InitializePanels();
        }
        else
        {
            // キャンバスを非表示
            canvas.SetActive(false);
        }
    }

    private void InitializePanels()
    {
        // 全てのパネルを非表示にし、最初のパネルを表示
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        if (panels.Length > 0)
        {
            panels[0].SetActive(true);
        }
    }

    public void OnPanelClick()
    {
        if (currentPanelIndex < panels.Length)
        {
            // 現在のパネルを非表示にする
            panels[currentPanelIndex].SetActive(false);

            // 次のパネルを表示（ただし最後のパネルの場合は何もしない）
            currentPanelIndex++;
            if (currentPanelIndex < panels.Length)
            {
                panels[currentPanelIndex].SetActive(true);
            }
        }
    }
}
