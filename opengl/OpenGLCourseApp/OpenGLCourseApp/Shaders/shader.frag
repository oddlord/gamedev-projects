#version 330

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;
in vec4 directionalLightSpacePos;

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
uniform sampler2D directionalShadowMap;

uniform Material material;

uniform vec3 eyePosition;

float CalcDirectionalShadowFactor(DirectionalLight light)
{
	vec3 projCoords = directionalLightSpacePos.xyz / directionalLightSpacePos.w;
	// convert into values between [0, 1]
	projCoords = (projCoords * 0.5f) + 0.5f;

	float closest = texture(directionalShadowMap, projCoords.xy).r;
	float current = projCoords.z;

	// 1 = full shadow
	// 0 = no shadow
	float shadow = current > closest ? 1.f : 0.f;

	return shadow;
}

vec4 CalcLightByDirection(Light light, vec3 direction, float shadowFactor)
{
	vec4 debugColour = DEBUG_BLUE;

	vec4 ambientColour = vec4(light.colour, 1.f) * light.ambientIntensity;

	// the diffuse factor is the cos of the angle between the normal and the light direction
	// i.e. how much of that diffuse light hits this specific surface
	// parallel = angle 0 degrees = cos 1
	// perpendicular = angle 90 degrees = cos 0
	// normalise so that the dot product is = to the cos of the angle

	// when computing the dot product between a direction and a normal
	// the normal (or the direction, either of the two) has to be inverted
	// otherwise they would be facing different directions
	// i.e. the light comes towards the mesh
	// but the normal is pointing away from it
	float diffuseFactor = max(0.f, dot(normalize(normal), -normalize(direction)));
	vec4 diffuseColour = vec4(light.colour, 1.f) * light.diffuseIntensity * diffuseFactor;

	vec4 specularColor = vec4(0.f, 0.f, 0.f, 1.f);

	if (diffuseFactor > 0.f && light.diffuseIntensity > 0.f)
	{
		debugColour += DEBUG_RED;
		vec3 fragToEye = normalize(eyePosition - fragPos);
		vec3 reflectedVertex = normalize(reflect(direction, normalize(normal)));

		float specularFactor = dot(fragToEye, reflectedVertex);
		if (specularFactor > 0.f)
		{
			debugColour += DEBUG_GREEN;
			specularFactor = pow(specularFactor, material.shininess);
			specularColor = vec4(light.colour, 1.f) * material.specularIntensity * specularFactor;
		}
	}
	
	// return debugColour;
	return (ambientColour + (1.f - shadowFactor) * (diffuseColour + specularColor));
}

vec4 CalcDirectionalLight()
{
	float shadowFactor = CalcDirectionalShadowFactor(directionalLight);
	return CalcLightByDirection(directionalLight.base, directionalLight.direction, shadowFactor);
}

vec4 CalcPointLight(PointLight pLight)
{
	// vector from the point light position to the fragment
	vec3 direction = fragPos - pLight.position;
	float distance = length(direction);
	direction = normalize(direction);

	vec4 dColour = CalcLightByDirection(pLight.base, direction, 0.f);

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