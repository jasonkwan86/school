#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <time.h>
#include <limits.h>
#include "libpnm.h"
#include "lz77_encoding_function.h"

#define UNDEFINED -1

int max(int a, int b) { return (a > b) ? a : b; }

int getLargestOffset(struct lz77token *tokens, int tokenArraySize)
{
    int largestOffset = INT_MIN;
    for (int i = 0; i < tokenArraySize; i++)
    {
        if (tokens[i].offset > largestOffset)
        {
            largestOffset = tokens[i].offset;
        }
    }
    if (largestOffset == INT_MIN)
    {
        printf("ERROR. LARGEST OFFSET IS STILL INT_MIN.\n");
    }

    return largestOffset;
}

int getLargestMatchLength(struct lz77token *tokens, int tokenArraySize)
{
    int largestMatchLength = INT_MIN;
    for (int i = 0; i < tokenArraySize; i++)
    {
        if (tokens[i].matchLength > largestMatchLength)
        {
            largestMatchLength = tokens[i].offset;
        }
    }
    if (largestMatchLength == INT_MIN)
    {
        printf("ERROR. LARGEST MATCH LENGTH IS STILL INT_MIN.\n");
    }

    return largestMatchLength;
}

// Write the compressed image to a file with the same name as the input file name, but with the searching
// buffer size and “.lz” concatenated to it. This compressed file should include:
//  A header that includes enough information to allow the decoder to decompress the compressed image.
//  An array for all offsets generated from the LZ77 compression.
//  An array for all matching_lengths generated from the LZ77 compression.
//  An array for all next_symbols generated from the LZ77 compression.
void SaveCompressedFile(char *in_PGM_filename_Ptr, char *searchingBufferSize, struct lz77token *tokens, int tokenArraySize, int width, int height, int maxGrayValue)
{
    FILE *filePointer;
    char compressedFileName[50] = "";
    strcat(compressedFileName, in_PGM_filename_Ptr);
    strcat(compressedFileName, ".");
    strcat(compressedFileName, searchingBufferSize);
    strcat(compressedFileName, ".lz");

    filePointer = fopen(compressedFileName, "w");

    // HEADER VALUES
    // Width Height
    // Max gray value
    // Token array size
    // List of tokens
    fprintf(filePointer, "%d %d\n", width, height);
    fprintf(filePointer, "%d\n", maxGrayValue);
    fprintf(filePointer, "%d\n", tokenArraySize);

    for (int i = 0; i < tokenArraySize; i++)
    {
        fprintf(filePointer, "%d %d %d\n", tokens[i].offset, tokens[i].matchLength, tokens[i].nextMismatchedSymbol);
    }
    fclose(filePointer);

    return;
}

// Write the offset histogram data (in comma-separated values format, .csv) to a file with the same name as
// the input file name, but with the searching buffer size and “.offsets.csv” concatenated to it.
// This file will have lines of “offset value, frequency” where offset values are recorded in increasing order.
// You need to omit any line with zero frequency.
void CalculateAndSaveOffsetHistogramData(char *in_PGM_filename_Ptr, char *searchingBufferSize, struct lz77token *tokens, int tokenArraySize, float *averageOffset, float *standardDeviationOffset)
{
    *averageOffset = 0;
    FILE *filePointer;
    char fileName[50] = "";
    strcat(fileName, in_PGM_filename_Ptr);
    strcat(fileName, ".");
    strcat(fileName, searchingBufferSize);
    strcat(fileName, ".offsets.csv");

    filePointer = fopen(fileName, "w");

    long int sumOfAllOffsets = 0;
    int offsetsFrequencyArrayLength = getLargestOffset(tokens, tokenArraySize) + 1;
    int offsetsFrequencyArray[offsetsFrequencyArrayLength];
    memset(offsetsFrequencyArray, 0, offsetsFrequencyArrayLength * sizeof(int));
    for (int i = 0; i < tokenArraySize; i++)
    {
        sumOfAllOffsets += tokens[i].offset;
        offsetsFrequencyArray[tokens[i].offset]++;
    }
    for (int i = 0; i < offsetsFrequencyArrayLength; i++)
    {
        if (offsetsFrequencyArray[i] != 0)
        {
            fprintf(filePointer, "%d, %d\n", i, offsetsFrequencyArray[i]);
        }
    }
    fclose(filePointer);
    *averageOffset = (float)sumOfAllOffsets / tokenArraySize;

    // Calculate match length standard deviation
    for (int offset = 0; offset < offsetsFrequencyArrayLength; offset++)
    {
        if (offsetsFrequencyArray[offset] != 0)
        {
            *standardDeviationOffset += offsetsFrequencyArray[offset] * pow(*averageOffset - offset, 2);
        }
    }
    *standardDeviationOffset /= tokenArraySize;
    *standardDeviationOffset = sqrt(*standardDeviationOffset);

    return;
}

