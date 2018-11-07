#pragma once

#include <GLAD\glad.h>
#include <GLM/glm.hpp>
#include <GLM\gtc\matrix_transform.hpp>

#include "ShadowMap.h"

class Light
{
public:
	Light();
	Light(GLfloat shadowWidth, GLfloat shadowHeight, 
		GLfloat red, GLfloat green, GLfloat blue,
		GLfloat intensity, GLfloat dIntensity);

	ShadowMap* GetShadowMap();

	~Light();

protected:
	static constexpr GLfloat DEFAULT_SHADOW_WIDTH = 800.f;
	static constexpr GLfloat DEFAULT_SHADOW_HEIGHT = 600.f;
	static constexpr GLfloat DEFAULT_RED = 1.f;
	static constexpr GLfloat DEFAULT_GREEN = 1.f;
	static constexpr GLfloat DEFAULT_BLUE = 1.f;
	static constexpr GLfloat DEFAULT_INTENSITY = 0.5f;
	static constexpr GLfloat DEFAULT_D_INTENSITY = 0.5f;

	glm::vec3 colour;
	GLfloat ambientIntensity;
	GLfloat diffuseIntensity;

	glm::mat4 lightProj;

	ShadowMap* shadowMap;
};

