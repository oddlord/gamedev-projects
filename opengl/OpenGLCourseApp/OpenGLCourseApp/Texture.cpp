#include "stdafx.h"
#include "Texture.h"

#include <iostream>
#include <string>

Texture::Texture() : Texture("", false) {}
Texture::Texture(fs::path fileLoc, bool alpha)
{
	textureID = 0;
	hasAlpha = alpha;
	width = 0;
	height = 0;
	bitDepth = 0;
	fileLocation = fileLoc;
}

bool Texture::LoadTexture()
{
	// unsigned char because we want an array of bytes
	unsigned char* texData = stbi_load(fileLocation.string().c_str(), &width, &height, &bitDepth, 0);
	if (!texData)
	{
		std::cout << "Failed to find: " << fileLocation << std::endl;
		return false;
	}

	// Generating a Texture ID
	glGenTextures(1, &textureID);
	// Binding Texture ID
	glBindTexture(GL_TEXTURE_2D, textureID);

	// Repeat S (U/X) and T (V/Y) axis
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	// Linear filter when minimizing and magnifying texture
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

	GLenum format = hasAlpha ? GL_RGBA : GL_RGB;
	glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, texData);
	glGenerateMipmap(GL_TEXTURE_2D);

	// Unbinding Texture ID
	glBindTexture(GL_TEXTURE_2D, 0);

	// Raw image data in texData no longer needed
	stbi_image_free(texData);

	return true;
}

void Texture::UseTexture()
{
	// Selecting the Texture Unit 0
	glActiveTexture(GL_TEXTURE0);

	// Binding Texture ID
	glBindTexture(GL_TEXTURE_2D, textureID);
}

void Texture::ClearTexture()
{
	glDeleteTextures(1, &textureID);
	textureID = 0;
	width = 0;
	height = 0;
	bitDepth = 0;
	fileLocation = "";
}


Texture::~Texture()
{
	ClearTexture();
}
