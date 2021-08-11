using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_CharacterSprite : MonoBehaviour
{
    public GameObject myBody;
    private void Start()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
    private void Update()
    {
        if (myBody != null) this.transform.position = myBody.transform.position;
    }
}
