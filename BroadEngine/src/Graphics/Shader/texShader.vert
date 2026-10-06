#version 330 core

layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat2 transform;
uniform vec2 translation;
uniform vec4 uvRect;


void main()
{
    gl_Position = vec4((aPosition.xy * transform) + translation, 0.0, 1.0);
    texCoord = uvRect.xy + uvRect.zw * aTexCoord;
}
