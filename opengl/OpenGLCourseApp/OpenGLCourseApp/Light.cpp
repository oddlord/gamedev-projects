#include "stdafx.h"
#include "Light.h"

Light::Light(GLfloat red, GLfloat green, GLfloat blue, GLfloat intensity,
	GLfloat xDir, GLfloat yDir, GLfloat zDir, GLfloat dIntensity)
{
	colour = glm::vec3(red, green, blue);
	ambientIntensity = intensity;

	direction = glm::vec3(xDir, yDir, zDir);
	diffuseIntensity = dIntensity;
}

void Light::UseLight(GLuint ambientColourUnifLoc, GLuint ambientIntensityUnifLoc,
	GLuint diffuseIntensityUnifLoc, GLuint directionUnifLoc)
{
	glUniform3f(ambientColourUnifLoc, colour.x, colour.y, colour.z);
	glUniform1f(ambientIntensityUnifLoc, ambientIntensity);

	glUniform3f(directionUnifLoc, direction.x, direction.y, direction.z);
	glUniform1f(diffuseIntensityUnifLoc, diffuseIntensity);
}


Light::~Light()
{
}
