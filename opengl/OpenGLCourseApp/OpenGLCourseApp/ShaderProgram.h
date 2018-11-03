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

	GLuint GetModelUnifLoc();
	GLuint GetProjectionUnifLoc();
	GLuint GetViewUnifLoc();
	GLuint GetEyePositionUnifLoc();
	GLuint GetAmbientColourUnifLoc();
	GLuint GetAmbientIntensityUnifLoc();
	GLuint GetDiffuseIntensityUnifLoc();
	GLuint GetDirectionUnifLoc();
	GLuint GetSpecularIntensityUnifLoc();
	GLuint GetShininessUnifLoc();

	void UseShaderProgram();
	static void UnbindShaderProgram();
	void ClearShaderProgram();

	~ShaderProgram();

private:
	GLuint shaderProgramID;
	GLuint modelUnifLoc;
	GLuint projectionUnifLoc;
	GLuint viewUnifLoc;
	GLuint eyePositionUnifLoc;
	GLuint ambientColourUnifLoc;
	GLuint ambientIntensityUnifLoc;
	GLuint diffuseIntensityUnifLoc;
	GLuint directionUnifLoc;
	GLuint specularIntensityUnifLoc;
	GLuint shininessUnifLoc;

	void CompileShaders(std::string vertexCode, std::string fragmentCode);
	void AddShader(std::string shaderCode, GLenum shaderType);
};

