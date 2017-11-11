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
* Filename: CSG.cs
* Created:  2017/11/11 13:14:13
* Author:   To Hard The Mind
* Purpose:  
* ==============================================================================
*/

using UnityEngine;

public class CSG : CSGBase {
    protected override void OverRideScene() {
        PictureGenerate<CSG>.OverWriteScene(scene);
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<CSG>.GenLightPic();
    }

    static Result scene(float x, float y) {
        Result r1 = new Result(SDFMethods.circleSDF(x, y, 0.3f, 0.3f, 0.10f), 2.0f);
        Result r2 = new Result(SDFMethods.circleSDF(x, y, 0.3f, 0.7f, 0.05f), 0.8f);
        Result r3 = new Result(SDFMethods.circleSDF(x, y, 0.7f, 0.5f, 0.10f), 0.0f);
        return SDFMethods.unionOp(SDFMethods.unionOp(r1, r2), r3);
    }

}