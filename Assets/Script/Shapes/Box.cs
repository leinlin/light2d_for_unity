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
* Filename: Box
* Created:  2017/11/12 2:09:28
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class Box : CSGBase {
    protected override void OverRidePictureGen() {
        PictureGenerate<Box>.OverRideScene(scene);
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<Box>.GenLightPic();
    }

    static Result scene(float x, float y) {
        float radio = 0.1f; //去掉角，变圆
        Result c = new Result(SDFMethods.boxSDF(x, y, 0.5f, 0.5f, SDFMethods.TWO_PI / 16.0f, 0.3f, 0.1f) - radio, 1.0f);
        return c;
    }
}