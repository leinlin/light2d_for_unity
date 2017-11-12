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
* Filename: HalfCycle
* Created:  2017/11/12 1:57:33
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class HalfCycle : CSGBase {
    protected override void OverRidePictureGen() {
        PictureGenerate<HalfCycle>.OverRideScene(scene);
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<HalfCycle>.GenLightPic();
    }

    static Result scene(float x, float y) {
        Result a = new Result(SDFMethods.circleSDF(x, y, 0.5f, 0.5f, 0.2f), 1.0f);
        Result b = new Result(SDFMethods.planeSDF(x, y, 0.0f, 0.5f, 0.0f, 1.0f), 0.8f);
        return SDFMethods.intersectOp(a, b);
    }
}