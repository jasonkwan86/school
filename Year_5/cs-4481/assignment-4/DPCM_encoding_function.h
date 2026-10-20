#ifndef DPCM_ENCODING_FUNCTION_H
#define DPCM_ENCODING_FUNCTION_H

#define FIRST_PIXEL_PREDICTION 128

#define WEST 1
#define NORTH 2
#define WEST_AND_NORTH_HALF 3
#define CALIC 4

#define UNDEFINED_TERNARY_CODE -1
#define WEST_TERNARY_CODE 0
#define NORTH_TERNARY_CODE 1
#define EXIT_BINARY_TERNARY_CODE 2

#define NUMBER_OF_DIRECTIONS 7

int uniqueElementsInArray(int *array, int arrayMaxSize);

void Encode_Using_DPCM(char *in_PGM_filename_Ptr, int prediction_rule, float *avg_abs_error_Ptr, float *std_abs_error_Ptr);

#endif
