#pragma once

#include <GLEW/glew.h>

class Mesh
{
public:
	Mesh();

	void CreateMesh(unsigned int numOfVertices, GLfloat vertices[], unsigned int numOfIndices, unsigned int indices[]);
	void RenderMesh();
	void ClearMesh();

	~Mesh();

private:
	GLuint _VAO; // Vertex Array Object
	GLuint _VBO; // Vertex Buffer Object
	GLuint _IBO; // Index Buffer Object
	GLsizei _indexCount;
};

