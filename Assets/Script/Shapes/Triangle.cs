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
* Filename: Triangle
* Created:  2017/11/12 2:20:29
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class Triangle : CSGBase {
    protected override void OverRideScene() {
        PictureGenerate<Triangle>.OverWriteScene(scene);
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<Triangle>.GenLightPic();
    }

    static Result scene(float x, float y) {
        float radio = 0.1f; //去掉角，变圆
        Result c = new Result(SDFMethods.triangleSDF(x, y, 0.5f, 0.2f, 0.8f, 0.8f, 0.3f, 0.6f) - radio, 1.0f);
        return c;
    }
}