#include "libpnm.h"
#include "read_huffman_encoded_data.h"
#include "huffman_decode_image.h"

// calls the functions developed in Q5 and Q6
// to Huffman decode an input compressed image and store the uncompressed PGM image in a file.

int main(int argc, char *argv[])
{
    if (argc != 3)
    {
        printf("There must be 2 arguments:\n");
        printf("1: <input compressed filename 1> ()\n");
        printf("2: <output pgm filename> ()\n");
        
        return 0;
    }

    char *inputCompressedFilename = argv[1];
    char *outputPgmFilename = argv[2];
    struct PGM_Image *image;

    int imageWidth = 0;
    int imageHeight = 0;
    int maxGrayValue = 0;
    int numberOfNodes = 0;
    struct node *huffmanNodeArray;
    long int lengthOfEncodedImageArray;

    unsigned char *encodedImage = read_huffman_encoded_data(inputCompressedFilename, &imageWidth, &imageHeight, &maxGrayValue, &numberOfNodes, &huffmanNodeArray, &lengthOfEncodedImageArray);
    image = huffman_decode_image(imageWidth, imageHeight, maxGrayValue, numberOfNodes, huffmanNodeArray, lengthOfEncodedImageArray, encodedImage);
    save_PGM_Image(image, outputPgmFilename, 1);

    return 0;
}