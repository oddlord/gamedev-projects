#include "stdafx.h"

#include "common.h"
#include "Mesh.h"
#include "ShaderProgram.h"
#include "Utils.h"
#include "Window.h"

#include <cmath>
#include <iostream>
#include <string>
#include <vector>

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <GLM/glm.hpp>
#include <GLM/gtc/matrix_transform.hpp>
#include <GLM/gtc/type_ptr.hpp>

// #define GLM_ENABLE_EXPERIMENTAL
// #include <GLM/gtx/string_cast.hpp>

// Log enabled/disabled
bool logEnabled = true;

// Window dimensions
const GLint WIDTH = 800;
const GLint HEIGHT = 600;

Window mainWindow;

std::vector<Mesh*> meshList;
std::vector<ShaderProgram*> shaderProgramList;

/*const float toRadians = 3.14159265f / 180.f;

bool direction = true;
float triOffset = 0.f;
const float triMaxOffset = 0.7f;
const float triIncrement = 0.001f;

float curAngle = 0.f;
const float angleIncrement = 0.1f;

bool sizeDirection = true;
float curSize = 0.4f;
const float maxSize = 0.8f;
const float minSize = 0.1f;
const float sizeIncrement = 0.0001f;*/

// Shaders
static const std::string shaderFolder = "Shaders/";
// Vertex shader
static const std::string vShader = shaderFolder + "shader.vert";
// Fragment shader
static const std::string fShader = shaderFolder + "shader.frag";

void CreatePyramid()
{
	LOG("Creating pyramid");

	GLfloat vertices[] = {
		-1.f, -1.f, 0.f,
		1.f, -1.f, 0.f,
		1.f, -1.f, 1.f,
		-1.f, -1.f, 1.f,
		0.f, 1.f, 0.5f,
		0.f, -1.f, 1.f,
		0.f, 1.f, 0.f
	};

	/*unsigned int indices[] = {
		0, 1, 6,
		1, 5, 6,
		5, 0, 6,
		0, 1, 5
	};*/

	unsigned int indices[] = {
		0, 1, 4,
		1, 2, 4,
		2, 3, 4, 
		3, 0, 4,
		0, 1, 2,
		0, 2, 3
	};

	Mesh* pyramid = new Mesh();
	pyramid->CreateMesh(sizeof(vertices) / sizeof(vertices[0]), vertices, sizeof(indices) / sizeof(indices[0]), indices);
	meshList.push_back(pyramid);

	Mesh* pyramid2 = new Mesh();
	pyramid2->CreateMesh(sizeof(vertices) / sizeof(vertices[0]), vertices, sizeof(indices) / sizeof(indices[0]), indices);
	meshList.push_back(pyramid2);
}

void CreateShaderProgram()
{
	LOG("Creating shader program");

	ShaderProgram* shaderProgram = new ShaderProgram();
	shaderProgram->CreateFromFiles(vShader, fShader);
	shaderProgramList.push_back(shaderProgram);
}

int main()
{
	mainWindow = Window(WIDTH, HEIGHT);
	mainWindow.Initialise();

	CreatePyramid();
	CreateShaderProgram();

	LOG("Creating projection matrix");
	glm::mat4 projection = glm::perspective(glm::radians(60.0f), mainWindow.getBufferWidth() / mainWindow.getBufferHeight(), 0.1f, 100.f);

	LOG("Retrieving shader program");
	ShaderProgram* shaderProgram = shaderProgramList[0];
	GLuint uniformModelID = shaderProgram->GetModelLocation();
	GLuint uniformProjectionID = shaderProgram->GetProjectionLocation();

	// Loop until window close
	LOG("Entering main loop");
	while (!mainWindow.getShouldClose())
	{
		logEnabled = false;
		LOG("New frame computation started");

		// Get and handle user input events
		glfwPollEvents();

		/*if (direction)
		{
			triOffset += triIncrement;
		}
		else
		{
			triOffset -= triIncrement;
		}

		if (abs(triOffset) >= triMaxOffset)
		{
			direction = !direction;
		}

		curAngle += angleIncrement;
		if (curAngle >= 360)
		{
			curAngle -= 360;
		}

		if (sizeDirection)
		{
			curSize += sizeIncrement;
		}
		else
		{
			curSize -= sizeIncrement;
		}

		if (curSize >= maxSize || curSize <= minSize)
		{
			sizeDirection = !sizeDirection;
		}*/

		// Clear window
		LOG("Clearing window");
		glClearColor(0.f, 0.f, 0.f, 1.f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		shaderProgram->UseShaderProgram(); // bind shader program

		glm::mat4 model(1.f);
		model = glm::translate(model, glm::vec3(0.f, -0.3f, -2.5f));
		// model = glm::rotate(model, curAngle * toRadians, glm::vec3(0.f, 1.f, 0.f));
		model = glm::scale(model, glm::vec3(0.4f, 0.4f, 1.f));
		glUniformMatrix4fv(uniformModelID, 1, GL_FALSE, glm::value_ptr(model));
		glUniformMatrix4fv(uniformProjectionID, 1, GL_FALSE, glm::value_ptr(projection));
		meshList[0]->RenderMesh();

		model = glm::mat4(1.f);
		model = glm::translate(model, glm::vec3(0.f, 0.7f, -2.5f));
		model = glm::scale(model, glm::vec3(0.4f, 0.4f, 1.f));
		glUniformMatrix4fv(uniformModelID, 1, GL_FALSE, glm::value_ptr(model));
		meshList[1]->RenderMesh();

		LOG("Unbinding shader program");
		glUseProgram(0); // unbind shader program

		mainWindow.swapBuffers();

		logEnabled = true;
	}

	return 0;
}