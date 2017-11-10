using System;
using System.Collections;
using UnityEngine;

public class CamSize : MonoBehaviour {

    public float screenHeight = 1080;

    [ContextMenu("Set Root Radio")]
    void SetRootRadio() {
        Camera cam = GetComponent<Camera>();
        if (cam == null) throw new NullReferenceException("this componet must attach to a gameObject has camera component");
        float radio = 2 * cam.orthographicSize / screenHeight;
        transform.localScale = Vector3.one * radio;
    }
}
