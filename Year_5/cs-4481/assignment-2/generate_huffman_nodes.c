#include "libpnm.h"
#include "generate_huffman_nodes.h"

#define DEFAULT_GRAY_INDEX -1

// Should utilize the input data to generate all Huffman nodes in the form of ordered pairs of pixel
// values, where a node is declared as: struct node {int first_value; int second_value;};
// The number of nodes is less by one than number_of_non_zero_values_in_the_frequency_array. The function will
// return a pointer to a node array which contains all the Huffman nodes in order (see the Huffman lecture for more
// details).

// If max_gray_value = 255, then
// pixel_frequency.size == 255 + 1 == 256
// pixel_frequency[0] contains the frequency of pixel value 0
// ...
// pixel_frequency[255] contains the frequency of pixel value 255

long int min(long int a, long int b) { return (a < b) ? a : b; }
long int max(long int a, long int b) { return (a > b) ? a : b; }

struct node *generate_huffman_nodes(long int *pixel_frequency, int max_gray_value, int number_of_non_zero_values_in_the_frequency_array)
{
    if (number_of_non_zero_values_in_the_frequency_array == 1)
    {
        struct node *huffmanNodes = malloc(sizeof(struct node));
        // Get the only value that occurs
        for (int gray = 0; gray < max_gray_value; gray++)
        {
            if (pixel_frequency[gray] != 0)
            {
                huffmanNodes->first_value = gray;

                // Not read, just set so its not random
                huffmanNodes->second_value = 0;
                break;
            }
        }

        return huffmanNodes;
    }

    int numberOfHuffmanNodes = number_of_non_zero_values_in_the_frequency_array - 1;
    // Also equal to number of unique pixels
    int sizeOfFrequencyArray = max_gray_value + 1;
    struct node *huffmanNodes = malloc(numberOfHuffmanNodes * sizeof(struct node));

    // Gray value is the index of pixel_frequency
    for (int grayValue = 0; grayValue < numberOfHuffmanNodes; grayValue++)
    {
        int leastFrequentIndex = DEFAULT_GRAY_INDEX, secondLeastFrequentIndex = DEFAULT_GRAY_INDEX;

        for (int index = 0; index < sizeOfFrequencyArray; index++)
        {
            long int currentPixelFrequency = pixel_frequency[index];
            if (currentPixelFrequency == 0)
            {
                continue;
            }
            if (leastFrequentIndex == DEFAULT_GRAY_INDEX || currentPixelFrequency < pixel_frequency[leastFrequentIndex])
            {
                leastFrequentIndex = index;
            }
        }

        for (int index = 0; index < sizeOfFrequencyArray; index++)
        {
            long int currentPixelFrequency = pixel_frequency[index];
            // Second least frequent value can be same as first, but index must be different
            if (currentPixelFrequency == 0 || index == leastFrequentIndex)
            {
                continue;
            }
            if (secondLeastFrequentIndex == DEFAULT_GRAY_INDEX || currentPixelFrequency < pixel_frequency[secondLeastFrequentIndex])
            {
                secondLeastFrequentIndex = index;
            }
        }

        if (leastFrequentIndex == DEFAULT_GRAY_INDEX || secondLeastFrequentIndex == DEFAULT_GRAY_INDEX)
        {
            printf("CRITICAL ERROR. LEAST OR SECOND LEAST FREQUENT INDEX IS STILL DEFAULT.\n");
            return NULL;
        }

        if (leastFrequentIndex == secondLeastFrequentIndex)
        {
            printf("CRITICAL ERROR. LEAST FREQUENT INDEX IS SAME AS SECOND LEAST.\n");
            return NULL;
        }

        // Get the lowest gray value index, not based on frequency
        int smallestIndex = min(leastFrequentIndex, secondLeastFrequentIndex);
        int secondSmallestIndex = max(leastFrequentIndex, secondLeastFrequentIndex);

        huffmanNodes[grayValue].first_value = smallestIndex;
        huffmanNodes[grayValue].second_value = secondSmallestIndex;

        // update smallest index with sum of frequencies, erase the largest index
        pixel_frequency[smallestIndex] = pixel_frequency[leastFrequentIndex] + pixel_frequency[secondLeastFrequentIndex];
        pixel_frequency[secondSmallestIndex] = 0;
    }

    return huffmanNodes;
}

