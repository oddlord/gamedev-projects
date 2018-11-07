#include "stdafx.h"
#include "SpotLight.h"


SpotLight::SpotLight() : SpotLight(DEFAULT_SHADOW_WIDTH, DEFAULT_SHADOW_HEIGHT,
	DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE,
	DEFAULT_INTENSITY, DEFAULT_D_INTENSITY,
	DEFAULT_X_POS, DEFAULT_Y_POS, DEFAULT_Z_POS,
	DEFAULT_CONSTANT, DEFAULT_LINEAR, DEFAULT_EXPONENT,
	DEFAULT_X_DIR, DEFAULT_Y_DIR, DEFAULT_Z_DIR,
	DEFAULT_EDGE) {}

SpotLight::SpotLight(GLfloat shadowWidth, GLfloat shadowHeight,
	GLfloat red, GLfloat green, GLfloat blue,
	GLfloat intensity, GLfloat dIntensity,
	GLfloat xPos, GLfloat yPos, GLfloat zPos,
	GLfloat con, GLfloat lin, GLfloat exp,
	GLfloat xDir, GLfloat yDir, GLfloat zDir,
	GLfloat edg) : PointLight(shadowWidth, shadowHeight,
		red, green, blue,
		intensity, dIntensity,
		xPos, yPos, zPos,
		con, lin, exp)
{
	direction = glm::normalize(glm::vec3(xDir, yDir, zDir));

	edge = edg;
	procEdge = cosf(glm::radians(edge));
}

void SpotLight::UseLight(GLuint ambientColourUnifLoc, GLuint ambientIntensityUnifLoc, GLuint diffuseIntensityUnifLoc,
	GLuint positionUnifLoc, GLuint constantUnifLoc, GLuint linearUnifLoc, GLuint exponentUnifLoc,
	GLuint directionUnifLoc, GLuint edgeUnifLoc)
{
	glUniform3f(ambientColourUnifLoc, colour.x, colour.y, colour.z);
	glUniform1f(ambientIntensityUnifLoc, ambientIntensity);
	glUniform1f(diffuseIntensityUnifLoc, diffuseIntensity);

	glUniform3fv(positionUnifLoc, 1, glm::value_ptr(position));
	glUniform1f(constantUnifLoc, constant);
	glUniform1f(linearUnifLoc, linear);
	glUniform1f(exponentUnifLoc, exponent);

	glUniform3fv(directionUnifLoc, 1, glm::value_ptr(direction));
	glUniform1f(edgeUnifLoc, procEdge);
}

void SpotLight::SetFlash(glm::vec3 pos, glm::vec3 dir)
{
	position = pos;
	direction = dir;
}


SpotLight::~SpotLight() {}
