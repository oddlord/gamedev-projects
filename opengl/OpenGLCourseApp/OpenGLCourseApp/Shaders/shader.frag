#version 330

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

out vec4 colour;

struct DirectionalLight
{
	vec3 colour;
	float ambientIntensity;
	vec3 direction;
	float diffuseIntensity;
};

struct Material
{
	float specularIntensity;
	float shininess;
};

uniform sampler2D textureSampler;
uniform DirectionalLight directionalLight;
uniform Material material;

uniform vec3 eyePosition;

void main()
{
	vec4 debugRed = vec4(1.f, 0.f, 0.f, 1.f);
	vec4 debugGreen = vec4(0.f, 1.f, 0.f, 1.f);
	vec4 debugBlue = vec4(0.f, 0.f, 1.f, 1.f);
	vec4 debugColour = debugBlue;

	vec4 ambientColour = vec4(directionalLight.colour, 1.f) * directionalLight.ambientIntensity;

	// the diffuse factor is the cos of the angle between the normal and the light direction
	// i.e. how much of that diffuse light hits this specific surface
	// parallel = angle 0 degrees = cos 1
	// perpendicular = angle 90 degrees = cos 0
	// normalise so that the dot product is = to the cos of the angle
	float diffuseFactor = max(0.f, dot(normalize(normal), normalize(directionalLight.direction)));
	vec4 diffuseColour = vec4(directionalLight.colour, 1.f) * directionalLight.diffuseIntensity * diffuseFactor;

	vec4 specularColor = vec4(0.f, 0.f, 0.f, 1.f);

	if (diffuseFactor > 0.f)
	{
		debugColour += debugRed;
		vec3 fragToEye = normalize(eyePosition - fragPos);
		vec3 reflectedVertex = normalize(reflect(directionalLight.direction, normalize(normal)));

		float specularFactor = dot(fragToEye, reflectedVertex);
		if (specularFactor > 0.f)
		{
			debugColour += debugGreen;
			specularFactor = pow(specularFactor, material.shininess);
			specularColor = vec4(directionalLight.colour, 1.f) * material.specularIntensity * specularFactor;
		}
	}

	colour = texture(textureSampler, texCoord) * (ambientColour + diffuseColour + specularColor);
	// colour = debugColour;
}