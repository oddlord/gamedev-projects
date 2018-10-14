#include "stdafx.h"

#include "Mesh.h"
#include "Utils.h"

Mesh::Mesh()
{
	_VAO = 0;
	_VBO = 0;
	_IBO = 0;
	_indexCount = 0;
}

void Mesh::CreateMesh(unsigned int numOfVertices, GLfloat vertices[], unsigned int numOfIndices, unsigned int indices[])
{
	LOG("Creating Mesh");
	_indexCount = numOfIndices;

	LOG("Creating VAO");
	glGenVertexArrays(1, &_VAO);
	LOG("Created VAO " << _VAO);
	LOG("Binding VAO " << _VAO);
	glBindVertexArray(_VAO); // bind VAO

	LOG("Creating IBO");
	glGenBuffers(1, &_IBO);
	LOG("Created IBO " << _IBO);
	LOG("Binding IBO " << _IBO);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, _IBO); // bind IBO

	glBufferData(GL_ELEMENT_ARRAY_BUFFER, numOfIndices * sizeof(indices[0]), indices, GL_STATIC_DRAW);

	LOG("Creating VBO");
	glGenBuffers(1, &_VBO);
	LOG("Created VBO " << _VBO);
	LOG("Binding VBO " << _VBO);
	glBindBuffer(GL_ARRAY_BUFFER, _VBO); // bind VBO

	glBufferData(GL_ARRAY_BUFFER, numOfVertices * sizeof(vertices[0]), vertices, GL_STATIC_DRAW);

	// first attribute: layout location index in the vertex shader
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);
	glEnableVertexAttribArray(0);

	LOG("Unbinding VBO");
	glBindBuffer(GL_ARRAY_BUFFER, 0); // unbind VBO
	LOG("Unbinding VAO");
	glBindVertexArray(0); // unbind VAO
	LOG("Unbinding IBO");
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0); // unbind IBO
}

void Mesh::RenderMesh()
{
	LOG("Rendering mesh");

	glBindVertexArray(_VAO); // bind VAO
	glDrawElements(GL_TRIANGLES, _indexCount, GL_UNSIGNED_INT, 0);
	glBindVertexArray(0); // unbind VAO
}

void Mesh::ClearMesh()
{
	if (_IBO != 0)
	{
		glDeleteBuffers(1, &_IBO);
		_IBO = 0;
	}

	if (_VBO != 0)
	{
		glDeleteBuffers(1, &_VBO);
		_VBO = 0;
	}

	if (_VAO != 0)
	{
		glDeleteVertexArrays(1, &_VAO);
		_VAO = 0;
	}

	_indexCount = 0;
}

Mesh::~Mesh()
{
	ClearMesh();
}
