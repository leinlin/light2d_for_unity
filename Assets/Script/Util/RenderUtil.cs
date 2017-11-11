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
* Filename: RenderUtil.cs
* Created:  2017/11/11 15:52:52
* Author:   To Hard The Mind
* Purpose:  
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public static class RenderUtil {
    public static void ShowAllSprite(Material spriteMaterial, GameObject gameObject) {
        Texture t = spriteMaterial.mainTexture;

        //获取图片的像素宽高//  
        int pixelHeight = t.height;
        int pixelWidth = t.width;

        //得到MeshFilter对象//  
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = null;
        if (meshFilter == null) {
            //为null时，自动添加//  
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = spriteMaterial;
        }
        //得到对应的网格对象//  
        Mesh mesh = meshFilter.mesh;
        //三角形顶点的坐标数组//  
        Vector3[] vertices = new Vector3[4];
        //三角形顶点数组//  
        int[] triangles = new int[6];


        float glWidth = pixelWidth / 2;
        float glHeight = pixelHeight / 2;
        //以当前对象的中心坐标为标准//  
        vertices[0] = new Vector3(-glWidth, -glHeight, 0);
        vertices[1] = new Vector3(-glWidth, glHeight, 0);
        vertices[2] = new Vector3(glWidth, -glHeight, 0);
        vertices[3] = new Vector3(glWidth, glHeight, 0);

        mesh.vertices = vertices;
        //绑定顶点顺序//  
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.triangles = triangles;

        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };
    }
}