using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_CharacterSprite : MonoBehaviour
{
    public GameObject myBody;

    private void Update()
    {
        if (myBody != null) this.transform.position = myBody.transform.position;
    }
}
