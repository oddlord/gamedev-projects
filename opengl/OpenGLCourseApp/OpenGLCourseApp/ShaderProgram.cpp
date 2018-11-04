#include "stdafx.h"

#include "ShaderProgram.h"
#include "Utils.h"

ShaderProgram::ShaderProgram()
{
	shaderProgramID = 0;
	projectionUnifLoc = 0;
	modelUnifLoc = 0;

	pointLightCount = 0;
	spotLightCount = 0;
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
	return directionalLightUnifLocs.colourUnifLoc;
}

GLuint ShaderProgram::GetAmbientIntensityUnifLoc()
{
	return directionalLightUnifLocs.ambientIntensityUnifLoc;
}

GLuint ShaderProgram::GetDiffuseIntensityUnifLoc()
{
	return directionalLightUnifLocs.diffuseIntensityUnifLoc;
}

GLuint ShaderProgram::GetDirectionUnifLoc()
{
	return directionalLightUnifLocs.directionUnifLoc;
}

GLuint ShaderProgram::GetSpecularIntensityUnifLoc()
{
	return specularIntensityUnifLoc;
}

GLuint ShaderProgram::GetShininessUnifLoc()
{
	return shininessUnifLoc;
}

void ShaderProgram::SetDirectionalLight(DirectionalLight* dLight)
{
	dLight->UseLight(directionalLightUnifLocs.colourUnifLoc, directionalLightUnifLocs.ambientIntensityUnifLoc,
		directionalLightUnifLocs.diffuseIntensityUnifLoc, directionalLightUnifLocs.directionUnifLoc);
}

void ShaderProgram::SetPointLights(PointLight* pLights, unsigned int lightCount)
{
	if (lightCount > MAX_POINT_LIGHTS)
	{
		lightCount = MAX_POINT_LIGHTS;
	}

	glUniform1i(pointLightCountUnifLoc, lightCount);

	for (size_t i = 0; i < lightCount; i++)
	{
		pLights[i].UseLight(pointLightUnifLocs[i].colourUnifLoc, pointLightUnifLocs[i].ambientIntensityUnifLoc, pointLightUnifLocs[i].diffuseIntensityUnifLoc,
			pointLightUnifLocs[i].positionUnifLoc, pointLightUnifLocs[i].constantUnifLoc, pointLightUnifLocs[i].linearUnifLoc, pointLightUnifLocs[i].exponentUnifLoc);
	}
}

void ShaderProgram::SetSpotLights(SpotLight* sLights, unsigned int lightCount)
{
	if (lightCount > MAX_SPOT_LIGHTS)
	{
		lightCount = MAX_SPOT_LIGHTS;
	}

	glUniform1i(spotLightCountUnifLoc, lightCount);

	for (size_t i = 0; i < lightCount; i++)
	{
		sLights[i].UseLight(spotLightUnifLocs[i].colourUnifLoc, spotLightUnifLocs[i].ambientIntensityUnifLoc, spotLightUnifLocs[i].diffuseIntensityUnifLoc,
			spotLightUnifLocs[i].positionUnifLoc, spotLightUnifLocs[i].constantUnifLoc, spotLightUnifLocs[i].linearUnifLoc, spotLightUnifLocs[i].exponentUnifLoc,
			spotLightUnifLocs[i].directionUnifLoc, spotLightUnifLocs[i].edgeUnifLoc);
	}
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
	directionalLightUnifLocs.colourUnifLoc = glGetUniformLocation(shaderProgramID, "directionalLight.base.colour");
	directionalLightUnifLocs.ambientIntensityUnifLoc = glGetUniformLocation(shaderProgramID, "directionalLight.base.ambientIntensity");
	directionalLightUnifLocs.diffuseIntensityUnifLoc = glGetUniformLocation(shaderProgramID, "directionalLight.base.diffuseIntensity");
	directionalLightUnifLocs.directionUnifLoc = glGetUniformLocation(shaderProgramID, "directionalLight.direction");
	specularIntensityUnifLoc = glGetUniformLocation(shaderProgramID, "material.specularIntensity");
	shininessUnifLoc = glGetUniformLocation(shaderProgramID, "material.shininess");

	pointLightCountUnifLoc = glGetUniformLocation(shaderProgramID, "pointLightCount");
	for (size_t i = 0; i < MAX_POINT_LIGHTS; i++)
	{
		char locBuff[100] = { '\0' };

		snprintf(locBuff, sizeof(locBuff), "pointLights[%d].base.colour", i);
		pointLightUnifLocs[i].colourUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "pointLights[%d].base.ambientIntensity", i);
		pointLightUnifLocs[i].ambientIntensityUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "pointLights[%d].base.diffuseIntensity", i);
		pointLightUnifLocs[i].diffuseIntensityUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "pointLights[%d].position", i);
		pointLightUnifLocs[i].positionUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "pointLights[%d].constant", i);
		pointLightUnifLocs[i].constantUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "pointLights[%d].linear", i);
		pointLightUnifLocs[i].linearUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "pointLights[%d].exponent", i);
		pointLightUnifLocs[i].exponentUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);
	}

	spotLightCountUnifLoc = glGetUniformLocation(shaderProgramID, "spotLightCount");
	for (size_t i = 0; i < MAX_SPOT_LIGHTS; i++)
	{
		char locBuff[100] = { '\0' };

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].base.base.colour", i);
		spotLightUnifLocs[i].colourUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].base.base.ambientIntensity", i);
		spotLightUnifLocs[i].ambientIntensityUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].base.base.diffuseIntensity", i);
		spotLightUnifLocs[i].diffuseIntensityUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].base.position", i);
		spotLightUnifLocs[i].positionUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].base.constant", i);
		spotLightUnifLocs[i].constantUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].base.linear", i);
		spotLightUnifLocs[i].linearUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].base.exponent", i);
		spotLightUnifLocs[i].exponentUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].direction", i);
		spotLightUnifLocs[i].directionUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);

		snprintf(locBuff, sizeof(locBuff), "spotLights[%d].edge", i);
		spotLightUnifLocs[i].edgeUnifLoc = glGetUniformLocation(shaderProgramID, locBuff);
	}
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
