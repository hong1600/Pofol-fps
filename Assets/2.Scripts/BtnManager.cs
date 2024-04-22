using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject optionUI;
    [SerializeField] Sprite volumeOn;
    [SerializeField] Sprite volumeOff;

    public void clickStart()
    {
        SceneManager.LoadScene(1);
    }

    public void clickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void clickContinue() 
    {

    }

    public void clickOption()
    {
        inventoryUI.SetActive(false);
        optionUI.SetActive(true);
    }

    public void clickExitMain()
    {
        SceneManager.LoadScene(0);
    }

    public void clickBack()
    {
        optionUI.SetActive(false);
        inventoryUI.SetActive(true);
    }

}
