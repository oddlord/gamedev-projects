#pragma once

#include <GLEW/glew.h>
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

	void swapBuffers();

	~Window();

private:
	GLFWwindow* mainWindow;

	GLint width;
	GLint height;

	GLint bufferWidth;
	GLint bufferHeight;
};

