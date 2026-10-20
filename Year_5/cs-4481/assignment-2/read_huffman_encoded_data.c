#include <stdio.h>
#include <stdlib.h>
#include "generate_huffman_nodes.h"

// image_width
// image_height
// max_gray_value
// the number of Huffman nodes
// all values in the huffman_node array
// length_of_encoded_image_array
// all values of the encoded_image array
unsigned char *read_huffman_encoded_data(char *compressed_file_name_ptr, int *image_width, int *image_height, int *max_gray_value, int *number_of_nodes, struct node **huffman_node, long int *length_of_encoded_image_array)
{
    FILE *filePointer;
    filePointer = fopen(compressed_file_name_ptr, "r");
    if (filePointer == NULL)
    {
        printf("Can't open the file %s", compressed_file_name_ptr);
        return 0;
    }

    fscanf(filePointer, "%d", image_width);
    fscanf(filePointer, "%d", image_height);
    fscanf(filePointer, "%d", max_gray_value);
    fscanf(filePointer, "%d", number_of_nodes);

    struct node *huffmanNodes = malloc(sizeof(struct node) * (*number_of_nodes));

    for (int i = 0; i < *number_of_nodes; i++)
    {
        fscanf(filePointer, "%d %d", &huffmanNodes[i].first_value, &huffmanNodes[i].second_value);
    }

    *huffman_node = huffmanNodes;
    fscanf(filePointer, "%ld", length_of_encoded_image_array);

    unsigned char *encodedImage = malloc(sizeof(unsigned char) * (*length_of_encoded_image_array));

    // Skip the last new line character
    // Make 1 for linux, 2 for windows
    // fseek(filePointer, 2, SEEK_CUR);
    fseek(filePointer, 1, SEEK_CUR);

    fread(encodedImage, 1, *length_of_encoded_image_array, filePointer);

    fclose(filePointer);

    return encodedImage;
}