#pragma once

#include <GLAD\glad.h>

class Material
{
public:
	Material();
	Material(GLfloat sIntensity, GLfloat shine);

	void UseMaterial(GLuint specularIntensityUnifLoc, GLuint shininessUnifLoc);

	~Material();

private:
	GLfloat specularIntensity;
	GLfloat shininess; // aka specular power
};

