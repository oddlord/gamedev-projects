#pragma once

#include <filesystem>
#include <iostream>
#include <string>

#include <GLEW/glew.h>

namespace fs = std::experimental::filesystem;

class ShaderProgram
{
public:
	ShaderProgram();

	void CreateFromString(std::string vertexCode, std::string fragmentCode);
	void CreateFromFiles(fs::path vertexShaderPath, fs::path fragmentShaderPath);

	GLuint GetModelLocation();
	GLuint GetProjectionLocation();
	GLuint GetViewLocation();

	void UseShaderProgram();
	static void UnbindShaderProgram();
	void ClearShaderProgram();

	~ShaderProgram();

private:
	GLuint shaderProgramID;
	GLuint uniformModelID;
	GLuint uniformProjectionID;
	GLuint uniformViewID;

	void CompileShaders(std::string vertexCode, std::string fragmentCode);
	void AddShader(std::string shaderCode, GLenum shaderType);
};

