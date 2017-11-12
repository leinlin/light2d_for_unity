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
    public Result(float sd, float emissive) : this(sd, emissive, 0) {
    }
    public Result(float sd, float emissive, float reflectivity) {
        this.sd = sd;
        this.emissive = emissive;
        this.reflectivity = reflectivity;
    }
    public float sd;
    public float emissive;
    public float reflectivity;
}

public class SDFMethods {
    public delegate float trace(float ox, float oy, float dx, float dy, int depth);

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
    //第四课
    public static float planeSDF(float x, float y, float nx, float ny, float d) {
        return x * nx + y * ny + d;
    }
}

public class PictureGenerate<T> where T : CSGBase {

    private const int W = 512;
    private const int H = 512;

    private static int MAX_STEP = 10;
    private static float MAX_DISTANCE = 2.0f;
    private static float N = 64;

    private const float EPSILON = 1e-6f;
    private const float BIAS = 1e-4f;
    private const int MAX_DEPTH = 3;

    private static Func<float, float, Result> scene = defaultScene;
    private static Func<int, int, Color> getColor = defaultColor;
    private static SDFMethods.trace trace = defaultTrace;

    #region public
    public static void SetTraceStep(int maxStep, float maxDistance, int traceTime) {
        MAX_STEP = maxStep;
        MAX_DISTANCE = maxDistance;
        N = traceTime;
    }
    private static Color defaultColor(int x, int y) {
        float bright = sample((float)x / W, (float)y / H);
        bright = Mathf.Min(1, bright);

        return new Color(bright, bright, bright);
    }
    public static Color reflectColor(int x, int y) {
        Color result = default(Color);

        float nx, ny;
        gradient((float)x / W, (float)y / H, out nx, out ny);
        result.r = Mathf.Max(Mathf.Min(nx, 1.0f), -1.0f) * 0.5f + 0.5f;
        result.g = Mathf.Max(Mathf.Min(ny, 1.0f), -1.0f) * 0.5f + 0.5f;
        result.b = 0;

        return result;
    }

    public static Texture2D GenLightPic() {
        Texture2D result = new Texture2D(W, H, TextureFormat.ARGB32, false);
        // 这里因为叶老师的图片保存时从左至右，从上至下
        // 而Unity的Texture 是从左至右，从下至上,所以 Y设置的时候 索引要做特殊处理
        for (int y = 0; y < H; y++) {
            for (int x = 0; x < W; x++) {
                result.SetPixel(x, H - 1 - y, getColor(x, y));
            }
        }
        result.Apply();

        return result;
    }
    public static void SwitchColorToReflectTest() {
        getColor = reflectColor;
    }

    public static void SwitchColorToNormal() {
        getColor = defaultColor;
    }

    public static void SwitchTraceToReflect() {
        trace = relectTrace;
    }
    public static void SwitchTraceToNormal() {
        trace = defaultTrace;
    }

    public static void OverRideScene(Func<float, float, Result> newScene) {
        if (newScene != null && newScene != scene) {
            scene = newScene;
        }
    }
    private static Result defaultScene(float x, float y) {
        Result a = new Result(SDFMethods.circleSDF(x, y, 0.5f, 0.5f, 0.10f), 1.0f);
        return a;
    }
    #endregion

    private static void gradient(float x, float y, out float nx, out float ny) {
        nx = (scene(x + EPSILON, y).sd - scene(x - EPSILON, y).sd) * (0.5f / EPSILON);
        ny = (scene(x, y + EPSILON).sd - scene(x, y - EPSILON).sd) * (0.5f / EPSILON);
    }

    private static float sample(float x, float y) {
        float sum = 0.0f;
        for (int i = 0; i < N; i++) {
            //float a = TWO_PI * UnityEngine.Random.Range(0.0f, 1.0f);
            //float a = TWO_PI * i / N;                              // 分层采样
            float a = SDFMethods.TWO_PI * (i + UnityEngine.Random.Range(0.0f, 1.0f)) / N; // 抖动采样

            sum += trace(x, y, Mathf.Cos(a), Mathf.Sin(a), 0);
        }
        return sum / N;
    }

    private static float defaultTrace(float ox, float oy, float dx, float dy, int depth) {
        float t = 0.0f;
        for (int i = 0; i < MAX_STEP && t < MAX_DISTANCE; i++) {
            Result r = scene(ox + dx * t, oy + dy * t);
            if (r.sd < EPSILON)
                return r.emissive;
            t += r.sd;
        }
        return 0.0f;
    }
    public static void reflect(float ix, float iy, float nx, float ny, out float rx, out float ry) {
        float idotn2 = (ix * nx + iy * ny) * 2.0f;
        rx = ix - idotn2 * nx;
        ry = iy - idotn2 * ny;
    }
    private static float relectTrace(float ox, float oy, float dx, float dy, int depth) {
        float t = 0.0f;
        for (int i = 0; i < MAX_STEP && t < MAX_DISTANCE; i++) {
            float x = ox + dx * t, y = oy + dy * t;
            Result r = scene(x, y);
            if (r.sd < EPSILON) {
                float sum = r.emissive;
                if (depth < MAX_DEPTH && r.reflectivity > 0.0f) {
                    float nx, ny, rx, ry;
                    gradient(x, y, out nx, out ny);
                    reflect(dx, dy, nx, ny, out rx, out ry);
                    sum += r.reflectivity * trace(x + nx * BIAS, y + ny * BIAS, rx, ry, depth + 1);
                }
                return sum;
            }
            t += r.sd;
        }
        return 0.0f;
    }
}