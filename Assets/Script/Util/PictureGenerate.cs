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
* Filename: PictureGenerate
* Created:  2017/11/11 23:50:39
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using System;
using UnityEngine;

//其实这里可以搞个对象池，优化下
public struct Result {
    public Result(float sd, float emissive) {
        this.sd = sd;
        this.emissive = emissive;
    }

    public float sd;
    public float emissive;
}

public class SDFMethods {
    public const float TWO_PI = 6.28318530718f;
    // 第一课
    public static float circleSDF(float x, float y, float cx, float cy, float r) {
        float ux = x - cx, uy = y - cy;
        return Mathf.Sqrt(ux * ux + uy * uy) - r;
    }

    // 第二课
    public static Result unionOp(Result a, Result b) {
        return a.sd < b.sd ? a : b;
    }
    public static Result intersectOp(Result a, Result b) {
        Result r = a.sd > b.sd ? b : a;
        r.sd = a.sd > b.sd ? a.sd : b.sd;
        return r;
    }
    public static Result subtractOp(Result a, Result b) {
        Result r = a;
        r.sd = (a.sd > -b.sd) ? a.sd : -b.sd;
        return r;
    }

    // 第三课
    public static float planeSDF(float x, float y, float px, float py, float nx, float ny) {
        return (x - px) * nx + (y - py) * ny;
    }
    public static float segmentSDF(float x, float y, float ax, float ay, float bx, float by) {
        float vx = x - ax, vy = y - ay, ux = bx - ax, uy = by - ay;
        float t = Mathf.Max(Mathf.Min((vx * ux + vy * uy) / (ux * ux + uy * uy), 1.0f), 0.0f);
        float dx = vx - ux * t, dy = vy - uy * t;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static float capsuleSDF(float x, float y, float ax, float ay, float bx, float by, float r) {
        return segmentSDF(x, y, ax, ay, bx, by) - r;
    }
    public static float boxSDF(float x, float y, float cx, float cy, float theta, float sx, float sy) {
        float costheta = Mathf.Cos(theta), sintheta = Mathf.Sin(theta);
        float dx = Mathf.Abs((x - cx) * costheta + (y - cy) * sintheta) - sx;
        float dy = Mathf.Abs((y - cy) * costheta - (x - cx) * sintheta) - sy;
        float ax = Mathf.Max(dx, 0.0f), ay = Mathf.Max(dy, 0.0f);
        return Mathf.Min(Mathf.Max (dx, dy), 0.0f) + Mathf.Sqrt(ax * ax + ay * ay);
    }
    public static float triangleSDF(float x, float y, float ax, float ay, float bx, float by, float cx, float cy) {
        float d = Mathf.Min(Mathf.Min(
            segmentSDF(x, y, ax, ay, bx, by),
            segmentSDF(x, y, bx, by, cx, cy)),
            segmentSDF(x, y, cx, cy, ax, ay));
        return (bx - ax) * (y - ay) > (by - ay) * (x - ax) &&
               (cx - bx) * (y - by) > (cy - by) * (x - bx) &&
               (ax - cx) * (y - cy) > (ay - cy) * (x - cx) ? -d : d;
    }
}

public class PictureGenerate<T> where T : CSGBase {

    private const int W = 512;
    private const int H = 512;

    private const int MAX_STEP = 10;
    private const float MAX_DISTANCE = 2.0f;
    private const float EPSILON = 1e-6f;
    private const float N = 64;

    private static Func<float, float, Result> scene = defaultScene;

    #region public
    public static Texture2D GenLightPic() {
        Texture2D result = new Texture2D(W, H, TextureFormat.ARGB32, false);
        // 这里因为叶老师的图片保存时从左至右，从上至下
        // 而Unity的Texture 是从左至右，从下至上,所以 Y设置的时候 索引要做特殊处理
        for (int y = 0; y < H; y++) {
            for (int x = 0; x < W; x++) {
                float bright = sample((float)x / W, (float)y / H);
                bright = Mathf.Min(1, bright);
                result.SetPixel(x, H - 1 - y, new Color(bright, bright, bright));
            }
        }
        result.Apply();

        return result;
    }
    public static void OverWriteScene(Func<float, float, Result> newScene) {
        if (newScene != null && newScene != scene) {
            scene = newScene;
        }
    }
    private static Result defaultScene(float x, float y) {
        Result a = new Result(SDFMethods.circleSDF(x, y, 0.5f, 0.5f, 0.10f), 1.0f);
        return a;
    }
    #endregion

    private static float sample(float x, float y) {
        float sum = 0.0f;
        for (int i = 0; i < N; i++) {
            //float a = TWO_PI * UnityEngine.Random.Range(0.0f, 1.0f);
            //float a = TWO_PI * i / N;                              // 分层采样
            float a = SDFMethods.TWO_PI * (i + UnityEngine.Random.Range(0.0f, 1.0f)) / N; // 抖动采样

            sum += trace(x, y, Mathf.Cos(a), Mathf.Sin(a));
        }
        return sum / N;
    }

    private static float trace(float ox, float oy, float dx, float dy) {
        float t = 0.0f;
        for (int i = 0; i < MAX_STEP && t < MAX_DISTANCE; i++) {
            Result r = scene(ox + dx * t, oy + dy * t);
            if (r.sd < EPSILON)
                return r.emissive;
            t += r.sd;
        }
        return 0.0f;
    }

}