#pragma once

#include <GLEW/glew.h>
#include <GLM/glm.hpp>
#include <GLM\gtc\matrix_transform.hpp>

#include "ShadowMap.h"

class Light
{
public:
	Light();
	Light(GLfloat sWidth, GLfloat sHeight,
		GLfloat red, GLfloat green, GLfloat blue,
		GLfloat intensity, GLfloat dIntensity);

	ShadowMap* GetShadowMap();

	~Light();

protected:
	static constexpr GLfloat DEFAULT_SHADOW_WIDTH = 1024.f;
	static constexpr GLfloat DEFAULT_SHADOW_HEIGHT = 1024.f;
	static constexpr GLfloat DEFAULT_RED = 1.f;
	static constexpr GLfloat DEFAULT_GREEN = 1.f;
	static constexpr GLfloat DEFAULT_BLUE = 1.f;
	static constexpr GLfloat DEFAULT_INTENSITY = 0.5f;
	static constexpr GLfloat DEFAULT_D_INTENSITY = 0.5f;

	glm::vec3 colour;
	GLfloat ambientIntensity;
	GLfloat diffuseIntensity;

	glm::mat4 lightProj;

private:
	ShadowMap* shadowMap;
	GLfloat shadowWidth;
	GLfloat shadowHeight;
	bool isShadowMapInit;

	void InitShadowMap();
};

