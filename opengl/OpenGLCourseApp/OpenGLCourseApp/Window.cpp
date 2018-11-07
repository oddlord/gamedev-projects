#include "stdafx.h"
#include "Window.h"

#include "Utils.h"

Window::Window() : Window(800, 600) {}
Window::Window(GLint windowWidth, GLint windowHeight)
{
	width = windowWidth;
	height = windowHeight;

	for (size_t i = 0; i < 1024; i++)
	{
		keys[i] = false;
	}

	bufferWidth = 0;
	bufferHeight = 0;

	lastX = 0.f;
	lastY = 0.f;
	xChange = 0.f;
	yChange = 0.f;
	mouseMoved = false;
}

int Window::Initialise()
{
	// Initialise GLFW
	if (!glfwInit())
	{
		LOGERROR("GLFW initialisation failed!");
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
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);

	mainWindow = glfwCreateWindow(width, height, "Test Window", NULL, NULL);
	if (!mainWindow)
	{
		LOGERROR("GLFW window creation failed!");
		glfwTerminate();
		return 1;
	}

	// Get the buffer size information
	glfwGetFramebufferSize(mainWindow, &bufferWidth, &bufferHeight);

	// Set context for GLEW/GLAD to use
	glfwMakeContextCurrent(mainWindow);

	// Handle Key & Mouse Input
	createCallbacks();
	glfwSetInputMode(mainWindow, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

	// Allow modern GLEW extension features
	// glewExperimental = GL_TRUE;

	// Initialise GLEW
	/*if (glewInit() != GLEW_OK)
	{
		LOGERROR("GLEW initialisation failed!");
		glfwDestroyWindow(mainWindow);
		glfwTerminate();
		return 1;
	}*/

	// Initialise GLAD
	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		printf("Failed to initialise GLAD!");
		return -1;
	}

	glEnable(GL_DEPTH_TEST);

	// Setup viewport size
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

	if (!window->mouseMoved)
	{
		window->lastX = xPos;
		window->lastY = yPos;
		window->mouseMoved = true;
	}

	window->xChange = xPos - window->lastX;
	window->yChange = window->lastY - yPos;

	window->lastX = xPos;
	window->lastY = yPos;
}
