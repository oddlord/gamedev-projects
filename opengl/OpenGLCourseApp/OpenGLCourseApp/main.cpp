#include "stdafx.h"

#include <cmath>
#include <iostream>
#include <string>
#include <vector>

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <GLM/glm.hpp>
#include <GLM/gtc/matrix_transform.hpp>
#include <GLM/gtc/type_ptr.hpp>

#include "Mesh.h"

// #define GLM_ENABLE_EXPERIMENTAL
// #include <GLM/gtx/string_cast.hpp>

// Window dimensions
const GLint WIDTH = 800;
const GLint HEIGHT = 600;

GLuint _VAO; // Vertex Array Object
GLuint _VBO; // Vertex Buffer Object
GLuint _IBO; // Index Buffer Object

std::vector<Mesh*> meshList;

const float toRadians = 3.14159265f / 180.f;

GLuint shaderProgram;
GLuint uniformModel;
GLuint uniformProjection;

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
const float sizeIncrement = 0.0001f;

enum InfoLogType { SHADER, PROGRAM };

// Vertex shader
static const char* vShader = "											\n\
#version 330															\n\
																		\n\
layout (location = 0) in vec3 pos;										\n\
																		\n\
out vec4 vCol;															\n\
																		\n\
uniform mat4 model;														\n\
uniform mat4 projection;												\n\
																		\n\
void main()																\n\
{																		\n\
	gl_Position = projection * model * vec4(pos, 1.0f);					\n\
	vCol = vec4(clamp(pos, 0.0f, 1.0f), 1.0f);							\n\
}";

// Fragment shader
static const char* fShader = "											\n\
#version 330															\n\
																		\n\
in vec4 vCol;															\n\
																		\n\
out vec4 colour;														\n\
																		\n\
void main()																\n\
{																		\n\
	colour = vCol;														\n\
}";

void CreatePyramid()
{
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

void PrintNL(std::string message)
{
	std::cout << message << std::endl;
}

bool CheckErrors(const GLuint target, const GLenum status, const InfoLogType infoLogType, std::string message)
{
	GLint result = 0;
	GLchar eLog[1024] = { 0 };

	glGetProgramiv(target, status, &result);
	if (!result)
	{
		switch (infoLogType)
		{
		case InfoLogType::SHADER :
			glGetShaderInfoLog(target, sizeof(eLog), NULL, eLog);
			break;
		case InfoLogType::PROGRAM:
			glGetProgramInfoLog(target, sizeof(eLog), NULL, eLog);
			break;
		}

		PrintNL(message + ": " + eLog);
		return false;
	}

	return true;
}

void AddShader(GLuint program, const char* shaderCode, GLenum shaderType)
{
	GLuint shader = glCreateShader(shaderType);

	const GLchar* code[1];
	code[0] = shaderCode;

	GLint codeLength[1];
	codeLength[0] = strlen(shaderCode);

	glShaderSource(shader, 1, code, codeLength);
	glCompileShader(shader);

	CheckErrors(shader, GL_COMPILE_STATUS, InfoLogType::SHADER, "Error compiling the %d shader" + shaderType);

	glAttachShader(program, shader);
}

void CompileShaders()
{
	shaderProgram = glCreateProgram();
	
	if (!shaderProgram)
	{
		PrintNL("Error creating shader program!");
		return;
	}

	AddShader(shaderProgram, vShader, GL_VERTEX_SHADER);
	AddShader(shaderProgram, fShader, GL_FRAGMENT_SHADER);

	GLint result = 0;
	GLchar eLog[1024] = { 0 };

	// Linking the shader program
	glLinkProgram(shaderProgram);
	if (!CheckErrors(shaderProgram, GL_LINK_STATUS, InfoLogType::PROGRAM, "Error linking program"))
	{
		return;
	}

	// Validating the shader program
	glValidateProgram(shaderProgram);
	if (!CheckErrors(shaderProgram, GL_VALIDATE_STATUS, InfoLogType::PROGRAM, "Error validating program"))
	{
		return;
	}

	uniformModel = glGetUniformLocation(shaderProgram, "model");
	uniformProjection = glGetUniformLocation(shaderProgram, "projection");
}

int main()
{
	// Initialise GLFW
	if (!glfwInit())
	{
		PrintNL("GLFW initialisation failed!");
		glfwTerminate();
		return 1;
	}

	// Setup GLFW window properties
	// OpenGL version 3.3
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3); // version 3.x
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3); // version x.3
	// Core profile - no backwards compatibility
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	// Allow forward compatibility
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GLU_TRUE);

	GLFWwindow* mainWindow = glfwCreateWindow(WIDTH, HEIGHT, "Test Window", NULL, NULL);
	if (!mainWindow)
	{
		PrintNL("GLFW window creation failed!");
		glfwTerminate();
		return 1;
	}

	// Get the buffer size information
	int bufferWidth;
	int bufferHeight;
	glfwGetFramebufferSize(mainWindow, &bufferWidth, &bufferHeight);

	// Set context for GLEW to use
	glfwMakeContextCurrent(mainWindow);

	// Allow modern extension features
	glewExperimental = GL_TRUE;

	// Initialise GLEW
	if (glewInit() != GLEW_OK)
	{
		PrintNL("GLEW initialisation failed!");
		glfwDestroyWindow(mainWindow);
		glfwTerminate();
		return 1;
	}

	glEnable(GL_DEPTH_TEST);

	// Setup viewport size
	// Viewport = the part of the window where we can draw
	// buffer width/height represent the ACTUAL width/height of the window
	glViewport(0, 0, bufferWidth, bufferHeight);

	CreatePyramid();
	CompileShaders();

	glm::mat4 projection = glm::perspective(glm::radians(60.0f), (GLfloat)bufferWidth / (GLfloat)bufferHeight, 0.1f, 100.f);

	// Loop until window close
	while (!glfwWindowShouldClose(mainWindow))
	{
		// Get and handle user input events
		glfwPollEvents();

		if (direction)
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
		}

		// Clear window
		glClearColor(0.f, 0.f, 0.f, 1.f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		glUseProgram(shaderProgram); // bind shader program

		glm::mat4 model(1.f);
		model = glm::translate(model, glm::vec3(triOffset, -0.3f, -2.5f));
		// model = glm::rotate(model, curAngle * toRadians, glm::vec3(0.f, 1.f, 0.f));
		model = glm::scale(model, glm::vec3(0.4f, 0.4f, 1.f));
		glUniformMatrix4fv(uniformModel, 1, GL_FALSE, glm::value_ptr(model));
		glUniformMatrix4fv(uniformProjection, 1, GL_FALSE, glm::value_ptr(projection));
		meshList[0]->RenderMesh();

		model = glm::mat4(1.f);
		model = glm::translate(model, glm::vec3(-triOffset, 0.7f, -2.5f));
		model = glm::scale(model, glm::vec3(0.4f, 0.4f, 1.f));
		glUniformMatrix4fv(uniformModel, 1, GL_FALSE, glm::value_ptr(model));
		meshList[1]->RenderMesh();

		glUseProgram(0); // unbind shader program

		glfwSwapBuffers(mainWindow);
	}

	return 0;
}