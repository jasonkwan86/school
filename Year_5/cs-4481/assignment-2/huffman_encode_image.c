#include <math.h>
#include <string.h>
#include <stdlib.h>
#include "libpnm.h"
#include "generate_huffman_nodes.h"
#include "huffman_encode_image.h"

#define DEFAULT_COLOR -1

int addHuffmanCodeToString(unsigned char *masterStr, struct huffman_code *huffmanCode, long int *initialMasterStringLength)
{
    // Assume master string has enough space
    for (int i = 0; i < huffmanCode->developingCodeLength; i++)
    {
        masterStr[*initialMasterStringLength + i] = huffmanCode->developingCode[i];
    }

    (*initialMasterStringLength) += huffmanCode->developingCodeLength;
    return 0;
}

int addCharToHuffmanCode(struct huffman_code *huffmanCode, unsigned char charToAdd)
{
    if (huffmanCode->developingCodeLength >= 100)
    {
        printf("Not enough space to add char to huffman code\n");
        return -1;
    }
    huffmanCode->developingCode[huffmanCode->developingCodeLength] = charToAdd;
    huffmanCode->developingCodeLength++;

    return 0;
}

int addLeftAndRightValuesToHuffmanCodes(struct huffman_code *leftHuffmanCode, struct huffman_code *rightHuffmanCode)
{
    if (leftHuffmanCode->developingCodeLength != 0)
    {
        // Copy what's written so far to the right huffman code
        for (int i = 0; i < leftHuffmanCode->developingCodeLength; i++)
        {
            addCharToHuffmanCode(rightHuffmanCode, leftHuffmanCode->developingCode[i]);
        }
    }
    // left value should be 0, right is 1
    addCharToHuffmanCode(leftHuffmanCode, '0');
    addCharToHuffmanCode(rightHuffmanCode, '1');

    return 0;
}

// Generate a Huffman code for each pixel value in the provided image
// Use the generated Huffman codes to Huffman encode the provided image
// Stores in the * length_of_encoded_image_array the length of the encoded image array.
// Return a pointer to an unsigned character array, which will contain the Huffman encoded image after encoding it
unsigned char *huffman_encode_image(struct PGM_Image *input_pgm_image, struct node *huffman_node, int number_of_nodes, long int *length_of_encoded_image_array)
{
    if (number_of_nodes == ONE_COLOR_NODE_COUNT)
    {
        unsigned char *packedHuffmanEncodedImage = calloc(0, sizeof(unsigned char));

        *length_of_encoded_image_array = 0;
        return packedHuffmanEncodedImage;
    }

    // Step 1: Make huffman codes
    // Some huffman codes will be empty but that's fine, they won't be used
    int huffmanCodesArrayLength = input_pgm_image->maxGrayValue + 1;
    
    // Remember each huffman code represents a gray value
    struct huffman_code *huffmanCodes = calloc(huffmanCodesArrayLength, sizeof(struct huffman_code));
    
    if (huffmanCodes == NULL)
    {
        printf("Unsuccessful in allocating memory for huffman codes.\n");
        return NULL;
    }

    for (int grayValue = number_of_nodes - 1; grayValue >= 0; grayValue--)
    {
        // Left and right values are the gray values, which are also the indices of the frequency array
        int leftValue = huffman_node[grayValue].first_value;
        int rightValue = huffman_node[grayValue].second_value;

        addLeftAndRightValuesToHuffmanCodes(&huffmanCodes[leftValue], &huffmanCodes[rightValue]);
    }
    
    // Step 2: Use huffman encoding
    // Amount of space allocated is an upper bound, not every code is 8 bits long
    unsigned char *huffmanEncodedImage = malloc(input_pgm_image->width * input_pgm_image->height * sizeof(unsigned char) * 8);
    if (huffmanEncodedImage == NULL)
    {
        printf("Memory not allocated correctly to encoded image.\n");
    }
    
    long int charsWrittenSoFar = 0;
    for (int row = 0; row < input_pgm_image->height; row++)
    {
        for (int column = 0; column < input_pgm_image->width; column++)
        {
            int grayValue = input_pgm_image->image[row][column];
            if (huffmanCodes[grayValue].developingCodeLength == 0)
            {
                printf("CRITICAL ERROR. Can't find code for this gray value.\n");
                return NULL;
            }
            addHuffmanCodeToString(huffmanEncodedImage, &huffmanCodes[grayValue], &charsWrittenSoFar);
        }
    }

    // Must bitpack it before sending
    long int bitsWritten = 0;
    unsigned char *packedHuffmanEncodedImage = calloc(input_pgm_image->width * input_pgm_image->height, sizeof(unsigned char));
    for (long int i = 0; i < charsWrittenSoFar; i++)
    {
        long int byte = bitsWritten / 8;
        unsigned char currentChar = huffmanEncodedImage[bitsWritten];
        if (currentChar == '0' || currentChar == '1')
        {
            int oneOrZero = currentChar == '1' ? 1 : 0;

            packedHuffmanEncodedImage[byte] <<= 1;
            packedHuffmanEncodedImage[byte] |= oneOrZero;
        }
        else
        {
            printf("Error, shouldn't be reading anything but a 0 or 1\n");
        }
        bitsWritten++;
    }

    *length_of_encoded_image_array = (int)ceil(bitsWritten / 8);
    
    return packedHuffmanEncodedImage;
}
