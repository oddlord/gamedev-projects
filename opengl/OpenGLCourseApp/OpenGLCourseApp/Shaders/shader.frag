#version 330

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

out vec4 colour;

const int MAX_POINT_LIGHTS = 3;
const int MAX_SPOT_LIGHTS = 3;

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

struct SpotLight
{
	PointLight base;
	vec3 direction;
	float edge;
};

struct Material
{
	float specularIntensity;
	float shininess;
};

uniform DirectionalLight directionalLight;

uniform int pointLightCount;
uniform PointLight pointLights[MAX_POINT_LIGHTS];

uniform int spotLightCount;
uniform SpotLight spotLights[MAX_SPOT_LIGHTS];

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

vec4 CalcPointLight(PointLight pLight)
{
	// vector from the point light position to the fragment
	vec3 direction = fragPos - pLight.position;
	float distance = length(direction);
	direction = normalize(direction);

	vec4 dColour = CalcLightByDirection(pLight.base, direction);

	// attenuation factor calculated as
	// a*d^2 + b*d + c
	// where d = distance
	// and a/b/c parameters chosen
	float attenuation = pLight.exponent * distance * distance +
		pLight.linear*distance +
		pLight.constant;

	return (dColour / attenuation);
}

vec4 CalcPointLights()
{
	vec4 totalColour = vec4 (0.f, 0.f, 0.f, 0.f);

	for (int i = 0; i < pointLightCount; i++)
	{
		totalColour += CalcPointLight(pointLights[i]);
	}

	return totalColour;
}

vec4 CalcSpotLight(SpotLight sLight)
{
	// vector from the point light position to the fragment
	vec3 rayDirection = normalize(fragPos - sLight.base.position);
	// angle between the vector between the light and the fragment and the spotlight direction
	float slFactor = dot(rayDirection, sLight.direction);

	if (slFactor > sLight.edge)
	{
		vec4 pColour = CalcPointLight(sLight.base);
		// this is to scale (blur) between the center and the edge
		pColour = pColour * (1.f - (1.f - slFactor)*(1.f/(1.f - sLight.edge)));

		return pColour;
	}
	else
	{
		return vec4(0.f, 0.f, 0.f, 0.f);
	}
}

vec4 CalcSpotLights()
{
	vec4 totalColour = vec4 (0.f, 0.f, 0.f, 0.f);

	for (int i = 0; i < spotLightCount; i++)
	{
		totalColour += CalcSpotLight(spotLights[i]);
	}

	return totalColour;
}

void main()
{
	vec4 finalColour = CalcDirectionalLight();
	finalColour += CalcPointLights();
	finalColour += CalcSpotLights();

	colour = texture(textureSampler, texCoord) * finalColour;
	// colour = finalColour;
}