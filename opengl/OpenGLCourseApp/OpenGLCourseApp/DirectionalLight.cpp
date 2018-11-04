#include "stdafx.h"
#include "DirectionalLight.h"

DirectionalLight::DirectionalLight() : DirectionalLight(DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE,
	DEFAULT_INTENSITY, DEFAULT_D_INTENSITY,
	DEFAULT_X_DIR, DEFAULT_Y_DIR, DEFAULT_Z_DIR) {}
DirectionalLight::DirectionalLight(GLfloat red, GLfloat green, GLfloat blue,
	GLfloat intensity, GLfloat dIntensity,
	GLfloat xDir, GLfloat yDir, GLfloat zDir) : Light(red, green, blue,
		intensity, dIntensity)
{
	direction = glm::vec3(xDir, yDir, zDir);
}

void DirectionalLight::UseLight(GLuint ambientColourUnifLoc, GLuint ambientIntensityUnifLoc,
	GLuint diffuseIntensityUnifLoc, GLuint directionUnifLoc)
{
	glUniform3f(ambientColourUnifLoc, colour.x, colour.y, colour.z);
	glUniform1f(ambientIntensityUnifLoc, ambientIntensity);
	glUniform1f(diffuseIntensityUnifLoc, diffuseIntensity);

	glUniform3f(directionUnifLoc, direction.x, direction.y, direction.z);
}


DirectionalLight::~DirectionalLight() {}
