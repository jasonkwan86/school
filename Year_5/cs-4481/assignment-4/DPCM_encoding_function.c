#include <stdio.h>
#include <time.h>
#include <math.h>
#include <string.h>
#include <limits.h>
#include "libpnm.h"
#include "DPCM_encoding_function.h"

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

// Write the ABSOLUTE prediction error histogram data (in comma-separated values format, .csv) to a file with
// the same name as the input file name, but with the prediction rule code and “.errors.csv” concatenated
// to it.
// This file will have lines of “ABSOLUTE prediction error value, frequency” where ABSOLUTE prediction
// error values are recorded in an increasing order. You need to omit any line with zero frequency. Note that
// ABSOLUTE prediction error values  [0, MAX_GRAY_VALUE].
void savePredictionErrorHistogram(char *inputFileName, int predictionRule, int maxGrayValue, int *predictionErrorsArray, int predictionErrorsArraySize, float *averageAbsolutePredictionError, float *standardDeviationAbsolutePredictionError)
{
    char predictionRuleString[50];
    snprintf(predictionRuleString, 50, "%d", predictionRule);

    char histogramFileName[50] = "";
    strcat(histogramFileName, inputFileName);
    strcat(histogramFileName, ".");
    strcat(histogramFileName, predictionRuleString);
    strcat(histogramFileName, ".errors.csv");
    
    int predictionErrorsFrequencyArraySize = maxGrayValue + 1;
    int predictionErrorsFrequencyArray[predictionErrorsFrequencyArraySize];   // Index is prediction error, value is frequency
    memset(predictionErrorsFrequencyArray, 0, predictionErrorsFrequencyArraySize * sizeof(int));
    
    int sumOfAllPredictionErrors = 0;
    for (int i = 0; i < predictionErrorsArraySize; i++)
    {
        sumOfAllPredictionErrors += abs(predictionErrorsArray[i]);
        predictionErrorsFrequencyArray[predictionErrorsArray[i]]++;
    }
    *averageAbsolutePredictionError = (float)sumOfAllPredictionErrors / predictionErrorsArraySize;
    
    FILE *histogramFile;
    histogramFile = fopen(histogramFileName, "w");
    // Don't include prediction errors of 0
    for (int predictionError = 1; predictionError < predictionErrorsFrequencyArraySize; predictionError++)
    {
        if (predictionErrorsFrequencyArray[predictionError] != 0)
        {
            fprintf(histogramFile, "%d, %d\n", predictionError, predictionErrorsFrequencyArray[predictionError]);
            *standardDeviationAbsolutePredictionError += predictionErrorsFrequencyArray[predictionError] * pow(predictionError - *averageAbsolutePredictionError, 2);
        }
    }
    fclose(histogramFile);
    
    // Calculate prediction error standard deviation
    *standardDeviationAbsolutePredictionError /= predictionErrorsFrequencyArraySize;
    *standardDeviationAbsolutePredictionError = sqrt(*standardDeviationAbsolutePredictionError);

    return;
}

void saveCompressedFile(char *in_PGM_filename_Ptr, struct PGM_Image image, int prediction_rule, int *predictionArray, int predictionErrorArraySize)
{
    FILE *compressedFilePointer;
    char predictionRuleString[50];
    snprintf(predictionRuleString, 50, "%d", prediction_rule);

    char compressedFileName[50] = "";
    strcat(compressedFileName, in_PGM_filename_Ptr);
    strcat(compressedFileName, ".");
    strcat(compressedFileName, predictionRuleString);
    strcat(compressedFileName, ".DPCM");

    compressedFilePointer = fopen(compressedFileName, "w");

    // HEADER VALUES
    // Width Height
    // Max gray value
    // Prediction rule
    // Array size
    // List of prediction errors
    fprintf(compressedFilePointer, "%d %d\n", image.width, image.height);
    fprintf(compressedFilePointer, "%d\n", image.maxGrayValue);
    fprintf(compressedFilePointer, "%d\n", prediction_rule);
    fprintf(compressedFilePointer, "%d\n", predictionErrorArraySize);

    for (int i = 0; i < predictionErrorArraySize; i++)
    {
        fprintf(compressedFilePointer, "%d\n", predictionArray[i]);
    }
}

