using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour {

    #region Singleton
    public static playerManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    public GameObject player;
    public static int candyleft=5;
    public static int HousedUnicron = 0;
}
