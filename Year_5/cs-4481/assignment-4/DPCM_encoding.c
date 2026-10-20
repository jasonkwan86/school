#include <stdio.h>
#include <stdlib.h>
#include "DPCM_encoding_function.h"

// Take an input from the command line
// an input pgm image name and
// a prediction rule code.
// Then, it will call Encode_Using_DPCM function, which will produce:
// a DPCM compressed file (as described in DPCM_encoding_function.c)
// an ABSOLUTE prediction error histogram data file (as described in DPCM_encoding_function.c)
// The main function needs to report the averages and the standard deviations of the ABSOLUTE prediction
// errors, as well as the compression time.
int main(int argc, char *argv[])
{
    if (argc != 3)
    {
        printf("There must be 2 arguments:\n");
        printf("1: <input filename>\n");
        printf("2: <prediction rule code> (1 for WEST, 2 for NORTH, 3 for WEST AND NORTH, 4 for CALIC )\n");

        return 0;
    }

    char *inputFilename = argv[1];
    int predictionRuleCode = atoi(argv[2]);

    float averageAbsoluteError = 0;
    float standardDeviationAbsoluteError = 0;

    Encode_Using_DPCM(inputFilename, predictionRuleCode, &averageAbsoluteError, &standardDeviationAbsoluteError);

    printf("Average absolute error: %f\n", averageAbsoluteError);
    printf("Standard deviation of absolute error: %f\n", standardDeviationAbsoluteError);

    return 0;
}