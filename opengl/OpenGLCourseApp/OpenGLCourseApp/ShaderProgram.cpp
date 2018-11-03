#include "stdafx.h"

#include "ShaderProgram.h"
#include "Utils.h"

ShaderProgram::ShaderProgram()
{
	shaderProgramID = 0;
	projectionUnifLoc = 0;
	modelUnifLoc = 0;
	ambientColourUnifLoc = 0;
	ambientIntensityUnifLoc = 0;
}

void ShaderProgram::CreateFromString(std::string vertexCode, std::string fragmentCode)
{
	CompileShaders(vertexCode, fragmentCode);
}

void ShaderProgram::CreateFromFiles(fs::path vertexShaderPath, fs::path fragmentShaderPath)
{
	std::string vertexString = Utils::ReadFile(vertexShaderPath);
	std::string fragmentString = Utils::ReadFile(fragmentShaderPath);

	const char* vertexCode = vertexString.c_str();
	const char* fragmentCode = fragmentString.c_str();

	CreateFromString(vertexCode, fragmentCode);
}

GLuint ShaderProgram::GetModelUnifLoc()
{
	return modelUnifLoc;
}

GLuint ShaderProgram::GetProjectionUnifLoc()
{
	return projectionUnifLoc;
}

GLuint ShaderProgram::GetViewUnifLoc()
{
	return viewUnifLoc;
}

GLuint ShaderProgram::GetEyePositionUnifLoc()
{
	return eyePositionUnifLoc;
}

GLuint ShaderProgram::GetAmbientColourUnifLoc()
{
	return ambientColourUnifLoc;
}

GLuint ShaderProgram::GetAmbientIntensityUnifLoc()
{
	return ambientIntensityUnifLoc;
}

GLuint ShaderProgram::GetDiffuseIntensityUnifLoc()
{
	return diffuseIntensityUnifLoc;
}

GLuint ShaderProgram::GetDirectionUnifLoc()
{
	return directionUnifLoc;
}

GLuint ShaderProgram::GetSpecularIntensityUnifLoc()
{
	return specularIntensityUnifLoc;
}

GLuint ShaderProgram::GetShininessUnifLoc()
{
	return shininessUnifLoc;
}

void ShaderProgram::UseShaderProgram()
{
	glUseProgram(shaderProgramID);
}

void ShaderProgram::UnbindShaderProgram()
{
	glUseProgram(0);
}

void ShaderProgram::ClearShaderProgram()
{
	if (shaderProgramID != 0)
	{
		glDeleteProgram(shaderProgramID);
		shaderProgramID = 0;
	}

	modelUnifLoc = 0;
	projectionUnifLoc = 0;
	viewUnifLoc = 0;
	ambientColourUnifLoc = 0;
	ambientIntensityUnifLoc = 0;
}


ShaderProgram::~ShaderProgram()
{
	ClearShaderProgram();
}

void ShaderProgram::CompileShaders(std::string vertexCode, std::string fragmentCode)
{
	shaderProgramID = glCreateProgram();

	if (!shaderProgramID)
	{
		LOGERROR("Error creating shader program!");
		return;
	}

	AddShader(vertexCode, GL_VERTEX_SHADER);
	AddShader(fragmentCode, GL_FRAGMENT_SHADER);

	GLint result = 0;
	GLchar eLog[1024] = { 0 };

	// Linking the shader program
	glLinkProgram(shaderProgramID);
	if (!Utils::CheckErrors(shaderProgramID, GL_LINK_STATUS, InfoLogType::PROGRAM, "Error linking program"))
	{
		return;
	}

	// Validating the shader program
	glValidateProgram(shaderProgramID);
	if (!Utils::CheckErrors(shaderProgramID, GL_VALIDATE_STATUS, InfoLogType::PROGRAM, "Error validating program"))
	{
		return;
	}

	modelUnifLoc = glGetUniformLocation(shaderProgramID, "model");
	projectionUnifLoc = glGetUniformLocation(shaderProgramID, "projection");
	viewUnifLoc = glGetUniformLocation(shaderProgramID, "view");
	eyePositionUnifLoc = glGetUniformLocation(shaderProgramID, "eyePosition");
	ambientColourUnifLoc = glGetUniformLocation(shaderProgramID, "directionalLight.colour");
	ambientIntensityUnifLoc = glGetUniformLocation(shaderProgramID, "directionalLight.ambientIntensity");
	directionUnifLoc = glGetUniformLocation(shaderProgramID, "directionalLight.direction");
	diffuseIntensityUnifLoc = glGetUniformLocation(shaderProgramID, "directionalLight.diffuseIntensity");
	specularIntensityUnifLoc = glGetUniformLocation(shaderProgramID, "material.specularIntensity");
	shininessUnifLoc = glGetUniformLocation(shaderProgramID, "material.shininess");
}

void ShaderProgram::AddShader(std::string shaderCode, GLenum shaderType)
{
	GLuint shaderID = glCreateShader(shaderType);

	const char* shaderCodeCStr = shaderCode.c_str();

	const GLchar* code[1];
	code[0] = shaderCodeCStr;

	GLint codeLength[1];
	codeLength[0] = strlen(shaderCodeCStr);

	glShaderSource(shaderID, 1, code, codeLength);
	glCompileShader(shaderID);

	Utils::CheckErrors(shaderID, GL_COMPILE_STATUS, InfoLogType::SHADER, "Error compiling the shader");

	glAttachShader(shaderProgramID, shaderID);
}
