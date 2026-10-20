#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <time.h>
#include <limits.h>
#include "libpnm.h"
#include "DPCM_encoding_function.h"
#include "DPCM_decoding_function.h"

int uniqueElementsInArray(int *array, int arrayMaxSize)
{
    int DEFAULT = INT_MAX;
    int uniqueArray[arrayMaxSize];
    memset(uniqueArray, DEFAULT, arrayMaxSize * sizeof(int));
    int uniqueElementsCount = 0;
    for (int i = 0; i < arrayMaxSize; i++)
    {
        bool elementSeenBefore = false;
        for (int k = 0; k < arrayMaxSize; k++)
        {
            if (array[i] == uniqueArray[k])
            {
                elementSeenBefore = true;
                break;
            }
        }

        if (!elementSeenBefore)
        {
            // Add to unique elements array
            uniqueArray[uniqueElementsCount] = array[i];
            uniqueElementsCount++;
        }
    }

    return uniqueElementsCount;
}

void readCompressedFile(char *in_compressed_filename_Ptr, int *width, int *height, int *maxGrayValue, int *predictionRule, int **predictionErrorArray, int *numberOfArrayElements)
{
    FILE *compressedFilePointer;
    compressedFilePointer = fopen(in_compressed_filename_Ptr, "r");

    if (compressedFilePointer == NULL)
    {
        printf("Failed to read compressed file %s\n", in_compressed_filename_Ptr);
    }

    // 1. Image width, height
    // 2. Max gray value
    // 3. Prediction Rule
    // 4. Number of array elements
    // 5. Prediction Array, + t array if prediction_rule == 4 (CALIC)
    fscanf(compressedFilePointer, "%d %d\n", width, height);
    fscanf(compressedFilePointer, "%d\n", maxGrayValue);
    fscanf(compressedFilePointer, "%d\n", predictionRule);
    fscanf(compressedFilePointer, "%d\n", numberOfArrayElements);

    int arrayMemorySize = *numberOfArrayElements * sizeof(int);
    int *tempPredictionErrorArray = malloc(arrayMemorySize * 2);

    for (int i = 0; i < *numberOfArrayElements; i++)
    {
        fscanf(compressedFilePointer, "%d\n", &tempPredictionErrorArray[i]);
    }
    *predictionErrorArray = tempPredictionErrorArray;

    fclose(compressedFilePointer);
}

