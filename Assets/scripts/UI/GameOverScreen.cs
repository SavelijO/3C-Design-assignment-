using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text wavesSurvivedText;
    public void SetUp(int score)
    {
        gameObject.SetActive(true);
        wavesSurvivedText.text = score.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("3C");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
