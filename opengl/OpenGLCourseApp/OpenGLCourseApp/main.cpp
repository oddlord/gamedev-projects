#include "stdafx.h"

#define STB_IMAGE_IMPLEMENTATION

#include "Camera.h"
#include "common.h"
#include "Light.h"
#include "Mesh.h"
#include "ShaderProgram.h"
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
const GLint WIDTH = 800;
const GLint HEIGHT = 600;

Window mainWindow;

std::vector<Mesh*> meshList;
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
Texture brickTexture(brickTexturePath);
// Dirt texture
fs::path dirtTextureFile("ground_dirt_3299_9359_Small.jpg");
fs::path dirtTexturePath = texturesFolder / dirtTextureFile;
Texture dirtTexture(dirtTexturePath);
// Wood texture
fs::path woodTextureFile("wood_plain_210_251_Small.jpg");
fs::path woodTexturePath = texturesFolder / woodTextureFile;
Texture woodTexture(woodTexturePath);

Light mainLight;

void CreatePyramid()
{
	GLfloat vertices[] = {
	//	x		y		z		u		v
		-1.f,	-1.f,	0.f,	0.f,	0.f,
		1.f,	-1.f,	0.f,	1.f,	0.f,
		1.f,	-1.f,	1.f,	1.f,	1.f,
		-1.f,	-1.f,	1.f,	0.f,	1.f,
		0.f,	1.f,	0.5f,	0.5f,	0.5f
	};

	unsigned int indices[] = {
		0, 1, 4,
		1, 2, 4,
		2, 3, 4, 
		3, 0, 4,
		0, 1, 2,
		0, 2, 3
	};

	const int numOfVertices = sizeof(vertices) / sizeof(vertices[0]);
	const int numOfIndices = sizeof(indices) / sizeof(indices[0]);

	Mesh* pyramid = new Mesh();
	pyramid->CreateMesh(numOfVertices, vertices, numOfIndices, indices);
	meshList.push_back(pyramid);

	Mesh* pyramid2 = new Mesh();
	pyramid2->CreateMesh(numOfVertices, vertices, numOfIndices, indices);
	meshList.push_back(pyramid2);

	Mesh* pyramid3 = new Mesh();
	pyramid3->CreateMesh(numOfVertices, vertices, numOfIndices, indices);
	meshList.push_back(pyramid3);
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

	CreatePyramid();
	CreateShaderProgram();

	camera = Camera(glm::vec3(0.f, 0.f, 0.f), glm::vec3(0.f, 1.f, 0.f), -90.f, 0.f, 5.f, 0.5f);

	brickTexture.LoadTexture();
	dirtTexture.LoadTexture();
	woodTexture.LoadTexture();

	mainLight = Light(1.f, 1.f, 1.f, 1.f);

	ShaderProgram* shaderProgram = shaderProgramList[0];
	GLuint uniformModelID = shaderProgram->GetModelLocation();
	GLuint uniformProjectionID = shaderProgram->GetProjectionLocation();
	GLuint uniformViewID = shaderProgram->GetViewLocation();
	GLuint uniformAmbientColourID = shaderProgram->GetAmbientColourLocation();
	GLuint uniformAmbientIntensityID = shaderProgram->GetAmbientIntensityLocation();

	glm::mat4 projection = glm::perspective(glm::radians(60.0f), mainWindow.getBufferWidth() / mainWindow.getBufferHeight(), 0.1f, 100.f);

	glm::vec3 modelScale(0.4f, 0.4f, 1.f);

	glm::mat4 model1(1.f);
	model1 = glm::translate(model1, glm::vec3(0.f, -0.3f, -2.5f));
	model1 = glm::scale(model1, modelScale);

	glm::mat4 model2(1.f);
	model2 = glm::translate(model2, glm::vec3(0.f, 0.7f, -2.5f));
	model2 = glm::scale(model2, modelScale);

	glm::mat4 model3(1.f);
	model3 = glm::translate(model3, glm::vec3(0.f, -0.3f, -4.f));
	model3 = glm::scale(model3, modelScale);

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

		mainLight.UseLight(uniformAmbientColourID, uniformAmbientIntensityID);

		glm::mat4 view = camera.calculateViewMatrix();

		glUniformMatrix4fv(uniformProjectionID, 1, GL_FALSE, glm::value_ptr(projection));
		glUniformMatrix4fv(uniformViewID, 1, GL_FALSE, glm::value_ptr(view));

		glUniformMatrix4fv(uniformModelID, 1, GL_FALSE, glm::value_ptr(model1));
		dirtTexture.UseTexture();
		meshList[0]->RenderMesh();

		glUniformMatrix4fv(uniformModelID, 1, GL_FALSE, glm::value_ptr(model2));
		brickTexture.UseTexture();
		meshList[1]->RenderMesh();

		glUniformMatrix4fv(uniformModelID, 1, GL_FALSE, glm::value_ptr(model3));
		woodTexture.UseTexture();
		meshList[2]->RenderMesh();

		ShaderProgram::UnbindShaderProgram(); // unbind shader program

		mainWindow.swapBuffers();

		logEnabled = true;
	}

	return 0;
}