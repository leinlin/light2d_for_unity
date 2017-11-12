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
* Filename: CSGIntersect
* Created:  2017/11/12 1:11:27
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class CSGIntersect : CSGBase {
    protected override void OverRidePictureGen() {
        PictureGenerate<CSGIntersect>.OverRideScene(scene);
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<CSGIntersect>.GenLightPic();
    }

    static Result scene(float x, float y) {
        Result a = new Result(SDFMethods.circleSDF(x, y, 0.4f, 0.5f, 0.20f), 1.0f);
        Result b = new Result(SDFMethods.circleSDF(x, y, 0.6f, 0.5f, 0.20f), 0.8f);
        return SDFMethods.intersectOp(a, b);
    }
}