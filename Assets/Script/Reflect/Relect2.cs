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
* Filename: Relect2
* Created:  2017/11/12 16:19:14
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class Relect2 : CSGBase {
    protected override void OverRidePictureGen() {
        PictureGenerate<Relect2>.OverRideScene(scene);
        PictureGenerate<Relect2>.SwitchTraceToReflect();
        PictureGenerate<Relect2>.SetTraceStep(64, 5.0f, 128);
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<Relect2>.GenLightPic();
    }
    static Result scene(float x, float y) {
        Result a = new Result(SDFMethods.circleSDF(x, y, 0.4f, 0.2f, 0.1f), 2.0f, 0.0f);
        Result d = new Result(SDFMethods.planeSDF(x, y, 0.0f, -1.0f, 0.5f), 0.0f, 0.9f);
        Result e = new Result(SDFMethods.circleSDF(x, y, 0.5f, 0.5f, 0.4f), 0.0f, 0.9f);
        return SDFMethods.unionOp(a, SDFMethods.subtractOp(d, e));

        //Result a = new Result(SDFMethods.circleSDF(x, y, 0.4f, 0.2f, 0.1f), 2.0f, 0.0f);
        //Result b = new Result(SDFMethods.boxSDF(x, y, 0.5f, 0.8f, SDFMethods.TWO_PI / 16.0f, 0.1f, 0.1f), 0.0f, 0.9f);
        //Result c = new Result(SDFMethods.boxSDF(x, y, 0.8f, 0.5f, SDFMethods.TWO_PI / 16.0f, 0.1f, 0.1f), 0.0f, 0.9f);
        //return SDFMethods.unionOp(SDFMethods.unionOp(a, b), c);
    }

}