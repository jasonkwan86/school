#include <string.h>
#include <math.h>
#include "libpnm.h"
#include "generate_huffman_nodes.h"

#define ROOT -1
#define INTERNAL -2
#define UNSET -3

bool isDecodingNodeInternal(struct huffmanDecodingNode *decodingNode)
{
    return decodingNode->grayValue == ROOT || decodingNode->grayValue == INTERNAL || decodingNode->grayValue == UNSET;
}

// Build a Huffman tree from the provided huffman_node
// Use the generated Huffman tree to Huffman decode the provided compressed image (decoded_image)
// Store this decoding result as a PGM image structure
struct PGM_Image *huffman_decode_image(int image_width, int image_height, int max_gray_value, int number_of_nodes, struct node *huffman_node, long int length_of_encoded_image_array, unsigned char *encoded_image)
{
    if (number_of_nodes == 1)
    {
        struct PGM_Image *decodedImage = malloc(sizeof(struct PGM_Image));
        create_PGM_Image(decodedImage, image_width, image_height, max_gray_value);

        int onlyGrayValue = huffman_node[0].first_value;

        // Length of encoded image array is in bytes
        for (int row = 0; row < image_height; row++)
        {
            for (int column = 0; column < image_width; column++)
            {
                decodedImage->image[row][column] = onlyGrayValue;
            }
        }

        return decodedImage;
    }

    // Step 1: Build huffman tree
    int numberOfDecodingNodes = 2 * number_of_nodes + 1;
    struct huffmanDecodingNode *decodingNodesArray = malloc(sizeof(struct huffmanDecodingNode) * numberOfDecodingNodes);

    for (int i = 0; i < numberOfDecodingNodes; i++)
    {
        decodingNodesArray[i].grayValue = UNSET;
        decodingNodesArray[i].leftValue = -1;
        decodingNodesArray[i].rightValue = -1;
    }
    
    // Setup root and its children
    int leftValue = huffman_node[number_of_nodes - 1].first_value;
    int rightValue = huffman_node[number_of_nodes - 1].second_value;
    
    // Left values of decoder nodes are indices of decoder array
    decodingNodesArray[0].grayValue = ROOT;
    decodingNodesArray[0].leftValue = 1;
    decodingNodesArray[0].rightValue = 2;

    decodingNodesArray[1].grayValue = leftValue;
    decodingNodesArray[2].grayValue = rightValue;

    int decodingNodeIndex = 3;

    // Setup all other decoder nodes
    for (int nodeIndex = number_of_nodes - 2; nodeIndex >= 0; nodeIndex--)
    {
        // Remember left and rights of huffman nodes are gray values
        int leftGrayValue = huffman_node[nodeIndex].first_value;
        int rightGrayValue = huffman_node[nodeIndex].second_value;

        int found = 0;
        // Do linear search for array with gray value == left value
        for (int searchNodeIndex = 0; searchNodeIndex < numberOfDecodingNodes; searchNodeIndex++)
        {
            // Overwrite the leaf node with gray value equal to left gray value of decoder node
            if (decodingNodesArray[searchNodeIndex].grayValue == leftGrayValue)
            {
                // printf("Found gray value of %d at index %d\n", leftGrayValue, searchNodeIndex);
                if (decodingNodesArray[searchNodeIndex].leftValue != -1 || decodingNodesArray[searchNodeIndex].rightValue != -1)
                {
                    printf("Shouldn't overwrite indices\n");
                    return NULL;
                }
                
                decodingNodesArray[searchNodeIndex].grayValue = INTERNAL;
                decodingNodesArray[searchNodeIndex].leftValue = decodingNodeIndex;
                decodingNodesArray[searchNodeIndex].rightValue = decodingNodeIndex + 1;
                
                decodingNodesArray[decodingNodeIndex].grayValue = leftGrayValue;
                decodingNodesArray[decodingNodeIndex + 1].grayValue = rightGrayValue;
                found = 1;
                break;
            }
        }
        if (found == 0)
        {
            printf("Decoder was not able to find decoder node with gray value %d. Shouldn't happen.\n", leftGrayValue);
            return NULL;
        }
        decodingNodeIndex += 2;
    }

    // Step 2: decode image and store
    struct PGM_Image *decodedImage = malloc(sizeof(struct PGM_Image));
    create_PGM_Image(decodedImage, image_width, image_height, max_gray_value);

    struct huffmanDecodingNode *currentDecodingNode = &decodingNodesArray[0];

    long int currentBit = 0;
    long int pixelsWritten = 0;
    // Length of encoded image array is in bytes
    while (currentBit < length_of_encoded_image_array * 8)
    {
        // Reading encoded image rows from left to right so
        int row = pixelsWritten / image_width;
        int column = pixelsWritten % image_width;
        while (isDecodingNodeInternal(currentDecodingNode))
        {
            long int currentByte = currentBit / 8;
            unsigned char currentChar = encoded_image[currentByte];
            // if bitsSeenThisByte is 0, the mask is 10000000
            // if bitsSeenThisByte is 1, the mask is 01000000
            // if bitsSeenThisByte is 7, the mask is 00000001
            int bitsSeenThisByte = currentBit % 8;
            int mask = 1 << (7 - bitsSeenThisByte);

            if ((currentChar & mask) == 0)
            {
                // This bit is a 0
                currentDecodingNode = &decodingNodesArray[currentDecodingNode->leftValue];
            }
            else
            {
                // This bit is a 1
                currentDecodingNode = &decodingNodesArray[currentDecodingNode->rightValue];
            }
            currentBit++;
        }

        decodedImage->image[row][column] = currentDecodingNode->grayValue;
        currentDecodingNode = &decodingNodesArray[0];
        pixelsWritten++;
        if (pixelsWritten >= image_width * image_height)
        {
            break;
        }
    }

    return decodedImage;
}
