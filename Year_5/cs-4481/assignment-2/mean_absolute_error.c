#include <stdlib.h>
#include "libpnm.h"

float min(float a, float b) { return (a < b) ? a : b; }
float max(float a, float b) { return (a > b) ? a : b; }

// The function should return a float value that represents the mean absolute error between the two provided images.
// If the two images are not PGMs of the same size, the function should return -1.
// If the max_gray_values of the two images are not the same, you need to scale the image with the smaller
// max_gray_value to make its max_gray_value as the larger value
float mean_absolute_error(char *file_name_1_ptr, char *file_name_2_ptr)
{
    struct PGM_Image image1;
    struct PGM_Image image2;

    int img1Result = load_PGM_Image(&image1, file_name_1_ptr);
    int img2Result = load_PGM_Image(&image2, file_name_2_ptr);

    bool hasUnequalMaxGrayValues = image1.maxGrayValue != image2.maxGrayValue;

    struct PGM_Image *imageWithSmallerMaxGrayValue;
    struct PGM_Image *imageWithGreaterMaxGrayValue;
    
    if (img1Result != 0)
    {
        printf("Could not open file 1.");
        return -1;
    }
    if (img2Result != 0)
    {
        printf("Could not open file 2.");
        return -1;
    }
    if (image1.width != image2.width)
    {
        printf("Images are not the same width.");
        return -1;
    }
    if (image1.height != image2.height)
    {
        printf("Images are not the same height.");
        return -1;
    }
    if (hasUnequalMaxGrayValues)
    {
        if (image1.maxGrayValue > image2.maxGrayValue)
        {
            imageWithGreaterMaxGrayValue = &image1;
            imageWithSmallerMaxGrayValue = &image2;
        }
        else
        {
            imageWithGreaterMaxGrayValue = &image2;
            imageWithSmallerMaxGrayValue = &image1;
        }
    }

    float finalDifference = 0;
    for (int row = 0; row < image1.height; row++)
    {
        for (int column = 0; column < image1.width; column++)
        {
            float currentDifference = 0;
            if (hasUnequalMaxGrayValues)
            {
                float grayWithLesserMax = imageWithSmallerMaxGrayValue->image[row][column];
                float grayWithGreaterMax = imageWithGreaterMaxGrayValue->image[row][column];

                currentDifference = abs(grayWithGreaterMax - grayWithLesserMax * imageWithGreaterMaxGrayValue->maxGrayValue / imageWithSmallerMaxGrayValue->maxGrayValue);
            }
            else
            {
                currentDifference = abs(image1.image[row][column] - image2.image[row][column]);
            }
            finalDifference += currentDifference;
        }
    }

    long int dataPoints = image1.height * image1.width;

    return finalDifference / dataPoints;
}