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
* Filename: Checkerboard.cs
* Created:  2017/12/2 14:45:23
* Author:   To Hard The Mind
* Purpose:  
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class Checkerboard : MonoBehaviour {
    public Material spriteMaterial;
    void Start() {
        Texture2D t = spriteMaterial.mainTexture as Texture2D;
        Texture2D tmp = new Texture2D(t.width, t.height, TextureFormat.ARGB32, true);

        for (int x = 0; x < t.width; x++) {
            for (int y = 0; y < t.height; y++) {
                int v = x + y;
                if (v % 2 == 0) {
                    tmp.SetPixel(x, y, t.GetPixel(x, y));
                }
                else {
                    tmp.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
        }
        for (int x = 0; x < t.width; x++) {
            for (int y = 0; y < t.height; y++) {
                int v = x + y;
                if (v % 2 != 0) {
                    Color l = Color.black;
                    Color r = Color.black;
                    Color u = Color.black;
                    Color d = Color.black;
                    int count = 0;
                    if (x - 1 >= 0) {
                        l = tmp.GetPixel(x - 1, y);
                        count++;
                    }
                    if (x + 1 < t.width) {
                        r = tmp.GetPixel(x + 1, y);
                        count++;
                    }
                    if (y + 1 >= 0) {
                        u = tmp.GetPixel(x, y + 1);
                        count++;
                    }
                    if (y - 1 >= 0) {
                        d = tmp.GetPixel(x, y - 1);
                        count++;
                    }
                    Color result = (l + r + u + d) / count;
                    tmp.SetPixel(x, y, result);
                }
            }
        }
        tmp.Apply();

        Material mt = new Material(spriteMaterial.shader);

        mt.mainTexture = tmp;

        RenderUtil.ShowAllSprite(mt, gameObject);
    }


}