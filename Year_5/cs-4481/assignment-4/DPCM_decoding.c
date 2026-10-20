#include <stdio.h>
#include "DPCM_decoding_function.h"

int main(int argc, char *argv[])
{
    if (argc != 2)
    {
        printf("There must be 1 argument:\n");
        printf("1: <input filename> ()\n");

        return 0;
    }

    char *inputFileName = argv[1];

    Decode_Using_DPCM(inputFileName);
}