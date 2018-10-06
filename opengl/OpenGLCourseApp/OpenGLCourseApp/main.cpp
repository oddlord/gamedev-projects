#include "stdafx.h"

#include <iostream>
#include <string>

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

using namespace std;

// Window dimensions
const GLint WIDTH = 800;
const GLint HEIGHT = 600;

GLuint VAO;
GLuint VBO;
GLuint shaderProgram;

enum InfoLogType { SHADER, PROGRAM };

// Vertex shader
static const char* vShader = "									\n\
#version 330													\n\
																\n\
layout (location = 0) in vec3 pos;								\n\
																\n\
void main()														\n\
{																\n\
	gl_Position = vec4(0.4 * pos.x, 0.4 * pos.y, pos.z, 1.0);	\n\
}";

// Fragment shader
static const char* fShader = "									\n\
#version 330													\n\
																\n\
out vec4 colour;												\n\
																\n\
void main()														\n\
{																\n\
	colour = vec4(1.0, 0.0, 0.0, 1.0);							\n\
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

		glGenBuffers(1, &VBO);
		glBindBuffer(GL_ARRAY_BUFFER, VBO); // bind VBO
			glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

			// first attribute: layout location index in the vertex shader
			glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);
			glEnableVertexAttribArray(0);

		glBindBuffer(GL_ARRAY_BUFFER, 0); // unbind VBO
	glBindVertexArray(0); // unbind VAO
}

int checkErrors(const GLuint target, const GLenum status, const InfoLogType infoLogType, string message)
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

		cout << message << ": " << eLog << endl;
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

	if (!checkErrors(shader, GL_COMPILE_STATUS, InfoLogType::SHADER, "Error compiling the %d shader" + shaderType))
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
		cout << "Error creating shader program!" << endl;
		return;
	}

	AddShader(shaderProgram, vShader, GL_VERTEX_SHADER);
	AddShader(shaderProgram, fShader, GL_FRAGMENT_SHADER);

	GLint result = 0;
	GLchar eLog[1024] = { 0 };

	// Linking the shader program
	glLinkProgram(shaderProgram);
	if (!checkErrors(shaderProgram, GL_LINK_STATUS, InfoLogType::PROGRAM, "Error linking program"))
	{
		return;
	}

	// Validating the shader program
	glValidateProgram(shaderProgram);
	if (!checkErrors(shaderProgram, GL_VALIDATE_STATUS, InfoLogType::PROGRAM, "Error validating program"))
	{
		return;
	}
}

int main()
{
	// Initialise GLFW
	if (!glfwInit())
	{
		cout << "GLFW initialisation failed!" << endl;
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
		cout << "GLFW window creation failed!" << endl;
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
		cout << "GLEW initialisation failed!" << endl;
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

		// Clear window
		glClearColor(0.f, 0.f, 0.f, 1.f);
		glClear(GL_COLOR_BUFFER_BIT);

		glUseProgram(shaderProgram);
			glBindVertexArray(VAO);
				glDrawArrays(GL_TRIANGLES, 0, 3);
			glBindVertexArray(0);
		glUseProgram(0);

		glfwSwapBuffers(mainWindow);
	}

	return 0;
}