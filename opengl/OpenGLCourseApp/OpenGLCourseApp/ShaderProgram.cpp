#include "stdafx.h"

#include "ShaderProgram.h"
#include "Utils.h"

ShaderProgram::ShaderProgram()
{
	shaderProgramID = 0;
	uniformProjectionID = 0;
	uniformModelID = 0;
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

GLuint ShaderProgram::GetModelLocation()
{
	return uniformModelID;
}

GLuint ShaderProgram::GetProjectionLocation()
{
	return uniformProjectionID;
}

GLuint ShaderProgram::GetViewLocation()
{
	return uniformViewID;
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

	uniformModelID = 0;
	uniformProjectionID = 0;
	uniformViewID = 0;
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

	uniformModelID = glGetUniformLocation(shaderProgramID, "model");
	uniformProjectionID = glGetUniformLocation(shaderProgramID, "projection");
	uniformViewID = glGetUniformLocation(shaderProgramID, "view");
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
