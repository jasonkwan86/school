#include <stdio.h>
#include "generate_huffman_nodes.h"

// Store the following:
// image_width
// image_height
// max_gray_value
// the number of Huffman nodes
// all values in huffman_node array
// length_of_encoded_image_array
// all values in encoded_image array
void store_huffman_encoded_data(char *compressed_file_name_ptr, int image_width, int image_height, int max_gray_value, int number_of_nodes, struct node *huffman_node, long int length_of_encoded_image_array, unsigned char *encoded_image)
{
    FILE *filePointer;
    filePointer = fopen(compressed_file_name_ptr, "w");

    if (filePointer == NULL)
    {
        printf("Error opening file %s.\n", compressed_file_name_ptr);
        return;
    }

    fprintf(filePointer, "%d\n", image_width);
    fprintf(filePointer, "%d\n", image_height);
    fprintf(filePointer, "%d\n", max_gray_value);
    fprintf(filePointer, "%d\n", number_of_nodes);

    for (int i = 0; i < number_of_nodes; i++)
    {
        fprintf(filePointer, "%d %d\n", huffman_node[i].first_value, huffman_node[i].second_value);
    }

    fprintf(filePointer, "%ld\n", length_of_encoded_image_array);
    fwrite(encoded_image, sizeof(unsigned char), length_of_encoded_image_array, filePointer);

    fclose(filePointer);

    return;
}
