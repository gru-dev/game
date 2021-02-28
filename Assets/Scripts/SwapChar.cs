using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapChar : MonoBehaviour
{
    public GameObject charA, charB;

    int activeChar = 1;

    // Start is called before the first frame update
    void Start()
    {
        charA.gameObject.SetActive(true);
        charB.gameObject.SetActive(false);
    }

    // Update is called once per frame
    // void FixedUpdate()
    //{

    //}

    public void Swap()
    {
        switch (activeChar)
        {
            case 1:
                activeChar = 2;
                charA.gameObject.SetActive(false);
                charB.gameObject.SetActive(true);
                break;
            case 2:
                activeChar = 1;
                charA.gameObject.SetActive(true);
                charB.gameObject.SetActive(false);
                break;
        }
    }

    public int getCharKey()
    {
        return activeChar;
    }
}
