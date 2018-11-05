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

Window mainWindow;

std::vector<ShaderProgram*> shaderProgramList;

Camera camera;

GLfloat deltaTime = 0.f;
GLfloat lastTime = 0.f;

const float toRadians = 3.14159265f / 180.f;

// Shaders
fs::path shadersFolder("Shaders");
// Vertex shader
fs::path vShaderFile("shader.vert");
fs::path vShaderPath = shadersFolder / vShaderFile;
// Fragment shader
fs::path fShaderFile("shader.frag");
fs::path fShaderPath = shadersFolder / fShaderFile;

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

Material shinyMaterial;
Material dullMaterial;

Model xwing;
Model blackhawk;

Mesh* createFloor()
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

	Mesh* floorMesh = new Mesh();
	floorMesh->CreateMesh(numOfVertices, vertices, numOfIndices, indices, vLength, uvOffset, normalOffset);

	return floorMesh;
}

Mesh* CreatePyramid()
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

	Mesh* pyramidMesh = new Mesh();
	pyramidMesh->CreateMesh(numOfVertices, vertices, numOfIndices, indices, vLength, uvOffset, normalOffset);

	return pyramidMesh;
}

void CreateShaderProgram()
{
	ShaderProgram* shaderProgram = new ShaderProgram();
	shaderProgram->CreateFromFiles(vShaderPath, fShaderPath);
	shaderProgramList.push_back(shaderProgram);
}

int main()
{
	mainWindow = Window(WIDTH, HEIGHT);
	mainWindow.Initialise();

	Mesh* pyramidMesh1 = CreatePyramid();
	Mesh* pyramidMesh2 = CreatePyramid();
	Mesh* pyramidMesh3 = CreatePyramid();
	Mesh* floorMesh = createFloor();

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

	mainLight = DirectionalLight(1.f, 1.f, 1.f, // color light
		0.3f, 0.6f,								// ambient/diffuse intensity
		2.f, -1.f, -2.f);						// direction

	unsigned int pointLightCount = 0;
	pointLights[0] = PointLight(0.f, 0.f, 1.f,
		0.1f, 0.4f,
		4.f, 0.f, 0.f,
		0.3f, 0.2f, 0.1f);
	pointLightCount++;
	pointLights[1] = PointLight(0.f, 1.f, 0.f,
		0.1f, 0.8f,
		-4.f, 2.f, 0.f,
		0.3f, 0.1f, 0.1f);
	pointLightCount++;

	unsigned int spotLightCount = 0;
	spotLights[0] = SpotLight(1.f, 1.f, 1.f,
		0.1f, 2.f,
		4.f, 2.f, -5.f,
		0.3f, 0.1f, 0.1f,
		0.f, -1.f, 0.f,
		20.f);
	spotLightCount++;
	spotLights[1] = SpotLight(1.f, 0.f, 0.f,
		0.1f, 1.f,
		4.f, 0.f, -5.f,
		1.f, 0.f, 0.f,
		0.f, -1.f, -100.f,
		20.f);
	spotLightCount++;

	ShaderProgram* shaderProgram = shaderProgramList[0];
	GLuint modelUnifLoc = shaderProgram->GetModelUnifLoc();
	GLuint projectionUnifLoc = shaderProgram->GetProjectionUnifLoc();
	GLuint viewUnifLoc = shaderProgram->GetViewUnifLoc();
	GLuint eyePosUnifLoc = shaderProgram->GetEyePositionUnifLoc();
	GLuint specularIntensityUnifLoc = shaderProgram->GetSpecularIntensityUnifLoc();
	GLuint shininessUnifLoc = shaderProgram->GetShininessUnifLoc();

	glm::mat4 projection = glm::perspective(glm::radians(60.0f), mainWindow.getBufferWidth() / mainWindow.getBufferHeight(), 0.1f, 100.f);

	glm::mat4 pyramidModel1(1.f);
	pyramidModel1 = glm::translate(pyramidModel1, glm::vec3(0.f, -0.3f, -5.f));

	glm::mat4 pyramidModel2(1.f);
	pyramidModel2 = glm::translate(pyramidModel2, glm::vec3(0.f, 2.f, -5.f));

	glm::mat4 pyramidModel3(1.f);
	pyramidModel3 = glm::translate(pyramidModel3, glm::vec3(0.f, -0.3f, -8.f));

	glm::mat4 floorModel(1.f);
	floorModel = glm::translate(floorModel, glm::vec3(0.f, -2.f, -5.f));

	glm::mat4 xwingModel(1.f);
	xwingModel = glm::translate(xwingModel, glm::vec3(-7.f, 0.f, 10.f));
	xwingModel = glm::scale(xwingModel, glm::vec3(0.006f, 0.006f, 0.006f));

	glm::mat4 blackhawkModel(1.f);
	blackhawkModel = glm::translate(blackhawkModel, glm::vec3(-3.f, 2.f, -5.f));
	blackhawkModel = glm::rotate(blackhawkModel, -90.f * toRadians, glm::vec3(1.f, 0.f, 0.f));
	blackhawkModel = glm::scale(blackhawkModel, glm::vec3(0.4f, 0.4f, 0.4f));

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

		// Clear window
		glClearColor(0.f, 0.f, 0.f, 1.f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		shaderProgram->UseShaderProgram(); // bind shader program

		glm::vec3 lowerFlashPos = camera.getCameraPosition();
		lowerFlashPos.y -= 0.3f;
		// spotLights[0].SetFlash(lowerFlashPos, camera.getCameraDirection());

		shaderProgram->SetDirectionalLight(&mainLight);
		shaderProgram->SetPointLights(pointLights, pointLightCount);
		shaderProgram->SetSpotLights(spotLights, spotLightCount);

		glm::mat4 view = camera.calculateViewMatrix();

		glUniformMatrix4fv(projectionUnifLoc, 1, GL_FALSE, glm::value_ptr(projection));
		glUniformMatrix4fv(viewUnifLoc, 1, GL_FALSE, glm::value_ptr(view));
		glUniform3fv(eyePosUnifLoc, 1, glm::value_ptr(camera.getCameraPosition()));

		glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(pyramidModel1));
		dirtTexture.UseTexture();
		dullMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
		pyramidMesh1->RenderMesh();

		glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(pyramidModel2));
		brickTexture.UseTexture();
		dullMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
		pyramidMesh2->RenderMesh();

		glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(pyramidModel3));
		woodTexture.UseTexture();
		shinyMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
		pyramidMesh3->RenderMesh();

		glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(floorModel));
		dirtTexture.UseTexture();
		shinyMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
		floorMesh->RenderMesh();

		glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(xwingModel));
		shinyMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
		xwing.RenderModel();

		glUniformMatrix4fv(modelUnifLoc, 1, GL_FALSE, glm::value_ptr(blackhawkModel));
		shinyMaterial.UseMaterial(specularIntensityUnifLoc, shininessUnifLoc);
		blackhawk.RenderModel();

		ShaderProgram::UnbindShaderProgram(); // unbind shader program

		mainWindow.swapBuffers();

		logEnabled = true;
	}

	return 0;
}