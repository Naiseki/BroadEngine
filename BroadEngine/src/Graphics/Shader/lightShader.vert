#version 330 core

layout(location = 0) in vec3 Position;

uniform vec2 u_resolution;
uniform float u_lightDistance;

out vec2 resolution;
out float lightDistance;

void main(void)
{   
    resolution = u_resolution;
    lightDistance = u_lightDistance;
    gl_Position = vec4(Position, 1.0);
}
