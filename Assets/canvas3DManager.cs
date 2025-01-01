using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常にカメラの方を向くオブジェクト回転をカメラに固定
/// </summary>
public class canvas3DManager : MonoBehaviour {
	void LateUpdate() {
		// 回転をカメラと同期させる
		transform.rotation = Camera.main.transform.rotation;
	}
}
