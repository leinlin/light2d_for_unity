/*
               #########                       
              ############                     
              #############                    
             ##  ###########                   
            ###  ###### #####                  
            ### #######   ####                 
           ###  ########## ####                
          ####  ########### ####               
         ####   ###########  #####             
        #####   ### ########   #####           
       #####   ###   ########   ######         
      ######   ###  ###########   ######       
     ######   #### ##############  ######      
    #######  #####################  ######     
    #######  ######################  ######    
   #######  ###### #################  ######   
   #######  ###### ###### #########   ######   
   #######    ##  ######   ######     ######   
   #######        ######    #####     #####    
    ######        #####     #####     ####     
     #####        ####      #####     ###      
      #####       ###        ###      #        
        ###       ###        ###               
         ##       ###        ###               
__________#_______####_______####______________

                我们的未来没有BUG              
* ==============================================================================
* Filename: Lighter2D.cs
* Created:  2017/11/10 9:05:37
* Author:   To Hard The Mind
* Purpose:  
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Lighter2D : MonoBehaviour {

    #region member
    private int m_verticesCount = 4;
    private Material spriteMaterial;
    private const int W = 512;
    private const int H = 512;

    private const int MAX_STEP = 10;

    private const float MAX_DISTANCE = 2.0f;
    private const float EPSILON = 1e-6f;
    private const float TWO_PI = 6.28318530718f;
    private const float N = 64;
    #endregion

    // Use this for initialization
    void Start() {
        initSprite();
    }

    #region light2d
    float sample(float x, float y) {
        float sum = 0.0f;
        for (int i = 0; i < N; i++) {
            //float a = TWO_PI * UnityEngine.Random.Range(0.0f, 1.0f);
            //float a = TWO_PI * i / N;                              // 分层采样
            float a = TWO_PI * (i + UnityEngine.Random.Range(0.0f, 1.0f)) / N; // 抖动采样

            sum += trace(x, y, Mathf.Cos(a), Mathf.Sin(a));
        }
        return sum / N;
    }

    float trace(float ox, float oy, float dx, float dy) {
        float t = 0.0f;
        for (int i = 0; i < MAX_STEP && t < MAX_DISTANCE; i++) {
            float sd = circleSDF(ox + dx * t, oy + dy * t, 0.5f, 0.5f, 0.1f);
            if (sd < EPSILON)
                return 2.0f;
            t += sd;
        }
        return 0.0f;
    }

    private Texture2D GenLightPic() {
        Texture2D result = new Texture2D(W, H, TextureFormat.ARGB32, false);
        for (int x = 0; x < W; x++) {
            for (int y = 0; y < H; y++) {
                float bright = sample((float)x / W, (float)y / H);
                bright = Mathf.Min(1, bright);
                result.SetPixel(x, y, new Color(bright, bright, bright));
            }
        }
        result.Apply();

        return result;
    }

    float circleSDF(float x, float y, float cx, float cy, float r) {
        float ux = x - cx, uy = y - cy;
        return Mathf.Sqrt(ux * ux + uy * uy) - r;
    }
    #endregion

    #region show
    //根据宽高生成对应的面//  
    private void initSprite() {
        Shader s = Shader.Find("Unlit/Texture");
        spriteMaterial = new Material(s);
        Texture2D t = GenLightPic();
        spriteMaterial.mainTexture = t;

        //获取图片的像素宽高//  
        int pixelHeight = t.height;
        int pixelWidth = t.width;

        Debug.Log("pixeW:" + pixelWidth + ",pixeH:" + pixelHeight);

        //得到MeshFilter对象//  
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = null;
        if (meshFilter == null) {
            //为null时，自动添加//  
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = spriteMaterial;
        }
        //得到对应的网格对象//  
        Mesh mesh = meshFilter.mesh;
        //三角形顶点的坐标数组//  
        Vector3[] vertices = new Vector3[m_verticesCount];
        //三角形顶点数组//  
        int[] triangles = new int[m_verticesCount * 3];


        float glWidth = pixelWidth / 2;
        float glHeight = pixelHeight / 2;
        //以当前对象的中心坐标为标准//  
        vertices[0] = new Vector3(-glWidth, -glHeight, 0);
        vertices[1] = new Vector3(-glWidth, glHeight, 0);
        vertices[2] = new Vector3(glWidth, -glHeight, 0);
        vertices[3] = new Vector3(glWidth, glHeight, 0);

        mesh.vertices = vertices;
        //绑定顶点顺序//  
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.triangles = triangles;

        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };
    }
    #endregion

}