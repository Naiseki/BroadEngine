#version 330 core

out vec4 outputColorF;

in vec2 resolution;
in float lightDistance;


float distSqr(vec2 a, vec2 b)
{
    float dx = a.x - b.x;
    float dy = a.y - b.y;
    return dx * dx + dy * dy;
}


void main()
{
    float a = distance(gl_FragCoord.xy / resolution, vec2(0.5, 0.5)) / lightDistance;
    outputColorF = vec4(0.0, 0.0, 0.0, a);
}