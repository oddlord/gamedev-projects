#include "stdafx.h"
#include "Light.h"

Light::Light() : Light(DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE,
	DEFAULT_INTENSITY, DEFAULT_D_INTENSITY) {}
Light::Light(GLfloat red, GLfloat green, GLfloat blue,
	GLfloat intensity, GLfloat dIntensity)
{
	colour = glm::vec3(red, green, blue);
	ambientIntensity = intensity;
	diffuseIntensity = dIntensity;
}


Light::~Light() {}
