#pragma once

#include "Light.h"

#include <GLM/gtc/type_ptr.hpp>

class PointLight :
	public Light
{
public:
	PointLight();
	PointLight(GLfloat shadowWidth, GLfloat shadowHeight,
		GLfloat red, GLfloat green, GLfloat blue,
		GLfloat intensity, GLfloat dIntensity,
		GLfloat xPos, GLfloat yPos, GLfloat zPos,
		GLfloat con, GLfloat lin, GLfloat exp);

	void UseLight(GLuint ambientColourUnifLoc, GLuint ambientIntensityUnifLoc, GLuint diffuseIntensityUnifLoc,
		GLuint positionUnifLoc, GLuint constantUnifLoc, GLuint linearUnifLoc, GLuint exponentUnifLoc);

	~PointLight();

protected:
	static constexpr GLfloat DEFAULT_X_POS = 0.f;
	static constexpr GLfloat DEFAULT_Y_POS = 0.f;
	static constexpr GLfloat DEFAULT_Z_POS = 0.f;
	static constexpr GLfloat DEFAULT_CONSTANT = 1.f;
	static constexpr GLfloat DEFAULT_LINEAR = 0.f;
	static constexpr GLfloat DEFAULT_EXPONENT = 0.f;

	glm::vec3 position;

	GLfloat constant;
	GLfloat linear;
	GLfloat exponent;
};

