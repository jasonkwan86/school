#include <stdio.h>
#include <limits.h>
#include "libpnm.h"
#include "lz77_encoding_function.h"

int main(int argc, char *argv[])
{
    if (argc != 3)
    {
        printf("There must be 2 arguments:\n");
        printf("1: <input filename> ()\n");
        printf("2: <searching buffer size> ()\n");

        return 0;
    }

    char *inputFilename = argv[1];
    int searchingBufferSize = atoi(argv[2]);

    // Main function should report 
    // - averages and standard deviations of both the offsets and the match lengths, as well as the compression times
    float averageOffset;
    float standardDeviationOffset;
    float averageLength;
    float standardDeviationLength;

    Encode_Using_LZ77(inputFilename, searchingBufferSize, &averageOffset, &standardDeviationOffset, &averageLength, &standardDeviationLength);

    printf("Offset Average: %f\n", averageOffset);
    printf("Offset Standard Deviation: %f\n", standardDeviationOffset);
    printf("Match Length Average: %f\n", averageLength);
    printf("Match Length Standard Deviation: %f\n", standardDeviationLength);

    return 0;
}