// Write the match length histogram data (in comma-separated values format, .csv) to a file with the same
// name as the input file name, but with the searching buffer size and “.lengths.csv” concatenated to it.
// This file will have lines of “length value, frequency” where length values are recorded in increasing order.
// You need to omit any line with zero frequency.
void SaveMatchLengthHistogramData(char *in_PGM_filename_Ptr, char *searchingBufferSize, struct lz77token *tokens, int tokenArraySize, float *averageMatchLength, float *standardDeviationMatchLength)
{
    FILE *filePointer;
    char fileName[50] = "";
    strcat(fileName, in_PGM_filename_Ptr);
    strcat(fileName, ".");
    strcat(fileName, searchingBufferSize);
    strcat(fileName, ".lengths.csv");

    filePointer = fopen(fileName, "w");

    long int sumOfAllMatchLengths = 0;
    int matchLengthsFrequencyArraySize = getLargestMatchLength(tokens, tokenArraySize) + 1;
    int matchLengthsFrequencyArray[matchLengthsFrequencyArraySize];   // Index is match length, value is frequency
    memset(matchLengthsFrequencyArray, 0, matchLengthsFrequencyArraySize * sizeof(int));

    for (int i = 0; i < tokenArraySize; i++)
    {
        sumOfAllMatchLengths += tokens[i].matchLength;
        matchLengthsFrequencyArray[tokens[i].matchLength]++;
    }
    for (int i = 0; i < matchLengthsFrequencyArraySize; i++)
    {
        if (matchLengthsFrequencyArray[i] != 0)
        {
            fprintf(filePointer, "%d, %d\n", i, matchLengthsFrequencyArray[i]);
        }
    }
    fclose(filePointer);
    *averageMatchLength = (float)sumOfAllMatchLengths / tokenArraySize;
    
    // Calculate match length standard deviation
    for (int matchLength = 0; matchLength < matchLengthsFrequencyArraySize; matchLength++)
    {
        if (matchLengthsFrequencyArray[matchLength] != 0)
        {
            *standardDeviationMatchLength += matchLengthsFrequencyArray[matchLength] * pow(matchLength - *averageMatchLength, 2);
        }
    }
    *standardDeviationMatchLength /= tokenArraySize;
    *standardDeviationMatchLength = sqrt(*standardDeviationMatchLength);

    return;
}

// Function should return
// Offset and match length
int searchingBufferContainsTestMatchLengthAt(int *toBeEncodedDataArray, int toBeEncodedDataArraySize, int pixelsReadSoFar, int searchingBufferSize, int testMatchLength)
{
    int smallestIndexOfSearchBuffer = max(0, pixelsReadSoFar - searchingBufferSize);

    // Start searching at offset = pixelsReadSoFar - testMatchLength, lower it if no match is found
    for (int currentOffset = pixelsReadSoFar - testMatchLength; currentOffset >= smallestIndexOfSearchBuffer; currentOffset--)
    {
        bool isSubstring = true;
        // Check if substring matches slice of search buffer
        for (int currentTestMatchLength = 0; currentTestMatchLength < testMatchLength; currentTestMatchLength++)
        {
            // printf("Comparing %d to %d\n", toBeEncodedDataArray[currentOffset + currentTestMatchLength], toBeEncodedDataArray[pixelsReadSoFar + currentTestMatchLength]);
            // Compare part of searching buffer vs current test match length
            if (toBeEncodedDataArray[currentOffset + currentTestMatchLength] != toBeEncodedDataArray[pixelsReadSoFar + currentTestMatchLength])
            {
                isSubstring = false;
                break;
            }
        }
        if (isSubstring)
        {
            return currentOffset;
        }
    }

    return UNDEFINED;
}

