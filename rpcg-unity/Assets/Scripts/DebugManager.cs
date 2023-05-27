using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{

    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftControl)) return;

        DebugDrawCard();
        DebugPause();
        DebugRestart();
    }

    private void DebugDrawCard()
    {
        if (!Input.GetKeyDown(KeyCode.D)) return;
        BattleManager.Instance.DrawCard();
    }

    private void DebugPause()
    {
        if (!Input.GetKeyDown(KeyCode.P)) return;
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }

    private void DebugRestart()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
