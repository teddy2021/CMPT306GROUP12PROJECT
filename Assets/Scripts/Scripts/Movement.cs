using UnityEngine;

public class Movement : MonoBehaviour
{

    public float MovementSpeed;

    public int walkDistance = 200;
    private int currPos = 0;
    private bool rightDirection = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 

    }

    void FixedUpdate()
    {
        // swap walk direction if goes passed specified distance 'walkDistance'
        if (currPos > walkDistance)
        {
            rightDirection = false;
        }
        if (currPos < 0)
        {
            rightDirection = true;
        }

        // move sprite
        if (rightDirection)
        {
            transform.position += new Vector3(1, 0, 0) * Time.fixedDeltaTime* MovementSpeed;
            currPos += 1;
        }
        else
        {
            transform.position += new Vector3(-1, 0, 0) * Time.fixedDeltaTime * MovementSpeed;
            currPos -= 1;
        }
    }
}
