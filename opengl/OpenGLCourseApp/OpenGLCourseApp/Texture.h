#pragma once

#include <filesystem>

#include <GLEW/glew.h>

#include <stb_image.h>

namespace fs = std::experimental::filesystem;

class Texture
{
public:
	Texture() : Texture("") {};
	Texture(fs::path fileLoc);

	void LoadTexture();
	void UseTexture();
	void ClearTexture();

	~Texture();

private:
	GLuint textureID;
	int width;
	int height;
	int bitDepth;

	fs::path fileLocation;
};

