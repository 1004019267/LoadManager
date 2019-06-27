/**
 *Copyright(C) 2019 by #COMPANY#
 *All rights reserved.
 *FileName:     #SCRIPTFULLNAME#
 *Author:       #AUTHOR#
 *Version:      #VERSION#
 *UnityVersion:#UNITYVERSION#
 *Date:         #DATE#
 *Description:   
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
            LoadManager.Instance.Back();
        });
    }
}
