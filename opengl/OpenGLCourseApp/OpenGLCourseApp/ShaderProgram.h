#pragma once

#include <filesystem>
#include <iostream>
#include <string>

#include <GLEW/glew.h>

#include "common.h"
#include "DirectionalLight.h"
#include "PointLight.h"
#include "SpotLight.h"

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

	void SetDirectionalLight(DirectionalLight* dLight);
	void SetPointLights(PointLight* pLights, unsigned int lightCount);
	void SetSpotLights(SpotLight* sLights, unsigned int lightCount);
	void SetTexture(GLuint textureUnit);
	void SetDirectionalShadowMap(GLuint textureUnit);
	void SetDirectionalLightTransform(glm::mat4* lTransform);

	void UseShaderProgram();
	static void UnbindShaderProgram();
	void ClearShaderProgram();

	~ShaderProgram();

private:
	unsigned int pointLightCount;
	unsigned int spotLightCount;

	GLuint shaderProgramID;
	GLuint modelUnifLoc;
	GLuint projectionUnifLoc;
	GLuint viewUnifLoc;
	GLuint eyePositionUnifLoc;
	GLuint specularIntensityUnifLoc;
	GLuint shininessUnifLoc;
	GLuint textureUnifLoc;
	GLuint directionalLightTransformUnifLoc;
	GLuint directionalShadowMapUnifLoc;

	struct
	{
		GLuint colourUnifLoc;
		GLuint ambientIntensityUnifLoc;
		GLuint diffuseIntensityUnifLoc;

		GLuint directionUnifLoc;
	} directionalLightUnifLocs;

	GLuint pointLightCountUnifLoc;

	struct
	{
		GLuint colourUnifLoc;
		GLuint ambientIntensityUnifLoc;
		GLuint diffuseIntensityUnifLoc;

		GLuint positionUnifLoc;
		GLuint constantUnifLoc;
		GLuint linearUnifLoc;
		GLuint exponentUnifLoc;
	} pointLightUnifLocs[MAX_POINT_LIGHTS];

	GLuint spotLightCountUnifLoc;

	struct
	{
		GLuint colourUnifLoc;
		GLuint ambientIntensityUnifLoc;
		GLuint diffuseIntensityUnifLoc;

		GLuint positionUnifLoc;
		GLuint constantUnifLoc;
		GLuint linearUnifLoc;
		GLuint exponentUnifLoc;

		GLuint directionUnifLoc;
		GLuint edgeUnifLoc;
	} spotLightUnifLocs[MAX_SPOT_LIGHTS];

	void CompileShaders(std::string vertexCode, std::string fragmentCode);
	void AddShader(std::string shaderCode, GLenum shaderType);
};

