#ifndef LZ77_ENCODING_FUNCTION
#define LZ77_ENCODING_FUNCTION

struct lz77token
{
    int offset, matchLength, nextMismatchedSymbol;
};

void SaveCompressedFile(char *in_PGM_filename_Ptr, char *searchingBufferSize, struct lz77token *tokens, int tokenArraySize, int width, int height, int maxGrayValue);

void CalculateAndSaveOffsetHistogramData(char *in_PGM_filename_Ptr, char *searchingBufferSize, struct lz77token *tokens, int tokenArraySize, float *averageOffset, float *standardDeviationOffset);

void SaveMatchLengthHistogramData(char *in_PGM_filename_Ptr, char *searchingBufferSize, struct lz77token *tokens, int tokenArraySize, float *averageMatchLength, float *standardDeviationMatchLength);

void Encode_Using_LZ77(char *in_PGM_filename_Ptr, unsigned int searching_buffer_size, float *avg_offset_Ptr, float *std_offset_Ptr, float *avg_length_Ptr, float *std_length_Ptr);

#endif