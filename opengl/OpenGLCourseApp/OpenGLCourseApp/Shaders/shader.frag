#version 330

in vec4 vertexColour;
in vec2 texCoord;

out vec4 colour;

struct DirectionalLight
{
	vec3 colour;
	float ambientIntensity;
};

uniform sampler2D textureSampler;
uniform DirectionalLight directionalLight;

void main()
{
	vec4 ambientColour = vec4(directionalLight.colour, 1.f) * directionalLight.ambientIntensity;

	colour = texture(textureSampler, texCoord) * ambientColour;
}