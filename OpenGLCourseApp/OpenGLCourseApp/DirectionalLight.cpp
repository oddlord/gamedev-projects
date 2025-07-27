#include "stdafx.h"
#include "DirectionalLight.h"

DirectionalLight::DirectionalLight() : DirectionalLight(DEFAULT_SHADOW_WIDTH, DEFAULT_SHADOW_HEIGHT,
	DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE,
	DEFAULT_INTENSITY, DEFAULT_D_INTENSITY,
	DEFAULT_X_DIR, DEFAULT_Y_DIR, DEFAULT_Z_DIR) {}
DirectionalLight::DirectionalLight(GLfloat sWidth, GLfloat sHeight,
	GLfloat red, GLfloat green, GLfloat blue,
	GLfloat intensity, GLfloat dIntensity,
	GLfloat xDir, GLfloat yDir, GLfloat zDir) : Light(sWidth, sHeight,
		red, green, blue,
		intensity, dIntensity)
{
	direction = glm::vec3(xDir, yDir, zDir);
	lightProj = glm::ortho(-5.f, 5.f, -5.f, 5.f, 0.1f, 20.f);
}

void DirectionalLight::UseLight(GLuint ambientColourUnifLoc, GLuint ambientIntensityUnifLoc,
	GLuint diffuseIntensityUnifLoc, GLuint directionUnifLoc)
{
	glUniform3f(ambientColourUnifLoc, colour.x, colour.y, colour.z);
	glUniform1f(ambientIntensityUnifLoc, ambientIntensity);
	glUniform1f(diffuseIntensityUnifLoc, diffuseIntensity);

	glUniform3f(directionUnifLoc, direction.x, direction.y, direction.z);
}

glm::mat4 DirectionalLight::CalculateLightTransform()
{
	return lightProj * glm::lookAt(-direction, glm::vec3(0.f, 0.f, 0.f), glm::vec3(0.f, 1.f, 0.f));
}


DirectionalLight::~DirectionalLight() {}
