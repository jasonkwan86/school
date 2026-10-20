#ifndef MAIN_H
#define MAIN_H
#include "libpnm.h"

typedef struct ImageProps {
    int width, height;
    bool raw;
    char * fileName;
} ImageProps;

int program1(ImageProps props);

int program2(ImageProps props);

int program3(ImageProps props);

bool isDimensionValid4(int dimension);

bool isDimensionValid6(int dimension);

bool isDimensionValid(int dimension, int modulus, int min);

int showErrorMessage(char *message);

#endif
