#include <stdio.h>
#include <string.h>
#include "libpnm.h"
#include "generate_pixel_frequency.h"
#include "generate_huffman_nodes.h"
#include "huffman_encode_image.h"
#include "store_huffman_encoded_data.h"

// calls the functions developed in Q1, Q2, Q3, and Q4
// to Huffman encode an input PGM image and store the compressed image in a file.

int main(int argc, char *argv[])
{
    if (argc != 3)
    {
        printf("There must be 2 arguments:\n");
        printf("1: <input filename> ()\n");
        printf("2: <output compressed filename> ()\n");
        
        return 0;
    }

    char *inputFilename = argv[1];
    char *outputFilename = argv[2];

    struct PGM_Image image;
    int result = load_PGM_Image(&image, inputFilename);

    if (result != 0)
    {
        printf("There was an error loading the pgm image.");
    }

    int numberOfNonZeroValues = 0;
    long int lengthOfEncodedImageArray = 0;
    long int *pixelFrequencyArray = generate_pixel_frequency(&image, &numberOfNonZeroValues);
    // Return 1 if there is only 1 value that occurs as a special case
    int numberOfHuffmanNodes = numberOfNonZeroValues == 1 ? 1 : numberOfNonZeroValues - 1;

    struct node *huffmanNodes = generate_huffman_nodes(pixelFrequencyArray, image.maxGrayValue, numberOfNonZeroValues);
    unsigned char *encodedImage = huffman_encode_image(&image, huffmanNodes, numberOfHuffmanNodes, &lengthOfEncodedImageArray);
    store_huffman_encoded_data(outputFilename, image.width, image.height, image.maxGrayValue, numberOfHuffmanNodes, huffmanNodes, lengthOfEncodedImageArray, encodedImage);
    
    return 0;
}
