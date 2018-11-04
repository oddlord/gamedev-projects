#pragma once

#include <GLEW/glew.h>
#include <GLM/glm.hpp>

class Light
{
public:
	Light();
	Light(GLfloat red, GLfloat green, GLfloat blue,
		GLfloat intensity, GLfloat dIntensity);

	~Light();

protected:
	static constexpr GLfloat DEFAULT_RED = 1.f;
	static constexpr GLfloat DEFAULT_GREEN = 1.f;
	static constexpr GLfloat DEFAULT_BLUE = 1.f;
	static constexpr GLfloat DEFAULT_INTENSITY = 0.5f;
	static constexpr GLfloat DEFAULT_D_INTENSITY = 0.5f;

	glm::vec3 colour;
	GLfloat ambientIntensity;
	GLfloat diffuseIntensity;
};

