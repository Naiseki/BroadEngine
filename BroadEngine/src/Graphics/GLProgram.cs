using System.Drawing;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics.Wgl;
using OpenTK.Mathematics;

namespace Broad.Graphics
{
    static class GLProgram
    {
        static int vbo;     //Vertex Buffer Object
        static int vao;     //Vertex Array Object
        static int ebo;     //Element Buffer Object
        static Shader shader;


        internal static void Init()
        {
            GL.ClearColor(Color4.Black);
            InitBuffers();
            InitShader();
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
        }


        internal static void Deinit()
        {
            GL.DeleteBuffers(3, new int[] {vbo, vao, ebo});
            shader.Dispose();
        }


        internal static void Render()
        {
            GL.DrawElements(PrimitiveType.Triangles, 6,  DrawElementsType.UnsignedInt, 0);
        }


        internal static void UseTexture(Texture texture)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
        }


        internal static void UseShader(Shader shader)
        {
            GL.UseProgram(shader.Handle);
        }


        static void InitBuffers()
        {
            float[] vertices =  {
                  1f,  1f,      1f, 1f,     //右上
                  1f, -1f,      1f, 0f,     //右下
                 -1f, -1f,      0f, 0f,     //左下
                 -1f,  1f,      0f, 1f      //左上
            };

            uint[] indices = {
                    0, 1, 3,
                    1, 2, 3
                };

            //頂点バッファを生成
            vbo = GL.GenBuffer();   
            //バッファをバインド                         
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);    
            //バッファにデータを登録
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);   

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);


            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }


        static void InitShader()
        {
            shader = new Shader("texShader.vert", "texShader.frag");
            shader.SetLocation("aPosition", 0);
            shader.SetLocation("aTexCoord", 2 * sizeof(float));
            Shaders.Add("textureShader", shader);

            shader = new Shader("lightShader.vert", "lightShader.frag");
            shader.InitLocation(0, 0);
            Shaders.Add("lightShader", shader);
        } 
    }
}