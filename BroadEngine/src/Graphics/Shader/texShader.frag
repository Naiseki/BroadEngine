#version 330 core

out vec4 fragColor;

in vec2 texCoord;
uniform vec4 mulColor;
uniform vec4 addColor;

uniform sampler2D texture1;

void main()
{
    vec4 texColor = texture(texture1, texCoord);
    if (texColor.a < 0.0001)
        discard;
    
    fragColor = texColor * mulColor + addColor;
}