#pragma once

#include <GLAD\glad.h>

class Mesh
{
public:
	Mesh();

	void CreateMesh(unsigned int numOfVertices, GLfloat vertices[], unsigned int numOfIndices, unsigned int indices[],
		unsigned int vLength, unsigned int uvOffset, unsigned int normalOffset);
	void RenderMesh();
	void ClearMesh();

	~Mesh();

private:
	GLuint vao; // Vertex Array Object
	GLuint vbo; // Vertex Buffer Object
	GLuint ibo; // Index Buffer Object
	GLsizei indexCount;
};

