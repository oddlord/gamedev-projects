#pragma once

#include "Light.h"

class DirectionalLight :
	public Light
{
public:
	DirectionalLight();
	DirectionalLight(GLfloat shadowWidth, GLfloat shadowHeight, 
		GLfloat red, GLfloat green, GLfloat blue,
		GLfloat intensity, GLfloat dIntensity,
		GLfloat xDir, GLfloat yDir, GLfloat zDir);

	void UseLight(GLuint ambientColourUnifLoc, GLuint ambientIntensityUnifLoc,
		GLuint diffuseIntensityUnifLoc, GLuint directionUnifLoc);
	
	glm::mat4 CalculateLightTransform();

	~DirectionalLight();

private:
	static constexpr GLfloat DEFAULT_X_DIR = 0.f;
	static constexpr GLfloat DEFAULT_Y_DIR = -1.f;
	static constexpr GLfloat DEFAULT_Z_DIR = 0.f;

	glm::vec3 direction;
};

