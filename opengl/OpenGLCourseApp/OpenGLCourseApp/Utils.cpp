#include "stdafx.h"

#include "Utils.h"

#include <fstream>

#include <GLM\glm.hpp>

Utils::Utils() {}

bool Utils::CheckErrors(const GLuint target, const GLenum status, const InfoLogType infoLogType, std::string message)
{
	GLint result = 0;
	GLchar eLog[1024] = { 0 };

	glGetProgramiv(target, status, &result);
	if (!result)
	{
		switch (infoLogType)
		{
		case InfoLogType::SHADER:
			glGetShaderInfoLog(target, sizeof(eLog), NULL, eLog);
			break;
		case InfoLogType::PROGRAM:
			glGetProgramInfoLog(target, sizeof(eLog), NULL, eLog);
			break;
		}

		LOGERROR(": " << eLog);
		return false;
	}

	return true;
}

std::string Utils::ReadFile(fs::path filePath)
{
	std::string content;
	std::ifstream fileStream(filePath, std::ios::in);

	if (!fileStream.is_open())
	{
		LOGERROR("Failed to read " << filePath << "! File doesn't exist.");
		return "";
	}

	std::string line;
	while (!fileStream.eof())
	{
		std::getline(fileStream, line);
		content.append(line + "\n");
	}

	fileStream.close();
	return content;
}

void Utils::calcAverageNormals(GLfloat* vertices, unsigned int verticesCount, unsigned int* indices, unsigned int indicesCount,
							   unsigned int vLength, unsigned int normalOffset)
{
	for (size_t i = 0; i < indicesCount; i+=3)
	{
		// compute the starting index of the three vertices in the "vertices" matrix
		unsigned int in0 = indices[i] * vLength;
		unsigned int in1 = indices[i + 1] * vLength;
		unsigned int in2 = indices[i + 2] * vLength;

		// compute vectors between 2nd and 1st coordinates and 3rd and 1st
		glm::vec3 v1(vertices[in1] - vertices[in0], vertices[in1 + 1] - vertices[in0 + 1], vertices[in1 + 2] - vertices[in0 + 2]);
		glm::vec3 v2(vertices[in2] - vertices[in0], vertices[in2 + 1] - vertices[in0 + 1], vertices[in2 + 2] - vertices[in0 + 2]);
		// get the vector perpendicular to these two vectors
		// i.e. calculating the cross product
		glm::vec3 normal = glm::normalize(glm::cross(v1, v2));

		// add the calculated normal to the normals of the three vertices
		// in the "vertices" matrix
		// i.e. computes the "mean" normal vector
		in0 += normalOffset;
		in1 += normalOffset;
		in2 += normalOffset;

		vertices[in0] += normal.x;
		vertices[in0 + 1] += normal.y;
		vertices[in0 + 2] += normal.z;

		vertices[in1] += normal.x;
		vertices[in1 + 1] += normal.y;
		vertices[in1 + 2] += normal.z;

		vertices[in2] += normal.x;
		vertices[in2 + 1] += normal.y;
		vertices[in2 + 2] += normal.z;
	}

	// normalise one last time after all the vectors summations
	for (size_t i = 0; i < verticesCount; i+=vLength)
	{
		unsigned int nOffset = i + normalOffset;
		glm::vec3 vec(vertices[nOffset], vertices[nOffset + 1], vertices[nOffset + 2]);
		vec = glm::normalize(vec);

		vertices[nOffset] = vec.x;
		vertices[nOffset + 1] = vec.y;
		vertices[nOffset + 2] = vec.z;
	}
}

void Utils::printMatrix(GLfloat* matrix, unsigned int rows, unsigned int cols)
{
	for (unsigned int r = 0; r < rows; r++)
	{
		for (unsigned int c = 0; c < cols - 1; c++)
		{
			std::cout << matrix[r*cols + c] << " / ";
		}
		std::cout << matrix[r*cols + (cols - 1)] << std::endl;
	}
	std::cout << std::endl;
}


Utils::~Utils() {}
