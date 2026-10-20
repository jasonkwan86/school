#include <stdio.h>
#include <string.h>
#include <time.h>
#include "libpnm.h"
#include "lz77_encoding_function.h"
#include "lz77_decoding_function.h"

void readlz77CompressedFile(char *in_compressed_filename_Ptr, int *width, int *height, int *maxGrayValue, struct lz77token **tokenArray, int *tokenArraySize)
{
    FILE *filePointer;
    filePointer = fopen(in_compressed_filename_Ptr, "r");

    if (filePointer == NULL)
    {
        printf("Error reading compressed file %s.\n", in_compressed_filename_Ptr);
    }

    // HEADER VALUES
    // Width Height
    // Max gray value
    // Token array size
    // List of tokens
    fscanf(filePointer, "%d %d", width, height);
    fscanf(filePointer, "%d", maxGrayValue);
    fscanf(filePointer, "%d", tokenArraySize);

    struct lz77token *tokens = malloc(*tokenArraySize * sizeof(struct lz77token));
    for (int i = 0; i < *tokenArraySize; i++)
    {
        fscanf(filePointer, "%d %d %d", &tokens[i].offset, &tokens[i].matchLength, &tokens[i].nextMismatchedSymbol);
    }

    *tokenArray = tokens;
    fclose(filePointer);
}

struct PGM_Image *decodeLz77Tokens(int width, int height, int maxGrayValue, struct lz77token *tokenArray, int tokenArraySize)
{
    clock_t startTime = clock(), endTime;
    double totalTime;
    
    struct PGM_Image *image = malloc(sizeof(struct PGM_Image));
    create_PGM_Image(image, width, height, maxGrayValue);

    int pixelsWritten = 0;
    // Pixel array will be our searching buffer 
    int numberOfPixels = width * height;
    int *pixelArray = malloc(numberOfPixels * sizeof(int));

    // Read tokens array
    for (int i = 0; i < tokenArraySize; i++)
    {
        int offset = tokenArray[i].offset;
        int matchLength = tokenArray[i].matchLength;

        // Step 1: Add subset
        int copyFromStartIndex = pixelsWritten - offset;
        for (int subArrayIndex = 0; subArrayIndex < matchLength; subArrayIndex++)
        {
            pixelArray[pixelsWritten + subArrayIndex] = pixelArray[copyFromStartIndex + subArrayIndex];
        }

        // Step 2: Add mismatched symbol
        pixelArray[pixelsWritten + matchLength] = tokenArray[i].nextMismatchedSymbol;
        pixelsWritten += matchLength + 1;
    }

    for (int currentPixel = 0; currentPixel < numberOfPixels; currentPixel++)
    {
        int row = currentPixel / width, column = currentPixel % width;
        image->image[row][column] = pixelArray[currentPixel];
    }

    endTime = clock();
    totalTime = (double)(endTime - startTime) / CLOCKS_PER_SEC * 1000;
    printf("Total decoding time in ms: %f\n", totalTime);

    return image;
}

// Read from a file called *in_compressed_filename_Ptr an LZ77 encoded image, which is generated using
// the Encode_Using_LZ77 function.
// Interpret the header that you saved at the beginning of the file
// Decode all pixel values.
// Write the reconstructed version of the image to a file with the same name as the input file name, but with
// “.pgm” concatenated to it.
void Decode_Using_LZ77(char *in_compressed_filename_Ptr)
{
    int width, height, maxGrayValue, tokenArraySize;
    struct lz77token *tokenArray;

    readlz77CompressedFile(in_compressed_filename_Ptr, &width, &height, &maxGrayValue, &tokenArray, &tokenArraySize);
    
    struct PGM_Image *image = decodeLz77Tokens(width, height, maxGrayValue, tokenArray, tokenArraySize);

    char outputFilename[50] = "";
    strcat(outputFilename, in_compressed_filename_Ptr);
    strcat(outputFilename, ".pgm");
    save_PGM_Image(image, outputFilename, 1);

    return;
}
