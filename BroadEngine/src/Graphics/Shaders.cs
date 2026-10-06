using System;
using System.IO;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using MathKit;

namespace Broad.Graphics
{
    static class Shaders
    {
        static Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();


        internal static void Add(string shaderMame, Shader shader)
        {
            shaders.Add(shaderMame, shader);
        }


        internal static Shader Get(string name)
        {
            return shaders[name];
        }
    }
}