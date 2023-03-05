using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
    public void DoDisableSelf()
    {
        gameObject.SetActive(false);
    }
}
