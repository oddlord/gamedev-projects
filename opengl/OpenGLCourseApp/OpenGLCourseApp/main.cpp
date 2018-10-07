#include "stdafx.h"

#include <cmath>
#include <iostream>
#include <string>

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <GLM/glm.hpp>
#include <GLM/gtc/matrix_transform.hpp>
#include <GLM/gtc/type_ptr.hpp>

// #define GLM_ENABLE_EXPERIMENTAL
// #include <GLM/gtx/string_cast.hpp>

// Window dimensions
const GLint WIDTH = 800;
const GLint HEIGHT = 600;

const float toRadians = 3.14159265f / 180.f;

GLuint VAO;
GLuint VBO;
GLuint shaderProgram;
GLuint uniformModel;

bool direction = true;
float triOffset = 0.f;
const float triMaxOffset = 0.7f;
const float triIncrement = 0.001f;

float curAngle = 0.f;
const float angleIncrement = 0.01f;

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
uniform mat4 model;														\n\
																		\n\
void main()																\n\
{																		\n\
	gl_Position = model * vec4(pos, 1.0);								\n\
}";

// Fragment shader
static const char* fShader = "											\n\
#version 330															\n\
																		\n\
out vec4 colour;														\n\
																		\n\
void main()																\n\
{																		\n\
	colour = vec4(1.0, 0.0, 0.0, 1.0);									\n\
}";

void CreateTriangle()
{
	GLfloat vertices[] = {
		-1.f, -1.f, 0.f,
		1.f, -1.f, 0.f,
		0.f, 1.f, 0.f
	};

	glGenVertexArrays(1, &VAO);
	glBindVertexArray(VAO); // bind VAO
	{
		glGenBuffers(1, &VBO);
		glBindBuffer(GL_ARRAY_BUFFER, VBO); // bind VBO
		{
			glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

			// first attribute: layout location index in the vertex shader
			glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);
			glEnableVertexAttribArray(0);
		}
		glBindBuffer(GL_ARRAY_BUFFER, 0); // unbind VBO
	}
	glBindVertexArray(0); // unbind VAO
}

void PrintNL(std::string message)
{
	std::cout << message << std::endl;
}

int CheckErrors(const GLuint target, const GLenum status, const InfoLogType infoLogType, std::string message)
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
		return 1;
	}

	return 0;
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

	if (!CheckErrors(shader, GL_COMPILE_STATUS, InfoLogType::SHADER, "Error compiling the %d shader" + shaderType))
	{
		return;
	}

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

	// Setup viewport size
	// Viewport = the part of the window where we can draw
	// buffer width/height represent the ACTUAL width/height of the window
	glViewport(0, 0, bufferWidth, bufferHeight);

	CreateTriangle();
	CompileShaders();

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
		glClear(GL_COLOR_BUFFER_BIT);

		glUseProgram(shaderProgram);
		{
			glm::mat4 model(1.f);
			model = glm::rotate(model, curAngle * toRadians, glm::vec3(0.f, 0.f, 1.f));
			model = glm::translate(model, glm::vec3(triOffset, 0.f, 0.f));
			model = glm::scale(model, glm::vec3(curSize, curSize, 1.f));

			//std::cout << glm::to_string(model) << std::endl;
			//std::cin.ignore();

			glUniformMatrix4fv(uniformModel, 1, GL_FALSE, glm::value_ptr(model));

			glBindVertexArray(VAO);
			{
				glDrawArrays(GL_TRIANGLES, 0, 3);
			}
			glBindVertexArray(0);
		}
		glUseProgram(0);

		glfwSwapBuffers(mainWindow);
	}

	return 0;
}