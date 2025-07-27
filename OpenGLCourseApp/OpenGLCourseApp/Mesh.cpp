#include "stdafx.h"

#include "Mesh.h"
#include "Utils.h"

Mesh::Mesh()
{
	vao = 0;
	vbo = 0;
	ibo = 0;
	indexCount = 0;
}

void Mesh::CreateMesh(unsigned int numOfVertices, GLfloat vertices[], unsigned int numOfIndices, unsigned int indices[],
	unsigned int vLength, unsigned int uvOffset, unsigned int normalOffset)
{
	indexCount = numOfIndices;

	glGenVertexArrays(1, &vao);
	glBindVertexArray(vao); // bind VAO

	glGenBuffers(1, &ibo);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ibo); // bind IBO

	glBufferData(GL_ELEMENT_ARRAY_BUFFER, numOfIndices * sizeof(indices[0]), indices, GL_STATIC_DRAW);

	glGenBuffers(1, &vbo);
	glBindBuffer(GL_ARRAY_BUFFER, vbo); // bind VBO

	glBufferData(GL_ARRAY_BUFFER, numOfVertices * sizeof(vertices[0]), vertices, GL_STATIC_DRAW);

	// first attribute: layout location index in the vertex shader
	// Setting vertices x/y/z coordinates
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(vertices[0]) * vLength, 0);
	glEnableVertexAttribArray(0);

	// Setting texture u/v coordinates
	glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, sizeof(vertices[0]) * vLength, (void*)(sizeof(vertices[0]) * uvOffset));
	glEnableVertexAttribArray(1);

	// Setting normal direction
	glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, sizeof(vertices[0]) * vLength, (void*)(sizeof(vertices[0]) * normalOffset));
	glEnableVertexAttribArray(2);

	glBindBuffer(GL_ARRAY_BUFFER, 0); // unbind VBO
	glBindVertexArray(0); // unbind VAO
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0); // unbind IBO
}

void Mesh::RenderMesh()
{
	glBindVertexArray(vao); // bind VAO
	glDrawElements(GL_TRIANGLES, indexCount, GL_UNSIGNED_INT, 0);
	glBindVertexArray(0); // unbind VAO
}

void Mesh::ClearMesh()
{
	if (ibo != 0)
	{
		glDeleteBuffers(1, &ibo);
		ibo = 0;
	}

	if (vbo != 0)
	{
		glDeleteBuffers(1, &vbo);
		vbo = 0;
	}

	if (vao != 0)
	{
		glDeleteVertexArrays(1, &vao);
		vao = 0;
	}

	indexCount = 0;
}

Mesh::~Mesh()
{
	ClearMesh();
}
