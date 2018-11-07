#pragma once

#include <GLAD\glad.h>
#include <GLFW/glfw3.h>

class Window
{
public:
	Window();
	Window(GLint windowWidth, GLint windowHeight);

	int Initialise();

	GLfloat getBufferWidth();
	GLfloat getBufferHeight();

	bool getShouldClose();

	bool* getKeys();
	GLfloat getXChange();
	GLfloat getYChange();

	void swapBuffers();

	~Window();

private:
	GLFWwindow* mainWindow;

	GLint width;
	GLint height;

	GLint bufferWidth;
	GLint bufferHeight;

	bool keys[1024];

	GLfloat lastX;
	GLfloat lastY;
	GLfloat xChange;
	GLfloat yChange;
	bool mouseMoved;
	
	void createCallbacks();
	static void handleKeys(GLFWwindow* glfwWindow, int key, int code, int action, int mode);
	static void handleMouse(GLFWwindow* glfwWindow, double xPos, double yPos);
};

