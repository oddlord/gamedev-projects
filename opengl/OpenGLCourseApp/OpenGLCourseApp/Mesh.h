#pragma once

#include <GLEW/glew.h>

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
	GLuint _VAO; // Vertex Array Object
	GLuint _VBO; // Vertex Buffer Object
	GLuint _IBO; // Index Buffer Object
	GLsizei _indexCount;
};

