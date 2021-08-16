using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene_Controller : MonoBehaviour
{
    public bool Tutorials;
    public GameObject Toggle;

    public void Start_Game()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void ReadTutorialMessageToggle()
    {
        Tutorials = (Toggle.GetComponent<Toggle>().isOn);
        GameObject.FindGameObjectWithTag("Pref").GetComponent<Pref_Shuttle>().Tutorials = Tutorials;
    }
}
