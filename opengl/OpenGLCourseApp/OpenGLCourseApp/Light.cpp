#include "stdafx.h"
#include "Light.h"

Light::Light(GLfloat red, GLfloat green, GLfloat blue, GLfloat intensity)
{
	colour = glm::vec3(red, green, blue);
	ambientIntensity = intensity;
}

void Light::UseLight(GLuint ambientColourID, GLuint ambientIntensityID)
{
	glUniform3f(ambientColourID, colour.x, colour.y, colour.z);
	glUniform1f(ambientIntensityID, ambientIntensity);
}


Light::~Light()
{
}