void decodeAndSaveCompressedFile(char *in_filename_Ptr, int width, int height, int maxGrayValue, int predictionRule, int *predictionErrorAndTernaryCodeArray, int numberOfArrayElements)
{
    clock_t startTime = clock(), endTime;
    double totalTime;

    char decompressedFileName[50] = "";
    strcat(decompressedFileName, in_filename_Ptr);
    strcat(decompressedFileName, ".decompressed");

    struct PGM_Image *image = malloc(sizeof(struct PGM_Image));
    create_PGM_Image(image, width, height, maxGrayValue);

    int numberOfTernaryCodesWritten = 0;
    int lastTernaryCode = -1;

    // Actual = Prediction + Prediction error
    image->image[0][0] = FIRST_PIXEL_PREDICTION + predictionErrorAndTernaryCodeArray[0];

    // Process first row
    for (int column = 1; column < width; column++)
    {
        // West for first row
        image->image[0][column] = image->image[0][column - 1] + predictionErrorAndTernaryCodeArray[column];
    }

    // Process second row until end of the image
    for (int row = 1; row < height; row++)
    {
        for (int column = 0; column < width; column++)
        {
            int currentArrayIndex = row * width + column + numberOfTernaryCodesWritten;
            int currentPredictionErrorOrTernaryCode = predictionErrorAndTernaryCodeArray[currentArrayIndex];

            int n = image->image[row - 1][column];
            int w = image->image[row][column - 1];

            if (row == 1 || column == 0 || column == 1 || column == width - 1)
            {
                // Use North
                image->image[row][column] = n + currentPredictionErrorOrTernaryCode;
                continue;
            }
            if (predictionRule == WEST)
            {
                // West
                image->image[row][column] = w + currentPredictionErrorOrTernaryCode;
            }
            else if (predictionRule == NORTH)
            {
                // North
                image->image[row][column] = n + currentPredictionErrorOrTernaryCode;
            }
            else if (predictionRule == WEST_AND_NORTH_HALF)
            {
                // W/2 + N/2
                int sumOfHalves = (w + n) / 2;
                image->image[row][column] = sumOfHalves + currentPredictionErrorOrTernaryCode;
            }
            else if (predictionRule == CALIC)
            {
                // CALIC
                int prediction = 0;
                int ww = image->image[row][column - 2];
                int nw = image->image[row - 1][column - 1];
                int nn = image->image[row - 2][column];
                int ne = image->image[row - 1][column + 1];
                int nne = image->image[row - 2][column + 1];

                int dh = abs(ww - w) + abs(nw - n) + abs(n - ne);
                int dv = abs(nw - w) + abs(nn - n) + abs(nne + ne);

                int dv_dh = dv - dh;
                int dh_dv = dh - dv;

                if (dv_dh > 80)
                {
                    prediction = w;
                }
                else
                {
                    if (dh_dv > 80)
                    {
                        prediction = n;
                    }
                    else
                    {
                        int wn_over_2 = (w + n) / 2;
                        int ne_nw_over_4 = (ne - nw) / 4;
                        prediction = wn_over_2 + ne_nw_over_4;
                        if (dv_dh > 32)
                        {
                            prediction = 1 / 2 * prediction + 1 / 2 * w;
                        }
                        else if (dh_dv > 32)
                        {
                            prediction = 1 / 2 * prediction + 1 / 2 * n;
                        }
                        else if (dv_dh > 8)
                        {
                            prediction = 3 / 4 * prediction + 1 / 4 * w;
                        }
                        else if (dh_dv > 8)
                        {
                            prediction = 3 / 4 * prediction + 1 / 4 * n;
                        }
                    }
                }
                int directions[NUMBER_OF_DIRECTIONS] = {n, w, ww, nw, nn, ne, nne};
                int numberOfDistinctValues = uniqueElementsInArray(directions, NUMBER_OF_DIRECTIONS);
                if (numberOfDistinctValues <= 2)
                {
                    if (currentPredictionErrorOrTernaryCode == WEST_TERNARY_CODE)
                    {
                        image->image[row][column] = w;
                        lastTernaryCode = WEST_TERNARY_CODE;
                    }
                    else if (currentPredictionErrorOrTernaryCode == NORTH_TERNARY_CODE)
                    {
                        image->image[row][column] = n;
                        lastTernaryCode = NORTH_TERNARY_CODE;
                    }
                    else
                    {
                        // Exit to continuous mode
                        image->image[row][column] = prediction + predictionErrorAndTernaryCodeArray[currentArrayIndex + 1];
                        lastTernaryCode = EXIT_BINARY_TERNARY_CODE;
                    }
                    numberOfTernaryCodesWritten++;
                }
                else
                {
                    // We are already in continuous mode, just send the prediction
                    if (lastTernaryCode == EXIT_BINARY_TERNARY_CODE)
                    {
                        image->image[row][column] = prediction + currentPredictionErrorOrTernaryCode;
                    }
                    // We are leaving binary and entering continuous mode, sent code and prediction
                    else
                    {
                        image->image[row][column] = prediction + predictionErrorAndTernaryCodeArray[currentArrayIndex + 1];
                        lastTernaryCode = EXIT_BINARY_TERNARY_CODE;
                        numberOfTernaryCodesWritten++;
                    }
                }
            }
        }
    }

    char outputFilename[50] = "";
    strcat(outputFilename, in_filename_Ptr);
    strcat(outputFilename, ".pgm");
    save_PGM_Image(image, outputFilename, 1);
    
    endTime = clock(); 
    totalTime = (double)(endTime - startTime) / CLOCKS_PER_SEC;

    printf("Time spent decoding: %f seconds\n", totalTime);
}

// This function should:
// o Read from disk *in_filename_Ptr lossless DPCM encoded image, which is generated using the
// Encode_Using_DPCM function.
// o Interpret the header that you saved it at the beginning of the file
// o Decode all pixel values
// o Write the reconstructed version of the image to a file with the same name as the input file name, but with
// “.pgm” concatenated to it
void Decode_Using_DPCM (char *in_filename_Ptr)
{
    int width, height, maxGrayValue, predictionRule, numberOfArrayElements;
    int *predictionErrorArray;
    readCompressedFile(in_filename_Ptr, &width, &height, &maxGrayValue, &predictionRule, &predictionErrorArray, &numberOfArrayElements);
    decodeAndSaveCompressedFile(in_filename_Ptr, width, height, maxGrayValue, predictionRule, predictionErrorArray, numberOfArrayElements);
}
