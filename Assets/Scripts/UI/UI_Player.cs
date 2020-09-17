using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
	public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Stand() {
		if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Stand") {
			animator.Play("Stand");
		}
	}

	public void Move() {
		if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Move") {
			animator.Play("Move");
		}
	}
}
