#include "stdafx.h"
#include "Material.h"

Material::Material() : Material(0.f, 0.f) {}
Material::Material(GLfloat sIntensity, GLfloat shine)
{
	specularIntensity = sIntensity;
	shininess = shine;
}

void Material::UseMaterial(GLuint specularIntensityUnifLoc, GLuint shininessUnifLoc)
{
	glUniform1f(specularIntensityUnifLoc, specularIntensity);
	glUniform1f(shininessUnifLoc, shininess);

}


Material::~Material() {}
