#pragma once

#include <GLEW/glew.h>
#include <GLM/glm.hpp>

class Light
{
public:
	Light() : Light(1.f, 1.f, 1.f, 1.f, 0.f, -1.f, 0.f, 0.f) {};
	Light(GLfloat red, GLfloat green, GLfloat blue, GLfloat intensity,
		  GLfloat xDir, GLfloat yDir, GLfloat zDir, GLfloat dIntensity);

	void UseLight(GLuint ambientColourID, GLuint ambientIntensityID,
				  GLfloat diffuseIntensityID, GLfloat directionID);

	~Light();

private:
	glm::vec3 colour;
	GLfloat ambientIntensity;

	glm::vec3 direction;
	GLfloat diffuseIntensity;
};

