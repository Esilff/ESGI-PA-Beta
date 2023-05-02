using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtil
{
    public static void ToMenu(GameObject firstMenu, GameObject secondMenu)
    {
        firstMenu.SetActive(false);
        secondMenu.SetActive(true);
    }

    
}
