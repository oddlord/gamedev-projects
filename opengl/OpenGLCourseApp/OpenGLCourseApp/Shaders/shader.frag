#version 330

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

out vec4 colour;

const int MAX_POINT_LIGHTS = 3;
const vec4 DEBUG_RED = vec4(1.f, 0.f, 0.f, 0.f);
const vec4 DEBUG_GREEN = vec4(0.f, 1.f, 0.f, 0.f);
const vec4 DEBUG_BLUE = vec4(0.f, 0.f, 1.f, 0.f);

struct Light
{
	vec3 colour;
	float ambientIntensity;
	float diffuseIntensity;
};

struct DirectionalLight
{
	Light base;
	vec3 direction;
};

struct PointLight
{
	Light base;
	vec3 position;
	float constant;
	float linear;
	float exponent;
};

struct Material
{
	float specularIntensity;
	float shininess;
};

uniform DirectionalLight directionalLight;

uniform int pointLightCount;
uniform PointLight pointLights[MAX_POINT_LIGHTS];

uniform sampler2D textureSampler;
uniform Material material;

uniform vec3 eyePosition;

vec4 CalcLightByDirection(Light light, vec3 direction)
{
	// when computing the dot product between a direction and a normal
	// the normal has to be inverted
	// otherwise they would be facing different directions
	// i.e. the light comes towards the mesh
	// but the normal is pointing away from it

	vec4 debugColour = DEBUG_BLUE;

	vec4 ambientColour = vec4(light.colour, 1.f) * light.ambientIntensity;

	// the diffuse factor is the cos of the angle between the normal and the light direction
	// i.e. how much of that diffuse light hits this specific surface
	// parallel = angle 0 degrees = cos 1
	// perpendicular = angle 90 degrees = cos 0
	// normalise so that the dot product is = to the cos of the angle
	float diffuseFactor = max(0.f, dot(normalize(-normal), normalize(direction)));
	vec4 diffuseColour = vec4(light.colour, 1.f) * light.diffuseIntensity * diffuseFactor;

	vec4 specularColor = vec4(0.f, 0.f, 0.f, 1.f);

	if (diffuseFactor > 0.f && light.diffuseIntensity > 0.f)
	{
		debugColour += DEBUG_RED;
		vec3 fragToEye = normalize(eyePosition - fragPos);
		vec3 reflectedVertex = normalize(reflect(direction, normalize(-normal)));

		float specularFactor = dot(fragToEye, reflectedVertex);
		if (specularFactor > 0.f)
		{
			debugColour += DEBUG_GREEN;
			specularFactor = pow(specularFactor, material.shininess);
			specularColor = vec4(light.colour, 1.f) * material.specularIntensity * specularFactor;
		}
	}
	
	// return debugColour;
	return (ambientColour + diffuseColour + specularColor);
}

vec4 CalcDirectionalLight()
{
	return CalcLightByDirection(directionalLight.base, directionalLight.direction);
}

vec4 CalcPointLights()
{
	vec4 totalColour = vec4 (0.f, 0.f, 0.f, 0.f);

	for (int i = 0; i < pointLightCount; i++)
	{
		// vector from the point light position to the fragment
		vec3 direction = fragPos - pointLights[i].position;
		float distance = length(direction);
		direction = normalize(direction);

		vec4 colourDir = CalcLightByDirection(pointLights[i].base, direction);

		// attenuation factor calculated as
		// a*d^2 + b*d + c
		// where d = distance
		// and a/b/c parameters chosen
		float attenuation = pointLights[i].exponent * distance * distance +
			pointLights[i].linear*distance +
			pointLights[i].constant;

		totalColour += colourDir / attenuation;
	}

	return totalColour;
}

void main()
{
	vec4 finalColour = CalcDirectionalLight();
	finalColour += CalcPointLights();

	colour = texture(textureSampler, texCoord) * finalColour;
	// colour = finalColour;
}