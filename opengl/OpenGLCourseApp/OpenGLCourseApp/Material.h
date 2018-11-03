#pragma once

#include <GLEW\glew.h>

class Material
{
public:
	Material() : Material(0.f, 0.f) {};
	Material(GLfloat sIntensity, GLfloat shine);

	void UseMaterial(GLuint specularIntensityUnifLoc, GLuint shininessUnifLoc);

	~Material();

private:
	GLfloat specularIntensity;
	GLfloat shininess; // aka specular power
};

