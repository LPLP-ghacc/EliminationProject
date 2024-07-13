using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public string cameraModel = "AXON BODY 3 XAK3DW"; 
    public TextMeshProUGUI topRightBox;

    private void Update()
    {
        var readTime = DateTime.Now;

        topRightBox.text = readTime.ToString() + " -0500\n" + cameraModel;
    }
}
