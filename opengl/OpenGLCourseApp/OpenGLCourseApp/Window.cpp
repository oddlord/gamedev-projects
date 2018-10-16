#include "stdafx.h"
#include "Window.h"

#include "Utils.h"

Window::Window()
{
	width = 800;
	height = 600;

	for (size_t i = 0; i < 1024; i++)
	{
		keys[i] = false;
	}
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

	// Handle Key & Mouse Input
	createCallbacks();
	glfwSetInputMode(mainWindow, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

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

	// Binds the GLFWwindow with this Window object
	// Used to retrieve the Window object from the static callback
	glfwSetWindowUserPointer(mainWindow, this);

	return 0;
}

GLfloat Window::getBufferWidth() { return bufferWidth; }
GLfloat Window::getBufferHeight() { return bufferHeight; }

bool Window::getShouldClose() { return glfwWindowShouldClose(mainWindow); }

bool* Window::getKeys()
{
	return keys;
}

GLfloat Window::getXChange()
{
	GLfloat change = xChange;
	xChange = 0;
	return change;
}

GLfloat Window::getYChange()
{
	GLfloat change = yChange;
	yChange = 0;
	return change;
}

void Window::swapBuffers() { 
	LOG("Swapping buffers");
	glfwSwapBuffers(mainWindow);
}

Window::~Window()
{
	glfwDestroyWindow(mainWindow);
	glfwTerminate();
}

void Window::createCallbacks()
{
	glfwSetKeyCallback(mainWindow, handleKeys);
	glfwSetCursorPosCallback(mainWindow, handleMouse);
}

void Window::handleKeys(GLFWwindow * glfwWindow, int key, int code, int action, int mode)
{
	// Retrieving the Window object
	Window* window = static_cast<Window*>(glfwGetWindowUserPointer(glfwWindow));

	if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
	{
		glfwSetWindowShouldClose(glfwWindow, GL_TRUE);
	}

	if (key >= 0 && key < 1024)
	{
		if (action == GLFW_PRESS)
		{
			window->keys[key] = true;
		}
		else if (action == GLFW_RELEASE)
		{
			window->keys[key] = false;
		}
	}
}

void Window::handleMouse(GLFWwindow * glfwWindow, double xPos, double yPos)
{
	// Retrieving the Window object
	Window* window = static_cast<Window*>(glfwGetWindowUserPointer(glfwWindow));

	if (window->mouseFirstMoved)
	{
		window->lastX = xPos;
		window->lastY = yPos;
		window->mouseFirstMoved = false;
	}

	window->xChange = xPos - window->lastX;
	window->yChange = window->lastY - yPos;

	window->lastX = xPos;
	window->lastY = yPos;
}
