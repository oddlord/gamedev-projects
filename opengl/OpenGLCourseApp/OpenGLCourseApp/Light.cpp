#include "stdafx.h"
#include "Light.h"

Light::Light() : Light(DEFAULT_SHADOW_WIDTH, DEFAULT_SHADOW_HEIGHT,
	DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE,
	DEFAULT_INTENSITY, DEFAULT_D_INTENSITY) {}
Light::Light(GLfloat shadowWidth, GLfloat shadowHeight,
	GLfloat red, GLfloat green, GLfloat blue,
	GLfloat intensity, GLfloat dIntensity)
{
	shadowMap = new ShadowMap();
	shadowMap->Init(shadowWidth, shadowHeight);

	colour = glm::vec3(red, green, blue);
	ambientIntensity = intensity;
	diffuseIntensity = dIntensity;
}

ShadowMap * Light::GetShadowMap()
{
	return shadowMap;
}


Light::~Light() {}
