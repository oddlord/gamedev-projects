// S5L31 1:14:16

#include "stdafx.h"

#define STB_IMAGE_IMPLEMENTATION

#include "Camera.h"
#include "common.h"
#include "DirectionalLight.h"
#include "Material.h"
#include "Mesh.h"
#include "Model.h"
#include "PointLight.h"
#include "ShaderProgram.h"
#include "SpotLight.h"
#include "Texture.h"
#include "Utils.h"
#include "Window.h"

#include <cmath>
#include <filesystem>
#include <iostream>
#include <string>
#include <vector>

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <GLM/glm.hpp>
#include <GLM/gtc/matrix_transform.hpp>
#include <GLM/gtc/type_ptr.hpp>

namespace fs = std::experimental::filesystem;

// Log enabled/disabled
bool logEnabled = true;

// Window dimensions
const GLint WIDTH = 1024;
const GLint HEIGHT = 768;

const GLfloat SHADOW_WIDTH = 1024;
const GLfloat SHADOW_HEIGHT = 1024;

Window mainWindow;

std::vector<ShaderProgram*> shaderProgramList;
ShaderProgram* directionalShadowShader;

Camera camera;

GLfloat deltaTime = 0.f;
GLfloat lastTime = 0.f;

const float toRadians = 3.14159265f / 180.f;

GLuint modelUnifLoc = 0;
GLuint projectionUnifLoc = 0;
GLuint viewUnifLoc = 0;
GLuint eyePosUnifLoc = 0;
GLuint specularIntensityUnifLoc = 0;
GLuint shininessUnifLoc = 0;

// Shaders
fs::path shadersFolder("Shaders");
// Vertex shader
fs::path vShaderFile("shader.vert");
fs::path vShaderPath = shadersFolder / vShaderFile;
// Fragment shader
fs::path fShaderFile("shader.frag");
fs::path fShaderPath = shadersFolder / fShaderFile;
// Directional shadow map vertex shader
fs::path vDirShadowMapShaderFile("directional_shadow_map.vert");
fs::path vDirShadowMapShaderPath = shadersFolder / vShaderFile;
// Directional shadow map fragment shader
fs::path fDirShadowMapShaderFile("directional_shadow_map.frag");
fs::path fDirShadowMapShaderPath = shadersFolder / fShaderFile;

// Textures
fs::path texturesFolder("Textures");
// Brick texture
fs::path brickTextureFile("brick_red_4104_5283_Small.jpg");
fs::path brickTexturePath = texturesFolder / brickTextureFile;
Texture brickTexture(brickTexturePath, false);
// Dirt texture
fs::path dirtTextureFile("ground_dirt_3299_9359_Small.jpg");
fs::path dirtTexturePath = texturesFolder / dirtTextureFile;
Texture dirtTexture(dirtTexturePath, false);
// Wood texture
fs::path woodTextureFile("wood_plain_210_251_Small.jpg");
fs::path woodTexturePath = texturesFolder / woodTextureFile;
Texture woodTexture(woodTexturePath, false);
// White texture
fs::path plainTextureFile("plain.jpg");
fs::path plainTexturePath = texturesFolder / plainTextureFile;
Texture plainTexture(plainTexturePath, false);

DirectionalLight mainLight;
PointLight pointLights[MAX_POINT_LIGHTS];
SpotLight spotLights[MAX_SPOT_LIGHTS];

unsigned int pointLightCount = 0;
unsigned int spotLightCount = 0;

Material shinyMaterial;
Material dullMaterial;

Mesh pyramidMesh1;
Mesh pyramidMesh2;
Mesh pyramidMesh3;
Mesh floorMesh;

Model xwing;
Model blackhawk;

Mesh createFloor()
{
	GLfloat vertices[] = {
		//x		y		z			u		v			nx		ny		nz
		-10.f,	0.f,	10.f,		10.f,	10.f,		0.f,	0.f,	0.f,
		10.f,	0.f,	10.f,		0.f,	10.f,		0.f,	0.f,	0.f,
		10.f,	0.f,	-10.f,		0.f,	0.f,		0.f,	0.f,	0.f,
		-10.f,	0.f,	-10.f,		10.f,	0.f,		0.f,	0.f,	0.f
	};

	unsigned int indices[] = {
		1, 2, 0,
		3, 0, 2
	};

	const unsigned int numOfVertices = sizeof(vertices) / sizeof(vertices[0]);
	const unsigned int numOfIndices = sizeof(indices) / sizeof(indices[0]);
	const unsigned int vLength = 8;
	const unsigned int uvOffset = 3;
	const unsigned int normalOffset = 5;

	Utils::calcAverageNormals(vertices, numOfVertices, indices, numOfIndices, vLength, normalOffset);

	Mesh floorMesh = Mesh();
	floorMesh.CreateMesh(numOfVertices, vertices, numOfIndices, indices, vLength, uvOffset, normalOffset);

	return floorMesh;
}

