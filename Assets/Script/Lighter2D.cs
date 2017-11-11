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
        Shader s = Shader.Find("Unlit/Texture");
        Material spriteMaterial = new Material(s);
        Texture2D t = GenLightPic();
        spriteMaterial.mainTexture = t;

        RenderUtil.ShowAllSprite(spriteMaterial, gameObject);
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
        // 这里因为叶老师的图片保存时从左至右，从上至下
        // 而Unity的Texture 是从左至右，从下至上,所以 Y设置的时候 索引要做特殊处理
        for (int y = 0; y < H; y++) {
            for (int x = 0; x < W; x++) {
                float bright = sample((float)x / W, (float)y / H);
                bright = Mathf.Min(1, bright);
                result.SetPixel(x, H - 1 - y, new Color(bright, bright, bright));
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

}