// Read from file *in_PGM_filename_Ptr a PGM image
// Generate an LZ77 compressed file, where the maximum size of the searching buffer is
// searching_buffer_size. During encoding, if you find more than one match with the same length, select the
// one that has the smaller offset. Do not apply codeword encoding.
// Finally, the function will calculate the average and the standard deviation of the offsets and the match
// lengths and store them in *avg_offset_Ptr, *std_offset_Ptr, * avg_length_Ptr, and *std_length_Ptr,
// respectively
void Encode_Using_LZ77(char *in_PGM_filename_Ptr, unsigned int searching_buffer_size, float *avg_offset_Ptr, float *std_offset_Ptr, float *avg_length_Ptr, float *std_length_Ptr)
{
    clock_t startTime = clock(), endTime;
    double totalTime;
    
    struct PGM_Image image;
    int loadImageResult = load_PGM_Image(&image, in_PGM_filename_Ptr);

    if (loadImageResult != 0)
    {
        printf("There was an error loading the pgm image: %s.\n", in_PGM_filename_Ptr);
        return;
    }
    
    int totalNumberOfPixels = image.width * image.height;
    size_t pixelsToBeEncodedArraySize = totalNumberOfPixels * sizeof(int);
    
    int *pixelsToBeEncoded = malloc(pixelsToBeEncodedArraySize);
    for (int row = 0; row < image.height; row++)
    {
        for (int column = 0; column < image.width; column++)
        {
            pixelsToBeEncoded[row * image.width + column] = image.image[row][column];
        }
    }
    
    // Number of pixels is the upperbound of tokens
    struct lz77token *tokens = calloc(totalNumberOfPixels, sizeof(struct lz77token));
    
    long int pixelsReadSoFar = 0;
    int tokenArraySize = 0;
    while (pixelsReadSoFar < totalNumberOfPixels)
    {
        long int currentOffset = 0;
        long int currentMatchLength = 0;

        // Find longest match that is a suffix of search buffer
        for (int testingMatchLength = 1; testingMatchLength < searching_buffer_size; testingMatchLength++)
        {
            // Increase length of sliding window until we find the largest subset of the searching buffer
            // Testing match will be small array
            int subarrayIndex = searchingBufferContainsTestMatchLengthAt(pixelsToBeEncoded, totalNumberOfPixels, pixelsReadSoFar, searching_buffer_size, testingMatchLength);
            if (subarrayIndex != UNDEFINED)
            {
                currentOffset = pixelsReadSoFar - subarrayIndex;
                currentMatchLength = testingMatchLength;
            }
            else
            {
                // We found our longest subarray
                break;
            }
        }
        // Fill searching buffer from left to right instead of right to left
        tokens[tokenArraySize].offset = currentOffset;
        tokens[tokenArraySize].matchLength = currentMatchLength;
        tokens[tokenArraySize].nextMismatchedSymbol = pixelsToBeEncoded[pixelsReadSoFar + currentMatchLength];
        tokenArraySize++;
        pixelsReadSoFar += currentMatchLength + 1;
    }

    endTime = clock();
    totalTime = (double)(endTime - startTime) / CLOCKS_PER_SEC * 1000;
    printf("Total encoding time in ms: %f\n", totalTime);

    char searchingBufferSizeString[20];
    sprintf(searchingBufferSizeString, "%d", searching_buffer_size);
    SaveCompressedFile(in_PGM_filename_Ptr, searchingBufferSizeString, tokens, tokenArraySize, image.width, image.height, image.maxGrayValue);
    CalculateAndSaveOffsetHistogramData(in_PGM_filename_Ptr, searchingBufferSizeString, tokens, tokenArraySize, avg_offset_Ptr, std_offset_Ptr);
    SaveMatchLengthHistogramData(in_PGM_filename_Ptr, searchingBufferSizeString, tokens, tokenArraySize, avg_length_Ptr, std_length_Ptr);

    return;
}
