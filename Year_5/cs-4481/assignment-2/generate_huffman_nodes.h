#ifndef HUFFMAN_NODES
#define HUFFMAN_NODES

#define ONE_COLOR_NODE_COUNT 1

struct node *generate_huffman_nodes(long int *pixel_frequency, int max_gray_value, int number_of_non_zero_values_in_the_frequency_array);

struct node
{
    int first_value;
    int second_value;
};

struct huffmanDecodingNode
{
    int grayValue;
    int leftValue;
    int rightValue;
};

struct huffman_code
{
    // PGM uses 256 values so encoded value should always be less than 8
    unsigned char developingCode[100];
    int developingCodeLength;
};

#endif