void Encode_Using_DPCM(char *in_PGM_filename_Ptr, int prediction_rule, float *avg_abs_error_Ptr, float *std_abs_error_Ptr)
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

    int numberOfPixels = image.width * image.height;
    int lastTernaryCode = -1;
    int numberOfTernaryCodesWritten = 0;
    int *predictionErrorAndTernaryCodeArray = calloc(numberOfPixels * 2, sizeof(int));
    int *onlyPredictionErrorArray = calloc(numberOfPixels, sizeof(int));

    // Error = Pixel - Prediction
    predictionErrorAndTernaryCodeArray[0] = image.image[0][0] - FIRST_PIXEL_PREDICTION;

    // Do first row
    for (int column = 1; column < image.width; column++)
    {
        // West for first row
        predictionErrorAndTernaryCodeArray[column] = image.image[0][column] - image.image[0][column - 1];
    }

    // Finish every row after first
    for (int row = 1; row < image.height; row++)
    {
        for (int column = 0; column < image.width; column++)
        {
            int currentArrayIndex = row * image.width + column + numberOfTernaryCodesWritten;
            int currentPixel = image.image[row][column];
            int rowColumnIndex = row * image.width + column;

            int n = image.image[row - 1][column];
            int w = image.image[row][column - 1];

            if (row == 1 || column == 0 || column == 1 || column == image.width - 1)
            {
                // Use North
                int predictionError = currentPixel - n;
                predictionErrorAndTernaryCodeArray[currentArrayIndex] = predictionError;
                onlyPredictionErrorArray[rowColumnIndex] = predictionError;
                continue;
            }
            if (prediction_rule == WEST)
            {
                // West
                int predictionError = currentPixel - w;
                predictionErrorAndTernaryCodeArray[currentArrayIndex] = predictionError;
                onlyPredictionErrorArray[rowColumnIndex] = predictionError;
            }
            else if (prediction_rule == NORTH)
            {
                // North
                int predictionError = currentPixel - n;
                predictionErrorAndTernaryCodeArray[currentArrayIndex] = predictionError;
                onlyPredictionErrorArray[rowColumnIndex] = predictionError;
            }
            else if (prediction_rule == WEST_AND_NORTH_HALF)
            {
                // W/2 + N/2
                int sumOfHalves = (w + n) / 2;
                int predictionError = currentPixel - sumOfHalves;
                predictionErrorAndTernaryCodeArray[currentArrayIndex] = predictionError;
                onlyPredictionErrorArray[rowColumnIndex] = predictionError;
            }
            else if (prediction_rule == CALIC)
            {
                // CALIC
                int prediction = 0;
                int ww = image.image[row][column - 2];
                int nw = image.image[row - 1][column - 1];
                int nn = image.image[row - 2][column];
                int ne = image.image[row - 1][column + 1];
                int nne = image.image[row - 2][column + 1];

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
                int predictionError = currentPixel - prediction;
                int directions[NUMBER_OF_DIRECTIONS] = {n, w, ww, nw, nn, ne, nne};
                int numberOfDistinctValues = uniqueElementsInArray(directions, NUMBER_OF_DIRECTIONS);
                if (numberOfDistinctValues <= 2)
                {
                    if (currentPixel == w)
                    {
                        predictionErrorAndTernaryCodeArray[currentArrayIndex] = WEST_TERNARY_CODE;
                        lastTernaryCode = WEST_TERNARY_CODE;
                    }
                    else if (currentPixel == n)
                    {
                        predictionErrorAndTernaryCodeArray[currentArrayIndex] = NORTH_TERNARY_CODE;
                        lastTernaryCode = NORTH_TERNARY_CODE;
                    }
                    else
                    {
                        // Exit to continuous mode
                        predictionErrorAndTernaryCodeArray[currentArrayIndex] = EXIT_BINARY_TERNARY_CODE;
                        predictionErrorAndTernaryCodeArray[currentArrayIndex + 1] = predictionError;
                        lastTernaryCode = EXIT_BINARY_TERNARY_CODE;
                        onlyPredictionErrorArray[rowColumnIndex] = predictionError;
                    }
                    numberOfTernaryCodesWritten++;
                }
                else
                {
                    // We are already in continuous mode, just send the prediction
                    if (lastTernaryCode == EXIT_BINARY_TERNARY_CODE)
                    {
                        predictionErrorAndTernaryCodeArray[currentArrayIndex] = predictionError;
                    }
                    // We are leaving binary and entering continuous mode, send code and prediction
                    else
                    {
                        predictionErrorAndTernaryCodeArray[currentArrayIndex] = EXIT_BINARY_TERNARY_CODE;
                        predictionErrorAndTernaryCodeArray[currentArrayIndex + 1] = predictionError;
                        lastTernaryCode = EXIT_BINARY_TERNARY_CODE;
                        numberOfTernaryCodesWritten++;
                        onlyPredictionErrorArray[rowColumnIndex] = predictionError;
                    }
                }
            }
        }
    }
    
    saveCompressedFile(in_PGM_filename_Ptr, image, prediction_rule, predictionErrorAndTernaryCodeArray, numberOfPixels + numberOfTernaryCodesWritten);

    endTime = clock(); 
    totalTime = (double)(endTime - startTime) / CLOCKS_PER_SEC;

    savePredictionErrorHistogram(in_PGM_filename_Ptr, prediction_rule, image.maxGrayValue, onlyPredictionErrorArray, numberOfPixels, avg_abs_error_Ptr, std_abs_error_Ptr);

    printf("Time spent encoding: %f seconds\n", totalTime);
}
