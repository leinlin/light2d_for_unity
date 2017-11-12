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
* Filename: MShape
* Created:  2017/11/12 2:56:04
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using UnityEngine;

public class MShape : CSGBase {
    protected override void OverRidePictureGen() {
        PictureGenerate<MShape>.OverRideScene(scene);
    }
    protected override Texture2D GenerateTexture() {
        return PictureGenerate<MShape>.GenLightPic();
    }

    static Result scene(float x, float y) {

        #region M I
        Result a = new Result(SDFMethods.capsuleSDF(x, y, 0.15f, 0.1f, 0.15f, 0.4f, 0.03f), 1.0f);
        Result b = new Result(SDFMethods.capsuleSDF(x, y, 0.15f, 0.1f, 0.3f, 0.4f, 0.03f), 1.0f);
        Result c = new Result(SDFMethods.capsuleSDF(x, y, 0.3f, 0.4f, 0.45f, 0.1f, 0.03f), 1.0f);
        Result d = new Result(SDFMethods.capsuleSDF(x, y, 0.45f, 0.1f, 0.45f, 0.4f, 0.03f), 1.0f);
        Result e = new Result(SDFMethods.capsuleSDF(x, y, 0.7f, 0.1f, 0.7f, 0.4f, 0.03f), 1.0f);
        #endregion

        Result r = SDFMethods.unionOp(SDFMethods.unionOp(a, b), SDFMethods.unionOp(c, d));
        r = SDFMethods.unionOp(r, e);

        #region K
        Result f = new Result(SDFMethods.capsuleSDF(x, y, 0.2f, 0.6f, 0.2f, 0.9f, 0.03f), 1.0f);
        Result g = new Result(SDFMethods.capsuleSDF(x, y, 0.2f, 0.75f, 0.4f, 0.6f, 0.03f), 1.0f);
        Result h = new Result(SDFMethods.capsuleSDF(x, y, 0.2f, 0.75f, 0.4f, 0.9f, 0.03f), 1.0f);

        Result i = new Result(SDFMethods.capsuleSDF(x, y, 0.575f, 0.6f, 0.575f, 0.9f, 0.03f), 1.0f);
        Result j = new Result(SDFMethods.capsuleSDF(x, y, 0.575f, 0.9f, 0.825f, 0.9f, 0.03f), 1.0f);
        Result k = new Result(SDFMethods.capsuleSDF(x, y, 0.825f, 0.6f, 0.825f, 0.9f, 0.03f), 1.0f);

        #endregion

        f = SDFMethods.unionOp(f, g);
        f = SDFMethods.unionOp(f, h);

        i = SDFMethods.unionOp(i, j);
        i = SDFMethods.unionOp(i, k);

        r = SDFMethods.unionOp(r, f);
        r = SDFMethods.unionOp(r, i);
        return r;
    }
}