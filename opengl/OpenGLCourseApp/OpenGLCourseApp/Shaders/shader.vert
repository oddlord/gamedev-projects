#version 330

layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 tex;

out vec4 vertexColour;
out vec2 texCoord;

uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

void main()
{
	gl_Position = projection * view * model * vec4(pos, 1.f);
	vertexColour = vec4(clamp(pos, 0.f, 1.f), 1.f);
	texCoord = tex;
}