#include "libpnm.h"
#include "generate_huffman_nodes.h"

int addHuffmanCodeToString(unsigned char *masterStr, struct huffman_code *huffmanCode, long int *initialMasterStringLength);

int addCharToHuffmanCode(struct huffman_code *huffmanCode, unsigned char charToAdd);

int addLeftAndRightValuesToHuffmanCodes(struct huffman_code *leftHuffmanCode, struct huffman_code *rightHuffmanCode);

unsigned char *huffman_encode_image(struct PGM_Image *input_pgm_image, struct node *huffman_node, int number_of_nodes, long int *length_of_encoded_image_array);
