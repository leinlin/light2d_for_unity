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
* Filename: Relect1
* Created:  2017/11/12 15:47:32
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class Relect1 : CSGBase {
    protected override void OverRidePictureGen() {
        PictureGenerate<Relect1>.OverRideScene(scene);
        //PictureGenerate<Relect1>.SwitchColorToReflectTest();
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<Relect1>.GenLightPic();
    }

    static Result scene(float x, float y) {
        Result a = new Result(SDFMethods.circleSDF(x, y, 0.4f, 0.2f, 0.1f), 2.0f);
        Result b = new Result(SDFMethods.boxSDF(x, y, 0.5f, 0.8f, SDFMethods.TWO_PI / 16.0f, 0.1f, 0.1f), 0.0f);
        Result c = new Result(SDFMethods.boxSDF(x, y, 0.8f, 0.5f, SDFMethods.TWO_PI / 16.0f, 0.1f, 0.1f), 0.0f);
        return SDFMethods.unionOp(SDFMethods.unionOp(a, b), c);
    }

}