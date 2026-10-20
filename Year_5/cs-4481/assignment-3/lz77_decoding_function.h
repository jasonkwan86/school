#include "libpnm.h"
#include "lz77_encoding_function.h"

void readlz77CompressedFile(char *in_compressed_filename_Ptr, int *width, int *height, int *maxGrayValue, struct lz77token **tokenArray, int *tokenArraySize);

struct PGM_Image *decodeLz77Tokens(int width, int height, int maxGrayValue, struct lz77token *tokenArray, int tokenArraySize);

void Decode_Using_LZ77(char *in_compressed_filename_Ptr);
