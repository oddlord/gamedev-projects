#include "stdafx.h"

#include "Utils.h"

#include <fstream>

Utils::Utils()
{
}

bool Utils::CheckErrors(const GLuint target, const GLenum status, const InfoLogType infoLogType, std::string message)
{
	GLint result = 0;
	GLchar eLog[1024] = { 0 };

	glGetProgramiv(target, status, &result);
	if (!result)
	{
		switch (infoLogType)
		{
		case InfoLogType::SHADER:
			glGetShaderInfoLog(target, sizeof(eLog), NULL, eLog);
			break;
		case InfoLogType::PROGRAM:
			glGetProgramInfoLog(target, sizeof(eLog), NULL, eLog);
			break;
		}

		LOGERROR(": " << eLog);
		return false;
	}

	return true;
}

std::string Utils::ReadFile(std::string fileLocation)
{
	std::string content;
	std::ifstream fileStream(fileLocation, std::ios::in);

	if (!fileStream.is_open())
	{
		LOGERROR("Failed to read " << fileLocation << "! File doesn't exist.");
		return "";
	}

	std::string line;
	while (!fileStream.eof())
	{
		std::getline(fileStream, line);
		content.append(line + "\n");
	}

	fileStream.close();
	return content;
}


Utils::~Utils()
{
}
