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
* Filename: CSGSub
* Created:  2017/11/12 0:53:51
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class CSGSub : CSGBase {
    protected override void OverRideScene() {
        PictureGenerate<CSGSub>.OverWriteScene(scene);
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<CSGSub>.GenLightPic();
    }
    static Result scene(float x, float y) {
        Result a = new Result(SDFMethods.circleSDF(x, y, 0.4f, 0.5f, 0.20f), 1.0f);
        Result b = new Result(SDFMethods.circleSDF(x, y, 0.6f, 0.5f, 0.20f), 0.8f);
        return SDFMethods.subtractOp(a, b);
    }
}