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

void Light::UseLight(GLuint ambientColourID, GLuint ambientIntensityID,
					 GLfloat diffuseIntensityID, GLfloat directionID)
{
	glUniform3f(ambientColourID, colour.x, colour.y, colour.z);
	glUniform1f(ambientIntensityID, ambientIntensity);

	glUniform3f(directionID, direction.x, direction.y, direction.z);
	glUniform1f(diffuseIntensityID, diffuseIntensity);
}


Light::~Light()
{
}
