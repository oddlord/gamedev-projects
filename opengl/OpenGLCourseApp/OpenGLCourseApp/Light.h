#pragma once

#include <GLEW/glew.h>
#include <GLM/glm.hpp>

class Light
{
public:
	Light() : Light(1.f, 1.f, 1.f, 1.f) {};
	Light(GLfloat red, GLfloat green, GLfloat blue, GLfloat intensity);

	void UseLight(GLuint ambientColourID, GLuint ambientIntensityID);

	~Light();

private:
	glm::vec3 colour;
	GLfloat ambientIntensity;
};

