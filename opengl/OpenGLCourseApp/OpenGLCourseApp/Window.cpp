#include "stdafx.h"
#include "Window.h"

#include "Utils.h"

Window::Window()
{
	width = 800;
	height = 600;
}

Window::Window(GLint windowWidth, GLint windowHeight)
{
	width = windowWidth;
	height = windowHeight;
}

int Window::Initialise()
{
	// Initialise GLFW
	LOG("Initialising GLFW");
	if (!glfwInit())
	{
		LOGERROR("GLFW initialisation failed!");
		glfwTerminate();
		return 1;
	}

	// Setup GLFW window properties
	LOG("Setting up GLFW window properties");
	// OpenGL version 3.3
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3); // version 3.x
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3); // version x.3
												   // Core profile - no backwards compatibility
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	// Allow forward compatibility
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GLU_TRUE);

	LOG("Creating GLFW window");
	mainWindow = glfwCreateWindow(width, height, "Test Window", NULL, NULL);
	if (!mainWindow)
	{
		LOGERROR("GLFW window creation failed!");
		glfwTerminate();
		return 1;
	}

	// Get the buffer size information
	LOG("Getting buffer dimensions");
	glfwGetFramebufferSize(mainWindow, &bufferWidth, &bufferHeight);

	// Set context for GLEW to use
	LOG("Setting up window context");
	glfwMakeContextCurrent(mainWindow);

	// Allow modern extension features
	LOG("Allowing experimental features");
	glewExperimental = GL_TRUE;

	// Initialise GLEW
	LOG("Initialising GLEW");
	if (glewInit() != GLEW_OK)
	{
		LOGERROR("GLEW initialisation failed!");
		glfwDestroyWindow(mainWindow);
		glfwTerminate();
		return 1;
	}

	LOG("Enabling depth");
	glEnable(GL_DEPTH_TEST);

	// Setup viewport size
	LOG("Setting viewport size");
	// Viewport = the part of the window where we can draw
	// buffer width/height represent the ACTUAL width/height of the window
	glViewport(0, 0, bufferWidth, bufferHeight);

	return 0;
}

GLfloat Window::getBufferWidth() { return bufferWidth; }
GLfloat Window::getBufferHeight() { return bufferHeight; }

bool Window::getShouldClose() { return glfwWindowShouldClose(mainWindow); }

void Window::swapBuffers() { 
	LOG("Swapping buffers");
	glfwSwapBuffers(mainWindow);
}

Window::~Window()
{
	glfwDestroyWindow(mainWindow);
	glfwTerminate();
}
