#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "libpnm.h"

// Dynamically allocate a long int array of (max_gray_value + 1) elements,
// Read ALL pixels in the provided PGM image and record the frequency of occurrence of each pixel value in the
// allocated array
// Store in the *number_of_non_zero_values_in_the_frequency_array the number of non-zero values in the
// frequency array
// Return a pointer to the allocated array
long int *generate_pixel_frequency(struct PGM_Image *input_pgm_image, int *number_of_non_zero_values_in_the_frequency_array)
{
    int numberOfUniquePixels = input_pgm_image->maxGrayValue + 1;

    // Allocate and set all array values to 0
    long int *pixelFrequency = (long int *)calloc(numberOfUniquePixels, sizeof(long int));

    if (pixelFrequency == NULL)
    {
        printf("Memory allocation when generating pixel frequency failed.\n");
    }

    for (int row = 0; row < input_pgm_image->height; row++)
    {
        for (int col = 0; col < input_pgm_image->width; col++)
        {
            int pixelValue = input_pgm_image->image[row][col];
            pixelFrequency[pixelValue]++;
        }
    }

    for (int i = 0; i < numberOfUniquePixels; i++)
    {
        if (pixelFrequency[i] > 0)
        {
            (*number_of_non_zero_values_in_the_frequency_array)++;
        }
    }

    return pixelFrequency;
}

