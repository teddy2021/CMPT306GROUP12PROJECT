using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSourceUnit : MonoBehaviour
{
    public bool power = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        power = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if (power)
		{
            animator.SetBool("Power", true);
		}
		else
		{
            animator.SetBool("Power", false);
        }
    }
}
