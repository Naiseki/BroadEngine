using System.Reflection.Metadata;
using System.Security.Cryptography;
using System;
using System.IO;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using MathKit;

namespace Broad.Graphics
{
    class Shader: IDisposable
    {
        public readonly int Handle;
        private readonly Dictionary<string, int> _uniformLocations;
        bool disposed = false;


        public Shader(string vertName, string fragName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using Stream vertStream = assembly.GetManifestResourceStream(vertName);
            using Stream fragStream = assembly.GetManifestResourceStream(fragName);

            using var vertReader = new StreamReader(vertStream);
            var shaderSource = vertReader.ReadToEnd();
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, shaderSource);
            CompileShader(vertexShader);

            using var fragReader = new StreamReader(fragStream);
            shaderSource = fragReader.ReadToEnd();
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            _uniformLocations = new Dictionary<string, int>();

            for (int i = 0; i < numberOfUniforms; i++) {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }
        }

        private static void CompileShader(int shader)
        {
            // Try to compile the shader
            GL.CompileShader(shader);

            // Check for compilation errors
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }


        private static void InitLocations()
        {
            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            var texCoordLocation = 1;
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        }


        public void SetLocation(string locationName, int offset)
        {
            int location = GL.GetAttribLocation(Handle, locationName);
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), offset);
        }


        private static void LinkProgram(int program)
        {
            // We link the program
            GL.LinkProgram(program);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        // A wrapper function that enables the shader program.
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
        // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }


        public void InitLocation(int location, int offset)
        {
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), offset);
        }

        // Uniform setters
        // Uniforms are variables that can be set by user code, instead of reading them from the VBO.
        // You use VBOs for vertex-related data, and uniforms for almost everything else.

        // Setting a uniform is almost always the exact same, so I'll explain it here once, instead of in every method:
        //     1. Bind the program you want to set the uniform on
        //     2. Get a handle to the location of the uniform with GL.GetUniformLocation.
        //     3. Use the appropriate GL.Uniform* function to set the uniform.

        /// <summary>
        /// Set a uniform int on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform float on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        /// <remarks>
        ///   <para>
        ///   The matrix is transposed before being sent to the shader.
        ///   </para>
        /// </remarks>
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }


        public void SetMatrix3(string name, Float3x3 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix3(_uniformLocations[name], 9, true, ref data.C0.x);
        }


        public void SetMatrix2(string name, Float2x2 data)
        {
            GL.UseProgram(Handle);
            var mat = data.ToMatrix2();
            GL.UniformMatrix2(_uniformLocations[name], true, ref mat);
        }


        public void SetMatrix3(string name, Matrix3 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix3(_uniformLocations[name], true, ref data);
        }

        /// <summary>
        /// Set a uniform Vector3 on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetVec3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }

        public void SetVec2(string name, Float2 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform2(_uniformLocations[name], data.x, data.y);
        }

        public void SetVec4(string name, Float4 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform4(_uniformLocations[name], data.x, data.y, data.z, data.w);
        }


        public void SetVec4(string name, ColorF data)
        {
            GL.UseProgram(Handle);
            GL.Uniform4(_uniformLocations[name], data.R, data.G, data.B, data.A);
        }


        public void Dispose()
        {
            if (disposed)
                return;
            GL.DeleteProgram(Handle);
            disposed = true;
        }
    }
}