using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene_Controller : MonoBehaviour
{
    bool ready;
    // Start is called before the first frame update
    void Start()
    {
        ready = false;
        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            if (Input.anyKey)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        ready = true;
    }
}
