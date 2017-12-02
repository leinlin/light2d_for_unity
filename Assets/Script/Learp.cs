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
* Filename: Learp.cs
* Created:  2017/12/2 17:45:10
* Author:   To Hard The Mind
* Purpose:  
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class Learp : MonoBehaviour {
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
        tmp.Apply();

        Material mt = new Material(spriteMaterial.shader);

        mt.mainTexture = tmp;

        RenderUtil.ShowAllSprite(mt, gameObject);
    }
}