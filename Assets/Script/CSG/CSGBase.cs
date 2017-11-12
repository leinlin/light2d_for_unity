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
* Filename: CSGBase
* Created:  2017/11/12 0:55:12
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public abstract class CSGBase : MonoBehaviour {
    private void Awake() {
        OverRidePictureGen();
    }
    void Start() {
        Shader s = Shader.Find("Unlit/Texture");
        Material spriteMaterial = new Material(s);
        Texture2D t = GenerateTexture();
        spriteMaterial.mainTexture = t;

        RenderUtil.ShowAllSprite(spriteMaterial, gameObject);
    }
    protected abstract void OverRidePictureGen();
    protected abstract Texture2D GenerateTexture();
}