Mesh CreatePyramid()
{
	GLfloat vertices[] = {
		//x		y		z			u		v			nx		ny		nz
		-1.f,	-1.f,	1.f,		0.f,	0.f,		0.f,	0.f,	0.f,
		1.f,	-1.f,	1.f,		1.f,	0.f,		0.f,	0.f,	0.f,
		1.f,	-1.f,	-1.f,		1.f,	1.f,		0.f,	0.f,	0.f,
		-1.f,	-1.f,	-1.f,		0.f,	1.f,		0.f,	0.f,	0.f,
		0.f,	0.8f,	0.f,		0.5f,	0.5f,		0.f,	0.f,	0.f
	};

	// NOTE: the order of each indices triplet counts!
	// glm::cross implements the right-hand rule
	// so we have to choose the order correctly, in order to have the correct normal orientation
	// check Utils::calcAverageNormals for the use of glm::cross
	// https://en.wikipedia.org/wiki/Right-hand_rule
	unsigned int indices[] = {
		4, 0, 1,
		4, 1, 2,
		4, 2, 3, 
		4, 3, 0,
		1, 0, 2,
		3, 2, 0
	};

	const unsigned int numOfVertices = sizeof(vertices) / sizeof(vertices[0]);
	const unsigned int numOfIndices = sizeof(indices) / sizeof(indices[0]);
	const unsigned int vLength = 8;
	const unsigned int uvOffset = 3;
	const unsigned int normalOffset = 5;

	Utils::calcAverageNormals(vertices, numOfVertices, indices, numOfIndices, vLength, normalOffset);

	Mesh pyramidMesh = Mesh();
	pyramidMesh.CreateMesh(numOfVertices, vertices, numOfIndices, indices, vLength, uvOffset, normalOffset);

	return pyramidMesh;
}

void CreateShaderProgram()
{
	ShaderProgram* shaderProgram = new ShaderProgram();
	shaderProgram->CreateFromFiles(vShaderPath, fShaderPath);
	shaderProgramList.push_back(shaderProgram);

	directionalShadowShader = new ShaderProgram();
	directionalShadowShader->CreateFromFiles(vDirShadowMapShaderPath, fDirShadowMapShaderPath);
}

void RenderScene()
{
	glm::mat4 model(1.f);
	model = glm::translate(model, glm::vec3(0.f, -0.3f, -5.f));
	glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(model));
	dirtTexture.UseTexture();
	dullMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
	pyramidMesh1.RenderMesh();

	model = glm::mat4(1.f);
	model = glm::translate(model, glm::vec3(0.f, 2.f, -5.f));
	glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(model));
	brickTexture.UseTexture();
	dullMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
	pyramidMesh2.RenderMesh();

	model = glm::mat4(1.f);
	model = glm::translate(model, glm::vec3(0.f, -0.3f, -8.f));
	glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(model));
	woodTexture.UseTexture();
	shinyMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
	pyramidMesh3.RenderMesh();

	model = glm::mat4(1.f);
	model = glm::translate(model, glm::vec3(0.f, -2.f, -5.f));
	glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(model));
	dirtTexture.UseTexture();
	shinyMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
	floorMesh.RenderMesh();

	model = glm::mat4(1.f);
	model = glm::translate(model, glm::vec3(-7.f, 0.f, 10.f));
	model = glm::scale(model, glm::vec3(0.006f, 0.006f, 0.006f));
	glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(model));
	shinyMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
	xwing.RenderModel();

	model = glm::mat4(1.f);
	model = glm::translate(model, glm::vec3(-3.f, 2.f, -5.f));
	model = glm::rotate(model, -90.f * toRadians, glm::vec3(1.f, 0.f, 0.f));
	model = glm::scale(model, glm::vec3(0.4f, 0.4f, 0.4f));
	glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(model));
	shinyMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
	blackhawk.RenderModel();
}

void DirectionalShadowMapPass(DirectionalLight* light)
{
	directionalShadowShader->UseShaderProgram();

	glViewport(0, 0, light->GetShadowMap()->GetShadowWidth(), light->GetShadowMap()->GetShadowHeight());

	light->GetShadowMap()->Write();
	glClear(GL_DEPTH_BUFFER_BIT);

	modelUnifLoc = directionalShadowShader->GetModelUnifLoc();
	directionalShadowShader->SetDirectionalLightTransform(&light->CalculateLightTransform());

	RenderScene();

	glBindFramebuffer(GL_FRAMEBUFFER, 0);
}

