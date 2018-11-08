#pragma once
#include "PointLight.h"
class SpotLight :
	public PointLight
{
public:
	SpotLight();
	SpotLight(GLfloat sWidth, GLfloat sHeight,
		GLfloat red, GLfloat green, GLfloat blue,
		GLfloat intensity, GLfloat dIntensity,
		GLfloat xPos, GLfloat yPos, GLfloat zPos,
		GLfloat con, GLfloat lin, GLfloat exp,
		GLfloat xDir, GLfloat yDir, GLfloat zDir,
		GLfloat edg);

	void UseLight(GLuint ambientColourUnifLoc, GLuint ambientIntensityUnifLoc,
		GLuint diffuseIntensityUnifLoc, GLuint positionUnifLoc,
		GLuint constantUnifLoc, GLuint linearUnifLoc, GLuint exponentUnifLoc,
		GLuint directionUnifLoc, GLuint edgeUnifLoc);

	void SetFlash(glm::vec3 pos, glm::vec3 dir);

	~SpotLight();

private:
	static constexpr GLfloat DEFAULT_X_DIR = 0.f;
	static constexpr GLfloat DEFAULT_Y_DIR = -1.f;
	static constexpr GLfloat DEFAULT_Z_DIR = 0.f;
	static constexpr GLfloat DEFAULT_EDGE = 0.f;

	glm::vec3 direction;

	GLfloat edge;
	GLfloat procEdge;
};

