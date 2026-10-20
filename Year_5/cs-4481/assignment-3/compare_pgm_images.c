#include <stdio.h>
#include "mean_absolute_error.h"

// calls the function developed in Q7 to compare two images together
int main(int argc, char *argv[])
{
    if (argc != 3)
    {
        printf("There must be 2 arguments:\n");
        printf("1: <input PGM filename 1> ()\n");
        printf("2: <input PGM filename 2> ()\n");
        
        return 0;
    }

    char *inputFilename1 = argv[1];
    char *inputFilename2 = argv[2];

    float meanAbsoluteError = mean_absolute_error(inputFilename1, inputFilename2);
    if (meanAbsoluteError == -1)
    {
        printf("Could not open calculate mean absolute error.\n");
    }

    printf("Mean absolute difference between the two images is: %.2f\n", meanAbsoluteError);

    return 0;
}