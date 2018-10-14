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

void ShaderProgram::CreateFromFiles(std::string vertexLocation, std::string fragmentLocation)
{
	std::string vertexString = Utils::ReadFile(vertexLocation);
	std::string fragmentString = Utils::ReadFile(fragmentLocation);

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

void ShaderProgram::UseShaderProgram()
{
	LOG("Binding shader program " << shaderProgramID);
	glUseProgram(shaderProgramID);
}

void ShaderProgram::ClearShaderProgram()
{
	if (shaderProgramID != 0)
	{
		glDeleteProgram(shaderProgramID);
		shaderProgramID = 0;
	}
	uniformProjectionID = 0;
	uniformModelID = 0;
}


ShaderProgram::~ShaderProgram()
{
	ClearShaderProgram();
}

void ShaderProgram::CompileShaders(std::string vertexCode, std::string fragmentCode)
{
	LOG("Compiling shaders");

	LOG("Creating shader program");
	shaderProgramID = glCreateProgram();
	LOG("Created shader program " << shaderProgramID);

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
	LOG("Linking shader program");
	glLinkProgram(shaderProgramID);
	if (!Utils::CheckErrors(shaderProgramID, GL_LINK_STATUS, InfoLogType::PROGRAM, "Error linking program"))
	{
		return;
	}

	// Validating the shader program
	LOG("Validating shader program");
	glValidateProgram(shaderProgramID);
	if (!Utils::CheckErrors(shaderProgramID, GL_VALIDATE_STATUS, InfoLogType::PROGRAM, "Error validating program"))
	{
		return;
	}

	uniformModelID = glGetUniformLocation(shaderProgramID, "model");
	LOG("Model uniform ID: " << uniformModelID);
	uniformProjectionID = glGetUniformLocation(shaderProgramID, "projection");
	LOG("Projection uniform ID: " << uniformProjectionID);
}

void ShaderProgram::AddShader(std::string shaderCode, GLenum shaderType)
{
	LOG("Creating shader");
	GLuint shaderID = glCreateShader(shaderType);
	LOG("Created shader " << shaderID);

	const char* shaderCodeCStr = shaderCode.c_str();

	const GLchar* code[1];
	code[0] = shaderCodeCStr;

	GLint codeLength[1];
	codeLength[0] = strlen(shaderCodeCStr);

	LOG("Attaching shader source code");
	glShaderSource(shaderID, 1, code, codeLength);
	LOG("Compiling shader code");
	glCompileShader(shaderID);

	Utils::CheckErrors(shaderID, GL_COMPILE_STATUS, InfoLogType::SHADER, "Error compiling the shader");

	LOG("Attaching shader " << shaderID << " to shader program " << shaderProgramID);
	glAttachShader(shaderProgramID, shaderID);
}
