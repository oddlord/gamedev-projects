#pragma once

#include "common.h"

#include <iostream>
#include <string>

#include <GLEW/glew.h>

#define PATH_SEPARATOR '\\'

#define __FILENAME__ (strrchr(__FILE__, PATH_SEPARATOR) ? strrchr(__FILE__, PATH_SEPARATOR) + 1 : __FILE__)

#define LOG(msg) \
    if (logEnabled) std::cout << __FILENAME__ << "(" << __LINE__ << "): " << msg << std::endl;

#define LOGERROR(msg) \
	LOG("ERROR: " << msg);

enum InfoLogType { SHADER, PROGRAM };

class Utils
{
public:
	Utils();

	static bool CheckErrors(const GLuint target, const GLenum status, const InfoLogType infoLogType, std::string message);

	static std::string ReadFile(std::string fileLocation);

	~Utils();
};