void RenderPass(glm::mat4 projectionMatrix, glm::mat4 viewMatrix)
{
	shaderProgramList[0]->UseShaderProgram(); // bind shader program

	modelUnifLoc = shaderProgramList[0]->GetModelUnifLoc();
	projectionUnifLoc = shaderProgramList[0]->GetProjectionUnifLoc();
	viewUnifLoc = shaderProgramList[0]->GetViewUnifLoc();
	eyePosUnifLoc = shaderProgramList[0]->GetEyePositionUnifLoc();
	specularIntensityUnifLoc = shaderProgramList[0]->GetSpecularIntensityUnifLoc();
	shininessUnifLoc = shaderProgramList[0]->GetShininessUnifLoc();

	glViewport(0, 0, WIDTH, HEIGHT);

	// Clear window
	glClearColor(0.f, 0.f, 0.f, 1.f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glUniformMatrix4fv(projectionUnifLoc, 1, GL_FALSE, glm::value_ptr(projectionMatrix));
	glUniformMatrix4fv(viewUnifLoc, 1, GL_FALSE, glm::value_ptr(viewMatrix));
	glUniform3fv(eyePosUnifLoc, 1, glm::value_ptr(camera.getCameraPosition()));

	shaderProgramList[0]->SetDirectionalLight(&mainLight);
	shaderProgramList[0]->SetPointLights(pointLights, pointLightCount);
	shaderProgramList[0]->SetSpotLights(spotLights, spotLightCount);
	shaderProgramList[0]->SetDirectionalLightTransform(&mainLight.CalculateLightTransform());

	mainLight.GetShadowMap()->Read(GL_TEXTURE1);
	shaderProgramList[0]->SetTexture(0);
	shaderProgramList[0]->SetDirectionalShadowMap(1);

	glm::vec3 lowerFlashPos = camera.getCameraPosition();
	lowerFlashPos.y -= 0.3f;
	// spotLights[0].SetFlash(lowerFlashPos, camera.getCameraDirection());

	RenderScene();
}

int main()
{
	mainWindow = Window(WIDTH, HEIGHT);
	mainWindow.Initialise();

	Mesh pyramidMesh1 = CreatePyramid();
	Mesh pyramidMesh2 = CreatePyramid();
	Mesh pyramidMesh3 = CreatePyramid();
	Mesh floorMesh = createFloor();

	CreateShaderProgram();

	camera = Camera(glm::vec3(0.f, 0.f, 0.f), glm::vec3(0.f, 1.f, 0.f), -90.f, 0.f, 8.f, 0.5f);

	brickTexture.LoadTexture();
	dirtTexture.LoadTexture();
	woodTexture.LoadTexture();
	plainTexture.LoadTexture();

	shinyMaterial = Material(4.f, 256.f);
	dullMaterial = Material(0.3f, 4);

	xwing = Model();
	xwing.LoadModel("Models/x-wing.obj");

	blackhawk = Model();
	blackhawk.LoadModel("Models/uh60.obj");

	mainLight = DirectionalLight(SHADOW_WIDTH, SHADOW_HEIGHT,
		1.f, 1.f, 1.f,		// color light
		0.3f, 0.6f,			// ambient/diffuse intensity
		2.f, -1.f, -2.f);	// direction

	pointLights[0] = PointLight(SHADOW_WIDTH, SHADOW_HEIGHT,
		0.f, 0.f, 1.f,
		0.1f, 0.4f,
		4.f, 0.f, 0.f,
		0.3f, 0.2f, 0.1f);
	pointLightCount++;
	pointLights[1] = PointLight(SHADOW_WIDTH, SHADOW_HEIGHT,
		0.f, 1.f, 0.f,
		0.1f, 0.8f,
		-4.f, 2.f, 0.f,
		0.3f, 0.1f, 0.1f);
	pointLightCount++;

	spotLights[0] = SpotLight(SHADOW_WIDTH, SHADOW_HEIGHT,
		1.f, 1.f, 1.f,
		0.1f, 2.f,
		4.f, 2.f, -5.f,
		0.3f, 0.1f, 0.1f,
		0.f, -1.f, 0.f,
		20.f);
	spotLightCount++;
	spotLights[1] = SpotLight(SHADOW_WIDTH, SHADOW_HEIGHT,
		1.f, 0.f, 0.f,
		0.1f, 1.f,
		4.f, 0.f, -5.f,
		1.f, 0.f, 0.f,
		0.f, -1.f, -100.f,
		20.f);
	spotLightCount++;

	glm::mat4 projection = glm::perspective(glm::radians(60.0f), mainWindow.getBufferWidth() / mainWindow.getBufferHeight(), 0.1f, 100.f);

	// Loop until window close
	while (!mainWindow.getShouldClose())
	{
		logEnabled = true;

		GLfloat now = glfwGetTime();
		deltaTime = now - lastTime;
		lastTime = now;

		// Get and handle user input events
		glfwPollEvents();

		camera.keyControl(mainWindow.getKeys(), deltaTime);
		camera.mouseControl(mainWindow.getXChange(), mainWindow.getYChange());

		DirectionalShadowMapPass(&mainLight);
		RenderPass(projection, camera.calculateViewMatrix());

		ShaderProgram::UnbindShaderProgram(); // unbind shader program

		mainWindow.swapBuffers();

		logEnabled = true;
	}

	return 0;
}