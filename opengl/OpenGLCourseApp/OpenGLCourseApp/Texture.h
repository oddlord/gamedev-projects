#pragma once

#include <filesystem>

#include <GLAD\glad.h>

#include <stb_image.h>

namespace fs = std::experimental::filesystem;

class Texture
{
public:
	Texture();
	Texture(fs::path fileLoc, bool alpha);

	bool LoadTexture();
	void UseTexture();
	void ClearTexture();

	~Texture();

private:
	GLuint textureID;
	bool hasAlpha;
	int width;
	int height;
	int bitDepth;

	fs::path fileLocation;
};

