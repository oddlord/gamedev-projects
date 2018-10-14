#pragma once

#include <iostream>
#include <string>

#include <GLEW/glew.h>

class ShaderProgram
{
public:
	ShaderProgram();

	void CreateFromString(std::string vertexCode, std::string fragmentCode);
	void CreateFromFiles(std::string vertexLocation, std::string fragmentLocation);

	GLuint GetModelLocation();
	GLuint GetProjectionLocation();

	void UseShaderProgram();
	void ClearShaderProgram();

	~ShaderProgram();

private:
	GLuint shaderProgramID;
	GLuint uniformModelID;
	GLuint uniformProjectionID;

	void CompileShaders(std::string vertexCode, std::string fragmentCode);
	void AddShader(std::string shaderCode, GLenum shaderType);
};

