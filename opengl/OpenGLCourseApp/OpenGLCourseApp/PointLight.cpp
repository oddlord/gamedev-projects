#include "stdafx.h"
#include "PointLight.h"

#include <GLM/gtc/type_ptr.hpp>

PointLight::PointLight() : PointLight(DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE,
	DEFAULT_INTENSITY, DEFAULT_D_INTENSITY,
	DEFAULT_X_POS, DEFAULT_Y_POS, DEFAULT_Z_POS,
	DEFAULT_CONSTANT, DEFAULT_LINEAR, DEFAULT_EXPONENT) {}

PointLight::PointLight(GLfloat red, GLfloat green, GLfloat blue,
	GLfloat intensity, GLfloat dIntensity,
	GLfloat xPos, GLfloat yPos, GLfloat zPos,
	GLfloat con, GLfloat lin, GLfloat exp) : Light(red, green, blue,
		intensity, dIntensity)
{
	position = glm::vec3(xPos, yPos, zPos);
	constant = con;
	linear = lin;
	exponent = exp;
}

void PointLight::UseLight(GLuint ambientColourUnifLoc, GLuint ambientIntensityUnifLoc,
	GLuint diffuseIntensityUnifLoc, GLuint positionUnifLoc,
	GLuint constantUnifLoc, GLuint linearUnifLoc, GLuint exponentUnifLoc)
{
	glUniform3f(ambientColourUnifLoc, colour.x, colour.y, colour.z);
	glUniform1f(ambientIntensityUnifLoc, ambientIntensity);
	glUniform1f(diffuseIntensityUnifLoc, diffuseIntensity);

	glUniform3fv(positionUnifLoc, 1, glm::value_ptr(position));
	glUniform1f(constantUnifLoc, constant);
	glUniform1f(linearUnifLoc, linear);
	glUniform1f(exponentUnifLoc, exponent);
}


PointLight::~PointLight()
{
}
