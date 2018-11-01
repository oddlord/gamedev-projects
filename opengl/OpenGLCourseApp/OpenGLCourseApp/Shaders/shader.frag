#version 330

in vec4 vertexColour;
in vec2 texCoord;
in vec3 normal;

out vec4 colour;

struct DirectionalLight
{
	vec3 colour;
	float ambientIntensity;
	vec3 direction;
	float diffuseIntensity;
};

uniform sampler2D textureSampler;
uniform DirectionalLight directionalLight;

void main()
{
	vec4 ambientColour = vec4(directionalLight.colour, 1.f) * directionalLight.ambientIntensity;

	float diffuseFactor = max(0.f, dot(normalize(normal), normalize(directionalLight.direction)));
	vec4 diffuseColour = vec4(directionalLight.colour, 1.f) * directionalLight.diffuseIntensity * diffuseFactor;

	colour = texture(textureSampler, texCoord) * (ambientColour + diffuseColour);
}