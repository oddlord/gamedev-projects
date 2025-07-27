#pragma once

#include <stdio.h>
#include<GLEW\glew.h>

class ShadowMap
{
public:
	ShadowMap();

	virtual bool Init(GLuint width, GLuint height);

	virtual void Write();

	virtual void Read(GLenum textureUnit);

	GLuint GetShadowWidth();
	GLuint GetShadowHeight();

	~ShadowMap();

protected:
	GLuint FBO;
	GLuint shadowMap;

	GLuint shadowWidth;
	GLuint shadowHeight;
};

